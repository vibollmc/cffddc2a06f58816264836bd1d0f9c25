using QLVB.Core;
using QLVB.Core.Implementation;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using QLVB.WebUI.Common.NLog;
using QLVB.DAL;
using QLVB.Common.Utilities;

namespace QLVB.WebUI.Common.Files
{
    public class WebApiFileServices
    {
        //private static IAttachVanbanRepository _attachRepo;
        //private static ILogger _logger;

        //public WebApiFileServices(IAttachVanbanRepository attachRepo, ILogger logger)
        //{
        //    _attachRepo = attachRepo;
        //    _logger = logger;
        //}

        private static QLVBDatabase context = new QLVBDatabase();
        //private static RuleFileNameManager _ruleFileName = new RuleFileNameManager();

        #region CheckRequest

        /// <summary>
        /// kiem tra user login trong header
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool ValidUser(string username, string password)
        {
            bool kq = false;
            string userSynch = AppSettings.UserSynch;
            string passSynch = AppSettings.PassSynch;
            if ((username == userSynch) && (password == passSynch))
            {
                kq = true;
            }
            return kq;
            //return true;
        }

        public static string GetHeaderValue(string headerKey, HttpRequestMessage Request)
        {
            try
            {
                string defaultValue = "__error__";
                IEnumerable<string> headerValues = Request.Headers.GetValues(headerKey);
                defaultValue = headerValues.FirstOrDefault();
                return defaultValue;
            }
            catch //(Exception ex)
            {
                //_logger.Error(ex.Message);
                return "__error__";
            }
        }

        #endregion CheckRequest


        //==============================================
        // copy file vao dung thu muc vanbanden/di 
        // va backup file do vao folder dong bo
        //==============================================
        #region Copy_Backup_FileSynch

        /// <summary>
        /// copy file vao dung thu muc congvanden/congvanphathanh
        /// tuy theo ten file
        /// </summary>
        /// <param name="fileName"></param>
        public static void SetFileSynch(string tenfile)
        {
            Dictionary<string, string> result = GetFromFileName(AppSettings.QDTenVBAttach, tenfile);
            int intloai = 0;
            int idvanban = 0;
            foreach (var p in result)
            {
                if (p.Key == "loaivb") { intloai = Convert.ToInt32(p.Value); }
                if (p.Key == "idvanban") { idvanban = Convert.ToInt32(p.Value); }
            }

            if (intloai == 0)
            {   // không tìm thấy tên sổ văn bản 
                // nên không attach vao vanban den/di
            }
            else
            {
                var fileExt = System.IO.Path.GetExtension(tenfile).Substring(1);
                //=======================================================
                // dat ten file theo dinh dang: idvanban_intsttfile.*
                //=======================================================
                int intsttfile;
                var vb = context.AttachVanbans
                    //.Where(p => p.intloai == (int)EnumVanban.intloai_attachvanban.Vanbanden)
                        .Where(p => p.intloai == intloai)
                        .Where(p => p.intidvanban == idvanban);
                intsttfile = (vb.Count() == 0) ? 1 : vb.Count() + 1;

                string strmota = tenfile; //Path.GetFileName(file.FileName);
                //  dinh dang file : idvanban_intsttfile.*

                string fileName = idvanban.ToString() + "_" + intsttfile.ToString() + "." + fileExt;
                //string folderSavepath = FileServices.SetPathUpload(AppConts.FileCongvanden);
                string folderSavepath = FileServices.GetFolderPath(intloai);
                string fileSavepath = Path.Combine(folderSavepath, fileName);
                try
                {
                    //=========================================================
                    // kiem tra xem file nay co ton tai chua??
                    // neu ton tai roi thi dat lai ten moi (them bien dem)
                    //=========================================================
                    int count = 0;
                    while (System.IO.File.Exists(fileSavepath))
                    {
                        count++;
                        fileName = idvanban.ToString() + "_" + intsttfile.ToString()
                                    + "_" + count.ToString() + "." + fileExt;
                        fileSavepath = Path.Combine(folderSavepath, fileName);
                    }
                    //file.SaveAs(fileSavepath);
                    CopyFileSynch(tenfile, fileSavepath);
                    //==================================================
                    //  kiem tra file da duoc upload len server chua
                    //==================================================
                    if (System.IO.File.Exists(fileSavepath))
                    {
                        //==========================================
                        // insert vao database
                        //==========================================
                        int iduser = 0; //(int)SessionService.GetObject(AppConts.SessionUserId);
                        AttachVanban filevb = new AttachVanban();
                        filevb.intidnguoitao = iduser;
                        filevb.intidvanban = idvanban;
                        filevb.intloai = intloai; //(int)EnumVanban.intloai_attachvanban.Vanbanden;
                        filevb.inttrangthai = (int)enumAttachVanban.inttrangthai.IsActive;
                        filevb.strmota = strmota;
                        filevb.strngaycapnhat = DateTime.Now;
                        filevb.strtenfile = fileName;
                        //int intid = _fileRepo.Them(filevb);
                        context.AttachVanbans.Add(filevb);
                        context.SaveChanges();

                        //string strnhatky = _GetNhatkyAttach(intloai, strmota, idvanban);
                        //_logger.Info("Đính kèm file: " + strmota + " vào văn bản đến: " + idvanban);
                        //_logger.Info(strnhatky);
                    }
                }
                catch //(Exception ex)
                {
                    //_logger.Error(ex);
                }
            }
            BackupFileSynch(tenfile);

        }

