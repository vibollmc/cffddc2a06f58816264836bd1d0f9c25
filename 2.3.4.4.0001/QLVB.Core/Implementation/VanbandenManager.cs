using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Core.Contract;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Vanbanden;
using QLVB.DTO.File;
using QLVB.Common.Logging;
using QLVB.Common.Utilities;
using QLVB.Common.Sessions;
using QLVB.Common.Date;
//using System.Data.Objects;
using System.Data.Entity;
using System.Web;


namespace QLVB.Core.Implementation
{
    public class VanbandenManager : IVanbandenManager
    {
        #region Constructor

        private ILogger _logger;
        private ISessionServices _session;
        private IRoleManager _role;
        private IVanbandenRepository _vanbandenRepo;
        private IPhanloaiTruongRepository _pltruongRepo;
        private IPhanloaiVanbanRepository _plvanbanRepo;
        private ISoVanbanRepository _sovbRepo;
        private IKhoiphathanhRepository _khoiphRepo;
        private ILinhvucRepository _linhvucRepo;
        private ITinhchatvanbanRepository _tinhchatvbRepo;
        private ICanboRepository _canboRepo;
        private IDiachiluutruRepository _luutruRepo;
        private ITochucdoitacRepository _tochucRepo;
        private ICategoryRepository _categoryRepo;
        private IConfigRepository _configRepo;
        private IAttachVanbanRepository _fileRepo;
        private IHosocongviecRepository _hosocongviecRepo;
        private IHosovanbanRepository _hosovanbanRepo;
        private IDoituongxulyRepository _doituongRepo;
        //private IRelaHosovanban _relaHoso;
        private IHoibaovanbanRepository _hoibaovanbanRepo;
        private IVanbandiRepository _vanbandiRepo;
        private IChitietVanbandenRepository _chitietVBDenRepo;
        private IVanbandenCanboRepository _vanbandencanboRepo;
        private IDonvitructhuocRepository _donviRepo;
        private IFileManager _fileManager;
        private IHosoykienxulyRepository _hsykienRepo;
        private ITonghopCanboRepository _tonghopRepo;
        private IChucdanhRepository _chucdanhRepo;
        private IHosovanbanlienquanRepository _hsvblqRepo;
        private IVanbandenmailRepository _vbmailRepo;
        private IAttachMailRepository _fileMailRepo;
        private IRuleFileNameManager _fileNameManger;
        private IChitietHosoRepository _chitietHosoRepo;
        private IGuiVanbanRepository _guivbRepo;
        private IMailFormatManager _mailFormat;
        public VanbandenManager(ILogger logger, IRoleManager role, ISessionServices session,
            IVanbandenRepository vanbandenRepo, IPhanloaiTruongRepository pltruongRepo,
            IPhanloaiVanbanRepository plvanbanRepo, ISoVanbanRepository sovbRepo,
            IKhoiphathanhRepository khoiphRepo, ILinhvucRepository linhvucRepo,
            ITinhchatvanbanRepository tinhchatvbRepo, ICanboRepository canboRepo,
            IDiachiluutruRepository luutruRepo, ITochucdoitacRepository tochucRepo,
            ICategoryRepository categoryRepo, IConfigRepository configRepo,
            IAttachVanbanRepository fileRepo, IHosocongviecRepository hosocongviecRepo,
            IHosovanbanRepository hosovanbanRepo, IDoituongxulyRepository doituongRepo,
            IHoibaovanbanRepository hoibaovanbanRepo,
            IVanbandiRepository vanbandiRepo, IChitietVanbandenRepository chitietVBDenRepo,
            IVanbandenCanboRepository vanbandencanboRepo, IDonvitructhuocRepository donviRepo,
            IFileManager fileManager, IHosoykienxulyRepository hsykienRepo,
            ITonghopCanboRepository tonghopRepo, IChucdanhRepository chucdanhRepo,
            IHosovanbanlienquanRepository vblqRepo, IVanbandenmailRepository vbmailRepo,
            IAttachMailRepository fileMailRepo, IRuleFileNameManager fileNameManager,
            IChitietHosoRepository chitietHosoRepo, IGuiVanbanRepository guivbRepo,
            IMailFormatManager mailFormat)
        {
            _logger = logger;
            _role = role;
            _session = session;
            _vanbandenRepo = vanbandenRepo;
            _pltruongRepo = pltruongRepo;
            _plvanbanRepo = plvanbanRepo;
            _sovbRepo = sovbRepo;
            _khoiphRepo = khoiphRepo;
            _linhvucRepo = linhvucRepo;
            _tinhchatvbRepo = tinhchatvbRepo;
            _canboRepo = canboRepo;
            _luutruRepo = luutruRepo;
            _tochucRepo = tochucRepo;
            _categoryRepo = categoryRepo;
            _configRepo = configRepo;
            _fileRepo = fileRepo;
            _hosocongviecRepo = hosocongviecRepo;
            _hosovanbanRepo = hosovanbanRepo;
            _doituongRepo = doituongRepo;
            _hoibaovanbanRepo = hoibaovanbanRepo;
            _vanbandiRepo = vanbandiRepo;
            _chitietVBDenRepo = chitietVBDenRepo;
            _vanbandencanboRepo = vanbandencanboRepo;
            _donviRepo = donviRepo;
            _fileManager = fileManager;
            _hsykienRepo = hsykienRepo;
            _tonghopRepo = tonghopRepo;
            _chucdanhRepo = chucdanhRepo;
            _hsvblqRepo = vblqRepo;
            _vbmailRepo = vbmailRepo;
            _fileMailRepo = fileMailRepo;
            _fileNameManger = fileNameManager;
            _chitietHosoRepo = chitietHosoRepo;
            _guivbRepo = guivbRepo;
            _mailFormat = mailFormat;
        }

        #endregion Constructor

        #region Listvanban

        public ToolbarViewModel GetToolbar()
        {
            string strDisplay = "normal";
            string strNone = "none";
            ToolbarViewModel tbar = new ToolbarViewModel();
            tbar.Capnhatvb = _role.IsRole(RoleVanbanden.Capnhatvb) == true ? strDisplay : strNone;
            tbar.Capquyenxem = _role.IsRole(RoleVanbanden.Capquyenxem) == true ? strDisplay : strNone;
            tbar.Duyetvb = _role.IsRole(RoleVanbanden.Duyetvb) == true ? strDisplay : strNone;
            tbar.Email = _role.IsRole(RoleVanbanden.GuiEmail) == true ? strDisplay : strNone;
            tbar.PhanXLVB = _role.IsRole(RoleVanbanden.PhanXLvb) == true ? strDisplay : strNone;
            tbar.PhanQuytrinh = _role.IsRole(RoleVanbanden.PhanQuytrinh) == true ? strDisplay : strNone;
            tbar.Xoavb = _role.IsRole(RoleVanbanden.Xoavb) == true ? strDisplay : strNone;
            tbar.Xulyvb = _role.IsRole(RoleVanbanden.Xulyvb) == true ? strDisplay : strNone;

            int iduser = _session.GetUserId();

            int intOption = (int)enumcanbo.strRight.PhanXLNhieuVB;
            tbar.IsPhanXLNhieuVB = _canboRepo.IsOption(iduser, intOption);

            intOption = (int)enumcanbo.strRight.XulyNhanh;
            tbar.IsXulyNhanh = _canboRepo.IsOption(iduser, intOption);


            return tbar;
        }

        public CategoryViewModel GetCategory()
        {
            CategoryViewModel cat = new CategoryViewModel();
            cat.Category = _categoryRepo.CategoryVanbans
                .Where(p => p.inttrangthai == (int)enumCategoryVanban.inttrangthai.IsActive)
                .Where(p => p.intloai == (int)enumCategoryVanban.intloai.Vanbanden)
                .OrderBy(p => p.intorder);

            cat.Songayhienthi = _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);

            cat.Khoiphathanh = _khoiphRepo.GetActiveKhoiphathanhs.OrderBy(p => p.strtenkhoi);

            cat.Loaivanban = _plvanbanRepo.GetActivePhanloaiVanbans
                .Where(p => p.intloai == (int)enumPhanloaiVanban.intloai.vanbanden)
                .OrderBy(p => p.strtenvanban);

            cat.Sovanban = _sovbRepo.GetActiveSoVanbans
                .Where(p => p.intloai == (int)enumSovanban.intloai.Vanbanden)
                .OrderBy(p => p.strten);

            cat.Xulyvanban = new Xulyvanban();

            return cat;
        }

        #region EF
        public IEnumerable<ListVanbandenViewModel> GetListVanbanden__backup
            (string strngaydencat, int? idloaivb,
            int? idkhoiph, int? idsovb, string xuly,
            int? intsodenbd, int? intsodenkt, string strngaydenbd, string strngaydenkt,
            string strngaykybd, string strngaykykt, string strsokyhieu, string strnguoiky,
            string strnoigui, string strtrichyeu, string strnguoixuly
            )
        {
            #region temp
            //bool isSearch = false;

            //string strSearchValues = string.Empty;
            //// strSearchValues = "intsodenbd=1;intsodenkt=10;idloaivb=2;"
            ////===========================================================

            //var vanban = _vanbandenRepo.Vanbandens;

            ////====================================================
            //// kiem tra nhung van ban user duoc quyen xem/xuly
            ////====================================================
            //vanban = _GetQuyenXemVB(vanban);

            ////====================================================
            //// tuy chon category 
            ////====================================================
            //if (!string.IsNullOrEmpty(strngaydencat))
            //{
            //    DateTime? dtengayden = DateServices.FormatDateEn(strngaydencat);
            //    vanban = vanban.Where(p => p.strngayden == dtengayden);
            //    isSearch = true;
            //    strSearchValues += "strngaydencat=" + strngaydencat + ";";
            //}
            //if ((idloaivb != null) && (idloaivb != 0))
            //{
            //    vanban = vanban.Where(p => p.intidphanloaivanbanden == idloaivb);
            //    isSearch = true;
            //    strSearchValues += "idloaivb=" + idloaivb.ToString() + ";";
            //}
            //if ((idsovb != null) && (idsovb != 0))
            //{
            //    vanban = vanban.Where(p => p.intidsovanban == idsovb);
            //    isSearch = true;
            //    strSearchValues += "idsovb=" + idsovb.ToString() + ";";
            //}
            //if ((idkhoiph != null) && (idkhoiph != 0))
            //{
            //    vanban = vanban.Where(p => p.intidkhoiphathanh == idkhoiph);
            //    isSearch = true;
            //    strSearchValues += "idkhoiph=" + idkhoiph.ToString() + ";";
            //}
            //// tinh trang xu ly
            //if (!string.IsNullOrEmpty(xuly))
            //{

            //}

            ////====================================================
            //// Search van ban
            ////====================================================
            //if ((intsodenkt != null) && (intsodenkt != 0))
            //{
            //    if ((intsodenbd != null) && (intsodenbd != 0))
            //    {
            //        vanban = vanban.Where(p => p.intsoden >= intsodenbd)
            //                .Where(p => p.intsoden <= intsodenkt);
            //        isSearch = true;
            //        strSearchValues += "intsodenbd=" + intsodenbd.ToString() + ";intsodenkt=" + intsodenkt.ToString() + ";";
            //    }
            //}
            //else
            //{
            //    if ((intsodenbd != null) && (intsodenbd != 0))
            //    {
            //        vanban = vanban.Where(p => p.intsoden == intsodenbd);
            //        isSearch = true;
            //        strSearchValues += "intsodenbd=" + intsodenbd.ToString() + ";";
            //    }
            //}

            //if (!string.IsNullOrEmpty(strngaydenkt))
            //{
            //    if (!string.IsNullOrEmpty(strngaydenbd))
            //    {
            //        DateTime? dtngaydenbd = DateServices.FormatDateEn(strngaydenbd);
            //        DateTime? dtngaydenkt = DateServices.FormatDateEn(strngaydenkt);
            //        vanban = vanban.Where(p => p.strngayden >= dtngaydenbd)
            //                .Where(p => p.strngayden <= dtngaydenkt);
            //        isSearch = true;
            //        strSearchValues += "strngaydenbd=" + strngaydenbd + ";strngaydenkt=" + strngaydenkt + ";";
            //    }
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(strngaydenbd))
            //    {
            //        DateTime? dtngaydenbd = DateServices.FormatDateEn(strngaydenbd);
            //        vanban = vanban.Where(p => p.strngayden == dtngaydenbd);
            //        isSearch = true;
            //        strSearchValues += "strngaydenbd=" + strngaydenbd + ";";
            //    }
            //}

