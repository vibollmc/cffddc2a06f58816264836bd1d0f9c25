using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO.Vanbandientu;
using QLVB.Core.Contract;
using QLVB.Common.Logging;
using QLVB.Common.Crypt;
using QLVB.Common.Utilities;
using QLVB.DTO;
using QLVB.Common.Sessions;
using QLVB.Common.Date;
using System.Net.Http;
using Newtonsoft.Json;
using QLVB.DTO.Hethong;

namespace QLVB.Core.Implementation
{
    public class VanbandientuManager : IVanbandientuManager
    {
        #region Constructor

        private ILogger _logger;
        private ISessionServices _session;
        private IConfigRepository _configRepo;
        private IVanbandenmailRepository _vbdenMailRepo;
        private IRoleManager _role;
        private IMailManager _mail;
        private IGuiVanbanRepository _guivbRepo;
        private IVanbandiRepository _vbdiRepo;
        private IAttachVanbanRepository _fileRepo;
        private ITochucdoitacRepository _tochucRepo;
        private IFileManager _fileManager;
        private IAttachMailRepository _filemailRepo;
        private ISqlQuery _sqlRepo;
        private IVanbandenRepository _vbdenRepo;
        private ILogGuiTonghopVBRepository _guitonghopvbRepo;
        private ITonghopVanbanRepository _tonghopvbRepo;
        public VanbandientuManager(
                ILogger logger, IConfigRepository configRepo, ISessionServices session,
                IVanbandenmailRepository vbdenMailRepo, IRoleManager role,
                IMailManager mail, IGuiVanbanRepository guivbRepo,
                IVanbandiRepository vbdiRepo, IAttachVanbanRepository fileRepo,
                ITochucdoitacRepository tochucRepo, IFileManager fileManager,
                IAttachMailRepository filemailRepo, ISqlQuery sqlRepo,
                IVanbandenRepository vbdenRepo, ILogGuiTonghopVBRepository guitonghopvbRepo,
                ITonghopVanbanRepository tonghopvbRepo)
        {
            _logger = logger;
            _configRepo = configRepo;
            _session = session;
            _vbdenMailRepo = vbdenMailRepo;
            _role = role;
            _mail = mail;
            _guivbRepo = guivbRepo;
            _vbdiRepo = vbdiRepo;
            _fileRepo = fileRepo;
            _tochucRepo = tochucRepo;
            _fileManager = fileManager;
            _filemailRepo = filemailRepo;
            _sqlRepo = sqlRepo;
            _vbdenRepo = vbdenRepo;
            _guitonghopvbRepo = guitonghopvbRepo;
            _tonghopvbRepo = tonghopvbRepo;
        }

        #endregion Constructor

        #region vanbanden
        public IEnumerable<ListVanbandendientuViewModel> GetListVanbandendientu(
            string strngaykycat, string strngaynhancat, int? inttinhtrangcat,
            int? intsodenbd, int? intsodenkt, string strngaynhanbd, string strngaynhankt,
            string strngaykybd, string strngaykykt, string strngayguibd, string strngayguikt,
            string strkyhieu, string strnoigui, string strtrichyeu,
            string truclienthong, string strmadinhdanh
            )
        {
            var vanban = _vbdenMailRepo.Vanbandenmails;
            vanban = _GetVanbandenFromRequest(
                 strngaykycat, strngaynhancat, inttinhtrangcat,
             intsodenbd, intsodenkt, strngaynhanbd, strngaynhankt,
             strngaykybd, strngaykykt, strngayguibd, strngayguikt,
             strkyhieu, strnoigui, strtrichyeu,
             truclienthong, strmadinhdanh);
            
            var listvb = vanban
                .Select(p => new ListVanbandendientuViewModel
                {
                    intid = p.intid,
                    dtengayky = p.strngayky,
                    strkyhieu = p.strkyhieu,
                    intso = p.intso,
                    strtrichyeu = p.strtrichyeu,
                    strnoiguivb = p.strnoiguivb,
                    
                    strAddressSend = p.intnhanvanbantu == enumVanbandenmail.intnhanvanbantu.Email ? p.strAddressSend : p.strmadinhdanh,
                    //strAddressSend = p.strAddressSend 

                    dtengayguivb = p.strngayguivb,
                    dtengaynhanvb = p.strngaynhanvb,
                    inttrangthai = p.inttrangthai,
                    IsAttach = p.intattach == (int)enumVanbandenmail.intattach.Co ? true : false
                });

            return listvb;
        }

