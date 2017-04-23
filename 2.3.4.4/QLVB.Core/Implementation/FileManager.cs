using QLVB.Common.Logging;
using QLVB.Common.Sessions;
using QLVB.Common.Utilities;
using QLVB.Core.Contract;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace QLVB.Core.Implementation
{
    public class FileManager : IFileManager
    {
        #region Constructor

        private ILogger _logger;
        private ISessionServices _session;
        private IAttachVanbanRepository _fileVBRepo;
        private IRoleManager _role;
        private IAttachHosoRepository _fileHsRepo;
        private IDoituongxulyRepository _dtxlRepo;
        private IAttachMailRepository _fileMailRepo;
        private ICanboRepository _canboRepo;
        private ISoVanbanRepository _sovbRepo;
        private IVanbandenRepository _vbdenRepo;
        private IVanbandiRepository _vbdiRepo;

        public FileManager(ILogger logger, IAttachVanbanRepository fileRepo,
            IRoleManager role, IAttachHosoRepository fileHSRepo,
            IDoituongxulyRepository dtxlRepo, IAttachMailRepository fileMailRepo,
            ISessionServices session, ICanboRepository canboRepo,
            ISoVanbanRepository sovbRepo, IVanbandenRepository vbdenRepo,
            IVanbandiRepository vbdiRepo)
        {
            _logger = logger;
            _fileVBRepo = fileRepo;
            _role = role;
            _fileHsRepo = fileHSRepo;
            _dtxlRepo = dtxlRepo;
            _fileMailRepo = fileMailRepo;
            _session = session;
            _canboRepo = canboRepo;
            _sovbRepo = sovbRepo;
            _vbdenRepo = vbdenRepo;
            _vbdiRepo = vbdiRepo;
        }

        #endregion Constructor

        #region Common

        /// <summary>
        /// lay ~/folder cua file dinh kem
        /// </summary>
        /// <param name="strloai"></param>
        /// <param name="dtengaycapnhat"></param>
        /// <returns></returns>
        public string GetFolderDownload(string strloai, DateTime dtengaycapnhat)
        {
            string folderpath = "";
            try
            {
                string strnoidung = AppSettings.Noidung;
                switch (strloai)
                {
                    case AppConts.FileCongvanden:
                        strnoidung += "/" + AppConts.FileCongvanden;
                        break;

                    case AppConts.FileCongvanphathanh:
                        strnoidung += "/" + AppConts.FileCongvanphathanh;
                        break;

                    case AppConts.FileDongbo:
                        strnoidung += "/" + AppConts.FileDongbo;
                        break;

                    case AppConts.FileEmail:
                        strnoidung += "/" + AppConts.FileEmail;
                        break;

                    case AppConts.FileEmailInbox:
                        strnoidung += "/" + AppConts.FileEmailInbox;
                        break;

                    case AppConts.FileEmailOutbox:
                        strnoidung += "/" + AppConts.FileEmailOutbox;
                        break;

                    case AppConts.FileHoso:
                        strnoidung += "/" + AppConts.FileHoso;
                        break;

                    case AppConts.FileVanbanduthao:
                        strnoidung += "/" + AppConts.FileVanbanduthao;
                        break;
                }
                string strfoldernam = dtengaycapnhat.ToString("yyyy"); //dtengaycapnhat.Year.ToString();
                strnoidung += "/" + strfoldernam;

                string strfolderthang = dtengaycapnhat.ToString("MM");//dtengaycapnhat.Month.ToString("d2");
                strnoidung += "/" + strfolderthang;

                string strfolderngay = dtengaycapnhat.ToString("dd");// dtengaycapnhat.Day.ToString();
                strnoidung += "/" + strfolderngay;
                folderpath = strnoidung;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return folderpath;
        }

        /// <summary>
        /// tra ve duong dan va ten file de download
        /// </summary>
        /// <param name="strloai"></param>
        /// <param name="dtengaycapnhat"></param>
        /// <param name="strfilename"></param>
        /// <returns></returns>
        public string GetPhysicalPath(string strloai, DateTime dtengaycapnhat, string strfilename)
        {
            string physicalPath = "";
            try
            {
                string strnoidung = AppSettings.Noidung;
                switch (strloai)
                {
                    case AppConts.FileCongvanden:
                        strnoidung += "/" + AppConts.FileCongvanden;
                        break;

                    case AppConts.FileCongvanphathanh:
                        strnoidung += "/" + AppConts.FileCongvanphathanh;
                        break;

                    case AppConts.FileDongbo:
                        strnoidung += "/" + AppConts.FileDongbo;
                        break;

                    case AppConts.FileEmail:
                        strnoidung += "/" + AppConts.FileEmail;
                        break;

                    case AppConts.FileEmailInbox:
                        strnoidung += "/" + AppConts.FileEmailInbox;
                        break;

                    case AppConts.FileEmailOutbox:
                        strnoidung += "/" + AppConts.FileEmailOutbox;
                        break;

                    case AppConts.FileHoso:
                        strnoidung += "/" + AppConts.FileHoso;
                        break;

                    case AppConts.FileVanbanduthao:
                        strnoidung += "/" + AppConts.FileVanbanduthao;
                        break;
                }
                string strfoldernam = dtengaycapnhat.ToString("yyyy");
                strnoidung += "/" + strfoldernam;

                string strfolderthang = dtengaycapnhat.ToString("MM");
                strnoidung += "/" + strfolderthang;

                string strfolderngay = dtengaycapnhat.ToString("dd");
                strnoidung += "/" + strfolderngay;

                physicalPath = strnoidung;

                string filepath = physicalPath + "/" + strfilename; //Server.MapPath(folderPath);

                physicalPath = HttpContext.Current.Server.MapPath(filepath);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return physicalPath;
        }

        /// <summary>
        /// tạo physical Folder để lưu file tuy theo loai vb dinh kem
        /// </summary>
        /// <param name="strloai"></param>
        /// <returns>physical folder</returns>
        public string SetPathUpload(string strloai)
        {
            string folderpath = "";
            try
            {
                string strnoidung = AppSettings.Noidung;
                switch (strloai)
                {
                    case AppConts.FileCongvanden:
                        strnoidung += "/" + AppConts.FileCongvanden;
                        break;

                    case AppConts.FileCongvanphathanh:
                        strnoidung += "/" + AppConts.FileCongvanphathanh;
                        break;

                    case AppConts.FileDongbo:
                        strnoidung += "/" + AppConts.FileDongbo;
                        break;

                    case AppConts.FileEmail:
                        strnoidung += "/" + AppConts.FileEmail;
                        break;

                    case AppConts.FileEmailInbox:
                        strnoidung += "/" + AppConts.FileEmailInbox;
                        break;

                    case AppConts.FileEmailOutbox:
                        strnoidung += "/" + AppConts.FileEmailOutbox;
                        break;

                    case AppConts.FileHoso:
                        strnoidung += "/" + AppConts.FileHoso;
                        break;

                    case AppConts.FileVanbanduthao:
                        strnoidung += "/" + AppConts.FileVanbanduthao;
                        break;
                    case AppConts.FileEdxmlInbox:
                        strnoidung += "/" + AppConts.FileEdxmlInbox;
                        break;
                    case AppConts.FileEdxmlOutbox:
                        strnoidung += "/" + AppConts.FileEdxmlOutbox;
                        break;
                }
                DateTime ngayhientai = DateTime.Now;
                string strfoldernam = ngayhientai.ToString("yyyy");
                //folderpath += "\\" + strfoldernam;
                strnoidung += "/" + strfoldernam;
                folderpath = HttpContext.Current.Server.MapPath(strnoidung);
                if (!Directory.Exists(folderpath))
                {
                    Directory.CreateDirectory(folderpath);
                }

                string strfolderthang = ngayhientai.ToString("MM");
                //folderpath += "\\" + strfolderthang;
                strnoidung += "/" + strfolderthang;
                //HostingEnvironment.MapPath
                folderpath = HttpContext.Current.Server.MapPath(strnoidung);
                if (!Directory.Exists(folderpath))
                {
                    Directory.CreateDirectory(folderpath);
                }

                string strfolderngay = ngayhientai.ToString("dd");
                //folderpath += "\\" + strfolderngay;
                strnoidung += "/" + strfolderngay;
                folderpath = HttpContext.Current.Server.MapPath(strnoidung);
                if (!Directory.Exists(folderpath))
                {
                    Directory.CreateDirectory(folderpath);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return folderpath;
        }



        /// <summary>
        /// tra ve image tuy theo loai file 
        /// </summary>
        /// <param name="strtenfile"></param>
        /// <returns></returns>
        public string GetFileTypeImages(string strtenfile)
        {
            string imageUrl = string.Empty;
            string fileExt = GetFileExtention(strtenfile);
            switch (fileExt)
            {
                case "doc":
                    imageUrl = "~/Content/Images/FileType2/docx_win-32_32.png";
                    break;
                case "docx":
                    imageUrl = "~/Content/Images/FileType2/docx_win-32_32.png";
                    break;
                case "xls":
                    imageUrl = "~/Content/Images/FileType2/xlsx_win-32_32.png";
                    break;
                case "xlsx":
                    imageUrl = "~/Content/Images/FileType2/xlsx_win-32_32.png";
                    break;
                case "pdf":
                    imageUrl = "~/Content/Images/FileType2/pdf-32_32.png";
                    break;
                case "jpeg":
                    imageUrl = "~/Content/Images/FileType2/jpeg-32_32.png";
                    break;
                case "zip":
                    imageUrl = "~/Content/Images/FileType2/zip-32_32.png";
                    break;
                case "rar":
                    imageUrl = "~/Content/Images/FileType2/zip-32_32.png";
                    break;
                default:
                    imageUrl = "~/Content/Images/FileType2/text-32_32.png";
                    break;
            }
            return imageUrl;
        }


        public string GetFileExtention(string strtenfile)
        {
            string fileExt = "";
            if (!string.IsNullOrEmpty(strtenfile))
            {
                //int len = strtenfile.Length;
                //string[] split = strtenfile.Split(new Char[] { '.' });
                //foreach (string s in split)
                int i = strtenfile.LastIndexOf('.');
                fileExt = strtenfile.Substring(++i);
            }
            return fileExt.ToLower();
        }
        public bool CheckFileExits(string filepath)
        {
            while (System.IO.File.Exists(filepath))
            {
                // count++;
                // fileName = idvanban.ToString() + "_" + intsttfile.ToString()
                //            + "_" + count.ToString() + "." + fileExt;
                //fileSavepath = Path.Combine(folderSavepath, fileName);
            }

            return false;
        }

        #endregion Common

        #region FileReader

        /// <summary>
        /// lay duong dan cua file can xem online
        /// </summary>
        /// <returns></returns>
        private CommonFileViewer GetStringFileViewer(int idfile, int intloai)
        {
            int idcanbo = _session.GetUserId();
            CommonFileViewer file = new CommonFileViewer();
            switch (intloai)
            {
                case (int)enumDownloadFileViewModel.intloai.Vanbanden:
                    file = _GetFileVanban(idfile, intloai, idcanbo);
                    break;
                case (int)enumDownloadFileViewModel.intloai.Vanbandi:
                    file = _GetFileVanban(idfile, intloai, idcanbo);
                    break;
                case (int)enumDownloadFileViewModel.intloai.Vanbanduthao:

                    break;
                case (int)enumDownloadFileViewModel.intloai.VBDT:
                    file = _GetFileVBDT(idfile, intloai, idcanbo);
                    break;
                case 22: // email inbox

                    break;
                case 23: // email outbox

                    break;
                case (int)enumDownloadFileViewModel.intloai.HSCV_Ykien:
                    file = _GetFileHSCV(idfile, intloai, idcanbo);
                    break;
            }
            return file;
        }

        /// <summary>
        /// lay duong dan cua file van ban den/di
        /// </summary>
        /// <param name="idfile"></param>
        /// <param name="intloai"></param>
        /// <returns></returns>
        private CommonFileViewer _GetFileVanban(int idfile, int intloai, int idcanbo)
        {
            try
            {
                CommonFileViewer file = new CommonFileViewer();
                var vb = _fileVBRepo.AttachVanbans
                                   .Where(p => p.intid == idfile)
                    //.Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbanden)
                                   .Where(p => p.intloai == intloai)
                                   .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                                   .FirstOrDefault();
                int idvanban = (int)vb.intidvanban;
                int intloaivb = intloai;//(int)enumAttachVanban.intloai.Vanbanden;
                string strLoaiFile = CheckFolderFileVanbanDownload(intloaivb, idfile, idcanbo, idvanban);
                //if (string.IsNullOrEmpty(strLoaiFile))
                //{
                //    return null;
                //}                        
                file.dteNgaycapnhat = (DateTime)vb.strngaycapnhat;
                file.filename = vb.strtenfile;
                file.idfile = idfile;
                file.intloai = intloaivb;
                file.strLoaiFile = strLoaiFile;

                return file;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// lay duong dan cua file van ban dien tu
        /// </summary>
        /// <param name="idfile"></param>
        /// <param name="intloai"></param>
        /// <param name="idcanbo"></param>
        /// <returns></returns>
        private CommonFileViewer _GetFileVBDT(int idfile, int intloai, int idcanbo)
        {
            try
            {
                CommonFileViewer file = new CommonFileViewer();
                var vb = _fileMailRepo.AttachMails
                               .Where(p => p.intid == idfile)
                               .Where(p => p.intloai == (int)enumAttachMail.intloai.Vanbandendientu)
                               .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                               .FirstOrDefault();
                int idvanban = (int)vb.intidmail;
                int intloaivb = (int)enumAttachMail.intloai.Vanbandendientu;
                string strLoaiFile = CheckFolderFileVBDTDownload(intloaivb, idfile, idcanbo, idvanban);
                file.dteNgaycapnhat = (DateTime)vb.strngaycapnhat;
                file.filename = vb.strtenfile;
                file.idfile = idfile;
                file.intloai = intloaivb;
                file.strLoaiFile = strLoaiFile;
                return file;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// lay duong dan cua file ho so cong viec
        /// </summary>
        /// <param name="idfile"></param>
        /// <param name="intloai"></param>
        /// <param name="idcanbo"></param>
        /// <returns></returns>
        private CommonFileViewer _GetFileHSCV(int idfile, int intloai, int idcanbo)
        {
            try
            {
                CommonFileViewer file = new CommonFileViewer();
                var vb = _fileHsRepo.AttachHosos
                        .Where(p => p.intid == idfile)
                        .Where(p => p.inttrangthai == (int)enumAttachHoso.inttrangthai.IsActive)
                        .Where(p => p.intloai == (int)enumAttachHoso.intloai.Ykien)
                        .FirstOrDefault();
                int idhoso = (int)vb.intidhoso;
                int intloaivb = (int)enumAttachMail.intloai.Vanbandendientu;
                string strLoaiFile = CheckFolderFileHSCVDownload(intloaivb, idfile, idcanbo, idhoso);
                file.dteNgaycapnhat = (DateTime)vb.strngaycapnhat;
                file.filename = vb.strtenfile;
                file.idfile = idfile;
                file.intloai = intloaivb;
                file.strLoaiFile = strLoaiFile;
                return file;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// lay duong dan ~/ cua file pdf 
        /// </summary>
        /// <param name="idfile"></param>
        /// <param name="intloai"></param>
        /// <returns></returns>
        public PdfViewerModel GetPdfViewer(int idfile, int intloai)
        {
            int idcanbo = _session.GetUserId();
            try
            {
                var vb = _fileVBRepo.AttachVanbans
                        .Where(p => p.intid == idfile)
                        .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                        .FirstOrDefault();
                int idvanban = (int)vb.intidvanban;

                string strLoaiFile = CheckFolderFileVanbanDownload(intloai, idfile, idcanbo, idvanban);
                if (string.IsNullOrEmpty(strLoaiFile))
                {
                    return null;
                }

                string filename = vb.strtenfile;
                if (GetFileExtention(filename) == "pdf")
                {
                    string folderPath = GetFolderDownload(strLoaiFile, (DateTime)vb.strngaycapnhat);

                    // ~/Noidung : bo dau ~
                    // loi trien khai qlvb
                    //ViewerJS/#../Noidung/Congvanden/2014/4/23/35_2.pdf
                    //qlvb3/ViewerJS/#../qlvb3/Noidung/Congvanden/2014/4/23/35_2.pdf                    
                    folderPath = folderPath.Substring(1);

                    string filepath = folderPath + "/" + filename; //Server.MapPath(folderPath);

                    PdfViewerModel model = new PdfViewerModel();
                    model.idfile = idfile;
                    model.intloai = intloai;
                    model.filePath = filepath;
                    return model;
                }
                else
                {
                    return null;
                }
            }
            catch (FileNotFoundException ex)
            {
                //throw new HttpException(404, string.Format("Không tìm thấy file {0} .", idfile));
                _logger.Error(ex.Message);
                return null;
            }
        }

        public PdfViewerModel PdfReader(int idfile, int intloai)
        {
            CommonFileViewer file = GetStringFileViewer(idfile, intloai);
            if (GetFileExtention(file.filename) == "pdf")
            {
                string folderPath = GetFolderDownload(file.strLoaiFile, file.dteNgaycapnhat);

                // ~/Noidung : bo dau ~
                // loi trien khai qlvb
                //ViewerJS/#../Noidung/Congvanden/2014/4/23/35_2.pdf
                //qlvb3/ViewerJS/#../qlvb3/Noidung/Congvanden/2014/4/23/35_2.pdf                    
                string viewerjs = folderPath.Substring(1);

                string filepath = folderPath + "/" + file.filename; //Server.MapPath(folderPath);
                string fileviewer = viewerjs + "/" + file.filename;

                // kiem tra xem file co ton tai khong?
                string filesavepath = HttpContext.Current.Server.MapPath(filepath);

                if (System.IO.File.Exists(filesavepath))
                {
                    PdfViewerModel model = new PdfViewerModel();
                    model.idfile = idfile;
                    model.intloai = intloai;
                    model.filePath = fileviewer;
                    return model;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }

        public DocxViewerModel GetDocxViewer(int idfile, int intloai)
        {
            int idcanbo = _session.GetUserId();
            try
            {
                var vb = _fileVBRepo.AttachVanbans
                        .Where(p => p.intid == idfile)
                        .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                        .FirstOrDefault();
                int idvanban = (int)vb.intidvanban;

                string strLoaiFile = CheckFolderFileVanbanDownload(intloai, idfile, idcanbo, idvanban);
                if (string.IsNullOrEmpty(strLoaiFile))
                {
                    return null;
                }

                string filename = vb.strtenfile;
                if (GetFileExtention(filename) == "docx")
                {
                    string folderPath = GetFolderDownload(strLoaiFile, (DateTime)vb.strngaycapnhat);
                    string filepath = folderPath + "/" + filename; //Server.MapPath(folderPath);

                    string physicalPath = HttpContext.Current.Server.MapPath(filepath);

                    DocxViewerModel model = new DocxViewerModel();
                    model.intloai = intloai;
                    model.idfile = idfile;
                    model.physicalFilePath = physicalPath;
                    model.filename = filename;

                    return model;
                }
                else
                {
                    return null;
                }
            }
            catch (FileNotFoundException ex)
            {
                //throw new HttpException(404, string.Format("Không tìm thấy file {0} .", idfile));
                _logger.Error(ex.Message);
                return null;
            }
        }

        public DocxViewerModel DocxReader(int idfile, int intloai)
        {
            CommonFileViewer file = GetStringFileViewer(idfile, intloai);
            if (GetFileExtention(file.filename) == "docx")
            {
                string folderPath = GetFolderDownload(file.strLoaiFile, file.dteNgaycapnhat);
                string filepath = folderPath + "/" + file.filename;

                string physicalPath = HttpContext.Current.Server.MapPath(filepath);

                DocxViewerModel model = new DocxViewerModel();
                model.intloai = intloai;
                model.idfile = idfile;
                model.physicalFilePath = physicalPath;
                model.filename = file.filename;

                return model;
            }
            else
            {
                return null;
            }
        }

        #endregion FileReader

        #region Download

        public FileDownloadResult DownloadVanban(int idfile, int intloai)
        {
            int idcanbo = _session.GetUserId();

            try
            {
                var vb = _fileVBRepo.AttachVanbans
                        .Where(p => p.intid == idfile)
                        .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                        .FirstOrDefault();
                int idvanban = (int)vb.intidvanban;

                string strLoaiFile = CheckFolderFileVanbanDownload(intloai, idfile, idcanbo, idvanban);
                if (string.IsNullOrEmpty(strLoaiFile))
                {
                    return null;
                }

                string filename = vb.strtenfile;
                string folderPath = GetFolderDownload(strLoaiFile, (DateTime)vb.strngaycapnhat);
                string filepath = folderPath + "/" + filename; //Server.MapPath(folderPath);

                //var fileData = GetFileData(filename, filepath);
                string physicalFilePath = HttpContext.Current.Server.MapPath(filepath);

                return new FileDownloadResult(filename, vb.strmota, filepath, physicalFilePath);
            }
            catch (FileNotFoundException ex)
            {
                //throw new HttpException(404, string.Format("Không tìm thấy file {0} .", idfile));
                _logger.Error(ex.Message);
                return null;
            }
        }

        public FileDownloadResult DownloadVBDT(int idfile, int intloai)
        {
            int idcanbo = _session.GetUserId();

            try
            {
                var vb = _fileMailRepo.AttachMails
                        .Where(p => p.intid == idfile)
                        .Where(p => p.inttrangthai == (int)enumAttachMail.inttrangthai.IsActive)
                        .FirstOrDefault();
                int idvanban = (int)vb.intidmail;

                string strLoaiFile = CheckFolderFileVBDTDownload(intloai, idfile, idcanbo, idvanban);
                if (string.IsNullOrEmpty(strLoaiFile))
                {
                    return null;
                }

                string filename = vb.strtenfile;
                string folderPath = GetFolderDownload(strLoaiFile, (DateTime)vb.strngaycapnhat);
                string filepath = folderPath + "/" + filename;

                //var fileData = GetFileData(filename, filepath);
                string physicalFilePath = HttpContext.Current.Server.MapPath(filepath);

                return new FileDownloadResult(filename, vb.strmota, filepath, physicalFilePath);
            }
            catch (FileNotFoundException ex)
            {
                //throw new HttpException(404, string.Format("Không tìm thấy file {0} .", idfile));
                _logger.Error(ex.Message);
                return null;
            }
        }

        public FileDownloadResult DownloadHoso(int idfile, int intloai)
        {
            //int idcanbo = _session.GetUserId();
            // kiem tra user co quyen xem/xu ly ho so nay khong

            try
            {
                var hs = _fileHsRepo.AttachHosos
                        .Where(p => p.intid == idfile)
                        .Where(p => p.inttrangthai == (int)enumAttachHoso.inttrangthai.IsActive)
                    //.Where(p => p.intloai == intloai)
                        .FirstOrDefault();

                string strLoaiFile = AppConts.FileHoso;

                string filename = hs.strtenfile;
                string folderPath = GetFolderDownload(strLoaiFile, (DateTime)hs.strngaycapnhat);
                string filepath = folderPath + "/" + filename;

                string physicalFilePath = HttpContext.Current.Server.MapPath(filepath);

                return new FileDownloadResult(filename, hs.strmota, filepath, physicalFilePath);
            }
            catch (FileNotFoundException ex)
            {
                //throw new HttpException(404, string.Format("Không tìm thấy file {0} .", idfile));
                _logger.Error(ex.Message);
                return null;
            }
        }

        #region Private Methods

        /// <summary>
        /// kiem tra quyen va lay duong dan download cua vanbanden/di
        /// </summary>
        /// <returns></returns>
        public string CheckFolderFileVanbanDownload(int intloai, int idfile, int idcanbo, int idvanban)
        {
            string strLoaiFile = string.Empty;
            switch (intloai)
            {
                case (int)enumAttachVanban.intloai.Vanbanden:
                    strLoaiFile = AppConts.FileCongvanden;
                    if (!_role.IsDownloadFileVanbanden(idfile, idcanbo, idvanban))
                    {
                        _logger.Warn("KHÔNG CÓ QUYỀN DOWNLOAD VĂN BẢN ĐẾN, FILE ID: " + idfile.ToString());
                        return null;
                    }
                    break;

                case (int)enumAttachVanban.intloai.Vanbandi:
                    strLoaiFile = AppConts.FileCongvanphathanh;
                    if (!_role.IsDownloadFileVanbandi(idfile, idcanbo, idvanban))
                    {
                        _logger.Warn("KHÔNG CÓ QUYỀN DOWNLOAD VĂN BẢN ĐI, FILE ID: " + idfile.ToString());
                        return null;
                    }
                    break;
                //case (int)EnumVanban.intloai_attachvanban.Vanbandenmail:
                //    strLoaiFile = AppConts.FileEmail;
                //    break;
                //case (int)EnumVanban.intloai_attachvanban.EmailContent:
                //    strLoaiFile = AppConts.FileEmailContent;
                //    break;
                case (int)enumAttachVanban.intloai.Vanbanduthao:
                    strLoaiFile = AppConts.FileVanbanduthao;
                    break;
                //default:
                //    Console.WriteLine("Default case");
                //    break;
            }
            return strLoaiFile;
        }

        /// <summary>
        /// kiem tra quyen va lay duong dan download cua van ban dien tu / mail inbox,outbox
        /// </summary>
        /// <param name="intloai"></param>
        /// <param name="idfile"></param>
        /// <param name="idcanbo"></param>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        private string CheckFolderFileVBDTDownload(int intloai, int idfile, int idcanbo, int idvanban)
        {
            string strLoaiFile = string.Empty;
            switch (intloai)
            {
                case (int)enumAttachMail.intloai.Vanbandendientu:
                    strLoaiFile = AppConts.FileEmail;
                    //if (!_role.IsDownloadFileVanbanden(idfile, idcanbo, idvanban))
                    //{
                    //    _logger.Warn("KHÔNG CÓ QUYỀN DOWNLOAD VĂN BẢN ĐẾN ĐIỆN TỬ, FILE ID: " + idfile.ToString());
                    //    return null;
                    //}
                    break;

                case (int)enumAttachMail.intloai.MailInbox:
                    strLoaiFile = AppConts.FileEmailInbox;
                    //if (!_role.IsDownloadFileVanbandi(idfile, idcanbo, idvanban))
                    //{
                    //    _logger.Warn("KHÔNG CÓ QUYỀN DOWNLOAD VĂN BẢN MAIL INBOX, FILE ID: " + idfile.ToString());
                    //    return null;
                    //}
                    break;
                case (int)enumAttachMail.intloai.MailOutbox:
                    strLoaiFile = AppConts.FileEmailOutbox;
                    break;
                //default:
                //    Console.WriteLine("Default case");
                //    break;
            }
            return strLoaiFile;
        }

        /// <summary>
        /// kiem tra quyen va lay duong dan download cua ho so congviec
        /// </summary>
        /// <param name="intloai"></param>
        /// <param name="idfile"></param>
        /// <param name="idcanbo"></param>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        private string CheckFolderFileHSCVDownload(int intloai, int idfile, int idcanbo, int idhoso)
        {
            string strLoaiFile = string.Empty;
            switch (intloai)
            {
                case (int)enumAttachHoso.intloai.Ykien:
                    strLoaiFile = AppConts.FileHoso;
                    if (!_role.IsDownloadFileHosocongviec(idfile, idcanbo, idhoso))
                    {
                        _logger.Warn("KHÔNG CÓ QUYỀN DOWNLOAD HỒ SƠ CÔNG VIỆC, FILE ID: " + idfile.ToString());
                        return null;
                    }
                    break;

                case (int)enumAttachHoso.intloai.Phieutrinh:
                    //strLoaiFile = AppConts.FileCongvanphathanh;
                    //if (!_role.IsDownloadFileVanbandi(idfile, idcanbo, idhoso))
                    //{
                    //    _logger.Warn("KHÔNG CÓ QUYỀN DOWNLOAD VĂN BẢN ĐI, FILE ID: " + idfile.ToString());
                    //    return null;
                    //}
                    break;
            }
            return strLoaiFile;
        }
        private byte[] GetFileData(string fileName, string filePath)
        {
            var fullFilePath = string.Format("{0}/{1}", filePath, fileName);
            if (!File.Exists(fullFilePath))
                throw new FileNotFoundException("The file does not exist.", fullFilePath);
            return File.ReadAllBytes(fullFilePath);
        }


        #endregion Private Methods

        #endregion Download

        #region Upload

        #region Vanban

        /// <summary>
        /// luu file van ban vao server
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="idvanban"></param>
        /// <param name="idloai">loai file: vb den/di/duthao</param>
        /// <returns></returns>
        public ResultFunction UploadVanban(HttpPostedFileBase file, int idvanban, int idloai)
        {
            ResultFunction kq = new ResultFunction();

            var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
            //====================================
            //  kiem tra dinh dang file
            //====================================
            //if (!supportedTypes.Contains(fileExt))
            //{
            //  ModelState.AddModelError("photo", "Invalid type. Only the following types (jpg, jpeg, png) are supported.");
            //return View();
            //}
            //=======================================================
            // dat ten file theo dinh dang: idvanban_intsttfile.*
            //=======================================================
            int intsttfile;
            var vb = _fileVBRepo.AttachVanbans
                //.Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbanden)
                    .Where(p => p.intloai == idloai)
                    .Where(p => p.intidvanban == idvanban);
            intsttfile = (vb.Count() == 0) ? 1 : vb.Count() + 1;

            string strmota = Path.GetFileName(file.FileName);

            //  dinh dang file : idvanban_intsttfile.*
            string fileName = idvanban.ToString() + "_" + intsttfile.ToString() + "." + fileExt;

            string strLoaiFile = "";
            switch (idloai)
            {
                case (int)enumAttachVanban.intloai.Vanbanden:
                    strLoaiFile = AppConts.FileCongvanden;
                    break;

                case (int)enumAttachVanban.intloai.Vanbandi:
                    strLoaiFile = AppConts.FileCongvanphathanh;
                    break;
            }

            string folderSavepath = SetPathUpload(strLoaiFile);

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
                file.SaveAs(fileSavepath);
                //==================================================
                //  kiem tra file da duoc upload len server chua
                //==================================================
                if (System.IO.File.Exists(fileSavepath))
                {
                    //==========================================
                    // insert vao database
                    //==========================================
                    int iduser = _session.GetUserId();
                    AttachVanban filevb = new AttachVanban();
                    filevb.intidnguoitao = iduser;
                    filevb.intidvanban = idvanban;
                    filevb.intloai = idloai; //(int)EnumVanban.intloai_attachvanban.Vanbanden;
                    filevb.inttrangthai = (int)enumAttachVanban.inttrangthai.IsActive;
                    filevb.strmota = strmota;
                    filevb.strngaycapnhat = DateTime.Now;
                    filevb.strtenfile = fileName;
                    int intid = _fileVBRepo.Them(filevb);
                    _logger.Info("Đính kèm file: " + strmota + " vào văn bản : " + idvanban);
                }
                kq.id = (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                kq.id = (int)ResultViewModels.Error;
            }
            return kq;
        }

        /// <summary>
        /// danh dau xoa file van ban trong database
        /// </summary>
        /// <param name="idfile"></param>
        /// <param name="idvanban"></param>
        public void DeActiveFileUploadVanban(int idfile)
        {
            try
            {
                // xoa file dinh kem trong sql
                // chua xoa file vat ly trong noidung
                int idcanbo = _session.GetUserId();
                string strmota = _fileVBRepo.Xoa(idfile, idcanbo);
                _logger.Info("Xóa file đính kèm: " + strmota + ", idfile: " + idfile);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public void DeleteFileUploadVanban(string[] fileNames, int idvanban, int idloai)
        {
            // kiem tra quyen xoa file

            // lay ds tat ca cac file dinh kem cua idvanban
            // roi so sanh voi filename can xoa

            var fileAttach = _fileVBRepo.AttachVanbans
                //.Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbanden)
                            .Where(p => p.intloai == idloai)
                            .Where(p => p.intidvanban == idvanban)
                            .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                            .AsEnumerable();

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var file = fileAttach.Where(p => p.strmota == fullName)
                        .FirstOrDefault();

                    if (file.intid != 0)
                    {
                        // tim thay file
                        DeActiveFileUploadVanban(file.intid);

                        var fileName = file.strtenfile;

                        string strLoaiFile = "";
                        switch (idloai)
                        {
                            case (int)enumAttachVanban.intloai.Vanbanden:
                                strLoaiFile = AppConts.FileCongvanden;
                                break;

                            case (int)enumAttachVanban.intloai.Vanbandi:
                                strLoaiFile = AppConts.FileCongvanphathanh;
                                break;
                            //case (int)EnumVanban.intloai_attachvanban.Vanbandenmail:
                            //    strLoaiFile = AppConts.FileEmail;
                            //    break;
                            //case (int)EnumVanban.intloai_attachvanban.EmailContent:
                            //    strLoaiFile = AppConts.FileEmailContent;
                            //    break;
                            case (int)enumAttachVanban.intloai.Vanbanduthao:
                                strLoaiFile = AppConts.FileVanbanduthao;
                                break;
                            //default:
                            //    Console.WriteLine("Default case");
                            //    break;
                        }

                        string folderPath = GetFolderDownload(strLoaiFile, (DateTime)file.strngaycapnhat);
                        string filepath = folderPath;

                        var physicalPath = Path.Combine(
                            HttpContext.Current.Server.MapPath(filepath), fileName);

                        // TODO: Verify user permissions

                        if (System.IO.File.Exists(physicalPath))
                        {
                            // The files are not actually removed in this demo
                            System.IO.File.Delete(physicalPath);
                        }
                    }
                }
            }
        }

        #endregion Vanban

        #region Hoso

        /// <summary>
        ///  tra ve intiddoituongxuly cua idcanbo dang xem/xu ly ho so
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="idcanbo"></param>
        /// <returns>0:khong phai user dang trong luong xu ly</returns>
        private int GetIdDoituongxuly(int idhoso, int idcanbo)
        {
            int IdCurrentUser = 0;
            var dtxl = _dtxlRepo.GetCanboDangXulys
                .Where(p => p.intidhosocongviec == idhoso)
                .Where(p => p.intidcanbo == idcanbo)
                .OrderByDescending(p => p.intvaitro)
                .FirstOrDefault();
            if (dtxl != null)
            {
                IdCurrentUser = dtxl.intid;
            }
            else
            {
                IdCurrentUser = 0;
            }
            return IdCurrentUser;
        }

        /// <summary>
        /// luu cac file dinh kem trong hoso
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="idhoso"></param>
        /// <param name="idtailieu"></param>
        /// <param name="idloai"></param>
        /// <returns></returns>
        public ResultFunction UploadHoso(HttpPostedFileBase file, int idhoso, int idtailieu, int idloai)
        {
            ResultFunction kq = new ResultFunction();

            var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
            //====================================
            //  kiem tra dinh dang file
            //====================================
            //if (!supportedTypes.Contains(fileExt))
            //{
            //  ModelState.AddModelError("photo", "Invalid type. Only the following types (jpg, jpeg, png) are supported.");
            //return View();
            //}
            //=======================================================
            // dat ten file theo dinh dang: idhoso_idloai_intsttfile.*
            //=======================================================
            int intsttfile;

            var hs = _fileHsRepo.AttachHosos
                .Where(p => p.intloai == idloai)
                .Where(p => p.intidhoso == idhoso)
                .Where(p => p.intidtailieu == idtailieu);

            intsttfile = (hs.Count() == 0) ? 1 : hs.Count() + 1;

            string strmota = Path.GetFileName(file.FileName);

            //  dinh dang file : idhoso_idloai_intsttfile.*
            string fileName = idhoso.ToString()
                            + "_" + idloai.ToString()
                            + "_" + intsttfile.ToString() + "." + fileExt;

            string strLoaiFile = AppConts.FileHoso;

            string folderSavepath = SetPathUpload(strLoaiFile);

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
                    fileName = idhoso.ToString()
                            + "_" + idloai.ToString()
                            + "_" + intsttfile.ToString()
                            + "_" + count.ToString() + "." + fileExt;
                    fileSavepath = Path.Combine(folderSavepath, fileName);
                }
                file.SaveAs(fileSavepath);
                //==================================================
                //  kiem tra file da duoc upload len server chua
                //==================================================
                if (System.IO.File.Exists(fileSavepath))
                {
                    //==========================================
                    // insert vao database
                    //==========================================
                    int iduser = _session.GetUserId();
                    int iddtxl = GetIdDoituongxuly(idhoso, iduser);

                    AttachHoso filevb = new AttachHoso();
                    filevb.intidhoso = idhoso;
                    filevb.intidtailieu = idtailieu;
                    filevb.intloai = idloai; //(int)enumAttachHoso.intloai.ykien
                    filevb.inttrangthai = (int)enumAttachHoso.inttrangthai.IsActive;
                    filevb.intidnguoitao = iddtxl;

                    filevb.strtenfile = fileName;
                    filevb.strmota = strmota;
                    filevb.strngaycapnhat = DateTime.Now;

                    int intid = _fileHsRepo.Them(filevb);

                    //_logger.Info("Đính kèm file: " + strmota + " vào văn bản đến: " + idvanban);
                }
                kq.id = (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                kq.id = (int)ResultViewModels.Error;
            }
            return kq;
        }

        /// <summary>
        /// xoa file dinh kem trong ho so
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="idhoso"></param>
        /// <param name="idtailieu"></param>
        /// <param name="idloai"></param>
        public void DeleteFileUploadHoso(string[] fileNames, int idhoso, int idtailieu, int idloai)
        {
            var HosoAttach = _fileHsRepo.AttachHosos
                .Where(p => p.intloai == idloai)
                .Where(p => p.intidhoso == idhoso)
                .Where(p => p.intidtailieu == idtailieu)
                .AsEnumerable();

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var file = HosoAttach.Where(p => p.strmota == fullName)
                        .FirstOrDefault();

                    if (file.intid != 0)
                    {
                        // tim thay file
                        _fileHsRepo.Xoa(file.intid);

                        var fileName = file.strtenfile;
                        string strLoaiFile = AppConts.FileHoso;

                        string folderPath = GetFolderDownload(strLoaiFile, (DateTime)file.strngaycapnhat);

                        string filepath = folderPath;

                        var physicalPath = Path.Combine(
                            HttpContext.Current.Server.MapPath(filepath), fileName);

                        // TODO: Verify user permissions

                        if (System.IO.File.Exists(physicalPath))
                        {
                            System.IO.File.Delete(physicalPath);
                        }
                    }
                }
            }
        }

        #endregion Hoso

        #region ImageProfile

        /// <summary>
        /// update avatar cua user
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public ResultFunction UploadImageProfile(HttpPostedFileBase file)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                var allowedExtensions = new[] { ".png", ".jpeg", ".jpg", ".gif" };
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    // Not allowed
                    kq.id = (int)ResultViewModels.Error;
                    kq.message = "Bạn chỉ được chọn các file hình ảnh";
                }
                else
                {
                    //AppConts.ImageProfile; //"~/Content/Images/Users/";
                    DateTime ngayhientai = DateTime.Now;
                    string strnam = ngayhientai.ToString("yyyy");
                    string strthang = ngayhientai.ToString("MM");
                    string strngay = ngayhientai.ToString("dd");
                    string folderpath = HttpContext.Current.Server.MapPath(AppConts.ImageProfile);

                    int iduser = _session.GetUserId();
                    var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);

                    string filename = iduser.ToString() + "_" + strnam + strthang + strngay + "." + fileExt;
                    string fileSavepath = Path.Combine(folderpath, filename);
                    file.SaveAs(fileSavepath);

                    _canboRepo.UpdateImageProfile(iduser, filename);

                    _logger.Info("Cập nhật hình đại diện");
                    kq.id = (int)ResultViewModels.Success;
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
            }
            return kq;
        }

        #endregion ImageProfile

        #endregion Upload
    }
}