        /// <summary>
        /// copy file trong folder dong bo vao folder con cua no
        /// va xoa file goc di
        /// </summary>
        /// <param name="fileName">ten file vat ly nam trong dongbo</param>        
        private static void BackupFileSynch(string fileName)
        {
            try
            {
                string sourcePath = FileServices.SetPathSynch(); // folder dongbo                
                string backupPath = FileServices.SetPathUpload(AppConts.FileDongbo);// backup trong folder dongbo

                // Use Path class to manipulate file and directory paths. 
                string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                string backupFile = Path.Combine(backupPath, fileName);

                // To copy a file to another location and  
                // overwrite the destination file if it already exists.                
                System.IO.File.Copy(sourceFile, backupFile, true);
                System.IO.File.Delete(sourceFile);
            }
            catch //(IOException ex)
            {

            }
        }

        /// <summary>
        /// copy file trong folder dong bo vao thu muc vbden/di
        /// </summary>
        /// <param name="fileName">ten file vat ly nam trong dongbo</param>
        /// <param name="destFile">duongdan + tenfile can copy </param>
        private static void CopyFileSynch(string fileName, string destFile)
        {
            try
            {
                string sourcePath = FileServices.SetPathSynch(); // folder dongbo

                // Use Path class to manipulate file and directory paths. 
                string sourceFile = System.IO.Path.Combine(sourcePath, fileName);

                // To copy a file to another location and  
                // overwrite the destination file if it already exists.
                System.IO.File.Copy(sourceFile, destFile, true);

            }
            catch //(IOException ex)
            {

            }
        }

        #endregion Copy_Backup_FileSynch

        public static Dictionary<string, string> GetFromFileName(string strquytac, string strtenvanban)
        {
            // 1-T-YYYY
            // 0 - 2 - 4
            int posTenso = strquytac.IndexOf("T");
            int posSovb = strquytac.IndexOf("1");
            int posYear = strquytac.IndexOf("YYYY");
            // kiem tra vi tri nao =0 

            Dictionary<string, string> result = new Dictionary<string, string>();
            // tên sổ văn bản   : tensovb
            // số văn bản       : sovb
            // năm              : namvb 
            // loại sổ          : loaisovb (vbden=1, vbdi = 2)
            // idvanban         : idvanban

            int idvanban = 0;
            string strtenso = string.Empty;  // mã sổ văn bản
            int intso = 0;      // số văn bản
            int intyear = 0;

            // Số văn bản trước : 1-T-YYYY
            if (posSovb == 0)
            {
                int first_ = strtenvanban.IndexOf("-");
                string strSovb = strtenvanban.Substring(0, first_);
                intso = Convert.ToInt32(strSovb);
                result.Add("sovb", strSovb);

                strtenvanban = strtenvanban.Substring(first_ + 1);
                int second_ = strtenvanban.IndexOf("-");
                strtenso = strtenvanban.Substring(0, second_);
                result.Add("tensovb", strtenso);

                strtenvanban = strtenvanban.Substring(second_ + 1);
                string stryear = strtenvanban.Substring(0, 4);
                intyear = Convert.ToInt32(stryear);
                result.Add("namvb", stryear);

            }

            // tên sổ trước: T-1-YYYY
            if (posTenso == 0)
            {
                int first_ = strtenvanban.IndexOf("-");
                strtenso = strtenvanban.Substring(0, first_);
                result.Add("tensovb", strtenso);

                strtenvanban = strtenvanban.Substring(first_ + 1);
                int second_ = strtenvanban.IndexOf("-");
                string strSovb = strtenvanban.Substring(0, second_);
                intso = Convert.ToInt32(strSovb);
                result.Add("sovb", strSovb);

                strtenvanban = strtenvanban.Substring(second_ + 1);
                string stryear = strtenvanban.Substring(0, 4);
                intyear = Convert.ToInt32(stryear);
                result.Add("namvb", stryear);

            }

            int intloai = 0; // nếu không tìm thấy ký hiệu sổ văn bản

            var sovb = context.SoVanbans.Where(p => p.inttrangthai == (int)enumSovanban.inttrangthai.IsActive)
                        .Where(p => p.strkyhieu == strtenso).FirstOrDefault();
            if (sovb != null)
            {
                intloai = (int)sovb.intloai;
            }

            result.Add("loaivb", intloai.ToString());

            if (intloai == (int)enumSovanban.intloai.Vanbanden)
            {
                idvanban = context.Vanbandens.Where(p => p.intsoden == intso)
                                .Where(p => p.strngayden.Value.Year == intyear) // ==========
                                .Join(
                                    context.SoVanbans.Where(p => p.inttrangthai == (int)enumSovanban.inttrangthai.IsActive)
                                        .Where(p => p.strkyhieu == strtenso),
                                    v => v.intidsovanban,
                                    s => s.intid,
                                    (v, s) => v
                                )
                                .FirstOrDefault().intid;

            }
            if (intloai == (int)enumSovanban.intloai.Vanbanphathanh)
            {
                idvanban = context.Vanbandis.Where(p => p.intso == intso)
                                .Where(p => p.strngayky.Year == intyear)  // ==============
                                .Join(
                                    context.SoVanbans.Where(p => p.inttrangthai == (int)enumSovanban.inttrangthai.IsActive)
                                        .Where(p => p.strkyhieu == strtenso),
                                    v => v.intidsovanban,
                                    s => s.intid,
                                    (v, s) => v
                                )
                                .FirstOrDefault().intid;
            }

            result.Add("idvanban", idvanban.ToString());

            return result;
        }
    }
}