        private IQueryable<Vanbandenmail> _GetVanbandenFromRequest(
           string strngaykycat, string strngaynhancat, int? inttinhtrangcat,
           int? intsodenbd, int? intsodenkt, string strngaynhanbd, string strngaynhankt,
           string strngaykybd, string strngaykykt, string strngayguibd, string strngayguikt,
           string strkyhieu, string strnoigui, string strtrichyeu,
           string truclienthong, string strmadinhdanh
           )
        {
            bool isSearch = false;
            bool isCategory = false;
            string strSearchValues = string.Empty;
            // strSearchValues = "intsodenbd=1;intsodenkt=10;idloaivb=2;"
            //===========================================================
            var vanban = _vbdenMailRepo.Vanbandenmails;
            //====================================================
            // tuy chon category 
            //====================================================
            if (!string.IsNullOrEmpty(strngaykycat))
            {
                DateTime? dtengayky = DateServices.FormatDateEn(strngaykycat);
                vanban = vanban.Where(p => p.strngayky == dtengayky);
                isSearch = true;
                isCategory = true;
                strSearchValues += "strngaykycat=" + strngaykycat + ";";
            }
            if (!string.IsNullOrEmpty(strngaynhancat))
            {
                DateTime? dtengaynhan = DateServices.FormatDateEn(strngaynhancat);
                DateTime? dtengaynhankt = dtengaynhan.Value.AddDays(1);
                vanban = vanban.Where(p => p.strngaynhanvb > dtengaynhan)
                            .Where(p => p.strngaynhanvb < dtengaynhankt);
                isSearch = true;
                isCategory = true;
                strSearchValues += "strngaynhancat=" + strngaynhancat + ";";
            }
            if ((inttinhtrangcat != null) && (intsodenbd != 0))
            {
                int inttrangthai = 0;
                switch (inttinhtrangcat)
                {
                    case 1:
                        inttrangthai = (int)enumVanbandenmail.inttrangthai.Chuacapnhat;
                        vanban = vanban.Where(p => p.inttrangthai == inttrangthai);
                        break;
                    case 2:
                        inttrangthai = (int)enumVanbandenmail.inttrangthai.Dacapnhat;
                        vanban = vanban.Where(p => p.inttrangthai == inttrangthai);
                        break;
                    case 3: // tat ca van ban

                        break;
                };
                //vanban = vanban.Where(p => p.inttrangthai == inttrangthai);
                isSearch = true;
                isCategory = true;
                strSearchValues += "inttinhtrangcat=" + inttinhtrangcat.ToString() + ";";
            }
            //====================================================
            // Search van ban
            //====================================================
            if ((intsodenkt != null) && (intsodenkt != 0))
            {
                if ((intsodenbd != null) && (intsodenbd != 0))
                {
                    vanban = vanban.Where(p => p.intso >= intsodenbd)
                            .Where(p => p.intso <= intsodenkt);
                    isSearch = true;
                    strSearchValues += "intsodenbd=" + intsodenbd.ToString() + ";intsodenkt=" + intsodenkt.ToString() + ";";
                }
            }
            else
            {
                if ((intsodenbd != null) && (intsodenbd != 0))
                {
                    vanban = vanban.Where(p => p.intso == intsodenbd);
                    isSearch = true;
                    strSearchValues += "intsodenbd=" + intsodenbd.ToString() + ";";
                }
            }

            if (!string.IsNullOrEmpty(strngaynhankt))
            {
                if (!string.IsNullOrEmpty(strngaynhanbd))
                {
                    DateTime? dtngaynhanbd = DateServices.FormatDateEn(strngaynhanbd);
                    DateTime? dtngaynhankt = DateServices.FormatDateEn(strngaynhankt);
                    vanban = vanban.Where(p => p.strngaynhanvb >= dtngaynhanbd)
                            .Where(p => p.strngaynhanvb <= dtngaynhankt);
                    isSearch = true;
                    strSearchValues += "strngaynhanbd=" + strngaynhanbd + ";strngaynhankt=" + strngaynhankt + ";";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(strngaynhanbd))
                {
                    DateTime? dtngaynhanbd = DateServices.FormatDateEn(strngaynhanbd);
                    //vanban = vanban.Where(p => p.strngaynhanvb == dtngaynhanbd);
                    vanban = vanban.Where(p => System.Data.Entity.DbFunctions.TruncateTime(p.strngaynhanvb) == dtngaynhanbd);
                    isSearch = true;
                    strSearchValues += "strngaynhanbd=" + strngaynhanbd + ";";
                }
            }

            if (!string.IsNullOrEmpty(strngaykykt))
            {
                if (!string.IsNullOrEmpty(strngaykybd))
                {
                    DateTime? dtngaykybd = DateServices.FormatDateEn(strngaykybd);
                    DateTime? dtngaykykt = DateServices.FormatDateEn(strngaykykt);
                    vanban = vanban.Where(p => p.strngayky >= dtngaykybd)
                            .Where(p => p.strngayky <= dtngaykykt);
                    isSearch = true;
                    strSearchValues += "strngaykybd=" + strngaykybd + ";strngaykykt=" + strngaykykt + ";";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(strngaykybd))
                {
                    DateTime? dtngaykybd = DateServices.FormatDateEn(strngaykybd);
                    vanban = vanban.Where(p => System.Data.Entity.DbFunctions.TruncateTime(p.strngayky) == dtngaykybd);
                    isSearch = true;
                    strSearchValues += "strngaykybd=" + strngaykybd + ";";
                }
            }

            if (!string.IsNullOrEmpty(strngayguikt))
            {
                if (!string.IsNullOrEmpty(strngayguibd))
                {
                    DateTime? dtngayguibd = DateServices.FormatDateEn(strngayguibd);
                    DateTime? dtngayguikt = DateServices.FormatDateEn(strngayguikt);
                    vanban = vanban.Where(p => p.strngayguivb >= dtngayguibd)
                            .Where(p => p.strngayguivb <= dtngayguikt);
                    isSearch = true;
                    strSearchValues += "strngayguibd=" + strngayguibd + ";strngayguikt=" + strngayguikt + ";";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(strngayguibd))
                {
                    DateTime? dtngayguibd = DateServices.FormatDateEn(strngayguibd);
                    vanban = vanban.Where(p => System.Data.Entity.DbFunctions.TruncateTime(p.strngayguivb) == dtngayguibd);
                    isSearch = true;
                    strSearchValues += "strngayguibd=" + strngayguibd + ";";
                }
            }

            if (!string.IsNullOrEmpty(strkyhieu))
            {
                // neu la so thi tim =
                // neu la chu thi tim like
                vanban = vanban.Where(p => p.strkyhieu.Contains(strkyhieu));
                isSearch = true;
                strSearchValues += "strkyhieu=" + strkyhieu + ";";
            }

            if (!string.IsNullOrEmpty(strtrichyeu))
            {
                vanban = vanban.Where(p => p.strtrichyeu.Contains(strtrichyeu));
                isSearch = true;
                strSearchValues += "strtrichyeu=" + strtrichyeu + ";";
            }

            if (!string.IsNullOrEmpty(strnoigui))
            {
                vanban = vanban.Where(p => p.strnoiguivb.Contains(strnoigui));
                isSearch = true;
                strSearchValues += "strnoigui=" + strnoigui + ";";
            }

            if (!string.IsNullOrEmpty(truclienthong))
            {
                string maEdxml = AppSettings.MaEdxmlDiaphuong;
                switch (truclienthong)
                {
                    case "Tructinh":
                        vanban = vanban.Where(p => p.intnhanvanbantu == enumVanbandenmail.intnhanvanbantu.TrucLienThongTinh);
                        break;
                    case "TrucChinhphu":
                        vanban = vanban.Where(p => p.intnhanvanbantu == enumVanbandenmail.intnhanvanbantu.TrucLienThongChinhPhu);
                        break;
                    case "Email":
                        vanban = vanban.Where(p => p.intnhanvanbantu == enumVanbandenmail.intnhanvanbantu.Email);
                        break;
                    //case "other":
                    //    vanban = vanban.Where(p => !p.strmadinhdanh.Contains(maEdxml));
                    //    break;
                    //case "all":
                    //    vanban = vanban.Where(p => !string.IsNullOrEmpty(p.strmadinhdanh));
                    //    break;
                    default: // ma dia phuong 
                        vanban = vanban.Where(p => p.strmadinhdanh.Contains(maEdxml));
                        break;
                }
                isSearch = true;
                strSearchValues += "truclienthong=" + truclienthong + ";";
            }

            if (!string.IsNullOrEmpty(strmadinhdanh))
            {
                vanban = vanban.Where(p => p.strmadinhdanh.Contains(strmadinhdanh));
                isSearch = true;
                strSearchValues += "strmadinhdanh=" + strmadinhdanh + ";";
            }
            //========================================================
            // end search
            //========================================================
            if (!isSearch)
            {   // khong phai la search thi gioi han ngay hien thi                
                int intngay = _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);

                DateTime? dtengaybd = DateTime.Now.AddDays(-intngay);
                vanban = vanban.Where(p => p.strngayky >= dtengaybd);

                // reset session
                _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.NoSearch);
            }
            else
            {   // luu cac gia tri search vao session
                _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.SearchVBDenDientu);
                _session.InsertObject(AppConts.SessionSearchListValues, strSearchValues);

                // tim kiem thi hien thi tat ca