            //if (!string.IsNullOrEmpty(strngaykykt))
            //{
            //    if (!string.IsNullOrEmpty(strngaykybd))
            //    {
            //        DateTime? dtngaykybd = DateServices.FormatDateEn(strngaykybd);
            //        DateTime? dtngaykykt = DateServices.FormatDateEn(strngaykykt);
            //        vanban = vanban.Where(p => p.strngayky >= dtngaykybd)
            //                .Where(p => p.strngayky <= dtngaykykt);
            //        isSearch = true;
            //        strSearchValues += "strngaykybd=" + strngaykybd + ";strngaykykt=" + strngaykykt + ";";
            //    }
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(strngaykybd))
            //    {
            //        DateTime? dtngaykybd = DateServices.FormatDateEn(strngaykybd);
            //        vanban = vanban.Where(p => p.strngayky == dtngaykybd);
            //        isSearch = true;
            //        strSearchValues += "strngaykybd=" + strngaykybd + ";";
            //    }
            //}

            //if (!string.IsNullOrEmpty(strsokyhieu))
            //{
            //    // neu la so thi tim =
            //    // neu la chu thi tim like
            //    vanban = vanban.Where(p => p.strkyhieu.Contains(strsokyhieu));
            //    isSearch = true;
            //    strSearchValues += "strsokyhieu=" + strsokyhieu + ";";
            //}

            //if (!string.IsNullOrEmpty(strtrichyeu))
            //{
            //    vanban = vanban.Where(p => p.strtrichyeu.Contains(strtrichyeu));
            //    isSearch = true;
            //    strSearchValues += "strtrichyeu=" + strtrichyeu + ";";
            //}

            //if (!string.IsNullOrEmpty(strnguoixuly))
            //{
            //    vanban = vanban.Where(p => p.strnoinhan.Contains(strnguoixuly));
            //    isSearch = true;
            //    strSearchValues += "strnguoixuly=" + strnguoixuly + ";";
            //}

            //if (!string.IsNullOrEmpty(strnguoiky))
            //{
            //    vanban = vanban.Where(p => p.strnguoiky.Contains(strnguoiky));
            //    isSearch = true;
            //    strSearchValues += "strnguoiky=" + strnguoiky + ";";
            //}

            //if (!string.IsNullOrEmpty(strnoigui))
            //{
            //    vanban = vanban.Where(p => p.strnoiphathanh.Contains(strnoigui));
            //    isSearch = true;
            //    strSearchValues += "strnoigui=" + strnoigui + ";";
            //}


            ////========================================================
            //// end search
            ////========================================================
            //bool isViewVBDenDaXL = _configRepo.GetConfigToBool(ThamsoHethong.IsViewVBDenDaXL);

            //if (!isSearch)
            //{   // khong phai la search thi gioi han ngay hien thi                
            //    int intngay = _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);
            //    vanban = vanban.Where(p => System.Data.Entity.DbFunctions.DiffDays(p.strngayden, DateTime.Now) < intngay);

            //    // reset session
            //    _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.NoSearch);
            //}
            //else
            //{   // luu cac gia tri search vao session
            //    _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.SearchVBDen);
            //    _session.InsertObject(AppConts.SessionSearchValues, strSearchValues);

            //    // tim kiem thi hien thi tat ca
            //    isViewVBDenDaXL = true;
            //}
            #endregion temp

            var vanban = _GetVanbandenFromRequest
                ((int)EnumSession.SearchType.SearchVBDen,
                strngaydencat, idloaivb,
                idkhoiph, idsovb, xuly,
                intsodenbd, intsodenkt, strngaydenbd, strngaydenkt,
                strngaykybd, strngaykykt, strsokyhieu, strnguoiky,
                strnoigui, strtrichyeu, strnguoixuly
                );
            //====================================================
            // chon cac truong tra ve view list van ban
            //====================================================
            bool isViewVBDenDaXL = _configRepo.GetConfigToBool(ThamsoHethong.IsViewVBDenDaXL);

            int _searchType = Convert.ToInt32(_session.GetObject(AppConts.SessionSearchType));
            if (_searchType == (int)EnumSession.SearchType.SearchVBDen)
            {
                isViewVBDenDaXL = true;
            }
            //====================================================
            // kiem tra nhung van ban user duoc quyen xem/xuly
            //====================================================
            bool isViewAllvb = _role.IsRole(RoleVanbanden.Xemtatcavb);
            if (isViewAllvb)
            {   // co quyen xem tat ca van ban                
                var listvb = _GetListViewVanban__All(vanban);
                if (!string.IsNullOrEmpty(xuly))
                {
                    listvb = _SearchTinhtrangxuly(xuly, listvb);
                }
                return listvb;
            }
            else
            {
                var vanbanxuly = _GetQuyenXemVB(vanban, isViewAllvb);
                var listvbxuly = _GetListViewVanban__Canbo(vanbanxuly, isViewVBDenDaXL);

                var listvbpublic = _GetListViewVanban__Pulic(vanban, isViewVBDenDaXL);
                var listvbtonghop = listvbxuly.Union(listvbpublic);

                if (!string.IsNullOrEmpty(xuly))
                {
                    listvbtonghop = _SearchTinhtrangxuly(xuly, listvbtonghop);
                }
                return listvbtonghop;
            }

        }


        /// <summary>
        /// lay tat ca VANBANDEN ma user dc quyen xem
        /// </summary>
        /// <returns></returns>
        private IQueryable<Vanbanden> _GetVanbandenFromRequest
            (int intType,
            string strngaydencat, int? idloaivb,
            int? idkhoiph, int? idsovb, string xuly,
            int? intsodenbd, int? intsodenkt, string strngaydenbd, string strngaydenkt,
            string strngaykybd, string strngaykykt, string strsokyhieu, string strnguoiky,
            string strnoigui, string strtrichyeu, string strnguoixuly
            )
        {
            bool isSearch = false;
            bool isCategory = false;
            string strSearchValues = string.Empty;
            // strSearchValues = "intsodenbd=1;intsodenkt=10;idloaivb=2;"
            //===========================================================
            var vanban = _vanbandenRepo.Vanbandens;
            //====================================================
            // tuy chon category 
            //====================================================
            if (!string.IsNullOrEmpty(strngaydencat))
            {
                DateTime? dtengayden = DateServices.FormatDateEn(strngaydencat);
                vanban = vanban.Where(p => p.strngayden == dtengayden);
                isSearch = true;
                isCategory = true;
                strSearchValues += "strngaydencat=" + strngaydencat + ";";
            }
            if ((idloaivb != null) && (idloaivb != 0))
            {
                vanban = vanban.Where(p => p.intidphanloaivanbanden == idloaivb);
                isSearch = true;
                isCategory = true;
                strSearchValues += "idloaivb=" + idloaivb.ToString() + ";";
            }
            if ((idsovb != null) && (idsovb != 0))
            {
                vanban = vanban.Where(p => p.intidsovanban == idsovb);
                isSearch = true;
                isCategory = true;
                strSearchValues += "idsovb=" + idsovb.ToString() + ";";
            }
            if ((idkhoiph != null) && (idkhoiph != 0))
            {
                vanban = vanban.Where(p => p.intidkhoiphathanh == idkhoiph);
                isSearch = true;
                isCategory = true;
                strSearchValues += "idkhoiph=" + idkhoiph.ToString() + ";";
            }
            // tinh trang xu ly
            if (!string.IsNullOrEmpty(xuly))
            {
                if (xuly == "chuaduyet")
                {
                    vanban = vanban.Where(p => p.inttrangthai == (int)enumVanbanden.inttrangthai.Chuaduyet);
                }
                isSearch = true;
                isCategory = true;
                strSearchValues += "xuly=" + xuly + ";";
            }

            //====================================================
            // Search van ban
            //====================================================
            if ((intsodenkt != null) && (intsodenkt != 0))
            {
                if ((intsodenbd != null) && (intsodenbd != 0))
                {
                    vanban = vanban.Where(p => p.intsoden >= intsodenbd)
                            .Where(p => p.intsoden <= intsodenkt);
                    isSearch = true;
                    strSearchValues += "intsodenbd=" + intsodenbd.ToString() + ";intsodenkt=" + intsodenkt.ToString() + ";";
                }
            }
            else
            {
                if ((intsodenbd != null) && (intsodenbd != 0))
                {
                    vanban = vanban.Where(p => p.intsoden == intsodenbd);
                    isSearch = true;
                    strSearchValues += "intsodenbd=" + intsodenbd.ToString() + ";";
                }
            }

            if (!string.IsNullOrEmpty(strngaydenkt))
            {
                if (!string.IsNullOrEmpty(strngaydenbd))
                {
                    DateTime? dtngaydenbd = DateServices.FormatDateEn(strngaydenbd);
                    DateTime? dtngaydenkt = DateServices.FormatDateEn(strngaydenkt);
                    vanban = vanban.Where(p => p.strngayden >= dtngaydenbd)
                            .Where(p => p.strngayden <= dtngaydenkt);
                    isSearch = true;
                    strSearchValues += "strngaydenbd=" + strngaydenbd + ";strngaydenkt=" + strngaydenkt + ";";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(strngaydenbd))
                {
                    DateTime? dtngaydenbd = DateServices.FormatDateEn(strngaydenbd);
                    vanban = vanban.Where(p => p.strngayden == dtngaydenbd);
                    isSearch = true;
                    strSearchValues += "strngaydenbd=" + strngaydenbd + ";";
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

            if (!string.IsNullOrEmpty(strsokyhieu))
            {
                // neu la so thi tim =
                // neu la chu thi tim like
                vanban = vanban.Where(p => p.strkyhieu.Contains(strsokyhieu));
                isSearch = true;
                strSearchValues += "strsokyhieu=" + strsokyhieu + ";";
            }

            if (!string.IsNullOrEmpty(strtrichyeu))
            {
                vanban = vanban.Where(p => p.strtrichyeu.Contains(strtrichyeu));
                isSearch = true;
                strSearchValues += "strtrichyeu=" + strtrichyeu + ";";
            }

            if (!string.IsNullOrEmpty(strnguoixuly))
            {
                vanban = vanban.Where(p => p.strnoinhan.Contains(strnguoixuly));
                isSearch = true;
                strSearchValues += "strnguoixuly=" + strnguoixuly + ";";
            }

            if (!string.IsNullOrEmpty(strnguoiky))
            {
                vanban = vanban.Where(p => p.strnguoiky.Contains(strnguoiky));
                isSearch = true;
                strSearchValues += "strnguoiky=" + strnguoiky + ";";
            }

            if (!string.IsNullOrEmpty(strnoigui))
            {
                vanban = vanban.Where(p => p.strnoiphathanh.Contains(strnoigui));
                isSearch = true;
                strSearchValues += "strnoigui=" + strnoigui + ";";
            }


            //========================================================
            // end search
            //========================================================
            //bool isViewVBDenDaXL = _configRepo.GetConfigToBool(ThamsoHethong.IsViewVBDenDaXL);

            if (!isSearch)
            {   // khong phai la search thi gioi han ngay hien thi                
                int intngay = _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);
                //vanban = vanban.Where(p => System.Data.Entity.DbFunctions.DiffDays(p.strngayden, DateTime.Now) < intngay);

                DateTime? dtengaybd = DateTime.Now.AddDays(-intngay);
                vanban = vanban.Where(p => p.strngayden >= dtengaybd);

                // reset session
                _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.NoSearch);
            }
            else
            {   // luu cac gia tri search vao session
                if (intType == (int)EnumSession.SearchType.SearchVBDen)
                {
                    _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.SearchVBDen);
                    _session.InsertObject(AppConts.SessionSearchTypeValues, strSearchValues);
                }
                if (intType == (int)EnumSession.SearchType.SearchVBDenLQ)
                {
                    _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.SearchVBDenLQ);
                    _session.InsertObject(AppConts.SessionSearchTypeValues, strSearchValues);
                }

                // tim kiem thi hien thi tat ca

                // Category thi gioi han ngay hien thi
                if (isCategory)
                {
                    int intngay = _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);
                    DateTime? dtengaybd = DateTime.Now.AddDays(-intngay);
                    vanban = vanban.Where(p => p.strngayden >= dtengaybd);
                }
            }

            return vanban;
        }


        private IEnumerable<ListVanbandenViewModel> _SearchTinhtrangxuly(string xuly, IEnumerable<ListVanbandenViewModel> vanban)
        {
            var listvb = vanban;
            switch (xuly)
            {
                case "daxuly":
                    listvb = vanban.Where(p => p.inttinhtrangxuly == (int)enumHosocongviec.inttrangthai.Dahoanthanh);
                    break;
                case "xulychinh":
                    int idcanbo = _session.GetUserId();
                    string strhoten = _canboRepo.GetAllCanbo.FirstOrDefault(p => p.intid == idcanbo).strhoten;
                    listvb = vanban.Where(p => p.inttinhtrangxuly == (int)enumHosocongviec.inttrangthai.Dangxuly)
                        .Where(p => p.strnoinhan.Contains(strhoten));
                    break;
                case "phoihopxl":
                    idcanbo = _session.GetUserId();
                    strhoten = _canboRepo.GetAllCanbo.FirstOrDefault(p => p.intid == idcanbo).strhoten;
                    listvb = vanban.Where(p => p.inttinhtrangxuly == (int)enumHosocongviec.inttrangthai.Dangxuly)
                        .Where(p => !p.strnoinhan.Contains(strhoten));
                    break;
            }
            return listvb;
        }

        /// <summary>
        /// lay nhung van ban ma user co quyen xem/xuly
        /// chua co van ban public
        /// </summary>
        /// <param name="vanban"></param>
        /// <returns></returns>
        private IQueryable<Vanbanden> _GetQuyenXemVB(IQueryable<Vanbanden> vanban, bool isViewAllvb)
        {
            var vbtonghop = vanban;
            int idcanbo = _session.GetUserId();
            try
            {
                if (isViewAllvb == false)
                {
                    // khong co quyen xem tat ca cac van ban den

                    var vbxuly = _hosovanbanRepo.Hosovanbans.Select(p => p.intidvanban);

                    bool IsXulyHoso = _configRepo.GetConfigToBool(ThamsoHethong.IsXulyHoso);
                    // lấy các văn bản thuộc quyền xử lý
                    if (IsXulyHoso)
                    {
                        vbxuly = _hosovanbanRepo.Hosovanbans
                            .Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden)
                            .Join(
                            // kiem tra vai tro trong doi tuong xu ly
                            // dung distinct de kiem tra TH 1 vb phan cho 1 nguoi 3 vai tro: ldgv,ldpt va xlc

                                // lay tat ca user co tham gia xu ly 
                                _doituongRepo.GetAllCanboxulys
                                    .Where(p => p.intidcanbo == idcanbo)
                                    .Select(p => p.intidhosocongviec)
                                    .Distinct()  // dung distinct o day chay nhanh hon
                                    ,
                                vb2 => vb2.intidhosocongviec,
                                dt => dt.Value,
                                (vb2, dt) => new { vb2.intidvanban }
                            )
                        .Select(p => p.intidvanban);
                    }
                    else
                    {
                        vbxuly = _hosovanbanRepo.Hosovanbans
                           .Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden)
                            .Join(
                            // kiem tra vai tro trong doi tuong xu ly
                            // dung distinct de kiem tra TH 1 vb phan cho 1 nguoi 3 vai tro: ldgv,ldpt va xlc

                                // chi lay user dang tham gia xu ly 
                                _doituongRepo.GetCanboDangXulys
                                    .Where(p => p.intidcanbo == idcanbo)
                                    .Select(p => p.intidhosocongviec)
                                    .Distinct()  // dung distinct o day chay nhanh hon
                                    ,
                                vb2 => vb2.intidhosocongviec,
                                dt => dt.Value,
                                (vb2, dt) => new { vb2.intidvanban }
                            )
                            .Select(p => p.intidvanban);
                    }

                    // lấy các văn bản được cấp quyền xem 
                    var vbxem = _vanbandencanboRepo.VanbandenCanbos
                            .Where(p => p.intidcanbo == idcanbo);

                    // tong hop vbxuly, vbxem va public

                    //vbtonghop = vbtonghop
                    //   .Where(t =>
                    //          vbxem.Any(p => p.intidvanban == t.intid)
                    //       || vbxuly.Any(c => c == t.intid)
                    //       || t.intpublic == (int)enumVanbanden.intpublic.Public
                    //   ); //chay rat cham

                    // khong co van ban public
                    vbtonghop = vbtonghop
                       .Where(t =>
                               vbxem.Any(p => p.intidvanban == t.intid)
                               || vbxuly.Any(c => c == t.intid)
                            )
                        .Where(t => t.intpublic == (int)enumVanbanden.intpublic.Private)
                       ;

                } // end isViewAllvb
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return vbtonghop;
        }

        /// <summary>
        /// tra ve list view van ban den tuy theo quyen xem vb cua can bo
        /// </summary>
        /// <param name="vanban"></param>
        /// <returns></returns>
        private IEnumerable<ListVanbandenViewModel> _GetListViewVanban(IQueryable<Vanbanden> vanban, bool isViewVBDenDaXL)
        {
            //===============================================================
            // cach 1: truc tiep dung dbcontext
            //IEnumerable<ListVanbandenModel> listvb2 = context.Vanbandens
            //    .Select(p => new ListVanbandenModel
            //    {
            //        intid = p.intid,
            //        intsoden = p.intsoden,
            //        ...................
            //        intvbdt = p.intvbdt,
            //        intattach = context.AttachVanbans.Where(f => f.intloai == (int)EnumVanban.intloai_attachvanban.Vanbanden)
            //                    .Any(f => f.intidvanban == p.intid)
            //    });

            // cach 2: dung qua DI:
            // lampa: groupjoin
            // join ... into
            // sql: left outer join
            //=================================================================
            int idcanbo = _session.GetUserId();

            var listvb = vanban
                .GroupJoin(
                    _fileRepo.AttachVanbans
                        .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                        .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbanden),
                    v => v.intid,
                    f => f.intidvanban,
                    (v, f) => new { v, f }
                )
                .Join(
                    _hosovanbanRepo.Hosovanbans.Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden),
                    v2 => v2.v.intid,
                    hsvb => hsvb.intidvanban,
                    (v2, hsvb) => new { v2, hsvb.intidhosocongviec }
                )
                .Join(
                    _hosocongviecRepo.Hosocongviecs,
                    v3 => v3.intidhosocongviec,
                    hscv => hscv.intid,
                    (v3, hscv) => new { v3, hscv.inttrangthai }
                )
                .Select(p => new ListVanbandenViewModel
                {
                    intid = p.v3.v2.v.intid,
                    intsoden = p.v3.v2.v.intsoden,
                    dtengayden = p.v3.v2.v.strngayden,
                    strkyhieu = p.v3.v2.v.strkyhieu,
                    strnoinhan = p.v3.v2.v.strnoinhan,
                    strnoiphathanh = p.v3.v2.v.strnoiphathanh,
                    strtrichyeu = p.v3.v2.v.strtrichyeu,
                    inttrangthai = p.v3.v2.v.inttrangthai,
                    IsVbdt = p.v3.v2.v.intdangvb == (int)enumVanbanden.intvbdt.VBDT ? true : false,
                    IsAttach = p.v3.v2.f.Any(),

                    inttinhtrangxuly = p.inttrangthai,
                    intidhoso = p.v3.intidhosocongviec

                })
                //.Distinct() 
                // dung distinct o day chay lau hon
                ;

            if (isViewVBDenDaXL)
            {
                return listvb;
            }
            else
            {
                // chi hien thi nhung van ban dang xu ly
                var listvbdangxuly = listvb.Where(p => p.inttinhtrangxuly == (int)enumHosocongviec.inttrangthai.Dangxuly);

                return listvbdangxuly;
            }

        }

        /// <summary>
        /// lay tat ca cac van ban(quyen hien thi tat ca van ban den)
        /// </summary>
        /// <param name="vanban"></param>
        /// <param name="isViewVBDenDaXL"></param>
        /// <returns></returns>
        private IEnumerable<ListVanbandenViewModel> _GetListViewVanban__All(IQueryable<Vanbanden> vanban)
        {

            int idcanbo = _session.GetUserId();

            var listvb = vanban
                .GroupJoin(
                    _fileRepo.AttachVanbans
                        .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                        .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbanden),
                    v => v.intid,
                    f => f.intidvanban,
                    (v, f) => new { v, f }
                )
                .GroupJoin(
                    _hosovanbanRepo.Hosovanbans.Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden),
                    v2 => v2.v.intid,
                    hsvb => hsvb.intidvanban,
                    (v2, hsvb) => new { v2, hsvb.FirstOrDefault().intidhosocongviec }
                )
                .GroupJoin(
                    _hosocongviecRepo.Hosocongviecs,
                    v3 => v3.intidhosocongviec,
                    hscv => hscv.intid,
                    (v3, hscv) => new { v3, hscv.FirstOrDefault().inttrangthai }
                )
                .Select(p => new ListVanbandenViewModel
                {
                    intid = p.v3.v2.v.intid,
                    intsoden = p.v3.v2.v.intsoden,
                    dtengayden = p.v3.v2.v.strngayden,
                    strkyhieu = p.v3.v2.v.strkyhieu,
                    strnoinhan = p.v3.v2.v.strnoinhan,
                    strnoiphathanh = p.v3.v2.v.strnoiphathanh,
                    strtrichyeu = p.v3.v2.v.strtrichyeu,
                    inttrangthai = p.v3.v2.v.inttrangthai,
                    IsVbdt = p.v3.v2.v.intdangvb == (int)enumVanbanden.intvbdt.VBDT ? true : false,
                    IsAttach = p.v3.v2.f.Any(),

                    inttinhtrangxuly = p.inttrangthai,
                    intidhoso = p.v3.intidhosocongviec

                })
                //.Distinct() 
                // dung distinct o day chay lau hon
                ;
            return listvb;
        }

        /// <summary>
        /// tat ca van ban thuoc quyen xu ly/ quyen xem cua can bo
        /// </summary>
        /// <param name="vanban"></param>
        /// <param name="isViewVBDenDaXL"></param>
        /// <returns></returns>
        private IEnumerable<ListVanbandenViewModel> _GetListViewVanban__Canbo(IQueryable<Vanbanden> vanban, bool isViewVBDenDaXL)
        {
            int idcanbo = _session.GetUserId();

            var listvb = vanban
                .GroupJoin(
                    _fileRepo.AttachVanbans
                        .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                        .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbanden),
                    v => v.intid,
                    f => f.intidvanban,
                    (v, f) => new { v, f }
                )
                .GroupJoin(
                    _hosovanbanRepo.Hosovanbans.Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden),
                    v2 => v2.v.intid,
                    hsvb => hsvb.intidvanban,
                    (v2, hsvb) => new { v2, hsvb.FirstOrDefault().intidhosocongviec }
                )
                .GroupJoin(
                    _hosocongviecRepo.Hosocongviecs,
                    v3 => v3.intidhosocongviec,
                    hscv => hscv.intid,
                    (v3, hscv) => new { v3, hscv.FirstOrDefault().inttrangthai }
                )
                .Select(p => new ListVanbandenViewModel
                {
                    intid = p.v3.v2.v.intid,
                    intsoden = p.v3.v2.v.intsoden,
                    dtengayden = p.v3.v2.v.strngayden,
                    strkyhieu = p.v3.v2.v.strkyhieu,
                    strnoinhan = p.v3.v2.v.strnoinhan,
                    strnoiphathanh = p.v3.v2.v.strnoiphathanh,
                    strtrichyeu = p.v3.v2.v.strtrichyeu,
                    inttrangthai = p.v3.v2.v.inttrangthai,
                    IsVbdt = p.v3.v2.v.intdangvb == (int)enumVanbanden.intvbdt.VBDT ? true : false,
                    IsAttach = p.v3.v2.f.Any(),

                    inttinhtrangxuly = p.inttrangthai,
                    intidhoso = p.v3.intidhosocongviec

                })
                //.Distinct() 
                // dung distinct o day chay lau hon
                ;

            if (isViewVBDenDaXL)
            {
                return listvb;
            }
            else
            {
                // chi hien thi nhung van ban dang xu ly
                var listvbdangxuly = listvb.Where(p => p.inttinhtrangxuly == (int)enumHosocongviec.inttrangthai.Dangxuly);

                return listvbdangxuly;
            }

        }

        /// <summary>
        /// lay tat ca van ban duoc public 
        /// </summary>
        /// <param name="vanban"></param>
        /// <param name="isViewVBDenDaXL"></param>
        /// <returns></returns>
        private IEnumerable<ListVanbandenViewModel> _GetListViewVanban__Pulic(IQueryable<Vanbanden> vanban, bool isViewVBDenDaXL)
        {

            int idcanbo = _session.GetUserId();

            var listvb = vanban.Where(p => p.intpublic == (int)enumVanbanden.intpublic.Public)
                .GroupJoin(
                    _fileRepo.AttachVanbans
                        .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                        .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbanden),
                    v => v.intid,
                    f => f.intidvanban,
                    (v, f) => new { v, f }
                )
                .GroupJoin(
                    _hosovanbanRepo.Hosovanbans.Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden),
                    v2 => v2.v.intid,
                    hsvb => hsvb.intidvanban,
                    (v2, hsvb) => new { v2, hsvb.FirstOrDefault().intidhosocongviec }
                )
                .GroupJoin(
                    _hosocongviecRepo.Hosocongviecs,
                    v3 => v3.intidhosocongviec,
                    hscv => hscv.intid,
                    (v3, hscv) => new { v3, hscv.FirstOrDefault().inttrangthai }
                )
                .Select(p => new ListVanbandenViewModel
                {
                    intid = p.v3.v2.v.intid,
                    intsoden = p.v3.v2.v.intsoden,
                    dtengayden = p.v3.v2.v.strngayden,
                    strkyhieu = p.v3.v2.v.strkyhieu,
                    strnoinhan = p.v3.v2.v.strnoinhan,
                    strnoiphathanh = p.v3.v2.v.strnoiphathanh,
                    strtrichyeu = p.v3.v2.v.strtrichyeu,
                    inttrangthai = p.v3.v2.v.inttrangthai,
                    IsVbdt = p.v3.v2.v.intdangvb == (int)enumVanbanden.intvbdt.VBDT ? true : false,
                    IsAttach = p.v3.v2.f.Any(),

                    inttinhtrangxuly = p.inttrangthai,
                    intidhoso = p.v3.intidhosocongviec

                })
                //.Distinct() 
                // dung distinct o day chay lau hon                
                ;

            return listvb;

        }

        #endregion EF


        #region RawSql

        public IEnumerable<ListVanbandenViewModel> GetListVanbanden
            (string strngaydencat, int? idloaivb,
            int? idkhoiph, int? idsovb, string xuly,
            int? intsodenbd, int? intsodenkt, string strngaydenbd, string strngaydenkt,
            string strngaykybd, string strngaykykt, string strsokyhieu, string strnguoiky,
            string strnoigui, string strtrichyeu, string strnguoixuly, string strdangvanban
            )
        {
            string strSearchValues = _SqlSearchVBDen
                (strngaydencat, idloaivb,
                idkhoiph, idsovb, xuly,
                intsodenbd, intsodenkt, strngaydenbd, strngaydenkt,
                strngaykybd, strngaykykt, strsokyhieu, strnguoiky,
                strnoigui, strtrichyeu, strnguoixuly, strdangvanban
                );

            bool isViewVBDenDaXL = _configRepo.GetConfigToBool(ThamsoHethong.IsViewVBDenDaXL);

            int _searchType = Convert.ToInt32(_session.GetObject(AppConts.SessionSearchType));
            if (_searchType == (int)EnumSession.SearchType.SearchVBDen)
            {
                isViewVBDenDaXL = true;
            }
            //====================================================
            // kiem tra nhung van ban user duoc quyen xem/xuly
            //====================================================
            bool isViewAllvb = _role.IsRole(RoleVanbanden.Xemtatcavb);

            int intloai_sql = AppSettings.LoaiVBDen;
            string strqueryvbden = string.Empty;
            switch (intloai_sql)
            {
                case 0:
                    strqueryvbden = _SqlGetAllVBden_v0(isViewVBDenDaXL, isViewAllvb, strSearchValues);
                    break;
                case 1:
                    strqueryvbden = _SqlGetAllVBden_v1(isViewVBDenDaXL, isViewAllvb, strSearchValues);
                    break;
                case 2:
                    strqueryvbden = _SqlGetAllVBden_v2(isViewVBDenDaXL, isViewAllvb, strSearchValues);
                    break;
            }

            string query = strqueryvbden; //_SqlGetAllVBden_v2(isViewVBDenDaXL, isViewAllvb, strSearchValues);

            IEnumerable<ListVanbandenViewModel> listvb = (IEnumerable<ListVanbandenViewModel>)_vanbandenRepo.RunSqlListVBDen(query);

            return listvb;

        }

        private string _SqlSearchVBDen(
            string strngaydencat, int? idloaivb,
            int? idkhoiph, int? idsovb, string xuly,
            int? intsodenbd, int? intsodenkt, string strngaydenbd, string strngaydenkt,
            string strngaykybd, string strngaykykt, string strsokyhieu, string strnguoiky,
            string strnoigui, string strtrichyeu, string strnguoixuly, string strdangvanban
            )
        {
            string strWhere = string.Empty;
            string query = string.Empty;

            bool isSearch = false;
            bool isCategory = false;
            string strSearchValues = string.Empty;
            // strSearchValues = "intsodenbd=1;intsodenkt=10;idloaivb=2;"
            //===========================================================
            // kiem tra cac gia tri string search

            //====================================================
            // tuy chon category 
            //====================================================
            if (!string.IsNullOrEmpty(strngaydencat))
            {
                strngaydencat = ValidateData.CheckInput(strngaydencat);

                DateTime? dtengayden = DateServices.FormatDateEn(strngaydencat);
                query = " strngayden='" + DateServices.FormatDateEn(dtengayden) + "' ";
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                //isCategory = true;
                strSearchValues += "strngaydencat=" + strngaydencat + ";";
            }
            if ((idloaivb != null) && (idloaivb != 0))
            {
                query = " intidphanloaivanbanden=" + idloaivb;
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                //isCategory = true;
                strSearchValues += "idloaivb=" + idloaivb.ToString() + ";";
            }
            if ((idsovb != null) && (idsovb != 0))
            {
                query = " intidsovanban=" + idsovb;
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                //isCategory = true;
                strSearchValues += "idsovb=" + idsovb.ToString() + ";";
            }
            if ((idkhoiph != null) && (idkhoiph != 0))
            {
                query = " intidkhoiphathanh=" + idkhoiph;
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                isCategory = true;
                strSearchValues += "idkhoiph=" + idkhoiph.ToString() + ";";
            }
            // tinh trang xu ly
            if (!string.IsNullOrEmpty(xuly))
            {
                xuly = ValidateData.CheckInput(xuly);
                switch (xuly)
                {
                    case "chuaduyet":
                        query = " vanbanden.inttrangthai=" + (int)enumVanbanden.inttrangthai.Chuaduyet;
                        break;
                    case "daxuly":
                        query = " hscv.inttrangthai=" + (int)enumHosocongviec.inttrangthai.Dahoanthanh;
                        break;
                    case "xulychinh":
                        int idcanbo = _session.GetUserId();
                        string strhoten = _canboRepo.GetAllCanbo.FirstOrDefault(p => p.intid == idcanbo).strhoten;
                        query = " hscv.inttrangthai=" + (int)enumHosocongviec.inttrangthai.Dangxuly
                            + " and strnoinhan like N'%" + strhoten + "%' ";
                        break;
                    case "phoihopxl":
                        idcanbo = _session.GetUserId();
                        strhoten = _canboRepo.GetAllCanbo.FirstOrDefault(p => p.intid == idcanbo).strhoten;
                        query = " hscv.inttrangthai=" + (int)enumHosocongviec.inttrangthai.Dangxuly
                            + " and strnoinhan not like N'%" + strhoten + "%' ";
                        break;
                }
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                isCategory = true;
                strSearchValues += "xuly=" + xuly + ";";
            }

            //====================================================
            // Search van ban
            //====================================================
            if ((intsodenkt != null) && (intsodenkt != 0))
            {
                if ((intsodenbd != null) && (intsodenbd != 0))
                {
                    query = " vanbanden.intsoden>=" + intsodenbd + " and vanbanden.intsoden<=" + intsodenkt;
                    if (string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += query;
                    }
                    else
                    {
                        strWhere += " and " + query;
                    }
                    isSearch = true;
                    strSearchValues += "intsodenbd=" + intsodenbd.ToString() + ";intsodenkt=" + intsodenkt.ToString() + ";";
                }
            }
            else
            {
                if ((intsodenbd != null) && (intsodenbd != 0))
                {
                    query = " vanbanden.intsoden=" + intsodenbd;
                    if (string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += query;
                    }
                    else
                    {
                        strWhere += " and " + query;
                    }
                    isSearch = true;
                    strSearchValues += "intsodenbd=" + intsodenbd.ToString() + ";";
                }
            }

            if (!string.IsNullOrEmpty(strngaydenkt))
            {
                strngaydenkt = ValidateData.CheckInput(strngaydenkt);
                if (!string.IsNullOrEmpty(strngaydenbd))
                {
                    strngaydenbd = ValidateData.CheckInput(strngaydenbd);
                    DateTime? dtngaydenbd = DateServices.FormatDateEn(strngaydenbd);
                    DateTime? dtngaydenkt = DateServices.FormatDateEn(strngaydenkt);
                    query = " strngayden>='" + DateServices.FormatDateEn(dtngaydenbd) + "' and strngayden<='" + DateServices.FormatDateEn(dtngaydenkt) + "' ";
                    if (string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += query;
                    }
                    else
                    {
                        strWhere += " and " + query;
                    }
                    isSearch = true;
                    strSearchValues += "strngaydenbd=" + strngaydenbd + ";strngaydenkt=" + strngaydenkt + ";";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(strngaydenbd))
                {
                    strngaydenbd = ValidateData.CheckInput(strngaydenbd);
                    DateTime? dtngaydenbd = DateServices.FormatDateEn(strngaydenbd);
                    query = " strngayden='" + DateServices.FormatDateEn(dtngaydenbd) + "' ";
                    if (string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += query;
                    }
                    else
                    {
                        strWhere += " and " + query;
                    }
                    isSearch = true;
                    strSearchValues += "strngaydenbd=" + strngaydenbd + ";";
                }
            }

            if (!string.IsNullOrEmpty(strngaykykt))
            {
                if (!string.IsNullOrEmpty(strngaykybd))
                {
                    strngaykybd = ValidateData.CheckInput(strngaykybd);
                    strngaykykt = ValidateData.CheckInput(strngaykykt);

                    DateTime? dtngaykybd = DateServices.FormatDateEn(strngaykybd);
                    DateTime? dtngaykykt = DateServices.FormatDateEn(strngaykykt);
                    //vanban = vanban.Where(p => p.strngayky >= dtngaykybd)
                    //        .Where(p => p.strngayky <= dtngaykykt);
                    query = " strngayky>='" + DateServices.FormatDateEn(dtngaykybd) + "' and strngayky<='" + DateServices.FormatDateEn(dtngaykykt) + "' ";
                    if (string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += query;
                    }
                    else
                    {
                        strWhere += " and " + query;
                    }
                    isSearch = true;
                    strSearchValues += "strngaykybd=" + strngaykybd + ";strngaykykt=" + strngaykykt + ";";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(strngaykybd))
                {
                    strngaykybd = ValidateData.CheckInput(strngaykybd);
                    DateTime? dtngaykybd = DateServices.FormatDateEn(strngaykybd);
                    //vanban = vanban.Where(p => p.strngayky == dtngaykybd);
                    query = " strngayky='" + DateServices.FormatDateEn(dtngaykybd) + "' ";
                    if (string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += query;
                    }
                    else
                    {
                        strWhere += " and " + query;
                    }
                    isSearch = true;
                    strSearchValues += "strngaykybd=" + strngaykybd + ";";
                }
            }

            if (!string.IsNullOrEmpty(strsokyhieu))
            {
                strsokyhieu = ValidateData.CheckInput(strsokyhieu);
                // neu la so thi tim =
                // neu la chu thi tim like
                strsokyhieu = strsokyhieu.Trim();
                Dictionary<bool, string> result = ValidateData.SearchExactly(strsokyhieu);
                if (result.ContainsKey(true))
                {   // co tim kiem chinh xac "abc"
                    query = " strkyhieu = N'" + result[true] + "' ";
                }
                else
                {
                    query = " strkyhieu like N'%" + strsokyhieu + "%' ";
                }
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                strSearchValues += "strsokyhieu=" + strsokyhieu + ";";
            }

            if (!string.IsNullOrEmpty(strtrichyeu))
            {
                strtrichyeu = ValidateData.CheckInput(strtrichyeu);
                strtrichyeu = strtrichyeu.Trim();
                bool isfulltext = AppSettings.IsFullText;
                if (isfulltext)
                {   // co fultext search
                    Dictionary<bool, string> result = ValidateData.SearchExactly(strtrichyeu);
                    string searchValues = string.Empty;
                    if (result.ContainsKey(true))
                    {   // co tim kiem chinh xac "abc"                    
                        searchValues = ValidateData.GhepChuoiFullTextSearch(result[true], (int)ValidateData.enumFullTextSearch.AND);
                    }
                    else
                    {
                        //query = " freetext(strtrichyeu,'" + strtrichyeu + "' ) ";
                        searchValues = ValidateData.GhepChuoiFullTextSearch(strtrichyeu, (int)ValidateData.enumFullTextSearch.OR);
                    }
                    query = " contains(strtrichyeu,'" + searchValues + "') ";
                }
                else
                {   // khong fulltext search
                    query = " strtrichyeu like N'%" + strtrichyeu + "%' ";
                }

                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                strSearchValues += "strtrichyeu=" + strtrichyeu + ";";
            }

            if (!string.IsNullOrEmpty(strnguoixuly))
            {
                strnguoixuly = ValidateData.CheckInput(strnguoixuly);
                //vanban = vanban.Where(p => p.strnoinhan.Contains(strnguoixuly));
                query = " strnoinhan like N'%" + strnguoixuly + "%' ";
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                strSearchValues += "strnguoixuly=" + strnguoixuly + ";";
            }

            if (!string.IsNullOrEmpty(strnguoiky))
            {
                strnguoiky = ValidateData.CheckInput(strnguoiky);
                //vanban = vanban.Where(p => p.strnguoiky.Contains(strnguoiky));
                query = " strnguoiky like N'%" + strnguoiky + "%' ";
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                strSearchValues += "strnguoiky=" + strnguoiky + ";";
            }

            if (!string.IsNullOrEmpty(strnoigui))
            {
                strnoigui = ValidateData.CheckInput(strnoigui);
                //vanban = vanban.Where(p => p.strnoiphathanh.Contains(strnoigui));
                query = " strnoiphathanh like N'%" + strnoigui + "%' ";
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                strSearchValues += "strnoigui=" + strnoigui + ";";
            }

            if (!string.IsNullOrEmpty(strdangvanban))
            {
                strdangvanban = ValidateData.CheckInput(strdangvanban);
                int intdangvb = 0;
                switch (strdangvanban)
                {
                    case "dientu":
                        intdangvb = (int)enumVanbanden.intvbdt.VBDT;
                        query = " intdangvb='" + intdangvb.ToString() + "' ";
                        break;
                    case "dientugiay":
                        intdangvb = (int)enumVanbanden.intvbdt.VBDT_Giay;
                        query = " intdangvb='" + intdangvb.ToString() + "' ";
                        break;
                    case "giay":
                        intdangvb = (int)enumVanbanden.intvbdt.VBGiay;
                        query = " intdangvb='" + intdangvb.ToString() + "' ";
                        break;
                    default:
                        break;
                }
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                strSearchValues += "strdangvanban=" + strdangvanban + ";";
            }

            //========================================================
            // end search
            //========================================================


            if (!isSearch)
            {   // khong phai la search thi gioi han ngay hien thi                
                int intngay = _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);
                //vanban = vanban.Where(p => System.Data.Entity.DbFunctions.DiffDays(p.strngayden, DateTime.Now) < intngay);

                DateTime? dtengaybd = DateTime.Now.AddDays(-intngay);
                //vanban = vanban.Where(p => p.strngayden >= dtengaybd);

                string strngaybd = DateServices.FormatDateEn(dtengaybd);
                query = " strngayden >='" + strngaybd + "' ";
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }

                // reset session
                _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.NoSearch);
            }
            else
            {   // luu cac gia tri search vao session
                _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.SearchVBDen);
                _session.InsertObject(AppConts.SessionSearchTypeValues, strSearchValues);

                // tim kiem thi hien thi tat ca

                // Category thi gioi han ngay hien thi
                if (isCategory)
                {
                    int intngay = _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);
                    DateTime? dtengaybd = DateTime.Now.AddDays(-intngay);
                    string strngaybd = DateServices.FormatDateEn(dtengaybd);
                    query = " strngayden >='" + strngaybd + "' ";
                    if (string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += query;
                    }
                    else
                    {
                        strWhere += " and " + query;
                    }
                    //vanban = vanban.Where(p => p.strngayden >= dtengaybd);
                }
            }

            return strWhere;
        }

        /// <summary>
        /// tra ve tat ca cac van ban den duoc quyen xem
        /// </summary>
        /// <param name="isViewVBDenDaXL"></param>
        /// <param name="isViewAllVBDen"></param>
        /// <returns></returns>
        private string _SqlGetAllVBden_v0(bool isViewVBDenDaXL, bool isViewAllVBDen, string strSearchValues)
        {
            string stridcanbo = _session.GetUserId().ToString();

            string strSelect = "Select distinct vanbanden.intid,strKyhieu,strNgayden as dtengayden ,vanbanden.intSoden,strNoiphathanh "
            + ",strTrichyeu,vanbanden.inttrangthai ,intidphanloaivanbanden,strnoinhan,hsvb.intidhosocongviec as intidhoso "
            + ", case when ( intdangvb = 1) then cast(1 as bit) else cast(0 as bit) end as IsVbdt "
            + " ,case when( (select attachvanban.intidvanban ) is not null)then cast(1 as bit) ELSE cast(0 as bit) END as IsAttach "
            + ",(select top 1 hscv.inttrangthai ) as inttinhtrangxuly "

            //+ ", case when( (select yk.intid ) is not null)then cast(1 as bit) ELSE cast(0 as bit) END as isykien "

            + " From vanbanden "
            + " left outer join attachvanban on vanbanden.intid = attachvanban.intidvanban "
            + " and attachvanban.intloai=" + (int)enumAttachVanban.intloai.Vanbanden
            + " and attachvanban.inttrangthai=" + (int)enumAttachVanban.inttrangthai.IsActive

            + " left outer join hosovanban hsvb on hsvb.intidvanban = vanbanden.intid and hsvb.intloai=" + (int)enumHosovanban.intloai.Vanbanden
            + " left outer join hosocongviec hscv on hscv.intid=hsvb.intidhosocongviec " // and hscv.intloai=" + (int)enumHosocongviec.intloai.Vanbanden

            //+ " inner join hosocongviec hscv on hscv.intid=hsvb.intidhosocongviec " // and hscv.intloai=" + (int)enumHosocongviec.intloai.Vanbanden
                //+ " left outer join doituongxuly dtxl on dtxl.intidhosocongviec = hsvb.intidhosocongviec and dtxl.intidcanbo=" + stridcanbo
                //+ " left outer join hosoykienxuly yk on yk.intiddoituongxuly = dtxl.intid "
            ;

            string query = string.Empty;
            if (isViewAllVBDen)
            {
                // xem tat ca van ban den
                if (!string.IsNullOrEmpty(strSearchValues))
                {
                    query += strSelect + " where " + strSearchValues;
                }
                else
                {
                    query = strSelect;
                }
            }
            else
            {   // chi duoc xem nhung van ban den thuoc quyen xu ly/xem

                string strExists =
                    " and  ( "
                        + " Exists (Select intidvanban as intidvanban from vanbandencanbo "
                                    + " where vanbandencanbo.intidvanban=vanbanden.intID and intidcanbo=" + stridcanbo + " ) "
                        + " or Exists "
                                + " (select Hosovanban.intidvanban from Hosovanban "
                                + " inner join Doituongxuly on Doituongxuly.intidhosocongviec=Hosovanban.intidhosocongviec "
                                + " where intloai=" + (int)enumHosovanban.intloai.Vanbanden
                                + " and Doituongxuly.intidcanbo=" + stridcanbo
                                + " and Hosovanban.intidvanban=vanbanden.intID) "
                    //+ " or vanbanden.intpublic =" + (int)enumVanbanden.intpublic.Public // chay rat cham

                    + " )  "
                    + " and vanbanden.intpublic=" + (int)enumVanbanden.intpublic.Private
                    ;
                string vbpublic = " and vanbanden.intpublic=" + (int)enumVanbanden.intpublic.Public;

                if (!string.IsNullOrEmpty(strSearchValues))
                {
                    query += strSelect + " where " + strSearchValues + strExists;
                    query = query + " union " + strSelect + " where " + strSearchValues + vbpublic;
                }
                else
                {
                    query = strSelect + " where " + strExists;
                    query = query + " union " + strSelect + " where " + vbpublic;
                }
                //query = strSelect + strExists;
                //query = query + " union " + strSelect + vbpublic;

                if (!isViewVBDenDaXL)
                {
                    query = query + " and hscv.inttrangthai=" + (int)enumHosocongviec.inttrangthai.Dangxuly;
                }
            }

            string strOrder = " order by strngayden desc, intsoden desc ";
            query += strOrder;

            return query;
        }

        /// <summary>
        /// tra ve tat ca cac van ban den duoc quyen xem . da toi uu sql query
        /// </summary>
        /// <param name="isViewVBDenDaXL"></param>
        /// <param name="isViewAllVBDen"></param>
        /// <param name="strSearchValues"></param>
        /// <returns></returns>
        private string _SqlGetAllVBden_v1(bool isViewVBDenDaXL, bool isViewAllVBDen, string strSearchValues)
        {
            string stridcanbo = _session.GetUserId().ToString();

            string strSelect = "Select distinct vanbanden.intid,strKyhieu,strNgayden as dtengayden ,vanbanden.intSoden,strNoiphathanh "
            + ",strTrichyeu,vanbanden.inttrangthai ,intidphanloaivanbanden,strnoinhan,hsvb.intidhosocongviec as intidhoso "
            + ", case when ( intdangvb = 1) then cast(1 as bit) else cast(0 as bit) end as IsVbdt "
            + " ,case when( (select attachvanban.intidvanban ) is not null)then cast(1 as bit) ELSE cast(0 as bit) END as IsAttach "
            + ",(select hscv.inttrangthai ) as inttinhtrangxuly "

            //+ ", case when( (select yk.intid ) is not null)then cast(1 as bit) ELSE cast(0 as bit) END as isykien "

            + " From vanbanden "
            + " left outer join attachvanban on vanbanden.intid = attachvanban.intidvanban "
            + " and attachvanban.intloai=" + (int)enumAttachVanban.intloai.Vanbanden
            + " and attachvanban.inttrangthai=" + (int)enumAttachVanban.inttrangthai.IsActive

            + " left outer join hosovanban hsvb on hsvb.intidvanban = vanbanden.intid and hsvb.intloai=" + (int)enumHosovanban.intloai.Vanbanden
            + " left outer join hosocongviec hscv on hscv.intid=hsvb.intidhosocongviec " // and hscv.intloai=" + (int)enumHosocongviec.intloai.Vanbanden
            ;

            string query = string.Empty;
            if (isViewAllVBDen)
            {
                // xem tat ca van ban den
                if (!string.IsNullOrEmpty(strSearchValues))
                {
                    query += strSelect + " where " + strSearchValues;
                }
                else
                {
                    query = strSelect;
                }
            }
            else
            {   // chi duoc xem nhung van ban den thuoc quyen xu ly/xem

                string strExists =
                    " and  (( vanbanden.intpublic=" + (int)enumVanbanden.intpublic.Private
                            + " and ( Exists (Select intidvanban as intidvanban from vanbandencanbo "
                                        + " where vanbandencanbo.intidvanban=vanbanden.intID and intidcanbo=" + stridcanbo + " ) "
                            + " or Exists "
                                    + " (select Hosovanban.intidvanban from Hosovanban "
                                    + " inner join Doituongxuly on Doituongxuly.intidhosocongviec=Hosovanban.intidhosocongviec "
                                    + " and intloai=" + (int)enumHosovanban.intloai.Vanbanden
                                    + " and Doituongxuly.intidcanbo=" + stridcanbo
                                    + " and Hosovanban.intidvanban=hsvb.intidvanban) "
                                + " ) "
                            + " ) "
                        + " or vanbanden.intpublic=" + (int)enumVanbanden.intpublic.Public
                    + " ) "
                    ;

                if (!string.IsNullOrEmpty(strSearchValues))
                {
                    query += strSelect + " where " + strSearchValues + strExists;
                }
                else
                {
                    query = strSelect + " where " + strExists;
                }

                if (!isViewVBDenDaXL)
                {
                    query = query + " and hscv.inttrangthai=" + (int)enumHosocongviec.inttrangthai.Dangxuly;
                }
            }

            string strOrder = " order by strngayden desc, intsoden desc ";
            query += strOrder;

            return query;
        }

        private string _SqlGetAllVBden_v2(bool isViewVBDenDaXL, bool isViewAllVBDen, string strSearchValues)
        {
            string stridcanbo = _session.GetUserId().ToString();

            string strSelect = "Select distinct vanbanden.intid,strKyhieu,strNgayden as dtengayden ,vanbanden.intSoden,strNoiphathanh "
            + ",strTrichyeu,vanbanden.inttrangthai ,intidphanloaivanbanden,strnoinhan"
            + ",hsvb.intidhosocongviec as intidhoso "
            + ", case when ( intdangvb = 1) then cast(1 as bit) else cast(0 as bit) end as IsVbdt "
            + " ,case when( (select attachvanban.intidvanban ) is not null)then cast(1 as bit) ELSE cast(0 as bit) END as IsAttach "
            + ",(select hscv.inttrangthai ) as inttinhtrangxuly ";

            //+ ", case when( (select yk.intid ) is not null)then cast(1 as bit) ELSE cast(0 as bit) END as isykien "

            string strFrom = " From vanbanden "
            + " left outer join attachvanban on vanbanden.intid = attachvanban.intidvanban "
                            + " and attachvanban.intloai=" + (int)enumAttachVanban.intloai.Vanbanden
                            + " and attachvanban.inttrangthai=" + (int)enumAttachVanban.inttrangthai.IsActive

            + " left outer join hosovanban hsvb on hsvb.intidvanban = vanbanden.intid and hsvb.intloai=" + (int)enumHosovanban.intloai.Vanbanden
            + " left outer join hosocongviec hscv on hscv.intid=hsvb.intidhosocongviec " // and hscv.intloai=" + (int)enumHosocongviec.intloai.Vanbanden
            ;

            string query = string.Empty;
            if (isViewAllVBDen)
            {
                // xem tat ca van ban den
                if (!string.IsNullOrEmpty(strSearchValues))
                {
                    query += strSelect + strFrom + " where " + strSearchValues;
                }
                else
                {
                    query = strSelect + strFrom;
                }
            }
            else
            {   // chi duoc xem nhung van ban den thuoc quyen xu ly/xem

                string strFromExists = " left outer join vanbandencanbo vbcb on vbcb.intidvanban=vanbanden.intid and intidcanbo =" + stridcanbo
                    + " left outer join doituongxuly dtxl on dtxl.intidhosocongviec=hsvb.intidhosocongviec and dtxl.intidcanbo=" + stridcanbo
                    ;
                string strExists =
                    " and ( "
                            + "( vanbanden.intpublic =" + (int)enumVanbanden.intpublic.Private
                                        + " and (vbcb.intidvanban is not null or dtxl.intid is not null)) "
                            + " or vanbanden.intpublic =" + (int)enumVanbanden.intpublic.Public
                    + "	) "
                    ;

                if (!string.IsNullOrEmpty(strSearchValues))
                {
                    query += strSelect + strFrom + strFromExists + " where " + strSearchValues + strExists;
                }
                else
                {
                    query = strSelect + strFrom + strFromExists + " where " + strExists;
                }

                if (!isViewVBDenDaXL)
                {
                    query = query + " and hscv.inttrangthai=" + (int)enumHosocongviec.inttrangthai.Dangxuly;
                }
            }

            string strOrder = " order by strngayden desc, intsoden desc ";
            query += strOrder;

            return query;
        }

        #endregion RawSql



        /// <summary>
        /// lay cac y kien cua chuyen vien trong van ban
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <returns>
        /// true: co y kien
        /// false: khong y kien
        /// </returns>
        public bool GetYkienvanbanden(int? idhosocongviec)
        {
            int idcanbo = _session.GetUserId();
            try
            {
                if (idhosocongviec > 0)
                {
                    bool ykien =
                        _hsykienRepo.Hosoykienxulys
                        .Join(
                            _doituongRepo.GetCanboDangXulys
                                .Where(p => p.intidcanbo == idcanbo)
                                .Where(p => p.intidhosocongviec == idhosocongviec),
                            yk => yk.intiddoituongxuly,
                            dt => dt.intid,
                            (yk, dt) => yk
                        )
                        .Any();
                    return ykien;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// lay cac y kien cua chuyen vien trong van ban
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns>
        /// true: co y kien
        /// false: khong y kien
        /// </returns>
        public ListVanbandenViewModel GetYkienvanbanden_2(int? idvanban)
        {
            int idcanbo = _session.GetUserId();

            var ykien =
                _hosovanbanRepo.Hosovanbans
                .Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden)
                .Where(p => p.intidvanban == idvanban)
                .Join(
                    _hosocongviecRepo.Hosocongviecs,
                    vb => vb.intidhosocongviec,
                    cv => cv.intid,
                    (vb, cv) => new { vb, cv }
                )
                .GroupJoin(
                    _doituongRepo.GetCanboDangXulys
                    .Join(
                        _hsykienRepo.Hosoykienxulys,
                        dt => dt.intid,
                        yk => yk.intiddoituongxuly,
                        (dt, yk) => new { dt, yk }
                    )
                    ,
                    vb2 => vb2.vb.intidhosocongviec,
                    dt => dt.dt.intidhosocongviec,
                    (vb2, dt) => new { vb2, dt }
                )
                //.Join(
                //    _hsykienRepo.Hosoykienxulys,
                //    vb3 => vb3.intid,
                //    yk => yk.intiddoituongxuly,
                //    (vb3, yk) => new { vb3, yk }
                //)
                .Select(p => new ListVanbandenViewModel
                {
                    intid = p.vb2.vb.intidvanban,
                    intidhoso = p.vb2.vb.intidhosocongviec,
                    inttinhtrangxuly = p.vb2.cv.inttrangthai,
                    isykien = p.dt.Any()
                });

            ListVanbandenViewModel result = ykien.FirstOrDefault();

            return result;

        }


        //========================================


        public SearchVBViewModel GetViewSearch()
        {
            SearchVBViewModel model = new SearchVBViewModel();
            model.Khoiphathanh = _khoiphRepo.GetActiveKhoiphathanhs.OrderBy(p => p.strtenkhoi);
            model.Loaivanban = _plvanbanRepo.GetActivePhanloaiVanbans
                .Where(p => p.intloai == (int)enumPhanloaiVanban.intloai.vanbanden)
                .OrderBy(p => p.strtenvanban);
            model.Sovanban = _sovbRepo.GetActiveSoVanbans
                .Where(p => p.intloai == (int)enumSovanban.intloai.Vanbanden)
                .OrderBy(p => p.strten);
            model.Nguoixuly = _canboRepo.GetActiveCanbo
                .Select(p => new CanboViewModel
                {
                    strhoten = p.strhoten
                });

            return model;
        }

        /// <summary>
        /// lay idhosocongviec cua van ban dang xem
        /// de hien thi nut Xu ly
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns>idhosocongviec</returns>
        public int GetIdHosocongviec(int idvanban)
        {
            int idhosocongviec = 0;
            // neu co ho so
            var hs = _hosovanbanRepo.Hosovanbans.Where(p => p.intidvanban == idvanban)
                    .Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden);
            if (hs.Count() != 0)
            {
                int idhoso = hs.FirstOrDefault().intidhosocongviec;
                // kiem tra xem user dang xem co duoc phan xu ly ho so nay khong
                // hay la dang co quyen xem tat ca van ban den

                int idcanbo = _session.GetUserId();
                var dt = _doituongRepo.GetAllCanboxulys
                        .Where(p => p.intidhosocongviec == idhoso)
                        .Where(p => p.intidcanbo == idcanbo)
                        .Select(p => p.intidcanbo);

                if (dt.Count() != 0) { idhosocongviec = idhoso; }
            }
            return idhosocongviec;
        }

        /// <summary>
        /// lay idhosocongviec cua van ban dang chon
        /// de hien thi menu phai
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        public int GetIdHosoCV(int idvanban)
        {
            int idhosocongviec = 0;
            // neu co ho so
            var hs = _hosovanbanRepo.Hosovanbans.Where(p => p.intidvanban == idvanban)
                    .Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden)
                    .FirstOrDefault();
            if (hs != null)
            {
                idhosocongviec = hs.intidhosocongviec;
            }
            return idhosocongviec;
        }


        #endregion Listvanban

        #region Listvanbanlienquan

        public IEnumerable<ListVanbandenlienquanViewModel> GetListVanbandenlienquan__
            (int idhoso,
            string strngaydencat, int? idloaivb,
            int? idkhoiph, int? idsovb, string xuly,
            int? intsodenbd, int? intsodenkt, string strngaydenbd, string strngaydenkt,
            string strngaykybd, string strngaykykt, string strsokyhieu, string strnguoiky,
            string strnoigui, string strtrichyeu, string strnguoixuly
            )
        {
            var vanban = _GetVanbandenFromRequest
                ((int)EnumSession.SearchType.SearchVBDenLQ,
                strngaydencat, idloaivb,
                idkhoiph, idsovb, xuly,
                intsodenbd, intsodenkt, strngaydenbd, strngaydenkt,
                strngaykybd, strngaykykt, strsokyhieu, strnguoiky,
                strnoigui, strtrichyeu, strnguoixuly
                );
            //====================================================
            // chon cac truong tra ve view list van ban
            //====================================================
            //bool isViewVBDenDaXL = _configRepo.GetConfigToBool(ThamsoHethong.IsViewVBDenDaXL);

            //int _searchType = Convert.ToInt32(_session.GetObject(AppConts.SessionSearchType));
            //if (_searchType == (int)EnumSession.SearchType.SearchVBDenLQ)
            //{
            //    isViewVBDenDaXL = true;
            //}
            var listvb = _GetListViewVanbanlienquan(vanban, idhoso);

            return listvb;
        }

        /// <summary>
        /// tra ve list view van ban den
        /// </summary>
        /// <param name="vanban"></param>
        /// <returns></returns>
        private IEnumerable<ListVanbandenlienquanViewModel> _GetListViewVanbanlienquan
            (IQueryable<Vanbanden> vanban, int idhoso)
        {
            int idcanbo = _session.GetUserId();

            var listvb = vanban
                .GroupJoin(
                    _fileRepo.AttachVanbans
                        .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                        .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbanden),
                    v => 1,
                    f => 1,
                    (v, f) => new { v, f }
                )
                .GroupJoin(
                    _hosovanbanRepo.Hosovanbans.Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden),
                    v2 => 1,
                    hsvb => 1,
                    (v2, hsvb) => new { v2, hsvb }
                )
                .GroupJoin(
                    _hosocongviecRepo.Hosocongviecs,
                    v3 => 1,
                    hscv => 1,
                    (v3, hscv) => new { v3, hscv }
                )
                .GroupJoin(
                    _hsvblqRepo.Hosovanbanlienquans
                        .Where(p => p.intidhosocongviec == idhoso)
                        .Where(p => p.intloai == (int)enumHosovanbanlienquan.intloai.Vanbanden),
                    v4 => 1,
                    vblq => 1,
                    (v4, vblq) => new { v4, vblq }
                )

                .Select(p => new ListVanbandenlienquanViewModel
                {
                    intid = p.v4.v3.v2.v.intid,
                    intsoden = p.v4.v3.v2.v.intsoden,
                    dtengayden = p.v4.v3.v2.v.strngayden,
                    strkyhieu = p.v4.v3.v2.v.strkyhieu,
                    strnoinhan = p.v4.v3.v2.v.strnoinhan,
                    strnoiphathanh = p.v4.v3.v2.v.strnoiphathanh,
                    strtrichyeu = p.v4.v3.v2.v.strtrichyeu,
                    inttrangthai = p.v4.v3.v2.v.inttrangthai,

                    IsVbdt = p.v4.v3.v2.v.intdangvb == (int)enumVanbanden.intvbdt.VBDT ? true : false,

                    IsAttach = p.v4.v3.v2.f.Any(a => a.intidvanban == p.v4.v3.v2.v.intid),

                    inttinhtrangxuly = p.v4.hscv.FirstOrDefault(a =>
                            a.intid == p.v4.v3.hsvb
                                .FirstOrDefault(b => b.intidvanban == p.v4.v3.v2.v.intid)
                                .intidhosocongviec).inttrangthai,

                    isykien = false, // khong xet y kien o van ban lien quan

                    isCheck = p.vblq.Any(a => a.intidvanban == p.v4.v3.v2.v.intid)

                })
                //.Distinct() 
                // dung distinct o day chay lau hon
                ;
            return listvb;

        }

        #region RawSql

        public IEnumerable<ListVanbandenlienquanViewModel> GetListVanbandenlienquan
            (int idhoso,
            string strngaydencat, int? idloaivb,
            int? idkhoiph, int? idsovb, string xuly,
            int? intsodenbd, int? intsodenkt, string strngaydenbd, string strngaydenkt,
            string strngaykybd, string strngaykykt, string strsokyhieu, string strnguoiky,
            string strnoigui, string strtrichyeu, string strnguoixuly
            )
        {
            string strSearchValues = _SqlSearchVBDen
                (strngaydencat, idloaivb,
                idkhoiph, idsovb, xuly,
                intsodenbd, intsodenkt, strngaydenbd, strngaydenkt,
                strngaykybd, strngaykykt, strsokyhieu, strnguoiky,
                strnoigui, strtrichyeu, strnguoixuly, null
                );


            //====================================================
            // kiem tra nhung van ban user duoc quyen xem/xuly
            //====================================================
            bool isViewAllvb = _role.IsRole(RoleVanbanden.XemtatcaVBLQ);

            string query = _SqlGetAllVBdenLienquan(isViewAllvb, strSearchValues, idhoso);
            //string strSelect = _SqlListView();
            //string strOrder = " order by strngayden desc, intsoden desc ";
            //string query = string.Empty;
            //query += strSelect + " from ( " + AllVBDen + " ) as P ";
            //if (!string.IsNullOrEmpty(strSearchValues))
            //{
            //    query += " where " + strSearchValues;
            //}
            //query += strOrder;

            IEnumerable<ListVanbandenlienquanViewModel> listvb = (IEnumerable<ListVanbandenlienquanViewModel>)_vanbandenRepo.RunSqlListVBDenLienquan(query);

            return listvb;

        }

        private string _SqlSearchVBDenLienquan(
            string strngaydencat, int? idloaivb,
            int? idkhoiph, int? idsovb, string xuly,
            int? intsodenbd, int? intsodenkt, string strngaydenbd, string strngaydenkt,
            string strngaykybd, string strngaykykt, string strsokyhieu, string strnguoiky,
            string strnoigui, string strtrichyeu, string strnguoixuly
            )
        {
            string strWhere = string.Empty;
            string query = string.Empty;

            bool isSearch = false;
            bool isCategory = false;
            string strSearchValues = string.Empty;
            // strSearchValues = "intsodenbd=1;intsodenkt=10;idloaivb=2;"
            //===========================================================
            // kiem tra cac gia tri string search

            //====================================================
            // tuy chon category 
            //====================================================
            if (!string.IsNullOrEmpty(strngaydencat))
            {
                strngaydencat = ValidateData.CheckInput(strngaydencat);

                DateTime? dtengayden = DateServices.FormatDateEn(strngaydencat);
                query = " strngayden='" + DateServices.FormatDateEn(dtengayden) + "' ";
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                //isCategory = true;
                strSearchValues += "strngaydencat=" + strngaydencat + ";";
            }
            if ((idloaivb != null) && (idloaivb != 0))
            {
                query = " intidphanloaivanbanden=" + idloaivb;
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                //isCategory = true;
                strSearchValues += "idloaivb=" + idloaivb.ToString() + ";";
            }
            if ((idsovb != null) && (idsovb != 0))
            {
                query = " intidsovanban=" + idsovb;
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                //isCategory = true;
                strSearchValues += "idsovb=" + idsovb.ToString() + ";";
            }
            if ((idkhoiph != null) && (idkhoiph != 0))
            {
                query = " intidkhoiphathanh=" + idkhoiph;
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                isCategory = true;
                strSearchValues += "idkhoiph=" + idkhoiph.ToString() + ";";
            }
            // tinh trang xu ly
            if (!string.IsNullOrEmpty(xuly))
            {
                xuly = ValidateData.CheckInput(xuly);
                switch (xuly)
                {
                    case "chuaduyet":
                        query = " inttrangthai=" + (int)enumVanbanden.inttrangthai.Chuaduyet;
                        break;
                    case "daxuly":
                        query = " inttinhtrangxuly=" + (int)enumHosocongviec.inttrangthai.Dahoanthanh;
                        break;
                    case "xulychinh":
                        int idcanbo = _session.GetUserId();
                        string strhoten = _canboRepo.GetAllCanbo.FirstOrDefault(p => p.intid == idcanbo).strhoten;
                        query = " inttinhtrangxuly=" + (int)enumHosocongviec.inttrangthai.Dangxuly
                            + " and strnoinhan like N'%" + strhoten + "%' ";
                        break;
                    case "phoihopxl":
                        idcanbo = _session.GetUserId();
                        strhoten = _canboRepo.GetAllCanbo.FirstOrDefault(p => p.intid == idcanbo).strhoten;
                        query = " inttinhtrangxuly=" + (int)enumHosocongviec.inttrangthai.Dangxuly
                            + " and strnoinhan not like N'%" + strhoten + "%' ";
                        break;
                }
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                isCategory = true;
                strSearchValues += "xuly=" + xuly + ";";
            }

            //====================================================
            // Search van ban
            //====================================================
            if ((intsodenkt != null) && (intsodenkt != 0))
            {
                if ((intsodenbd != null) && (intsodenbd != 0))
                {
                    query = " vanbanden.intsoden>=" + intsodenbd + " and vanbanden.intsoden<=" + intsodenkt;
                    if (string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += query;
                    }
                    else
                    {
                        strWhere += " and " + query;
                    }
                    isSearch = true;
                    strSearchValues += "intsodenbd=" + intsodenbd.ToString() + ";intsodenkt=" + intsodenkt.ToString() + ";";
                }
            }
            else
            {
                if ((intsodenbd != null) && (intsodenbd != 0))
                {
                    query = " vanbanden.intsoden=" + intsodenbd;
                    if (string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += query;
                    }
                    else
                    {
                        strWhere += " and " + query;
                    }
                    isSearch = true;
                    strSearchValues += "intsodenbd=" + intsodenbd.ToString() + ";";
                }
            }

            if (!string.IsNullOrEmpty(strngaydenkt))
            {
                strngaydenkt = ValidateData.CheckInput(strngaydenkt);
                if (!string.IsNullOrEmpty(strngaydenbd))
                {
                    strngaydenbd = ValidateData.CheckInput(strngaydenbd);
                    DateTime? dtngaydenbd = DateServices.FormatDateEn(strngaydenbd);
                    DateTime? dtngaydenkt = DateServices.FormatDateEn(strngaydenkt);
                    query = " strngayden>='" + DateServices.FormatDateEn(dtngaydenbd) + "' and strngayden<='" + DateServices.FormatDateEn(dtngaydenkt) + "' ";
                    if (string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += query;
                    }
                    else
                    {
                        strWhere += " and " + query;
                    }
                    isSearch = true;
                    strSearchValues += "strngaydenbd=" + strngaydenbd + ";strngaydenkt=" + strngaydenkt + ";";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(strngaydenbd))
                {
                    strngaydenbd = ValidateData.CheckInput(strngaydenbd);
                    DateTime? dtngaydenbd = DateServices.FormatDateEn(strngaydenbd);
                    query = " strngayden='" + DateServices.FormatDateEn(dtngaydenbd) + "' ";
                    if (string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += query;
                    }
                    else
                    {
                        strWhere += " and " + query;
                    }
                    isSearch = true;
                    strSearchValues += "strngaydenbd=" + strngaydenbd + ";";
                }
            }

            if (!string.IsNullOrEmpty(strngaykykt))
            {
                if (!string.IsNullOrEmpty(strngaykybd))
                {
                    strngaykybd = ValidateData.CheckInput(strngaykybd);
                    strngaykykt = ValidateData.CheckInput(strngaykykt);

                    DateTime? dtngaykybd = DateServices.FormatDateEn(strngaykybd);
                    DateTime? dtngaykykt = DateServices.FormatDateEn(strngaykykt);
                    //vanban = vanban.Where(p => p.strngayky >= dtngaykybd)
                    //        .Where(p => p.strngayky <= dtngaykykt);
                    query = " strngayky>='" + DateServices.FormatDateEn(dtngaykybd) + "' and strngayky<='" + DateServices.FormatDateEn(dtngaykykt) + "' ";
                    if (string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += query;
                    }
                    else
                    {
                        strWhere += " and " + query;
                    }
                    isSearch = true;
                    strSearchValues += "strngaykybd=" + strngaykybd + ";strngaykykt=" + strngaykykt + ";";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(strngaykybd))
                {
                    strngaykybd = ValidateData.CheckInput(strngaykybd);
                    DateTime? dtngaykybd = DateServices.FormatDateEn(strngaykybd);
                    //vanban = vanban.Where(p => p.strngayky == dtngaykybd);
                    query = " strngayky='" + DateServices.FormatDateEn(dtngaykybd) + "' ";
                    if (string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += query;
                    }
                    else
                    {
                        strWhere += " and " + query;
                    }
                    isSearch = true;
                    strSearchValues += "strngaykybd=" + strngaykybd + ";";
                }
            }

            if (!string.IsNullOrEmpty(strsokyhieu))
            {
                strsokyhieu = ValidateData.CheckInput(strsokyhieu);
                // neu la so thi tim =
                // neu la chu thi tim like
                strsokyhieu = strsokyhieu.Trim();
                Dictionary<bool, string> result = ValidateData.SearchExactly(strsokyhieu);
                if (result.ContainsKey(true))
                {   // co tim kiem chinh xac "abc"
                    query = " strkyhieu = N'" + result[true] + "' ";
                }
                else
                {
                    query = " strkyhieu like N'%" + strsokyhieu + "%' ";
                }
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                strSearchValues += "strsokyhieu=" + strsokyhieu + ";";
            }

            if (!string.IsNullOrEmpty(strtrichyeu))
            {
                strtrichyeu = ValidateData.CheckInput(strtrichyeu);
                strtrichyeu = strtrichyeu.Trim();
                bool isfulltext = AppSettings.IsFullText;
                if (isfulltext)
                {   // co su dung fulltext search
                    Dictionary<bool, string> result = ValidateData.SearchExactly(strtrichyeu);
                    string searchValues = string.Empty;
                    if (result.ContainsKey(true))
                    {   // co tim kiem chinh xac "abc"                    
                        searchValues = ValidateData.GhepChuoiFullTextSearch(result[true], (int)ValidateData.enumFullTextSearch.AND);
                    }
                    else
                    {
                        //query = " freetext(strtrichyeu,'" + strtrichyeu + "' ) ";
                        searchValues = ValidateData.GhepChuoiFullTextSearch(strtrichyeu, (int)ValidateData.enumFullTextSearch.OR);
                    }
                    query = " contains(strtrichyeu,'" + searchValues + "') ";
                }
                else
                {   // khong co fulltext search
                    query = " strtrichyeu like N'%" + strtrichyeu + "%' ";
                }

                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                strSearchValues += "strtrichyeu=" + strtrichyeu + ";";
            }

            if (!string.IsNullOrEmpty(strnguoixuly))
            {
                strnguoixuly = ValidateData.CheckInput(strnguoixuly);
                //vanban = vanban.Where(p => p.strnoinhan.Contains(strnguoixuly));
                query = " strnoinhan like N'%" + strnguoixuly + "%' ";
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                strSearchValues += "strnguoixuly=" + strnguoixuly + ";";
            }

            if (!string.IsNullOrEmpty(strnguoiky))
            {
                strnguoiky = ValidateData.CheckInput(strnguoiky);
                //vanban = vanban.Where(p => p.strnguoiky.Contains(strnguoiky));
                query = " strnguoiky like N'%" + strnguoiky + "%' ";
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                strSearchValues += "strnguoiky=" + strnguoiky + ";";
            }

            if (!string.IsNullOrEmpty(strnoigui))
            {
                strnoigui = ValidateData.CheckInput(strnoigui);
                //vanban = vanban.Where(p => p.strnoiphathanh.Contains(strnoigui));
                query = " strnoiphathanh like N'%" + strnoigui + "%' ";
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                strSearchValues += "strnoigui=" + strnoigui + ";";
            }


            //========================================================
            // end search
            //========================================================


            if (!isSearch)
            {   // khong phai la search thi gioi han ngay hien thi                
                int intngay = _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);
                //vanban = vanban.Where(p => System.Data.Entity.DbFunctions.DiffDays(p.strngayden, DateTime.Now) < intngay);

                DateTime? dtengaybd = DateTime.Now.AddDays(-intngay);
                //vanban = vanban.Where(p => p.strngayden >= dtengaybd);

                string strngaybd = DateServices.FormatDateEn(dtengaybd);
                query = " strngayden >='" + strngaybd + "' ";
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }

                // reset session
                _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.NoSearch);
            }
            else
            {   // luu cac gia tri search vao session
                _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.SearchVBDenLQ);
                _session.InsertObject(AppConts.SessionSearchTypeValues, strSearchValues);

                // tim kiem thi hien thi tat ca

                // Category thi gioi han ngay hien thi
                if (isCategory)
                {
                    int intngay = _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);
                    DateTime? dtengaybd = DateTime.Now.AddDays(-intngay);
                    string strngaybd = DateServices.FormatDateEn(dtengaybd);
                    query = " strngayden >='" + strngaybd + "' ";
                    if (string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += query;
                    }
                    else
                    {
                        strWhere += " and " + query;
                    }
                    //vanban = vanban.Where(p => p.strngayden >= dtengaybd);
                }
            }

            return strWhere;
        }

        /// <summary>
        /// tra ve tat ca cac van ban den duoc quyen xem
        /// </summary>        
        /// <param name="isViewAllVBDen"></param>
        /// <returns></returns>
        private string _SqlGetAllVBdenLienquan(bool isViewAllVBDen, string strSearchValues, int idhosocongviec)
        {
            string strSelect = "Select distinct vanbanden.intid,strKyhieu,strNgayden as dtengayden ,vanbanden.intSoden,strNoiphathanh "
            + ",strTrichyeu,vanbanden.inttrangthai ,intidphanloaivanbanden,strnoinhan,hsvb.intidhosocongviec as intidhoso "
            + ", case when ( intdangvb = 2) then cast(1 as bit) else cast(0 as bit) end as IsVbdt "
            + " ,case when( (select attachvanban.intidvanban ) is not null)then cast(1 as bit) ELSE cast(0 as bit) END as IsAttach "
            + ",(select top 1 hscv.inttrangthai ) as inttinhtrangxuly "

            + ",cast( 0 as bit ) as isykien "
            + " ,case when( (select vblq.intidvanban ) is not null)then cast(1 as bit) ELSE cast(0 as bit) END as isCheck "

            + " From vanbanden "
            + " left outer join attachvanban on vanbanden.intid = attachvanban.intidvanban "
            + " and attachvanban.intloai=" + (int)enumAttachVanban.intloai.Vanbanden
            + " and attachvanban.inttrangthai=" + (int)enumAttachVanban.inttrangthai.IsActive
            + " left outer join hosovanban hsvb on hsvb.intidvanban = vanbanden.intid and hsvb.intloai=" + (int)enumHosovanban.intloai.Vanbanden
            + " left outer join hosocongviec hscv on hscv.intid=hsvb.intidhosocongviec and hscv.intloai=" + (int)enumHosocongviec.intloai.Vanbanden

            + " left outer join hosovanbanlienquan vblq on vblq.intidvanban=vanbanden.intid  and vblq.intidhosocongviec = " + idhosocongviec.ToString() + " and vblq.intloai=" + (int)enumHosovanban.intloai.Vanbanden
            ;

            string query = string.Empty;
            if (isViewAllVBDen)
            {
                // xem tat ca van ban den
                if (!string.IsNullOrEmpty(strSearchValues))
                {
                    query += strSelect + " where " + strSearchValues;
                }
                else
                {
                    query = strSelect;
                }
            }
            else
            {   // chi duoc xem nhung van ban den thuoc quyen xu ly/xem
                string stridcanbo = _session.GetUserId().ToString();
                string strExists =
                    " and  ( "
                        + " Exists (Select distinct intidvanban as intidvanban from vanbandencanbo "
                                    + " where vanbandencanbo.intidvanban=vanbanden.intID and intidcanbo=" + stridcanbo + " ) "
                        + " or Exists "
                                + " (select distinct Hosovanban.intidvanban from Hosovanban "
                                + " inner join Doituongxuly on Doituongxuly.intidhosocongviec=Hosovanban.intidhosocongviec "
                                + " where intloai=" + (int)enumHosovanban.intloai.Vanbanden
                                + " and Doituongxuly.intidcanbo=" + stridcanbo
                                + " and Hosovanban.intidvanban=vanbanden.intID) "
                    //+ " or vanbanden.intpublic =" + (int)enumVanbanden.intpublic.Public // chay rat cham

                    + " )  "
                    + " and vanbanden.intpublic=" + (int)enumVanbanden.intpublic.Private
                    ;
                string vbpublic = " and vanbanden.intpublic=" + (int)enumVanbanden.intpublic.Public;

                if (!string.IsNullOrEmpty(strSearchValues))
                {
                    query += strSelect + " where " + strSearchValues + strExists;
                    query = query + " union " + strSelect + " where " + strSearchValues + vbpublic;
                }
                else
                {
                    query = strSelect + " where " + strExists;
                    query = query + " union " + strSelect + " where " + vbpublic;
                }
            }

            string strOrder = " order by strngayden desc, intsoden desc ";
            query += strOrder;

            return query;
        }


        #endregion RawSql


        public int SaveVBDenLienquan(List<int> listidvanban, int idhosocongviec)
        {
            try
            {
                // kiem tra cac van ban lien quan da co trong ho so
                var listvblq = _hsvblqRepo.Hosovanbanlienquans
                    .Where(p => p.intidhosocongviec == idhosocongviec)
                    .Where(p => p.intloai == (int)enumHosovanbanlienquan.intloai.Vanbanden)
                    .Where(p => listidvanban.Contains(p.intidvanban))
                    .Select(p => p.intidvanban);
                if (listvblq.Count() > 0)
                {   // neu co van ban trong hoso lien quan thi xoa khoi list idvanban
                    foreach (int p in listvblq)
                    {
                        listidvanban.Remove(p);
                    }
                }

                foreach (int id in listidvanban)
                {
                    Hosovanbanlienquan vblq = new Hosovanbanlienquan();
                    vblq.intidhosocongviec = idhosocongviec;
                    vblq.intidvanban = id;
                    vblq.intloai = (int)enumHosovanbanlienquan.intloai.Vanbanden;
                    vblq.inttrangthai = (int)enumHosovanbanlienquan.inttrangthai.Dahoanthanh;
                    _hsvblqRepo.Them(vblq);

                    // ghi nhat ky

                }
                return (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return (int)ResultViewModels.Error;
            }

        }


        /// <summary>
        /// lay cac van ban den lien quan den id hoso 
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        public IEnumerable<ListVanbandenViewModel> GetHosoVBDenLQ(int idhoso)
        {
            var vanban = _vanbandenRepo.Vanbandens
                .Join(
                    _hsvblqRepo.Hosovanbanlienquans
                        .Where(p => p.intidhosocongviec == idhoso)
                        .Where(p => p.intloai == (int)enumHosovanbanlienquan.intloai.Vanbanden),
                    v => v.intid,
                    vblq => vblq.intidvanban,
                    (v, vblq) => v
                )
                .GroupJoin(
                    _fileRepo.AttachVanbans
                        .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                        .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbanden),
                    v => 1,
                    f => 1,
                    (v, f) => new { v, f }
                )
                .Select(p => new ListVanbandenViewModel
                {
                    intid = p.v.intid,
                    intsoden = p.v.intsoden,
                    dtengayden = p.v.strngayden,
                    strkyhieu = p.v.strkyhieu,
                    strnoinhan = p.v.strnoinhan,
                    strnoiphathanh = p.v.strnoiphathanh,
                    strtrichyeu = p.v.strtrichyeu,
                    inttrangthai = p.v.inttrangthai,
                    IsVbdt = p.v.intdangvb == (int)enumVanbanden.intvbdt.VBDT ? true : false,
                    IsAttach = p.f.Any(a => a.intidvanban == p.v.intid),

                    inttinhtrangxuly = (int)enumHosocongviec.inttrangthai.Dahoanthanh,
                    isykien = false

                });

            return vanban;
        }



        #endregion Listvanbanlienquan

        #region ViewDetail

        public DetailVBDenViewModel GetViewDetail(int id)
        {
            int idcanbo = _session.GetUserId();
            //bool isView = RoleViewVanbanServices.IsViewVanbanden(idvanban, idcanbo);
            bool isView = _role.IsViewVanbanden(id, idcanbo);
            if (isView == false)
            {
                _logger.Warn("không có quyền xem văn bản: " + id.ToString());
                return new DetailVBDenViewModel();
            }


            var cv = _vanbandenRepo.Vanbandens.FirstOrDefault(p => p.intid == id);

            var hsvb = _hosovanbanRepo.Hosovanbans
                        .Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden)
                        .Where(p => p.intidvanban == id).ToList();
            int idhosocongviec = 0;
            string strtieude = "";
            if (hsvb.Count() != 0)
            {
                idhosocongviec = hsvb.FirstOrDefault().intidhosocongviec;
                strtieude = _hosocongviecRepo.Hosocongviecs
                            .FirstOrDefault(p => p.intid == idhosocongviec)
                            .strtieude;
            }

            string strvbmat = string.Empty;
            string strvbkhan = string.Empty;
            try
            {
                if ((cv.intidmat != null) && (cv.intidmat != 0))
                {
                    strvbmat = _tinhchatvbRepo.GetAllTinhchatvanbans.Where(p => p.intloai == (int)enumTinhchatvanban.intloai.Mat)
                            .FirstOrDefault(p => p.intid == cv.intidmat).strtentinhchatvb;
                }

                if ((cv.intidkhan != null) && (cv.intidkhan != 0))
                {
                    strvbkhan = _tinhchatvbRepo.GetAllTinhchatvanbans.Where(p => p.intloai == (int)enumTinhchatvanban.intloai.Khan)
                                    .FirstOrDefault(p => p.intid == cv.intidkhan).strtentinhchatvb;
                }
            }
            catch { }

            string strvanbandi = string.Empty;
            int idvanbandi = 0;
            string strvanbanphathanh = string.Empty;
            int idvanbanphathanh = 0;

            var hoibao = _hoibaovanbanRepo.Hoibaovanbans
                            .Where(p => p.intRecID == cv.intid)
                //.Where(p => p.intloai == (int)enumHoibaovanban.intloai.Vanbanden)
                //.FirstOrDefault();
                            .ToList();
            if (hoibao.Count > 0)
            {
                foreach (var hb in hoibao)
                {
                    if (hb.intloai == (int)enumHoibaovanban.intloai.Vanbanden)
                    {
                        idvanbandi = hb.intTransID;
                        var vbdi = _vanbandiRepo.Vanbandis.Where(p => p.intid == idvanbandi).FirstOrDefault();
                        strvanbandi = vbdi.intso.ToString() + "/" + vbdi.strkyhieu;
                    }
                    if (hb.intloai == (int)enumHoibaovanban.intloai.Vanbandi)
                    {
                        idvanbanphathanh = hb.intTransID;
                        var vbdi = _vanbandiRepo.Vanbandis.Where(p => p.intid == idvanbanphathanh).FirstOrDefault();
                        strvanbanphathanh = vbdi.intso.ToString() + "/" + vbdi.strkyhieu;
                    }
                }
            }

            IEnumerable<DownloadFileViewModel> downloadFiles = _fileRepo.AttachVanbans
                        .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbanden)
                        .Where(p => p.intidvanban == id)
                        .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                        .Select(p => new DownloadFileViewModel
                        {
                            intid = p.intid,
                            strtenfile = p.strmota
                            //intloai = (int)p.intloai
                        }).ToList();
            foreach (var f in downloadFiles)
            {
                f.fileExt = _fileManager.GetFileExtention(f.strtenfile);
                f.strfiletypeimages = _fileManager.GetFileTypeImages(f.strtenfile);
                //f.fileIcon = _fileManager.getfi
                f.intloai = (int)enumDownloadFileViewModel.intloai.Vanbanden;
            }
            //========================================
            var vanban = new DetailVBDenViewModel();

            //vanban = cv,
            vanban.intid = cv.intid;
            vanban.strngayden = DateServices.FormatDateVN(cv.strngayden);
            vanban.intsoden = (int)cv.intsoden;
            vanban.strkyhieu = cv.strkyhieu;

            try
            {
                vanban.strsovanban = _sovbRepo.GetAllSoVanbans.Where(p => p.intloai == (int)enumSovanban.intloai.Vanbanden)
                        .FirstOrDefault(p => p.intid == cv.intidsovanban).strten;
            }
            catch { }

            try
            {
                vanban.strloaivanban = _plvanbanRepo.GetAllPhanloaiVanbans.Where(p => p.intloai == (int)enumPhanloaiVanban.intloai.vanbanden)
                        .FirstOrDefault(p => p.intid == cv.intidphanloaivanbanden).strtenvanban;
            }
            catch { }

            try
            {
                vanban.strtenkhoiphathanh = _khoiphRepo.GetAllKhoiphathanhs.FirstOrDefault(p => p.intid == cv.intidkhoiphathanh).strtenkhoi;
            }
            catch { }
            vanban.strtencoquanphathanh = cv.strnoiphathanh;
            vanban.strtrichyeu = cv.strtrichyeu;
            vanban.strngayky = DateServices.FormatDateVN(cv.strngayky);
            vanban.strnguoiky = cv.strnguoiky;

            vanban.strvbmat = strvbmat;
            vanban.strvbkhan = strvbkhan;

            try
            {
                vanban.strnguoixulybandau = _canboRepo.GetAllCanbo.FirstOrDefault(p => p.intid == cv.intidnguoiduyet).strhoten;
            }
            catch { }

            vanban.strnguoixulychinh = cv.strnoinhan;
            vanban.strhantraloi = DateServices.FormatDateVN(cv.strhanxuly);
            vanban.strtraloivanban = cv.strtraloivanbanso;

            // vanban hoibao cua van ban den
            vanban.idvanbandi = idvanbandi;
            vanban.strvanbandi = strvanbandi;
            // vanban phat hanh cua vanban den
            vanban.idvanbanphathanh = idvanbanphathanh;
            vanban.strvanbanphathanh = strvanbanphathanh;

            vanban.intidhosocongviec = idhosocongviec;
            vanban.strhosovanban = strtieude;

            vanban.isattach = _fileRepo.AttachVanbans
                        .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                        .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbanden)
                        .Where(p => p.intidvanban == id)
                        .Any();

            vanban.DownloadFiles = downloadFiles;

            return vanban;
        }

        #endregion ViewDetail

        #region DuyetVanban

        public ResultFunction DuyetVanban(int idvanban, int intduyet)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                _vanbandenRepo.Duyet(idvanban, intduyet);
                kq.id = (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
                kq.message = "Lỗi ! Không duyệt/hủy duyệt văn bản được";
            }
            return kq;
        }

        #endregion DuyetVanban

        #region UpdateVanban

        /// <summary>
        /// load du lieu cua form them moi van ban
        /// </summary>
        /// <param name="idloaivb"></param>
        /// <param name="idsovb"></param>
        /// <param name="idvanban"></param>
        /// <param name="idmail"></param>
        /// <returns></returns>
        public ThemVanbanViewModel GetLoaitruong(int? idloaivb, int? idsovb,
            int? idvanban, int? idmail)
        {
            ThemVanbanViewModel vanban = new ThemVanbanViewModel();


            if (idmail > 0)
            {
                vanban.idmail = idmail;
                // cap nhat van ban dien tu
                if (idvanban > 0)
                {
                    var vb = _vanbandenRepo.Vanbandens
                            .FirstOrDefault(p => p.intid == idvanban);
                    vanban.Vanbanden = vb;
                }
                else
                {
                    var vbmail = _vbmailRepo.Vanbandenmails
                    .FirstOrDefault(p => p.intid == idmail);

                    vanban.Vanbanden = new Vanbanden();
                    vanban.Vanbanden.strngayden = DateTime.Now;
                    vanban.IsSave = false;

                    vanban.Vanbanden.strngayky = vbmail.strngayky;
                    //vanban.Vanbanden.intsoden = vbmail.intso;
                    try
                    {
                        if (vbmail.intso > 0)
                        {
                            vanban.Vanbanden.strkyhieu = vbmail.intso.ToString() + "/" + vbmail.strkyhieu;
                        }
                        else
                        {
                            vanban.Vanbanden.strkyhieu = vbmail.strkyhieu;
                        }                        
                    }
                    catch { }
                    vanban.Vanbanden.strtrichyeu = vbmail.strtrichyeu;
                    vanban.Vanbanden.strnguoiky = vbmail.strnguoiky;
                    vanban.Vanbanden.strnoiphathanh = vbmail.strnoiguivb;
                    vanban.Vanbanden.intidvanbandenmail = idmail;
                }

            }
            else
            {
                vanban.idmail = 0;
                if ((idvanban == 0) || (idvanban == null))
                {   // them moi van ban
                    vanban.Vanbanden = new Vanbanden();
                    vanban.Vanbanden.strngayden = DateTime.Now;
                    vanban.IsSave = false;
                }
                else
                {  // cap nhat van ban
                    var vb = _vanbandenRepo.Vanbandens
                            .FirstOrDefault(p => p.intid == idvanban);
                    vanban.Vanbanden = vb;

                    //  chua thay doi loai van ban thi chon loai van ban cu
                    if ((idloaivb == 0) || (idloaivb == null))
                    {
                        idloaivb = vb.intidphanloaivanbanden;
                    }

                    if ((idsovb == 0) || (idsovb == null))
                    {
                        idsovb = vb.intidsovanban;
                    }
                }

            }

            //int? idsovb = vb.intidsovanban;
            string strquytac = AppSettings.TraloiVB;
            vanban.strmota_traloivanban = _fileNameManger.GetQuytacTenFile(strquytac);

            //=============================================================

            vanban.PhanloaiVanban = _plvanbanRepo.GetActivePhanloaiVanbans
                    .Where(p => p.intloai == (int)enumPhanloaiVanban.intloai.vanbanden)
                    .OrderBy(p => p.strtenvanban);

            var loaivb = vanban.PhanloaiVanban.FirstOrDefault(p => p.IsDefault == true);

            // kiem tra dieu kien id loai van ban
            if ((idloaivb == 0) || (idloaivb == null))
            {   // khi them moi van ban den
                if (loaivb == null)
                {   // neu loai van ban khong co gia tri default thi chon loai vb dau tien
                    vanban.intidloaivanban = vanban.PhanloaiVanban.First().intid;
                }
                else
                {
                    vanban.intidloaivanban = loaivb.intid;
                }
            }
            else
            {   // change loai vb
                vanban.intidloaivanban = (int)idloaivb;
            }

            vanban.PhanloaiTruong = _pltruongRepo.PhanloaiTruongs
                    .Where(p => p.intidphanloaivanban == vanban.intidloaivanban)
                    .Where(p => p.intloai == (int)enumPhanloaiTruong.intloai.vanbanden)
                    .Where(p => p.intloaitruong == (int)enumPhanloaiTruong.intloaitruong.Default)
                    .OrderBy(p => p.intorder);

            vanban.Sovanban = _sovbRepo.GetActiveSoVanbans
                    .Where(p => p.intloai == (int)enumSovanban.intloai.Vanbanden)
                    .OrderBy(p => p.intorder);

            var sovb = vanban.Sovanban.FirstOrDefault(p => p.IsDefault == true);
            // kiem tra dieu kien id sovb
            if ((idsovb == 0) || (idsovb == null))
            {
                // khi them moi van ban den
                if (sovb == null)
                {   // neu so van ban khong co gia tri default thi chon so vb dau tien
                    vanban.intidsovanban = vanban.Sovanban.First().intid;
                }
                else
                {
                    vanban.intidsovanban = sovb.intid;
                }
            }
            else
            {   // change so vb
                vanban.intidsovanban = (int)idsovb;
            }

            vanban.Khoiphathanh = _khoiphRepo.GetActiveKhoiphathanhs
                    .OrderBy(p => p.strtenkhoi);

            int? intidkhoiph = _sovbRepo.GetActiveSoVanbans.First(p => p.intid == vanban.intidsovanban).intidkhoiph;

            if (idvanban > 0)
            {
                // vb da co 
                vanban.intidkhoiph = (int)vanban.Vanbanden.intidkhoiphathanh;
            }
            else
            {   // van ban moi
                if ((intidkhoiph == 0) || (intidkhoiph == null))
                {   // neu so van ban khong co chon default khoi phat hanh
                    // thi chon ngau nhieu khoi ph dau tien
                    vanban.intidkhoiph = vanban.Khoiphathanh.FirstOrDefault().intid;
                }
                else
                {
                    vanban.intidkhoiph = (int)intidkhoiph;
                }
            }



            //vanban.Tochucdoitac = _tochucRepo.GetActiveTochucdoitacs
            //                            .Where(p => p.intidkhoi == vanban.intidkhoiph)
            //                            .OrderBy(p => p.strtentochucdoitac);

            vanban.Linhvuc = _linhvucRepo.GetActiveLinhvucs.OrderBy(p => p.strtenlinhvuc);

            vanban.Vanbankhan = _tinhchatvbRepo.GetActiveTinhchatvanbans
                    .Where(p => p.intloai == (int)enumTinhchatvanban.intloai.Khan)
                    .OrderBy(p => p.strtentinhchatvb);

            vanban.Vanbanmat = _tinhchatvbRepo.GetActiveTinhchatvanbans
                    .Where(p => p.intloai == (int)enumTinhchatvanban.intloai.Mat)
                    .OrderBy(p => p.strtentinhchatvb);

            // nguoi xu ly ban dau
            vanban.Nguoiduyet = _canboRepo.GetActiveCanbo
                    .Where(p => p.intnguoixuly == (int)enumcanbo.intnguoixuly.IsActive
                             || p.intnguoixuly == (int)enumcanbo.intnguoixuly.IsDefault)
                    .Select(p => new CanboViewModel
                    {
                        intid = p.intid,
                        iddonvi = p.intdonvi,
                        intchucvu = p.intchucvu,
                        IsKyVB = p.intkivb == (int)enumcanbo.intkivb.Co ? true : false,
                        intnhomquyen = p.intnhomquyen,
                        IsNguoiXL = true, //p.intnguoixuly == (int)enumcanbo.intnguoixuly.IsActive ? true : false,
                        strhoten = p.strhoten,
                        strkyhieu = p.strkyhieu,
                        strmacanbo = p.strmacanbo
                    })
                    .OrderBy(p => p.strkyhieu)
                    .ThenBy(p => p.strhoten);

            if ((vanban.Vanbanden.intidnguoiduyet == 0) || (vanban.Vanbanden.intidnguoiduyet == null))
            {
                var nguoiduyet = _canboRepo.GetActiveCanbo
                        .FirstOrDefault(p => p.intnguoixuly == (int)enumcanbo.intnguoixuly.IsDefault);
                if (nguoiduyet != null)
                {
                    vanban.Vanbanden.intidnguoiduyet = nguoiduyet.intid;
                }
                else
                {
                    vanban.Vanbanden.intidnguoiduyet = 0;
                }
            }


            vanban.Diachiluutru = _luutruRepo.GetActiveDiachiluutrus.OrderBy(p => p.strtendonvi);

            // nguoi xu ly chinh
            vanban.Nguoixuly = _canboRepo.GetActiveCanbo
                    .Select(p => new CanboViewModel
                    {
                        intid = p.intid,
                        iddonvi = p.intdonvi,
                        intchucvu = p.intchucvu,
                        IsKyVB = p.intkivb == (int)enumcanbo.intkivb.Co ? true : false,
                        intnhomquyen = p.intnhomquyen,
                        IsNguoiXL = p.intnguoixuly == (int)enumcanbo.intnguoixuly.IsActive ? true : false,
                        strhoten = p.strhoten,
                        strkyhieu = p.strkyhieu,
                        strmacanbo = p.strmacanbo
                    })
                    .OrderBy(p => p.strkyhieu)
                    .ThenBy(p => p.strhoten);

            vanban.IsQPPL = vanban.Vanbanden.intquyphamphapluat == (int)enumVanbanden.intquyphamphapluat.Co ? true : false;

            return vanban;
        }

        /// <summary>
        /// lay ten cac don vi theo khoi phat hanh
        /// </summary>
        /// <param name="idkhoiph"></param>
        /// <returns></returns>
        public IEnumerable<ListTochucdoitacViewModel> GetTenDonvi(int idkhoiph)
        {
            var tochuc = _tochucRepo.GetActiveTochucdoitacs
                    .Where(p => p.intidkhoi == idkhoiph)
                    .Select(p => new ListTochucdoitacViewModel
                    {
                        strkyhieu = p.strmatochucdoitac,
                        strmadinhdanh = p.strmadinhdanh,
                        strten = p.strtentochucdoitac,
                        strkyhieu_ten = p.strmatochucdoitac + " || " + p.strtentochucdoitac
                    })
                    .OrderBy(p => p.strten);
            return tochuc;
        }

        /// <summary>
        /// lấy các trường: max số đến và khối phát hành khi 
        /// thay đổi sổ văn bản
        /// </summary>
        /// <param name="idsovb"></param>
        /// <returns></returns>
        public AjaxSovanban GetSovanban(int idsovb)
        {
            AjaxSovanban sovb = new AjaxSovanban();

            sovb.intsoden = _GetSodenvanban(idsovb);
            //==============================================
            int intidkhoiphathanh = 0;
            int? _intidkhoiph = _sovbRepo.GetActiveSoVanbans
                                    .Where(p => p.intid == idsovb)
                                    .FirstOrDefault().intidkhoiph;
            if ((_intidkhoiph != null) && (_intidkhoiph != 0))
            {
                intidkhoiphathanh = (int)_intidkhoiph;
            }
            else
            {
                // neu so van ban khong co chon default khoi phat hanh
                // thi chon ngau nhieu khoi ph dau tien
                intidkhoiphathanh = _khoiphRepo.GetActiveKhoiphathanhs.FirstOrDefault().intid;
            }
            sovb.idkhoiph = intidkhoiphathanh;
            return sovb;

        }
        /// <summary>
        /// lấy số đến lớn nhất của sổ vb
        /// </summary>
        /// <param name="idsovb"></param>
        /// <returns></returns>
        private int _GetSodenvanban(int idsovb)
        {
            int intsovbden = 0;
            int year = DateTime.Today.Year;
            try
            {
                var _intsoden = _vanbandenRepo.Vanbandens
                            .Where(p => p.intidsovanban == idsovb)
                            .Where(p => p.strngayden.Value.Year == year)
                            .Max(p => p.intsoden);
                if ((_intsoden == 0) || (_intsoden == null))
                {
                    intsovbden = 1;
                }
                else
                {
                    intsovbden = (int)_intsoden + 1;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return intsovbden;
        }

        /// <summary>
        /// kiem tra van ban den co bi trung khong
        /// </summary>
        /// <param name="strsokyhieu"></param>
        /// <param name="strngayky"></param>
        /// <param name="strcoquan"></param>
        /// <returns>        
        /// </returns>
        public CheckVBTrungViewModel KiemtraVBtrung(string strsokyhieu, string strngayky, string strcoquan, int idmail)
        {
            CheckVBTrungViewModel kq = new CheckVBTrungViewModel
            {
                idloai = -1,
                idvanban = -1,
                message = "Không có văn bản trùng"
            };
            DateTime? dteNgayky = DateServices.FormatDateEn(strngayky);
            var vb = _vanbandenRepo.Vanbandens
                    .Where(p => p.strkyhieu == strsokyhieu)
                    .Where(p => p.strngayky == dteNgayky)
                    .Where(p => p.strnoiphathanh == strcoquan)
                    .FirstOrDefault();

            if (vb != null)
            {
                string message = string.Empty;
                string strngayden = DateServices.FormatDateVN(vb.strngayden);
                string vanban = string.Empty;
                switch (vb.intdangvb)
                {
                    case (int)enumVanbanden.intvbdt.VBGiay: //0                        
                        if (idmail > 0)
                        {   // vb dang nhap la vbdt 
                            vanban = "Số ký hiệu đã tồn tại ! Văn bản giấy đến số: " + vb.intsoden.ToString();
                            vanban = vanban + ", ngày đến: " + strngayden;
                            vanban = vanban + ". Bạn có muốn cập nhật văn bản điện tử đính kèm không?";
                            message = vanban;
                            kq.idvanban = vb.intid;
                            kq.idloai = (int)vb.intdangvb;
                        }
                        else
                        {   // vb dang nhap la vbgiay = vb da co la vbgiay
                            vanban = "Số ký hiệu đã tồn tại ! Văn bản giấy đến số: " + vb.intsoden.ToString();
                            vanban = vanban + ", ngày đến: " + strngayden;
                            message = vanban;
                            kq.idvanban = 0;
                            kq.idloai = (int)enumVanbanden.intvbdt.VBDT_Giay;
                        }
                        break;
                    case (int)enumVanbanden.intvbdt.VBDT_Giay:  // 2   
                        vanban = "Số ký hiệu đã tồn tại ! Văn bản đến số: " + vb.intsoden.ToString();
                        vanban = vanban + ", ngày đến: " + strngayden;
                        message = vanban;
                        kq.idvanban = 0;
                        kq.idloai = (int)vb.intdangvb;
                        break;
                    case (int)enumVanbanden.intvbdt.VBDT:  //1       
                        if (idmail > 0)
                        {   // vb dang nhap la vb dien tu = vb da co la vbdt: chi thong bao
                            vanban = "Số ký hiệu đã tồn tại ! Văn bản điện tử đến số: " + vb.intsoden.ToString();
                            vanban = vanban + ", ngày đến: " + strngayden;
                            message = vanban;
                            kq.idvanban = 0;
                            kq.idloai = (int)enumVanbanden.intvbdt.VBDT_Giay;
                        }
                        else
                        {   // vb dang nhap la vbgiay 
                            vanban = "Số ký hiệu đã tồn tại ! Văn bản điện tử đến số: " + vb.intsoden.ToString();
                            vanban = vanban + ", ngày đến: " + strngayden;
                            vanban = vanban + ". Bạn có muốn cập nhật thời gian xử lý không?";
                            message = vanban;
                            kq.idvanban = vb.intid;
                            kq.idloai = (int)vb.intdangvb;
                        }
                        break;
                }

                kq.message = message;
            }
            return kq;
        }

        /// <summary>
        /// cap nhat thoi han xu ly vb, tiep nhan vb giay
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        public ResultFunction CapnhatThoihanXulyVBDT(int idvanban)
        {
            ResultFunction kq = new ResultFunction();

            try
            {
                // cap nhat dang vvb
                _vanbandenRepo.CapnhatDangVanban(idvanban, (int)enumVanbanden.intvbdt.VBDT_Giay);

                int idcanbo = _session.GetUserId();
                _SaveLogChitietVanban((int)enumChitietVanbanden.intvaitro.CapnhatVBGiay, idcanbo, idvanban);

                //cap nhat thoi han xu ly (neu co)
                var hsvb = _hosovanbanRepo.Hosovanbans.Where(p => p.intidvanban == idvanban)
                    .Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden
                        || p.intloai == (int)enumHosovanban.intloai.Vanbanden_quytrinh
                    ).FirstOrDefault();

                if (hsvb != null)
                {
                    int idhoso = hsvb.intidhosocongviec;
                    int intthoihanxuly = _configRepo.GetConfigToInt(ThamsoHethong.ThoihanXLVB);
                    var strthoihanxuly = DateServices.AddThoihanxuly(DateTime.Now, intthoihanxuly);
                    _hosocongviecRepo.CapnhatThoihanxuly(idhoso, strthoihanxuly);
                    // ghi vet

                    ChitietHoso ct = new ChitietHoso();
                    ct.intidcanbo = idcanbo;
                    ct.intidhosocongviec = hsvb.intidhosocongviec;
                    ct.intnguoitao = idcanbo;
                    ct.intvaitro = (int)enumDoituongxuly.intvaitro_chitiethoso.CapnhatVBGiay;
                    _chitietHosoRepo.Them(ct);

                }
                kq.id = (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
            }

            return kq;
        }

        /// <summary>
        /// cap nhat van ban dien tu dinh kem vao vb giay da co
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="idmail"></param>
        /// <returns></returns>
        public ResultFunction CapnhatVBDTDinhkem(int idvanban, int idmail)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                _UpdateVanbanMail(idvanban, idmail);

                // cap nhat dang vvb
                _vanbandenRepo.CapnhatDangVanban(idvanban, (int)enumVanbanden.intvbdt.VBDT_Giay);

                int idcanbo = _session.GetUserId();
                _SaveLogChitietVanban((int)enumChitietVanbanden.intvaitro.CapnhatVBDT, idcanbo, idvanban);

                kq.id = (int)ResultViewModels.Success;
                return kq;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return kq;
            }
        }
        /// <summary>
        /// save van ban moi
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// 0: Khong save duoc
        /// !=0: id van ban sau khi save
        /// </returns>
        public int Savevanban(ThemVanbanViewModel model)
        {
            int intidvanban = 0;
            try
            {
                Vanbanden vanban = model.Vanbanden;

                // thong tin mac dinh khi them moi van ban
                vanban.inttrangthai = (int)enumVanbanden.inttrangthai.Chuaduyet;
                vanban.intidnguoitao = _session.GetUserId();
                vanban.strngaytao = DateTime.Now;

                if ((model.idmail != 0) && (model.idmail != null))
                {
                    vanban.intdangvb = (int)enumVanbanden.intvbdt.VBDT;
                }
                else
                {
                    vanban.intdangvb = (int)enumVanbanden.intvbdt.VBGiay;
                }
                // kiểm tra số đến có trùng không (TH 2 người nhập cùng 1 lúc)
                int intsoden = _GetSodenvanban((int)model.Vanbanden.intidsovanban);
                vanban.intsoden = intsoden;


                // save van ban
                intidvanban = _vanbandenRepo.Them(vanban);

                if (!String.IsNullOrEmpty(vanban.strtraloivanbanso))
                {   // lien ket van ban
                    string strquytac = AppSettings.TraloiVB;
                    int idvanbandi = _fileNameManger.GetIdVanban(strquytac, vanban.strtraloivanbanso);
                    if (idvanbandi != 0)
                    {
                        _fileNameManger.LienketVanban((int)enumHoibaovanban.intloai.Vanbanden, intidvanban, idvanbandi);
                    }
                }

                // neu co truong nguoi xu ly thi tu dong phan xu ly van ban
                _PhanXLVB(intidvanban, vanban);

                // cap nhat vbdt
                if ((model.idmail != 0) && (model.idmail != null))
                {
                    _UpdateVanbanMail(intidvanban, (int)model.idmail);
                }


                string strtenvanban = " số đến: " + vanban.intsoden.ToString() + ", số, ký hiệu: " + vanban.strkyhieu + ", ngày đến: " + vanban.strngayden;
                _logger.Info("Thêm mới văn bản đến: " + strtenvanban);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return intidvanban;
        }

        /// <summary>
        /// tu dong phan xu ly van ban khi co ten nguoi xu ly chinh        
        /// </summary>
        /// <param name="idvanban"></param>
        private void _PhanXLVB(int idvanban, Vanbanden vb)
        {
            int idxulychinh = 0;
            if (!string.IsNullOrEmpty(vb.strnoinhan))
            {
                idxulychinh = _canboRepo.GetActiveCanbo
                        .Where(p => p.strhoten == vb.strnoinhan)
                        .FirstOrDefault().intid;
            }

            int idnguoiduyet = (int)vb.intidnguoiduyet;
            if (idxulychinh != 0)
            {
                // KIEM TRA PHAN 2 LAN 1 VAN BAN    
                if (_role.CheckPhanHosocongviec(idvanban, (int)enumHosocongviec.intloai.Vanbanden))
                {
                    //  duyet van ban dang phan xl
                    _vanbandenRepo.Duyet(idvanban, (int)enumVanbanden.inttrangthai.Daduyet);

                    //  insert vao table hosocongviec
                    Hosocongviec hscv = new Hosocongviec();
                    hscv.intidnguoinhap = _session.GetUserId();
                    hscv.intloai = (int)enumHosocongviec.intloai.Vanbanden;

                    int iduser = _session.GetUserId();
                    hscv.intiddonvi = (int)_canboRepo.GetActiveCanboByID(iduser).intdonvi;
                    hscv.strngaymohoso = DateTime.Now;

                    // neu co linh vuc thi tinh theo thoi han xu ly cua linh vuc
                    hscv.intlinhvuc = vb.intidlinhvuc;
                    int thoihanxuly = _configRepo.GetConfigToInt(ThamsoHethong.ThoihanXLVB);
                    hscv.strthoihanxuly = DateServices.AddThoihanxuly(DateTime.Now, thoihanxuly);

                    hscv.strtieude = vb.strtrichyeu;

                    int idhosocongviec = _hosocongviecRepo.Them(hscv);

                    // insert vao table tonghopcanbo
                    int intloaitonghop = (int)enumTonghopCanbo.intloai.Vanbanmoi;
                    _ThemTonghopCanbo(idvanban, idxulychinh, intloaitonghop);
                    _ThemTonghopCanbo(idvanban, idnguoiduyet, intloaitonghop);

                    //  insert vao table doituongxuly: lanh dao  giao viec, xu ly chinh
                    _ThemDoituongxuly(idhosocongviec, idnguoiduyet, (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaogiaoviec);
                    _ThemDoituongxuly(idhosocongviec, idxulychinh, (int)enumDoituongxuly.intvaitro_doituongxuly.Xulychinh);

                    //  insert vao table hosovanban
                    _ThemHosovanban(idhosocongviec, idvanban, (int)enumHosovanban.intloai.Vanbanden);

                    // chua lam 
                    //  copy file dinh kem cua van ban vao folder hoso
                    //  nhan tin SMS (neu co)
                }
            }
        }

        /// <summary>
        /// them moi can bo xu ly vao bang doituongxuly tuy theo vai tro
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <param name="idcanboxuly"></param>
        /// <param name="intvaitro"></param>
        private void _ThemDoituongxuly(int idhosocongviec, int idcanboxuly, int intvaitro)
        {
            try
            {
                Doituongxuly dt = new Doituongxuly();
                dt.intidhosocongviec = idhosocongviec;
                dt.intidcanbo = idcanboxuly;
                dt.intvaitro = intvaitro;
                dt.intnguoitao = _session.GetUserId();
                dt.strngaytao = DateTime.Now;
                _doituongRepo.Them(dt);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// them ho so van ban 
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <param name="idvanban"></param>
        private void _ThemHosovanban(int idhosocongviec, int idvanban, int intloai)
        {
            try
            {
                Hosovanban hsvb = new Hosovanban();
                hsvb.intidhosocongviec = idhosocongviec;
                hsvb.intidvanban = idvanban;
                hsvb.intloai = intloai;
                _hosovanbanRepo.Them(hsvb);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

        }

        /// <summary>
        /// cap nhat van ban mail
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="idmail"></param>
        private void _UpdateVanbanMail(int idvanban, int idmail)
        {
            _vbmailRepo.UpdateTrangthai(idmail);

            // copy file dinh kem vao van ban den
            var mail = _fileMailRepo.AttachMails
                      .Where(p => p.inttrangthai == (int)enumAttachMail.inttrangthai.IsActive)
                      .Where(p => p.intloai == (int)enumAttachMail.intloai.Vanbandendientu)
                      .Where(p => p.intidmail == idmail)
                      .ToList();

            foreach (var file in mail)
            {
                // van ban mail
                string strLoaiFile = AppConts.FileEmail;
                string folderPath = _fileManager.GetFolderDownload(strLoaiFile, (DateTime)file.strngaycapnhat);
                string filepath = folderPath + "/" + file.strtenfile;
                string fileExt = _fileManager.GetFileExtention(file.strtenfile);

                string physicalFilePathMail = HttpContext.Current.Server.MapPath(filepath);

                // van ban den


                // dat ten file theo dinh dang: idvanban_intsttfile.*
                //=======================================================
                int intsttfile;
                var vb = _fileRepo.AttachVanbans
                        .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbanden)
                        .Where(p => p.intidvanban == idvanban);
                intsttfile = (vb.Count() == 0) ? 1 : vb.Count() + 1;

                string strmota = file.strmota;

                //  dinh dang file : idvanban_intsttfile.*
                string fileName = idvanban.ToString() + "_" + intsttfile.ToString() + "." + fileExt;

                strLoaiFile = AppConts.FileCongvanden;
                string folderSavepath = _fileManager.SetPathUpload(strLoaiFile);

                string fileSavepath = System.IO.Path.Combine(folderSavepath, fileName);

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
                        fileSavepath = System.IO.Path.Combine(folderSavepath, fileName);
                    }
                    System.IO.File.Copy(physicalFilePathMail, fileSavepath, true);
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
                        filevb.intloai = (int)enumAttachVanban.intloai.Vanbanden;
                        filevb.inttrangthai = (int)enumAttachVanban.inttrangthai.IsActive;
                        filevb.strmota = strmota;
                        filevb.strngaycapnhat = DateTime.Now;
                        filevb.strtenfile = fileName;
                        int intid = _fileRepo.Them(filevb);
                        _logger.Info("Đính kèm file vbdt: " + strmota + " vào văn bản đến: " + idvanban);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }



        }


        #region Tonghopcanbo
        /// <summary>
        /// them thong tin van ban den moi chuyen toi user
        /// </summary>
        /// <param name="idvanban"></param>
        private void _ThemTonghopCanbo(int idvanban, int idcanbo, int intloai)
        {
            try
            {
                TonghopCanbo th = new TonghopCanbo();
                th.intidcanbo = idcanbo;
                th.intloaitailieu = (int)enumTonghopCanbo.intloaitailieu.Vanbanden;
                th.intidtailieu = idvanban;
                th.intloai = intloai;
                //th.inttrangthai = (int)enumTonghopCanbo.inttrangthai.Chuaxem;
                //th.strngaynhan = DateTime.Now;
                th.intidnguoitao = _session.GetUserId();

                _tonghopRepo.Them(th);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
        /// <summary>
        /// xoa thong tin tonghop canbo
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="idcanbo"></param>
        private void _XoaTonghopCanbo(int idvanban, int idcanbo)
        {
            //int intloaitailieu = (int)enumTonghopCanbo.intloaitailieu.Vanbanden;
            _tonghopRepo.CapnhatTrangthaiVBDen(idcanbo, idvanban);

        }
        #endregion Tonghopcanbo


        /// <summary>
        /// cap nhat van ban
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="model"></param>
        /// <returns>true/false</returns>
        public bool Editvanban(int idvanban, Vanbanden model)
        {
            bool flag = false;
            string strtenvanban = "";
            try
            {
                var vb = _vanbandenRepo.Vanbandens
                        .FirstOrDefault(p => p.intid == idvanban);
                if (vb.intid != 0)
                {
                    string strtraloivbcu = vb.strtraloivanbanso;
                    string strtraloivbmoi = model.strtraloivanbanso;
                    // thong tin mac dinh cua van ban
                    model.intidnguoisua = _session.GetUserId();

                    strtenvanban = " số đến: " + model.intsoden + ", số, ký hiệu: " + model.strkyhieu + ", ngày đến: " + model.strngayden;

                    _vanbandenRepo.Sua(idvanban, model);

                    // tao lien ket van ban den/di                     
                    if (!string.IsNullOrEmpty(strtraloivbmoi))
                    {
                        string strquytac = AppSettings.TraloiVB;
                        if (!string.IsNullOrEmpty(strtraloivbcu))
                        {
                            if (strtraloivbcu != strtraloivbmoi)
                            {   // neu tra loi van moi khac tra loi van ban cu 

                                // xoa lien ket cu                          
                                int idvanbandicu = _fileNameManger.GetIdVanban(strquytac, strtraloivbcu);
                                if (idvanbandicu != 0)
                                {
                                    _XoaLienketVanban(idvanban, idvanbandicu);
                                }
                                // them lien ket moi
                                int idvanbandi = _fileNameManger.GetIdVanban(strquytac, strtraloivbmoi);
                                if (idvanbandi != 0)
                                {
                                    _fileNameManger.LienketVanban((int)enumHoibaovanban.intloai.Vanbanden, idvanban, idvanbandi);
                                }
                            }
                            else
                            {
                                // neu tra loi vb cu giong tra loi vb moi thi giu nguyen
                            }
                        }
                        else
                        {   // neu tra loi van ban cu la rong (khong co)
                            // thi them lien ket moi vao
                            int idvanbandi = _fileNameManger.GetIdVanban(strquytac, strtraloivbmoi);
                            if (idvanbandi != 0)
                            {
                                _fileNameManger.LienketVanban((int)enumHoibaovanban.intloai.Vanbanden, idvanban, idvanbandi);

                            }
                        }
                    }
                    else
                    {   // neu khong nhap tra loi vb 
                        // xoa lien ket cu (neu co)                        
                        if (!string.IsNullOrEmpty(strtraloivbcu))
                        {
                            string strquytac = AppSettings.TraloiVB;
                            int idvanbandicu = _fileNameManger.GetIdVanban(strquytac, strtraloivbcu);
                            if (idvanbandicu != 0)
                            {
                                _XoaLienketVanban(idvanban, idvanbandicu);
                            }
                        }
                    }

                    _logger.Info("Cập nhật văn bản đến: " + strtenvanban);
                    flag = true; // cap nhat thanh cong
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return flag;
        }
        /// <summary>
        /// xoa lien ket van ban
        /// </summary>
        /// <param name="idvanbanden"></param>
        /// <param name="idvanbandi"></param>
        private void _XoaLienketVanban(int idvanbanden, int idvanbandi)
        {
            try
            {
                if (idvanbandi != 0)
                {
                    var hoibao = _hoibaovanbanRepo.Hoibaovanbans
                                .Where(p => p.intloai == (int)enumHoibaovanban.intloai.Vanbanden)
                                .Where(p => p.intRecID == idvanbanden)
                                .Where(p => p.intTransID == idvanbandi)
                                .ToList();
                    if (hoibao.Count() != 0)
                    {
                        foreach (var p in hoibao)
                        {
                            _hoibaovanbanRepo.Xoa((int)enumHoibaovanban.intloai.Vanbanden, idvanbandi, idvanbanden);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        public DeleteVBViewModel GetDeleteVanban(int id)
        {
            var vb = _vanbandenRepo.Vanbandens
                .FirstOrDefault(p => p.intid == id);
            if (vb != null)
            {
                DeleteVBViewModel model = new DeleteVBViewModel
                {
                    intid = id,
                    strtenvanban = vb.intsoden.ToString() + ", số, ký hiệu " + vb.strkyhieu
                    + ", ngày đến " + DateServices.FormatDateVN(vb.strngayden)
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
                int idcanbo = _session.GetUserId();

                var vb = _vanbandenRepo.Vanbandens.FirstOrDefault(p => p.intid == id);
                string strtenvanban = vb.intsoden.ToString() + ", số, ký hiệu " + vb.strkyhieu
                    + ", ngày đến " + vb.strngayden.ToString() + ", id " + id.ToString();
                // moi xoa van ban
                // chua xoa cac quan he xu ly
                _vanbandenRepo.Xoa(id);

                // xoa file dinh kem
                var files = _fileRepo.AttachVanbans
                     .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                     .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbanden)
                     .Where(p => p.intidvanban == id)
                     .ToList();
                foreach (var file in files)
                {
                    string strLoaiFile = AppConts.FileCongvanden;
                    string folderPath = _fileManager.GetFolderDownload(strLoaiFile, (DateTime)file.strngaycapnhat);
                    string filepath = folderPath + "/" + file.strtenfile;
                    string physicalFilePath = HttpContext.Current.Server.MapPath(filepath);
                    try
                    {
                        System.IO.File.Delete(physicalFilePath);
                        _fileRepo.Xoa(file.intid, idcanbo);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message);
                    }
                }

                try
                {
                    // neu van ban chua co hosocongviec thi bo qua
                    //xoa phan xu ly
                    // xoa hosovanban, hosocongviec
                    int idhosocongviec = _hosovanbanRepo.Xoa(id, (int)enumHosovanban.intloai.Vanbanden);
                    _hosocongviecRepo.Xoa(idhosocongviec);

                    // xoa doi tuong xuly
                    _doituongRepo.XoaCacCanbo(idhosocongviec);
                }
                catch
                {

                }
                _logger.Info("Xóa văn bản đến: " + strtenvanban);
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

        #endregion UpdateVanban

        #region Capquyenxem

        /// <summary>
        /// lay danh sach cac tat cac user trong don vi
        /// neu user da co quyen xem roi thi isCheck = true  
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <returns></returns>
        public ListUserCapquyenxemViewModel GetListUserCapquyenxem(int idvanban)
        {
            ListUserCapquyenxemViewModel user = new ListUserCapquyenxemViewModel();

            user.idvanban = idvanban;

            user.canbo = _canboRepo.GetActiveCanbo
                //.Join(
                //    _canbodonviRepo.CanboDonvis,
                //    cb => cb.intid,
                //    dv => dv.intidcanbo,
                //    (cb, dv) => new { cb, dv }
                //)
                        .GroupJoin(
                            _vanbandencanboRepo.VanbandenCanbos.Where(p => p.intidvanban == idvanban),
                            cb2 => 1,
                            vb => 1,
                            (cb2, vb) => new { cb2, vb }
                        )
                        .Select(p => new UserXemVBDenModel
                        {
                            intid = p.cb2.intid,
                            IsCheck = p.vb.Any(x => x.intidcanbo == p.cb2.intid),

                            strkyhieu = p.cb2.strkyhieu,
                            strhoten = p.cb2.strhoten,
                            intiddonvi = (int)p.cb2.intdonvi
                        })
                        .OrderBy(p => p.strkyhieu)
                        .ThenBy(p => p.strhoten);

            user.donvi = _donviRepo.Donvitructhuocs
                        .Where(p => p.inttrangthai == (int)enumDonvitructhuoc.inttrangthai.IsActive)
                //.Select(p => new phongbanModel
                //{
                //    Id = p.Id,
                //    ParentId = p.ParentId,
                //    intlevel = p.intlevel,
                //    strtendonvi = p.strtendonvi
                //})
                        .OrderBy(p => p.intlevel)
                        .ThenBy(p => p.ParentId)
                        .ThenBy(p => p.strtendonvi)
                        ;

            user.maxLevelDonvi = _donviRepo.Donvitructhuocs
                        .Where(p => p.inttrangthai == (int)enumDonvitructhuoc.inttrangthai.IsActive)
                        .Max(p => p.intlevel);

            var intpublic = _vanbandenRepo.Vanbandens.FirstOrDefault(p => p.intid == idvanban).intpublic;
            string strpublicbtn = string.Empty;
            if (intpublic == (int)enumVanbanden.intpublic.Public)
            {
                strpublicbtn = "Hủy Công cộng";
            }
            else
            {
                strpublicbtn = "Công cộng";
            }

            user.intpublic = intpublic;
            user.strpublic = strpublicbtn;
            return user;
        }

        public ResultFunction SaveCapquyenxem(int idvanban, List<int> listidcanbo)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                if (listidcanbo.Count() == 0)
                {   // khong chon can bo nao thi
                    // xoa het can bo(neu co)
                    var canbo = _vanbandencanboRepo.VanbandenCanbos.Where(p => p.intidvanban == idvanban).Select(p => p.intidcanbo);
                    List<int> deleteidcanbo = new List<int>();
                    if (canbo.Count() != 0)
                    {
                        foreach (int p in canbo) { deleteidcanbo.Add(p); }
                    }
                    if (deleteidcanbo.Count() != 0)
                    {
                        foreach (int p in deleteidcanbo) { _Xoa_Log_Capquyenxem(idvanban, p); }
                    }
                }
                else
                {
                    var canbo = _vanbandencanboRepo.VanbandenCanbos.Where(p => p.intidvanban == idvanban).Select(p => p.intidcanbo);

                    if (canbo.Count() == 0)
                    {   // chua co cap quyen xem thi them moi
                        foreach (int p in listidcanbo)
                        {
                            _Save_Log_Capquyenxem(idvanban, p);
                        }
                    }
                    else
                    {   // da co user duoc cap quyen xem roi
                        // don gian nhat la xoa het roi them moi
                        // nhung vay thi khong ghi nhat ky duoc
                        //===============================================                
                        // kiểm tra list idcanbo                
                        // so sánh vói ds canbo trong vanbandencanbo
                        // nếu chưa có: thêm vào
                        // và kiểm tra xem những ds canbo mà không có trong listidcanbo thì
                        // xóa khỏi vanbandencanbo
                        // vd: listid: 1,3,6,7
                        // canbo: 1,2,3,4
                        // kq: 1,3,6,7 xoa 2,4
                        //===============================================
                        foreach (int p in listidcanbo)
                        {
                            bool isfound = false;
                            foreach (int c in canbo)
                            {
                                if (p == c) { isfound = true; }
                            }
                            if (isfound == false)
                            {   // khong tim thay thi them moi
                                _Save_Log_Capquyenxem(idvanban, p);
                            }
                        }
                        // xoa cac user khong co trong listidcanbo
                        var canbo2 = _vanbandencanboRepo.VanbandenCanbos
                                .Where(p => p.intidvanban == idvanban)
                                .Select(p => p.intidcanbo).ToList();
                        foreach (int c in canbo2)
                        {
                            bool isfound = false;
                            foreach (int p in listidcanbo)
                            {
                                if (c == p) { isfound = true; }
                            }
                            if (isfound == false)
                            {
                                _Xoa_Log_Capquyenxem(idvanban, c);
                            }
                        }

                    }
                }

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

        /// <summary>
        /// them moi cap quyen xem van ban va ghi nhat ky
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="idcanbo"></param>
        private void _Save_Log_Capquyenxem(int idvanban, int idcanbo)
        {
            try
            {
                _vanbandencanboRepo.Them(idvanban, idcanbo);
                // ghi nhat ky trong chi tiet van ban di
                _SaveLogChitietVanban((int)enumChitietVanbanden.intvaitro.Capquyenxem, idcanbo, idvanban);

                //insert vao tonghopcanbo
                int intloaitonghop = (int)enumTonghopCanbo.intloai.Debiet;
                _ThemTonghopCanbo(idvanban, idcanbo, intloaitonghop);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// xoa quyen xem va ghi nhat ky 
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="idcanbo"></param>
        private void _Xoa_Log_Capquyenxem(int idvanban, int idcanbo)
        {
            try
            {
                _vanbandencanboRepo.Xoa(idvanban, idcanbo);
                // ghi nhat ky 
                _Log_Huyquyenxem((int)enumChitietVanbanden.intvaitro.Huyquyenxem, idcanbo, idvanban);

                // xoa trong tonghopcanbo
                _XoaTonghopCanbo(idvanban, idcanbo);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// ghi nhan cac thao tac tren van ban
        /// </summary>
        /// <param name="intvaitro"></param>
        /// <param name="idcanbo"></param>
        /// <param name="idvanban"></param>
        private void _SaveLogChitietVanban(int intvaitro, int idcanbo, int idvanban)
        {
            int idnguoitao = _session.GetUserId();
            ChitietVanbanden ct = new ChitietVanbanden();
            ct.intidcanbo = idcanbo;
            ct.intidvanban = idvanban;
            ct.intnguoitao = idnguoitao;
            ct.intvaitro = intvaitro;
            //ct.strngaytao = DateTime.Now; mac dinh
            _chitietVBDenRepo.Them(ct);
        }


        /// <summary>
        /// huy quyen xem/public cua canbo doi voi van ban
        /// </summary>
        /// <param name="intvaitro"></param>
        /// <param name="idcanbo"></param>
        /// <param name="idvanban"></param>
        private void _Log_Huyquyenxem(int intvaitro, int idcanbo, int idvanban)
        {
            int intvaitrocu = 0;
            switch (intvaitro)
            {
                case (int)enumChitietVanbanden.intvaitro.Huyquyenxem:
                    intvaitrocu = (int)enumChitietVanbanden.intvaitro.Capquyenxem;
                    break;
                case (int)enumChitietVanbanden.intvaitro.HuyquyenPublic:
                    intvaitrocu = (int)enumChitietVanbanden.intvaitro.CapquyenPublic;
                    break;
                default:
                    intvaitrocu = 0;
                    break;
            }
            if (intvaitrocu != 0)
            {
                try
                {
                    int idnguoichuyen = _session.GetUserId();
                    var ct = _chitietVBDenRepo.Chitietvanbandens
                            .Where(p => p.intidcanbo == idcanbo)
                            .Where(p => p.intidvanban == idvanban)
                            .Where(p => p.intvaitro == intvaitrocu)
                            .FirstOrDefault();
                    if (ct != null)
                    {
                        ct.intvaitrocu = intvaitrocu;
                        ct.intvaitro = intvaitro;
                        ct.intnguoichuyen = idnguoichuyen;
                        ct.strngaychuyen = DateTime.Now;
                        _chitietVBDenRepo.Sua(ct.intid, ct);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
            }

        }

        /// <summary>
        /// cap quyen xem van ban la public hoac private
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="intpublic">trang thai public/private hien co cua van ban dang xet</param>
        /// <returns></returns>
        public ResultFunction CapquyenxemPublic(int idvanban, int intpublic)
        {
            ResultFunction kq = new ResultFunction();
            int idcanbo = _session.GetUserId();
            int intcongcong = 0;
            int intvaitro = 0;
            if (intpublic == (int)enumVanbanden.intpublic.Private)
            {
                intcongcong = (int)enumVanbanden.intpublic.Public;
                intvaitro = (int)enumChitietVanbanden.intvaitro.CapquyenPublic;
            }
            if (intpublic == (int)enumVanbanden.intpublic.Public)
            {
                intcongcong = (int)enumVanbanden.intpublic.Private;
                intvaitro = (int)enumChitietVanbanden.intvaitro.HuyquyenPublic;
            }
            try
            {
                _vanbandenRepo.CapquyenxemPublic(idvanban, intcongcong);
                // ghi nhat ky 
                _SaveLogChitietVanban(intvaitro, idcanbo, idvanban);

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


        #endregion Capquyenxem

        #region Upload

        /// <summary>
        /// lay danh sach cac file dinh kem cua van ban
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        public UploadVBDenViewModel GetListFile(int idvanban)
        {
            UploadVBDenViewModel model = new UploadVBDenViewModel();
            model.idvanban = idvanban;

            var fileAttach = _fileRepo.AttachVanbans
                            .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbanden)
                            .Where(p => p.intidvanban == idvanban)
                            .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                            .OrderBy(p => p.intid)
                            .Select(p => new UploadFileViewModel
                            {
                                IdFile = p.intid,
                                Name = p.strmota,
                                Extension = p.intid.ToString()
                                //Size = 
                            })
                            ;
            model.listfile = fileAttach;
            return model;

        }


        #endregion Upload

        #region PhanXLNhieuVB

        public FormPhanXLNhieuVBViewModel GetFormPhanXLNhieuVB()
        {
            FormPhanXLNhieuVBViewModel hoso = new FormPhanXLNhieuVBViewModel();
            hoso.dteNgaymohoso = DateTime.Now;

            int inthanxuly = _configRepo.GetConfigToInt(ThamsoHethong.ThoihanXLVB);
            hoso.dteHanxuly = DateServices.AddThoihanxuly(DateTime.Now, inthanxuly);

            hoso.LanhdaogiaoviecModel = _canboRepo.GetActiveCanbo
                .Join(_chucdanhRepo.Chucdanhs.Where(p => p.intloai == (int)enumchucdanh.intloai.Lanhdao),
                        u => u.intchucvu,
                        c => c.intid,
                        (u, c) => u
                )
                .Select(p => new CanboViewModel
                {
                    intid = p.intid,
                    iddonvi = p.intdonvi,
                    intchucvu = p.intchucvu,
                    IsKyVB = p.intkivb == (int)enumcanbo.intkivb.Co ? true : false,
                    intnhomquyen = p.intnhomquyen,
                    IsNguoiXL = p.intnguoixuly == (int)enumcanbo.intnguoixuly.IsActive ? true : false,
                    strhoten = p.strhoten,
                    strkyhieu = p.strkyhieu,
                    strmacanbo = p.strmacanbo
                })
                .OrderBy(p => p.strkyhieu)
                .ThenBy(p => p.strhoten);

            hoso.LanhdaophutrachModel = hoso.LanhdaogiaoviecModel;

            hoso.XulychinhModel = _canboRepo.GetActiveCanbo
                .Select(p => new CanboViewModel
                {
                    intid = p.intid,
                    iddonvi = p.intdonvi,
                    intchucvu = p.intchucvu,
                    IsKyVB = p.intkivb == (int)enumcanbo.intkivb.Co ? true : false,
                    intnhomquyen = p.intnhomquyen,
                    IsNguoiXL = p.intnguoixuly == (int)enumcanbo.intnguoixuly.IsActive ? true : false,
                    strhoten = p.strhoten,
                    strkyhieu = p.strkyhieu,
                    strmacanbo = p.strmacanbo
                })
                .OrderBy(p => p.strkyhieu)
                .ThenBy(p => p.strhoten);

            hoso.listDonvi = _donviRepo.Donvitructhuocs
                  .Select(p => new QLVB.DTO.Donvi.EditDonviViewModel
                  {
                      intid = p.Id,
                      strtendonvi = p.strtendonvi
                  })
                  .OrderBy(p => p.strtendonvi);

            return hoso;
        }

        public IEnumerable<ListVanbandenViewModel> GetListPhanXLNhieuVB(string listid)
        {
            var listidvanban = ValidateData.CheckInput(listid);
            listidvanban = listidvanban.Substring(0, listidvanban.Length - 1);
            string strSearchValues = " vanbanden.intid in (" + listidvanban + ") ";

            string strqueryvbden = _SqlGetAllVBden_v1(true, true, strSearchValues);

            string query = strqueryvbden;

            IEnumerable<ListVanbandenViewModel> listvb = (IEnumerable<ListVanbandenViewModel>)_vanbandenRepo.RunSqlListVBDen(query);

            return listvb;
        }


        #endregion PhanXLNhieuVB

        #region Email

        public QLVB.DTO.Vanbandi.ListEmailDonviViewModel GetListEmailDonvi(int idvanban)
        {
            QLVB.DTO.Vanbandi.ListEmailDonviViewModel model = new QLVB.DTO.Vanbandi.ListEmailDonviViewModel();
            model.IsAutoSend = AppSettings.IsAutoSendMail;
            model.idvanban = idvanban;

            var vanban = _vanbandenRepo.GetVanbandenById(idvanban);

            model.strtieude = vanban.strtrichyeu;

            model.listfile = _mailFormat.GetFileVanbanToAttach(idvanban, (int)enumAttachVanban.intloai.Vanbanden);


            var alldonvi = _tochucRepo.GetActiveTochucdoitacs
                .Where(p =>
                    p.Isvbdt == (int)enumTochucdoitac.isvbdt.IsActive
                    || (p.stremail.Length > 5) //(p.stremail != null || p.stremail != "")
                )
                .Select(p => new QLVB.DTO.Vanbandi.EmailDonviViewModel
                {
                    iddonvi = p.intid,
                    intloai = p.intidkhoi,
                    strtendonvi = p.strtentochucdoitac,
                    IsVbdt = p.Isvbdt == (int)enumTochucdoitac.isvbdt.IsActive ? true : false,
                    stremailvbdt = string.IsNullOrEmpty(p.stremailvbdt) ? string.Empty : p.stremailvbdt,
                    stremail = string.IsNullOrEmpty(p.stremail) ? string.Empty : p.stremail
                    //IsSend = p.g.Any(a => a.intiddonvi == p.intid)
                })
                .ToList();
            model.donvi = alldonvi
                .Where(p => p.IsVbdt == true)
                .GroupJoin(
                    _guivbRepo.GuiVanbans
                        .Where(p => p.intidvanban == idvanban)
                        .Where(p => p.intloaivanban == (int)enumGuiVanban.intloaivanban.Vanbandi),
                    tc => tc.iddonvi,
                    g => g.intiddonvi,
                    (tc, g) => new { tc, g }
                )
                .Select(p => new QLVB.DTO.Vanbandi.EmailDonviViewModel
                {
                    iddonvi = p.tc.iddonvi,
                    intloai = p.tc.intloai,
                    strtendonvi = p.tc.strtendonvi,
                    IsVbdt = p.tc.IsVbdt,
                    stremailvbdt = string.IsNullOrEmpty(p.tc.stremailvbdt) ? string.Empty : p.tc.stremailvbdt,
                    stremail = string.IsNullOrEmpty(p.tc.stremail) ? string.Empty : p.tc.stremail,
                    IsSend = p.g.Any(a => a.intiddonvi == p.tc.iddonvi)
                })
                .OrderBy(p => p.strtendonvi);

            model.donvikhac = alldonvi
               .Where(p => p.IsVbdt == false)
               .GroupJoin(
                   _guivbRepo.GuiVanbans
                       .Where(p => p.intidvanban == idvanban)
                       .Where(p => p.intloaivanban == (int)enumGuiVanban.intloaivanban.Vanbandi),
                   tc => tc.iddonvi,
                   g => g.intiddonvi,
                   (tc, g) => new { tc, g }
               )
               .Select(p => new QLVB.DTO.Vanbandi.EmailDonviViewModel
               {
                   iddonvi = p.tc.iddonvi,
                   intloai = p.tc.intloai,
                   strtendonvi = p.tc.strtendonvi,
                   IsVbdt = p.tc.IsVbdt,
                   stremailvbdt = string.IsNullOrEmpty(p.tc.stremailvbdt) ? string.Empty : p.tc.stremailvbdt,
                   stremail = string.IsNullOrEmpty(p.tc.stremail) ? string.Empty : p.tc.stremail,
                   IsSend = p.g.Any(a => a.intiddonvi == p.tc.iddonvi)
               })
               .OrderBy(p => p.strtendonvi);

            return model;
        }

        public IEnumerable<QLVB.DTO.Vanbandientu.ListSendDonviViewModel> GetListSendDonvi(List<int> listiddonvi)
        {
            var alldonvi = _tochucRepo.GetActiveTochucdoitacs
                .Where(p =>
                    p.Isvbdt == (int)enumTochucdoitac.isvbdt.IsActive
                    || (p.stremail.Length > 5)
                )
                .Select(p => new QLVB.DTO.Vanbandi.EmailDonviViewModel
                {
                    iddonvi = p.intid,
                    intloai = p.intidkhoi,
                    strtendonvi = p.strtentochucdoitac,
                    IsVbdt = p.Isvbdt == (int)enumTochucdoitac.isvbdt.IsActive ? true : false,
                    stremailvbdt = string.IsNullOrEmpty(p.stremailvbdt) ? string.Empty : p.stremailvbdt,
                    stremail = string.IsNullOrEmpty(p.stremail) ? string.Empty : p.stremail
                })
                .ToList();

            var cacdonvi = alldonvi
                    .Where(p => listiddonvi.Contains(p.iddonvi))
                    .Select(p => new QLVB.DTO.Vanbandientu.ListSendDonviViewModel
                    {
                        intid = p.iddonvi,
                        stremailvbdt = p.IsVbdt == true ? p.stremailvbdt : p.stremail,
                        isvbdt = p.IsVbdt
                    });

            return cacdonvi;
        }

        #endregion Email


    }


}