                // Category thi gioi han ngay hien thi
                if (isCategory)
                {
                    int intngay = _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);
                    DateTime? dtengaybd = DateTime.Now.AddDays(-intngay);
                    vanban = vanban.Where(p => p.strngayky >= dtengaybd);
                }
            }

            return vanban;
        }

        public DetailVBDenViewModel GetViewDetail(int idvanban)
        {
            var vanban = _vbdenMailRepo.Vanbandenmails
                .FirstOrDefault(p => p.intid == idvanban);
            DetailVBDenViewModel model = new DetailVBDenViewModel();
            //model.FileAttachs = new FileAttachVBDTModel();
            if (vanban != null)
            {
                model.intid = vanban.intid;
                model.intso = vanban.intso;
                model.isattach = vanban.intattach == (int)enumVanbandenmail.intattach.Co ? true : false;
                model.stremail = vanban.strAddressSend;
                model.strkyhieu = vanban.strkyhieu;
                model.strngaygui = DateServices.FormatDateTimeVN(vanban.strngayguivb);
                model.strngayky = DateServices.FormatDateVN(vanban.strngayky);
                model.strngaynhan = DateServices.FormatDateTimeVN(vanban.strngaynhanvb);
                model.strnguoiky = vanban.strnguoiky;
                model.strnoiguivb = vanban.strnoiguivb;
                model.strnoinhan = vanban.strnoigui;
                model.strtrichyeu = vanban.strtrichyeu;
                //model.strvbkhan = vanban.intmat;

                var files = _filemailRepo.AttachMails
                    .Where(p => p.inttrangthai == (int)enumAttachMail.inttrangthai.IsActive)
                    .Where(p => p.intloai == (int)enumAttachMail.intloai.Vanbandendientu)
                    .Where(p => p.intidmail == idvanban)
                    .Select(p => new QLVB.DTO.File.DownloadFileViewModel
                    {
                        intid = p.intid,
                        strtenfile = p.strmota
                        //intloai = (int)p.intloai
                    }).ToList();
                foreach (var f in files)
                {
                    f.fileExt = _fileManager.GetFileExtention(f.strtenfile);
                    f.strfiletypeimages = _fileManager.GetFileTypeImages(f.strtenfile);
                    f.intloai = (int)QLVB.DTO.File.enumDownloadFileViewModel.intloai.VBDT;
                }
                model.DownloadFiles = files;

                //================================
                model.strmadinhdanh = vanban.strmadinhdanh;
            }
            return model;
        }

        public int GetSongayhienthi()
        {
            return _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);
        }

        public ToolbarVBDenViewModel GetToolbarVBDen()
        {
            string strDisplay = "normal";
            string strNone = "none";
            ToolbarVBDenViewModel tbar = new ToolbarVBDenViewModel();
            tbar.Capnhatvb = _role.IsRole(RoleVanbandendientu.Capnhat) == true ? strDisplay : strNone;
            //tbar.Email = 
            tbar.Xoavb = _role.IsRole(RoleVanbandendientu.Capnhat) == true ? strDisplay : strNone;

            return tbar;
        }

        public ResultFunction NhanEmail()
        {
            return _mail.NhanVBDT(20, 0);
        }

        public DeleteVBViewModel GetDeleteVanban(int id)
        {
            var vb = _vbdenMailRepo.Vanbandenmails
                .FirstOrDefault(p => p.intid == id);
            if (vb != null)
            {
                DeleteVBViewModel model = new DeleteVBViewModel
                {
                    intid = id,
                    strtenvanban = vb.intso.ToString() + ", ký hiệu " + vb.strkyhieu
                    + ", ngày ký " + DateServices.FormatDateVN(vb.strngayky)
                    + " của đơn vị " + vb.strnoiguivb
                };
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// xoa van ban
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultFunction DeleteVanban(int id)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                var vb = _vbdenMailRepo.Vanbandenmails.FirstOrDefault(p => p.intid == id);
                string strtenvanban = "id " + id.ToString() + ", số " + vb.intso.ToString() + ", ký hiệu " + vb.strkyhieu
                    + ", ngày ký " + DateServices.FormatDateVN(vb.strngayky)
                    + " của đơn vị " + vb.strnoiguivb;
                _vbdenMailRepo.Xoa(id);

                // xoa file
                var mail = _filemailRepo.AttachMails
                     .Where(p => p.inttrangthai == (int)enumAttachMail.inttrangthai.IsActive)
                     .Where(p => p.intloai == (int)enumAttachMail.intloai.Vanbandendientu)
                     .Where(p => p.intidmail == id)
                     .ToList();

                int idcanbo = _session.GetUserId();
                foreach (var file in mail)
                {
                    string strLoaiFile = AppConts.FileEmail;
                    string folderPath = _fileManager.GetFolderDownload(strLoaiFile, (DateTime)file.strngaycapnhat);
                    string filepath = folderPath + "/" + file.strtenfile;
                    string physicalFilePath = System.Web.HttpContext.Current.Server.MapPath(filepath);
                    try
                    {
                        System.IO.File.Delete(physicalFilePath);
                        _filemailRepo.Xoa(file.intid, idcanbo);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message);
                    }
                }

                _logger.Info("Xóa văn bản điện tử: " + strtenvanban);
                kq.id = (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
                kq.message = ex.Message;
            }
            return kq;
        }

        #endregion vanbanden

        #region vanbandi

        public IEnumerable<ListVanbandidientuViewModel> GetListVanbandidientu(
            string strngaykycat, string strngaynhancat, string strDonviguicat,
            int? intsodibd, int? intsodikt, string strngaynhanbd, string strngaynhankt,
            string strngaykybd, string strngaykykt, string strngayguibd, string strngayguikt,
            string strkyhieu, string strnoigui, string strtrichyeu
            )
        {
            var vanban = _GetVanbandiFromRequest(
                    strngaykycat, strngaynhancat, strDonviguicat,
                    intsodibd, intsodikt, strngaynhanbd, strngaynhankt,
                    strngaykybd, strngaykykt, strngayguibd, strngayguikt,
                    strkyhieu, strnoigui, strtrichyeu);

            var listvb = vanban //_vbdiRepo.Vanbandis
                //.Where(p => p.intguivbdt == (int)enumVanbandi.intguivbdt.Dagui)
                .GroupJoin(
                    _fileRepo.AttachVanbans
                        .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                        .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbandi),
                    v => 1,
                    f => 1,
                    (v, f) => new { v, f }
                )
                .Select(p => new ListVanbandidientuViewModel
                {
                    intid = p.v.intid,
                    intso = p.v.intso,
                    strsophu = !string.IsNullOrEmpty(p.v.strmorong) ? p.v.strmorong : "",
                    dtengayky = p.v.strngayky,
                    strkyhieu = p.v.strkyhieu,
                    strtrichyeu = p.v.strtrichyeu,

                    IsAttach = p.f.Any(a => a.intidvanban == p.v.intid)

                });

            return listvb;
        }

        private IQueryable<Vanbandi> _GetVanbandiFromRequest(
           string strngaykycat, string strngaynhancat, string strDonviguicat,
           int? intsodibd, int? intsodikt, string strngaynhanbd, string strngaynhankt,
           string strngaykybd, string strngaykykt, string strngayguibd, string strngayguikt,
           string strkyhieu, string strnoigui, string strtrichyeu
           )
        {
            bool isSearch = false;
            bool isCategory = false;
            string strSearchValues = string.Empty;
            // strSearchValues = "intsodenbd=1;intsodenkt=10;idloaivb=2;"
            //===========================================================
            var vanban = _vbdiRepo.Vanbandis
                        .Where(p => p.intguivbdt == (int)enumVanbandi.intguivbdt.Dagui);
            //====================================================
            // tuy chon category 
            //====================================================
            if (!string.IsNullOrEmpty(strngaykycat))
            {
                DateTime? dtengayky = DateServices.FormatDateEn(strngaykycat);
                vanban = vanban.Where(p => p.strngayky == dtengayky);
                isSearch = true;
                isCategory = true;
                strSearchValues += "strngaykycat=" + strngaykycat + ";";
            }
            if (!string.IsNullOrEmpty(strngaynhancat))
            {
                DateTime? dtengaynhan = DateServices.FormatDateEn(strngaynhancat);
                DateTime? dtengaynhankt = dtengaynhan.Value.AddDays(1);
                vanban = vanban.Where(p =>
                        _guivbRepo.GuiVanbans
                            .Where(g => g.strngaynhan >= dtengaynhan)
                            .Where(g => g.strngaynhan < dtengaynhankt)
                            .Any(g => g.intidvanban == p.intid)
                    );

                isSearch = true;
                isCategory = true;
                strSearchValues += "strngaynhancat=" + strngaynhancat + ";";
            }
            if (!string.IsNullOrEmpty(strDonviguicat))
            {
                vanban = vanban.Where(p =>
                        _guivbRepo.GuiVanbans
                        .Join(
                            _tochucRepo.GetAllTochucdoitacs
                                .Where(t => t.strtentochucdoitac.Contains(strDonviguicat)),
                            g => g.intiddonvi,
                            t => t.intid,
                            (g, t) => g
                        )
                        .Any(g => g.intidvanban == p.intid)
                    );
                isSearch = true;
                isCategory = true;
                strSearchValues += "strDonviguicat=" + strDonviguicat + ";";
            }
            //====================================================
            // Search van ban
            //====================================================
            if ((intsodikt != null) && (intsodikt != 0))
            {
                if ((intsodibd != null) && (intsodibd != 0))
                {
                    vanban = vanban.Where(p => p.intso >= intsodibd)
                            .Where(p => p.intso <= intsodikt);
                    isSearch = true;
                    strSearchValues += "intsodibd=" + intsodibd.ToString() + ";intsodikt=" + intsodikt.ToString() + ";";
                }
            }
            else
            {
                if ((intsodibd != null) && (intsodibd != 0))
                {
                    vanban = vanban.Where(p => p.intso == intsodibd);
                    isSearch = true;
                    strSearchValues += "intsodibd=" + intsodibd.ToString() + ";";
                }
            }

            if (!string.IsNullOrEmpty(strngaynhankt))
            {
                if (!string.IsNullOrEmpty(strngaynhanbd))
                {
                    DateTime? dtngaynhanbd = DateServices.FormatDateEn(strngaynhanbd);
                    DateTime? dtngaynhankt = DateServices.FormatDateEn(strngaynhankt);
                    DateTime? dtngaynhankt_1 = dtngaynhankt.Value.AddDays(1);
                    //vanban = vanban.Where(p => p.strngaynhanvb >= dtngaynhanbd)
                    //        .Where(p => p.strngaynhanvb <= dtngaynhankt);
                    vanban = vanban.Where(p =>
                        _guivbRepo.GuiVanbans
                            .Where(g => g.strngaynhan >= dtngaynhanbd)
                            .Where(g => g.strngaynhan < dtngaynhankt_1)
                            .Any(g => g.intidvanban == p.intid)
                    );

                    isSearch = true;
                    strSearchValues += "strngaynhanbd=" + strngaynhanbd + ";strngaynhankt=" + strngaynhankt + ";";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(strngaynhanbd))
                {
                    DateTime? dtngaynhanbd = DateServices.FormatDateEn(strngaynhanbd);
                    DateTime? dtngaynhankt_1 = dtngaynhanbd.Value.AddDays(1);
                    vanban = vanban.Where(p =>
                        _guivbRepo.GuiVanbans
                            .Where(g => g.strngaynhan >= dtngaynhanbd)
                            .Where(g => g.strngaynhan < dtngaynhankt_1)
                            .Any(g => g.intidvanban == p.intid)
                    );

                    isSearch = true;
                    strSearchValues += "strngaynhanbd=" + strngaynhanbd + ";";
                }
            }

            if (!string.IsNullOrEmpty(strngaykykt))
            {
                if (!string.IsNullOrEmpty(strngaykybd))
                {
                    DateTime? dtngaykybd = DateServices.FormatDateEn(strngaykybd);
                    DateTime? dtngaykykt = DateServices.FormatDateEn(strngaykykt);
                    vanban = vanban.Where(p => p.strngayky >= dtngaykybd)
                            .Where(p => p.strngayky <= dtngaykykt);
                    isSearch = true;
                    strSearchValues += "strngaykybd=" + strngaykybd + ";strngaykykt=" + strngaykykt + ";";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(strngaykybd))
                {
                    DateTime? dtngaykybd = DateServices.FormatDateEn(strngaykybd);
                    vanban = vanban.Where(p => p.strngayky == dtngaykybd);
                    isSearch = true;
                    strSearchValues += "strngaykybd=" + strngaykybd + ";";
                }
            }

            if (!string.IsNullOrEmpty(strngayguikt))
            {
                if (!string.IsNullOrEmpty(strngayguibd))
                {
                    DateTime? dtngayguibd = DateServices.FormatDateEn(strngayguibd);
                    DateTime? dtngayguikt = DateServices.FormatDateEn(strngayguikt);
                    DateTime? dtngayguikt_1 = dtngayguikt.Value.AddDays(1);
                    vanban = vanban.Where(p =>
                        _guivbRepo.GuiVanbans
                            .Where(g => g.strngaygui >= dtngayguibd)
                            .Where(g => g.strngaygui < dtngayguikt_1)
                            .Any(g => g.intidvanban == p.intid)
                        );

                    isSearch = true;
                    strSearchValues += "strngayguibd=" + strngayguibd + ";strngayguikt=" + strngayguikt + ";";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(strngayguibd))
                {
                    DateTime? dtngayguibd = DateServices.FormatDateEn(strngayguibd);
                    DateTime? dtngayguikt_1 = dtngayguibd.Value.AddDays(1);
                    vanban = vanban.Where(p =>
                        _guivbRepo.GuiVanbans
                            .Where(g => g.strngaygui >= dtngayguibd)
                            .Where(g => g.strngaygui < dtngayguikt_1)
                            .Any(g => g.intidvanban == p.intid)
                        );
                    isSearch = true;
                    strSearchValues += "strngayguibd=" + strngayguibd + ";";
                }
            }

            if (!string.IsNullOrEmpty(strkyhieu))
            {
                // neu la so thi tim =
                // neu la chu thi tim like
                vanban = vanban.Where(p => p.strkyhieu.Contains(strkyhieu));
                isSearch = true;
                strSearchValues += "strkyhieu=" + strkyhieu + ";";
            }

            if (!string.IsNullOrEmpty(strtrichyeu))
            {
                vanban = vanban.Where(p => p.strtrichyeu.Contains(strtrichyeu));
                isSearch = true;
                strSearchValues += "strtrichyeu=" + strtrichyeu + ";";
            }

            if (!string.IsNullOrEmpty(strnoigui))
            {
                vanban = vanban.Where(p =>
                        _guivbRepo.GuiVanbans
                        .Join(
                            _tochucRepo.GetAllTochucdoitacs
                                .Where(t => t.strtentochucdoitac.Contains(strnoigui)),
                            g => g.intiddonvi,
                            t => t.intid,
                            (g, t) => g
                        )
                        .Any(g => g.intidvanban == p.intid)
                    );
                isSearch = true;
                strSearchValues += "strnoigui=" + strnoigui + ";";
            }
            //========================================================
            // end search
            //========================================================
            if (!isSearch)
            {   // khong phai la search thi gioi han ngay hien thi                
                int intngay = _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);

                DateTime? dtengaybd = DateTime.Now.AddDays(-intngay);
                vanban = vanban.Where(p => p.strngayky >= dtengaybd);

                // reset session
                _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.NoSearch);
            }
            else
            {   // luu cac gia tri search vao session
                _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.SearchVBDiDientu);
                _session.InsertObject(AppConts.SessionSearchListValues, strSearchValues);

                // tim kiem thi hien thi tat ca

                // Category thi gioi han ngay hien thi
                if (isCategory)
                {
                    int intngay = _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);
                    DateTime? dtengaybd = DateTime.Now.AddDays(-intngay);
                    vanban = vanban.Where(p => p.strngayky >= dtengaybd);
                }
            }

            return vanban;
        }

        /// <summary>
        /// danh sach cac don vi da gui idvanban
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        public IEnumerable<ListDonviguiViewModels> GetListDonvigui(int idvanban)
        {
            var donviMail = _GetListDonviguiEmail(idvanban);
            var donviTructinh = _GetListDonviguiTructinh(idvanban);

            var donvi = donviMail.Union(donviTructinh);
            return donvi;
        }
        /// <summary>
        /// ds gui vbdt cho cac don vi tren email
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        private IEnumerable<ListDonviguiViewModels> _GetListDonviguiEmail(int idvanban)
        {
            var donvi = _guivbRepo.GuiVanbans
                .Where(p => p.intidvanban == idvanban)
                .Where(p => p.intloaivanban == (int)enumGuiVanban.intloaivanban.Vanbandi)
                .Where(p => p.intloaigui != (int)enumGuiVanban.intloaigui.Tructinh)
                .Join(
                    _tochucRepo.GetAllTochucdoitacs,
                    v => v.intiddonvi,
                    t => t.intid,
                    (v, t) => new { v, t }
                )
                .Select(p => new ListDonviguiViewModels
                {
                    intid = p.t.intid,
                    strtendonvi = p.t.strtentochucdoitac,
                    dtengaygui = p.v.strngaygui,
                    dtengaynhan = p.v.strngaynhan,
                    dtengayxuly = p.v.strngaydangxuly,
                    dtengayhoanthanh = p.v.strngayhoanthanh,
                    dtengayphancong = p.v.strngayphancong,
                    intloaigui = p.v.intloaigui,
                    strloaigui = "Email" 

                });
            
            return donvi;
        }
        /// <summary>
        /// ds gui vbdt cho cac don vi tren truc tinh
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        private IEnumerable<ListDonviguiViewModels> _GetListDonviguiTructinh(int idvanban)
        {
            var donvi = _guivbRepo.GuiVanbans
                .Where(p => p.intidvanban == idvanban)
                .Where(p => p.intloaivanban == (int)enumGuiVanban.intloaivanban.Vanbandi)
                .Where(p=>p.intloaigui == (int)enumGuiVanban.intloaigui.Tructinh)
                .Select(p => new ListDonviguiViewModels
                {
                    intid = p.intid,
                    strtendonvi = p.strtendonvi,
                    dtengaygui = p.strngaygui,
                    dtengaynhan = p.strngaynhan,
                    dtengayxuly = p.strngaydangxuly,
                    dtengayhoanthanh = p.strngayhoanthanh,
                    dtengayphancong=p.strngayphancong,
                    intloaigui = p.intloaigui,
                    strloaigui = "Trục tỉnh"
                });
          
            return donvi;
        }

        #endregion vanbandi

        #region AutoMail

        public ResultFunction AutoReceiveMail()
        {
            int EmailPerRequest = AppSettings.AutoEmailPerRequest;

            ResultFunction kq = _mail.NhanVBDT(EmailPerRequest, 1);

            return kq;
        }

        public ResultFunction AutoSendMail()
        {
            ResultFunction kq = _mail.AutoSendVBDT();
            return kq;
        }

        #endregion AutoMail

        #region TonghopVB

        public IEnumerable<TonghopVBDenViewModel> TonghopVBDen(
            string strngaykybd, string strngaykykt,
            string danhmuc, int? intSongaygui, int LoaiTonghop)
        {
            DateTime? dtengaykybd = DateServices.FormatDateEn(strngaykybd);
            DateTime? dtengaykykt = DateServices.FormatDateEn(strngaykykt);

            var tonghop = _vbdenMailRepo.Vanbandenmails
               .Where(p => p.strngayky >= dtengaykybd)
               .Where(p => p.strngayky <= dtengaykykt);

            if (intSongaygui >= 0)
            {
                tonghop = tonghop
                    .Where(p => System.Data.Entity.DbFunctions.DiffDays(p.strngayky, p.strngayguivb) <= intSongaygui);
            }


            bool isDanhmuc = (danhmuc == "true") ? true : false;
            try
            {
                switch (LoaiTonghop)
                {
                    case 0: //  email   
                        if (isDanhmuc)
                        {
                            var vb = tonghop
                                .GroupBy(p => p.strAddressSend)
                                .GroupJoin(
                                    _tochucRepo.GetAllTochucdoitacs,
                                    v => 1,
                                    t => 1,
                                    (v, t) => new { v, t }
                                )
                                .Select(p => new TonghopVBDenViewModel
                                {
                                    stremail = p.v.FirstOrDefault().strAddressSend,
                                    strten = p.t.FirstOrDefault(x => p.v.FirstOrDefault().strAddressSend.Contains(x.stremailvbdt)).strtentochucdoitac,
                                    intTongVB = p.v.Count(),
                                    intVBCapnhat = p.v.Where(x => x.inttrangthai == (int)enumVanbandenmail.inttrangthai.Dacapnhat).Count()
                                })
                                .OrderBy(p => p.stremail);

                            return vb;
                        }
                        else
                        {
                            var vb = tonghop
                                .GroupBy(p => p.strAddressSend)
                                .Select(p => new TonghopVBDenViewModel
                                {
                                    stremail = p.FirstOrDefault().strAddressSend,
                                    strten = p.FirstOrDefault().strnoiguivb,
                                    intTongVB = p.Count(),
                                    intVBCapnhat = p.Where(x => x.inttrangthai == (int)enumVanbandenmail.inttrangthai.Dacapnhat).Count()
                                })
                                .OrderBy(p => p.stremail);

                            return vb;
                        }


                    case 1: // ten donvi
                        var vb1 = tonghop
                           .GroupBy(p => p.strnoiguivb)
                           .Select(p => new TonghopVBDenViewModel
                           {
                               stremail = p.FirstOrDefault().strAddressSend,
                               strten = p.FirstOrDefault().strnoiguivb,
                               intTongVB = p.Count(),
                               intVBCapnhat = p.Where(x => x.inttrangthai == (int)enumVanbandenmail.inttrangthai.Dacapnhat).Count()
                           })
                           .OrderBy(p => p.strten);

                        return vb1;

                    default:
                        return null;
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }



        public IEnumerable<ListVanbandendientuViewModel> GetListVBDenDientuDonvi(
            string stremail, string strngaykybd, string strngaykykt, int? intSoNgaygui, int LoaiTonghop)
        {
            DateTime? dtengaykybd = DateServices.FormatDateEn(strngaykybd);
            DateTime? dtengaykykt = DateServices.FormatDateEn(strngaykykt);

            var tonghop = _vbdenMailRepo.Vanbandenmails
               .Where(p => p.strngayky >= dtengaykybd)
               .Where(p => p.strngayky <= dtengaykykt)
                //.Where(p => stremail.Contains(p.strAddressSend))
               ;

            switch (LoaiTonghop)
            {
                case 0: // email
                    tonghop = tonghop.Where(p => stremail.Contains(p.strAddressSend));
                    break;
                case 1: // ten don vi
                    tonghop = tonghop.Where(p => stremail.Contains(p.strnoiguivb));
                    break;
                default:
                    break;
            }

            if (intSoNgaygui >= 0)
            {
                tonghop = tonghop
                    .Where(p => System.Data.Entity.DbFunctions.DiffDays(p.strngayky, p.strngayguivb) <= intSoNgaygui);
            }

            var listvb = tonghop
                .Select(p => new ListVanbandendientuViewModel
                {
                    intid = p.intid,
                    dtengayky = p.strngayky,
                    strkyhieu = p.strkyhieu,
                    intso = p.intso,
                    strtrichyeu = p.strtrichyeu,
                    strnoiguivb = p.strnoiguivb,
                    dtengayguivb = p.strngayguivb,
                    dtengaynhanvb = p.strngaynhanvb,
                    inttrangthai = p.inttrangthai,
                    IsAttach = p.intattach == (int)enumVanbandenmail.intattach.Co ? true : false
                });

            return listvb;

        }



        public IEnumerable<TonghopVBDiViewModel> TonghopVBDi(string strngaykybd, string strngaykykt,
                int? intSongaygui, string LoaiTonghop)
        {
            switch (LoaiTonghop)
            {
                case "donvi":
                    return _TonghopVBDI(strngaykybd, strngaykykt, intSongaygui);
                case "chuyenvien":
                    return _TonghopVBDi_chuyenvien(strngaykybd, strngaykykt, intSongaygui);
                default:
                    return _TonghopVBDI(strngaykybd, strngaykykt, intSongaygui);
            }
           

        }

        /// <summary>
        /// tong hop van ban di theo don vi
        /// </summary>
        /// <param name="strngaykybd"></param>
        /// <param name="strngaykykt"></param>
        /// <param name="intSongaygui"></param>
        /// <param name="LoaiTonghop"></param>
        /// <returns></returns>
        private IEnumerable<TonghopVBDiViewModel> _TonghopVBDI(string strngaykybd, string strngaykykt,
                int? intSongaygui)
        {
            try
            {
                DateTime? dtengaykybd = DateServices.FormatDateEn(strngaykybd);
                DateTime? dtengaykykt = DateServices.FormatDateEn(strngaykykt);
                strngaykybd = DateServices.FormatDateEn(dtengaykybd);
                strngaykykt = DateServices.FormatDateEn(dtengaykykt);

                string sql = string.Empty;
                if (intSongaygui >= 0)
                {
                    sql = "select  g.intiddonvi as intid ,t.strtentochucdoitac as strten ,t.stremailvbdt as stremail,"
                    + " g.inttrangthainhan as inttrangthainhan , count( g.intidvanban) as intTongVB "
                    + " from guivanban g "

                    + " inner join vanbandi d on d.strngayky>='" + strngaykybd + "' and d.strngayky<='" + strngaykykt + "' "
                    + " and d.intid=g.intidvanban and g.intloaivanban=2"
                    + " and (datediff(day,d.strngayky,g.strngaygui)<=" + intSongaygui.ToString() + ")"

                    + " inner join tochucdoitac t on t.intid=g.intiddonvi"

                    + " group by t.strtentochucdoitac,t.stremailvbdt, g.intiddonvi,g.inttrangthainhan"
                    + " order by intiddonvi";
                }
                else
                {
                    sql = "select  g.intiddonvi as intid ,t.strtentochucdoitac as strten ,t.stremailvbdt as stremail,"
                    + " g.inttrangthainhan as inttrangthainhan , count( g.intidvanban) as intTongVB "
                    + " from guivanban g "

                    + " inner join vanbandi d on d.strngayky>='" + strngaykybd + "' and d.strngayky<='" + strngaykykt + "' "
                    + " and d.intid=g.intidvanban and g.intloaivanban=2 "
                        //+ " and (datediff(day,d.strngayky,g.strngaygui)<=" + intSongaygui.ToString() + ")"

                    + " inner join tochucdoitac t on t.intid=g.intiddonvi"

                    + " group by t.strtentochucdoitac,t.stremailvbdt,g.intiddonvi,g.inttrangthainhan "
                    + " order by intiddonvi";
                }


                IEnumerable<TonghopVBDiViewModel> tongvb = (IEnumerable<TonghopVBDiViewModel>)_sqlRepo.ExecSql_TonghopVBDiDientu(sql);

                var listvb = _ChuanhoaTonghopVBDi(tongvb);

                return listvb;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);

            }
            return null;
        }

        private List<TonghopVBDiViewModel> _ChuanhoaTonghopVBDi(IEnumerable<TonghopVBDiViewModel> tongvb)
        {
            List<TonghopVBDiViewModel> vbdt = new List<TonghopVBDiViewModel>();

            var _tongvb = tongvb.ToList();

            int intcurrent = 0;
            foreach (var t in _tongvb)
            {
                if (t.intid != intcurrent)
                {
                    int tonggui = 0;
                    int tongnhan = 0;
                    var _donvi = _tongvb.Where(p => p.intid == t.intid);
                    foreach (var d in _donvi)
                    {
                        if (d.inttrangthainhan == (int)QLVB.Domain.Entities.enumGuiVanban.inttrangthainhan.Danhan)
                        {
                            tongnhan = d.intTongVB;
                        }
                        else
                        {
                            tonggui = d.intTongVB;
                        }
                    }
                    tonggui += tongnhan;
                    intcurrent = t.intid;

                    TonghopVBDiViewModel _vb = new TonghopVBDiViewModel
                    {
                        intid = t.intid,
                        stremail = t.stremail,
                        strten = t.strten,
                        intTongVB = tonggui,
                        intVBCapnhat = tongnhan
                    };
                    vbdt.Add(_vb);
                }
            }
            return vbdt;
        }


        private IEnumerable<TonghopVBDiViewModel> _TonghopVBDi_chuyenvien(string strngaykybd, string strngaykykt,
                int? intSongaygui)
        {
            try
            {
                DateTime? dtengaykybd = DateServices.FormatDateEn(strngaykybd);
                DateTime? dtengaykykt = DateServices.FormatDateEn(strngaykykt);
                strngaykybd = DateServices.FormatDateEn(dtengaykybd);
                strngaykykt = DateServices.FormatDateEn(dtengaykykt);

                string sql = string.Empty;
                if (intSongaygui >= 0)
                {
                    sql = "select d.strnguoisoan as strten,"
                    + " g.inttrangthainhan as inttrangthainhan , count( g.intidvanban) as intTongVB "
                    + " from guivanban g "

                    + " inner join vanbandi d on d.strngayky>='" + strngaykybd + "' and d.strngayky<='" + strngaykykt + "' "
                    + " and d.intid=g.intidvanban and g.intloaivanban=2 and d.strnguoisoan is not null "
                    + " and (datediff(day,d.strngayky,g.strngaygui)<=" + intSongaygui.ToString() + ")"

                    //+ " inner join tochucdoitac t on t.intid=g.intiddonvi"

                    + " group by d.strnguoisoan, g.inttrangthainhan"
                    + " order by d.strnguoisoan";
                }
                else
                {
                    sql = "select d.strnguoisoan as strten,"
                    + " g.inttrangthainhan as inttrangthainhan , count( g.intidvanban) as intTongVB "
                    + " from guivanban g "

                    + " inner join vanbandi d on d.strngayky>='" + strngaykybd + "' and d.strngayky<='" + strngaykykt + "' "
                    + " and d.intid=g.intidvanban and g.intloaivanban=2 and d.strnguoisoan is not null "
                    //+ " and (datediff(day,d.strngayky,g.strngaygui)<=" + intSongaygui.ToString() + ")"

                    //+ " inner join tochucdoitac t on t.intid=g.intiddonvi"

                    + " group by d.strnguoisoan,g.inttrangthainhan "
                    + " order by d.strnguoisoan";
                }


                IEnumerable<TonghopVBDiViewModel> tongvb = (IEnumerable<TonghopVBDiViewModel>)_sqlRepo.ExecSql_TonghopVBDiDientu(sql);
                var listvb = _ChuanhoaTonghopVBDi_chuyenvien(tongvb);
                return listvb;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);

            }
            return null;
        }

        private List<TonghopVBDiViewModel> _ChuanhoaTonghopVBDi_chuyenvien(IEnumerable<TonghopVBDiViewModel> tongvb)
        {
            List<TonghopVBDiViewModel> vbdt = new List<TonghopVBDiViewModel>();

            var _tongvb = tongvb.ToList();

            string strcurrent = string.Empty;
            int count = 0;
            foreach (var t in _tongvb)
            {
                if (t.strten != strcurrent)
                {
                    int tonggui = 0;
                    int tongnhan = 0;
                    var _chuyenvien = _tongvb.Where(p => p.strten == t.strten);
                    foreach (var d in _chuyenvien)
                    {
                        if (d.inttrangthainhan == (int)QLVB.Domain.Entities.enumGuiVanban.inttrangthainhan.Danhan)
                        {
                            tongnhan = d.intTongVB;
                        }
                        else
                        {
                            tonggui = d.intTongVB;
                        }
                    }
                    tonggui += tongnhan;
                    strcurrent = t.strten;
                    count++;

                    TonghopVBDiViewModel _vb = new TonghopVBDiViewModel
                    {
                        intid = count,                        
                        strten = t.strten,
                        intTongVB = tonggui,
                        intVBCapnhat = tongnhan
                    };
                    vbdt.Add(_vb);
                }
            }
            return vbdt;
        }



        public string GetTenDonviDi(int iddonvi)
        {
            try
            {
                var donvi = _tochucRepo.GetAllTochucdoitacs
                    .FirstOrDefault(p => p.intid == iddonvi);
                return donvi.strtentochucdoitac;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return "";
            }
        }

        public string GetTenDonviDi(string stremail, string danhmuc)
        {
            bool isDanhmuc = (danhmuc == "true") ? true : false;
            try
            {
                string strten = string.Empty;
                if (isDanhmuc)
                {
                    strten = _tochucRepo.GetAllTochucdoitacs.
                        FirstOrDefault(p => stremail.Contains(p.stremailvbdt)).strtentochucdoitac;
                }
                else
                {
                    strten = _vbdenMailRepo.Vanbandenmails.Where(p => p.strAddressSend.Contains(stremail))
                        .FirstOrDefault().strnoiguivb;
                }
                return strten;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return "";
            }
        }


        /// <summary>
        /// tong hop van ban di da gui/chua gui dien tu
        /// </summary>
        /// <param name="strngaykybd"></param>
        /// <param name="strngaykykt"></param>
        /// <returns></returns>
        public IEnumerable<TonghopVBDiViewModel> TonghopVBDi_Dientu(string strngaykybd, string strngaykykt)
        {
            try
            {
                DateTime? dtengaykybd = DateServices.FormatDateEn(strngaykybd);
                DateTime? dtengaykykt = DateServices.FormatDateEn(strngaykykt);

                var vanbandagui = _vbdiRepo.Vanbandis
                        .Where(p => p.strngayky >= dtengaykybd)
                        .Where(p => p.strngayky <= dtengaykykt)
                        .Where(p => p.intguivbdt == (int)enumVanbandi.intguivbdt.Dagui)
                        .Count();
                var vanbanchuagui = _vbdiRepo.Vanbandis
                        .Where(p => p.strngayky >= dtengaykybd)
                        .Where(p => p.strngayky <= dtengaykykt)
                        .Where(p => p.intguivbdt != (int)enumVanbandi.intguivbdt.Dagui)
                        .Count();

                List<TonghopVBDiViewModel> listvb = new List<TonghopVBDiViewModel>();

                TonghopVBDiViewModel vbdagui = new TonghopVBDiViewModel();
                vbdagui.intid = (int)enumVanbandi.intguivbdt.Dagui;
                vbdagui.intTongVB = vanbandagui;
                vbdagui.strten = "Văn bản phát hành đã gửi điện tử";
                listvb.Add(vbdagui);

                TonghopVBDiViewModel vbchuagui = new TonghopVBDiViewModel();
                vbchuagui.intid = (int)enumVanbandi.intguivbdt.Chuagui;
                vbchuagui.intTongVB = vanbanchuagui;
                vbchuagui.strten = "Văn bản phát hành chưa gửi điện tử";
                listvb.Add(vbchuagui);

                TonghopVBDiViewModel TongVB = new TonghopVBDiViewModel();
                TongVB.intid = 3;
                TongVB.intTongVB = vanbanchuagui + vanbandagui;
                TongVB.strten = "Tổng số Văn bản phát hành";
                listvb.Add(TongVB);

                return listvb;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);

            }
            return null;
        }

        /// <summary>
        /// tong hop so lieu van ban giay/dien tu gui va nhan
        /// </summary>
        /// <param name="strngaybd"></param>
        /// <param name="strngaykt"></param>
        /// <returns></returns>
        public IEnumerable<TonghopVanbanViewModel> TonghopSolieuVB(string strngaybd, string strngaykt)
        {
            DateTime? dtengaybd = DateServices.FormatDateEn(strngaybd);
            DateTime? dtengaykt = DateServices.FormatDateEn(strngaykt);

            try
            {
                var vb = _tonghopvbRepo.TonghopVanbans
                .Where(p => p.Ngaybatdau >= dtengaybd)
                .Where(p => p.Ngayketthuc <= dtengaykt)
                .OrderBy(p => p.Ngaygui)
                .Select(p => new TonghopVanbanViewModel
                {
                    Ngaygui = p.Ngaygui,
                    Ngaytonghopbd = p.Ngaybatdau,
                    Ngaytonghopkt = p.Ngayketthuc,
                    VBDientuDen = p.VBDientuDen,
                    VBDientuDi = p.VBDientuDi,
                    VBGiayDen = p.VBGiayDen,
                    VBGiayDi = p.VBGiayDi
                });
                return vb;
            }catch(Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }           
           
        }

        public bool CheckSendSolieuVB()
        {
            
            return _configRepo.GetConfigToBool(ThamsoHethong.IsSendTonghopVB);
            
        }


        #endregion TonghopVB

        #region GuiTonghopVBveUBT

        /// <summary>
        /// lay so lieu ve gui nhan van ban gui ve web services
        /// </summary>
        /// <returns></returns>
        public TonghopVanbanViewModel GetTonghopVanban(DateTime ngaybd, DateTime ngaykt)
        {
            DateTime ngaygui = DateTime.Now;
            ngaybd = ngaybd.Date;
            DateTime ngayend = ngaykt.AddDays(1).Date;


            TonghopVanbanViewModel tonghop = new TonghopVanbanViewModel();

            try
            {
                var vbgiayden = _vbdenRepo.Vanbandens
                                .Where(p => p.strngayden >= ngaybd)
                                .Where(p => p.strngayden < ngayend)
                                .Count();

                var vbgiaydi = _vbdiRepo.Vanbandis
                                    .Where(p => p.strngayky >= ngaybd)
                                    .Where(p => p.strngayky < ngayend)
                                    .Count();

                var vbdtden = _vbdenMailRepo.Vanbandenmails
                                    .Where(p => p.strngaynhanvb >= ngaybd)
                                    .Where(p => p.strngaynhanvb < ngayend)
                                    .Count();

                var vbdtdi = _guivbRepo.GuiVanbans
                                    .Where(p => p.strngaygui >= ngaybd)
                                    .Where(p => p.strngaygui < ngayend)
                                    .Where(p => p.intloaivanban == (int)enumGuiVanban.intloaivanban.Vanbandi)
                                    .Where(p => p.inttrangthaigui == (int)enumGuiVanban.inttrangthaigui.Dagui)
                                    .Select(p => p.intidvanban)
                                    .Distinct()
                                    .Count();

                tonghop.VBDientuDen = vbdtden;
                tonghop.VBDientuDi = vbdtdi;
                tonghop.VBGiayDen = vbgiayden;
                tonghop.VBGiayDi = vbgiaydi;

                tonghop.Donvi = QLVB.Donvi.Donvi.GetTenDonVi();
                tonghop.Ngaygui = ngaygui;
                tonghop.Ngaytonghopbd = ngaybd;
                tonghop.Ngaytonghopkt = ngaykt;

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return tonghop;
        }

        /// <summary>
        /// lay ngay bat dau, ngay ket thuc de gui
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, DateTime> GetNgaybatdau_ketthuc()
        {
            DateTime ngaybd = DateTime.Now;
            DateTime ngaykt = DateTime.Now;
            DateTime ngayhientai = DateTime.Now;

            var logtonghop = _guitonghopvbRepo.GetLogTonghopByID();

            if (logtonghop == null)
            {
                // lan dau tien gui, lay ngay hom qua
                ngaybd = DateTime.Now.AddDays(-1);
                ngaykt = ngaybd;
            }
            else
            {
                // lay tu ngay da gui den ngay hom qua
                DateTime ngaygui = logtonghop.Ngaygui;
                if (ngaygui.Date < ngayhientai.Date)
                {
                    ngaybd = ngaygui;
                    ngaykt = ngayhientai.AddDays(-1);
                }
                else
                {   // neu da gui trong ngay roi ngaygui = ngayhientai
                    ngaybd = DateTime.Now;
                    ngaykt = DateTime.Now;
                }                
            }
            Dictionary<string, DateTime> ngaytonghop = new Dictionary<string, DateTime>();
            ngaytonghop.Add("ngaybd", ngaybd);
            ngaytonghop.Add("ngaykt", ngaykt);

            return ngaytonghop;
        }


        public SettingSendTonghopVBViewModel GetSettingSendTonghopVB()
        {
            SettingSendTonghopVBViewModel model = new SettingSendTonghopVBViewModel();

            model.IsSendTonghopVb = _configRepo.GetConfigToBool(ThamsoHethong.IsSendTonghopVB);
            model.IPAddress = _configRepo.GetConfig(ThamsoHethong.IPAddressUBT);
            model.TimeAutoSend = _configRepo.GetConfigToInt(ThamsoHethong.TimeAutoSendVB);
            model.TimeSend = _configRepo.GetConfigToInt(ThamsoHethong.TimeSend);

            return model;

        }

        public void UpdateNgayguiTonghopVB(TonghopVanbanViewModel tonghopvb)
        {
            try
            {
                var tonghop = _guitonghopvbRepo.GetLogTonghopByID();
                DateTime ngaygui = (DateTime)tonghopvb.Ngaygui;
                if (tonghop == null)
                {
                    _guitonghopvbRepo.Themmoi(ngaygui, (int)enumLogGuiTonghopVB.intTrangthai.Dagui);
                }
                else
                {
                    _guitonghopvbRepo.Capnhat(ngaygui, (int)enumLogGuiTonghopVB.intTrangthai.Dagui);
                }
                TonghopVanban vb = new TonghopVanban();
                vb.Ngaygui = tonghopvb.Ngaygui;
                vb.Ngaybatdau = tonghopvb.Ngaytonghopbd;
                vb.Ngayketthuc = tonghopvb.Ngaytonghopkt;

                vb.VBDientuDen = tonghopvb.VBDientuDen;
                vb.VBDientuDi = tonghopvb.VBDientuDi;
                vb.VBGiayDen = tonghopvb.VBGiayDen;
                vb.VBGiayDi = tonghopvb.VBGiayDi;

                _tonghopvbRepo.Them(vb);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
                   
        }


        #endregion GuiTonghopVBveUBT

        #region LuunhatkyVanbanguidientu

        /// <summary>
        /// luu nhat ky gui van ban dien tu
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="intloaivanban"></param>
        /// <param name="iddonvi"></param>
        /// <param name="strtendonvi"></param>
        /// <returns></returns>
        public int _SaveGuiVanban(int idvanban, int intloaivanban, int? iddonvi, string strtendonvi, int intloaigui)
        {
            try
            {
                GuiVanban vb = new GuiVanban
                {
                    intidvanban = idvanban,
                    intloaivanban = intloaivanban,
                    inttrangthaigui = (int)enumGuiVanban.inttrangthaigui.Dagui,
                    strngaygui = DateTime.Now,
                    inttrangthainhan = (int)enumGuiVanban.inttrangthainhan.Chuanhan,

                    //intiddonvi = iddonvi,
                    //strtendonvi = strtendonvi,
                    intloaigui = intloaigui

                };
                if (iddonvi != null) { vb.intiddonvi = iddonvi; }
                if (!string.IsNullOrEmpty(strtendonvi)) { vb.strtendonvi = strtendonvi; }

                return _guivbRepo.Them(vb);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return 0;
            }
        }

        #endregion LuunhatkyVanbanguidientu



    }
}
