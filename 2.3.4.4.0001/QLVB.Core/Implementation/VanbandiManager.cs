using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Core.Contract;
using QLVB.Common.Logging;
using QLVB.Domain.Abstract;
using QLVB.DTO.Vanbandi;
using QLVB.Common.Utilities;
using QLVB.Domain.Entities;
using QLVB.Common.Sessions;
using QLVB.Common.Date;
using QLVB.DTO;
using System.Data.Entity;
using QLVB.DTO.File;
using QLVB.DTO.Vanbandientu;
using QLVB.DTO.Edxml;

namespace QLVB.Core.Implementation
{
    public class VanbandiManager : IVanbandiManager
    {
        #region Constructor

        private ILogger _logger;
        private IRoleManager _role;
        private ISessionServices _session;
        private IVanbandiRepository _vanbandiRepo;
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
        private IVanbandenRepository _vanbandenRepo;
        private IHosovanbanRepository _hosovanbanRepo;
        private IHosocongviecRepository _hosocongviecRepo;
        private IHoibaovanbanRepository _hoibaovanbanRepo;
        private IChitietHosoRepository _chitietHosoRepo;
        private IVanbandiCanboRepository _vanbandicanboRepo;
        private IDonvitructhuocRepository _donviRepo;
        private IChitietVanbandiRepository _chitietVBDiRepo;
        private IRuleFileNameManager _ruleFileName;
        private IGuiVanbanRepository _guivbRepo;
        private IFileManager _fileManager;
        private IHosovanbanlienquanRepository _hsvblqRepo;
        private IEdxmlManager _edxmlManager;
        private IMailFormatManager _mailFormat;
        private ILuutruVanbanRepository _luutruVanbanRepo;

        public VanbandiManager(IVanbandiRepository vanbandiRepo, IPhanloaiTruongRepository pltruongRepo,
                IPhanloaiVanbanRepository plvanbanRepo, ISoVanbanRepository sovbRepo,
                IKhoiphathanhRepository khoiphRepo, ILinhvucRepository linhvucRepo,
                ITinhchatvanbanRepository tinhchatvbRepo, ICanboRepository canboRepo,
                IDiachiluutruRepository luutruRepo, ITochucdoitacRepository tochucRepo,
                ICategoryRepository categoryRepo, IConfigRepository configRepo,
                IAttachVanbanRepository fileRepo, ILogger logger,
                IVanbandenRepository vanbandenRepo, IHosovanbanRepository hosovanbanRepo,
                IHosocongviecRepository hosocongviecRepo, IHoibaovanbanRepository hoibaovanbanRepo,
                IChitietHosoRepository chitietHosoRepo, IVanbandiCanboRepository vanbandicanboRepo,
                IDonvitructhuocRepository donviRepo,
                IChitietVanbandiRepository chitietVBDiRepo, IRoleManager role,
                IRuleFileNameManager ruleFileName, IGuiVanbanRepository guivbRepo,
                ISessionServices session, IFileManager fileManager,
                IHosovanbanlienquanRepository hsvblqRepo, IEdxmlManager edxml,
                IMailFormatManager mailFormat, ILuutruVanbanRepository luutruVanbanRepo)
        {
            _vanbandiRepo = vanbandiRepo;
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
            _logger = logger;
            _vanbandenRepo = vanbandenRepo;
            _hosovanbanRepo = hosovanbanRepo;
            _hosocongviecRepo = hosocongviecRepo;
            _hoibaovanbanRepo = hoibaovanbanRepo;
            _chitietHosoRepo = chitietHosoRepo;
            _vanbandicanboRepo = vanbandicanboRepo;
            _donviRepo = donviRepo;

            _chitietVBDiRepo = chitietVBDiRepo;
            _role = role;
            _ruleFileName = ruleFileName;
            _guivbRepo = guivbRepo;
            _session = session;
            _fileManager = fileManager;
            _hsvblqRepo = hsvblqRepo;
            _edxmlManager = edxml;
            _mailFormat = mailFormat;
            _luutruVanbanRepo = luutruVanbanRepo;
        }
        #endregion Constructor

        #region ListVanban
        public ToolbarViewModel GetToolbar()
        {
            string strDisplay = "normal";
            string strNone = "none";
            ToolbarViewModel tbar = new ToolbarViewModel();
            tbar.Capnhatvb = _role.IsRole(RoleVanbandi.Capnhatvb) == true ? strDisplay : strNone;
            tbar.Capquyenxem = _role.IsRole(RoleVanbandi.Capquyenxem) == true ? strDisplay : strNone;
            tbar.Duyetvb = _role.IsRole(RoleVanbandi.Duyetvb) == true ? strDisplay : strNone;
            tbar.Email = _role.IsRole(RoleVanbandi.GuiEmail) == true ? strDisplay : strNone;
            tbar.Xoavb = _role.IsRole(RoleVanbandi.Xoavb) == true ? strDisplay : strNone;
            tbar.Luutru = _role.IsRole(RoleVanbandi.Luutruvanban) == true ? strDisplay : strNone;

            return tbar;
        }

        public CategoryViewModel GetCategory()
        {
            CategoryViewModel cat = new CategoryViewModel();
            cat.Category = _categoryRepo.CategoryVanbans
                .Where(p => p.inttrangthai == (int)enumCategoryVanban.inttrangthai.IsActive)
                .Where(p => p.intloai == (int)enumCategoryVanban.intloai.Vanbanphathanh)
                .OrderBy(p => p.intorder);

            cat.Songayhienthi = _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);


            cat.Loaivanban = _plvanbanRepo.GetActivePhanloaiVanbans
                .Where(p => p.intloai == (int)enumPhanloaiVanban.intloai.vanbandi)
                .OrderBy(p => p.strtenvanban);

            cat.Sovanban = _sovbRepo.GetActiveSoVanbans
                .Where(p => p.intloai == (int)enumSovanban.intloai.Vanbanphathanh)
                .OrderBy(p => p.strten);


            return cat;
        }

        public IEnumerable<ListVanbandiViewModel> GetListVanbandi(
            string strngaykycat, int? idloaivb, int? idsovb, string strvbphathanh, string hoibao,
            int? intsobd, int? intsokt, string strngaykybd, string strngaykykt,
            string strkyhieu, string strnguoiky, string strnguoisoan, string strnguoiduyet,
            string strnoinhan, string strtrichyeu, string strhantraloi, string strdonvisoan,
            int? idkhan, int? idmat
            )
        {
            #region temp
            //bool isSearch = false;
            //string strSearchValues = string.Empty;
            //// strSearchValues = "intsobd=1;intsokt=10;idloaivb=2;"

            //var vanban = _vanbandiRepo.Vanbandis;

            ////====================================================
            //// kiem tra nhung van ban user duoc quyen xem/xuly
            ////====================================================
            //vanban = _GetQuyenXemVB(vanban);

            ////====================================================
            //// tuy chon category 
            ////====================================================
            //if (!string.IsNullOrEmpty(strngaykycat))
            //{
            //    DateTime? dtengaykycat = DateServices.FormatDateEn(strngaykycat);
            //    vanban = vanban.Where(p => p.strngayky == dtengaykycat);
            //    isSearch = true;
            //    strSearchValues += "strngaykycat=" + strngaykycat + ";";
            //}
            //if ((idloaivb != null) && (idloaivb != 0))
            //{
            //    vanban = vanban.Where(p => p.intidphanloaivanbandi == idloaivb);
            //    isSearch = true;
            //    strSearchValues += "idloaivb=" + idloaivb.ToString() + ";";
            //}
            //if ((idsovb != null) && (idsovb != 0))
            //{
            //    vanban = vanban.Where(p => p.intidsovanban == idsovb);
            //    isSearch = true;
            //    strSearchValues += "idsovb=" + idsovb.ToString() + ";";
            //}
            ////=======================================================
            //// Search van ban            
            ////======================================================= 
            //if ((intsokt != null) && (intsokt != 0))
            //{
            //    if ((intsobd != null) && (intsobd != 0))
            //    {
            //        vanban = vanban.Where(p => p.intso >= intsobd)
            //                .Where(p => p.intso <= intsokt);
            //        isSearch = true;
            //        strSearchValues += "intsobd=" + intsobd.ToString() + ";intsokt=" + intsokt.ToString() + ";";
            //    }
            //}
            //else
            //{
            //    if ((intsobd != null) && (intsobd != 0))
            //    {
            //        vanban = vanban.Where(p => p.intso == intsobd);
            //        isSearch = true;
            //        strSearchValues += "intsobd=" + intsobd + ";";
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

            //if (!string.IsNullOrEmpty(strkyhieu))
            //{
            //    vanban = vanban.Where(p => p.strkyhieu.Contains(strkyhieu));
            //    isSearch = true;
            //    strSearchValues += "strkyhieu=" + strkyhieu + ";";
            //}

            //if (!string.IsNullOrEmpty(strnguoiky))
            //{
            //    vanban = vanban.Where(p => p.strnguoiky.Contains(strnguoiky));
            //    isSearch = true;
            //    strSearchValues += "strnguoiky=" + strnguoiky + ";";
            //}
            //if (!string.IsNullOrEmpty(strnguoisoan))
            //{
            //    vanban = vanban.Where(p => p.strnguoisoan.Contains(strnguoisoan));
            //    isSearch = true;
            //    strSearchValues += "strnguoisoan=" + strnguoisoan + ";";
            //}
            //if (!string.IsNullOrEmpty(strnguoiduyet))
            //{
            //    vanban = vanban.Where(p => p.strnguoiduyet.Contains(strnguoiduyet));
            //    isSearch = true;
            //    strSearchValues += "strnguoiduyet=" + strnguoiduyet + ";";
            //}
            //if (!string.IsNullOrEmpty(strnoinhan))
            //{
            //    vanban = vanban.Where(p => p.strnoinhan.Contains(strnoinhan));
            //    isSearch = true;
            //    strSearchValues += "strnoinhan=" + strnoinhan + ";";
            //}
            //if (!string.IsNullOrEmpty(strtrichyeu))
            //{
            //    vanban = vanban.Where(p => p.strtrichyeu.Contains(strtrichyeu));
            //    isSearch = true;
            //    strSearchValues += "strtrichyeu=" + strtrichyeu + ";";
            //}
            //if (!string.IsNullOrEmpty(strhantraloi))
            //{
            //    DateTime? dthantraloi = DateServices.FormatDateEn(strhantraloi);
            //    vanban = vanban.Where(p => p.strhanxuly == dthantraloi);
            //    isSearch = true;
            //    strSearchValues += "strhantraloi=" + strhantraloi + ";";
            //}
            //if (!string.IsNullOrEmpty(strdonvisoan))
            //{
            //    vanban = vanban.Where(p => p.strdonvisoan.Contains(strdonvisoan));
            //    isSearch = true;
            //    strSearchValues += "strdonvisoan=" + strdonvisoan + ";";
            //}
            ////========================================================
            //// end search
            ////========================================================
            //if (!isSearch)
            //{   // khong phai la search thi gioi han ngay hien thi
            //    int intngay = _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);
            //    vanban = vanban.Where(p => System.Data.Entity.DbFunctions.DiffDays(p.strngayky, DateTime.Now) < intngay);
            //    // reset session
            //    _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.NoSearch);
            //}
            //else
            //{   // luu cac gia tri search vao session
            //    _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.SearchVBDi);
            //    _session.InsertObject(AppConts.SessionSearchValues, strSearchValues);

            //}
            #endregion temp

            var vanban = _GetVanbandiFromRequest(
                strngaykycat, idloaivb, idsovb, strvbphathanh, hoibao,
                intsobd, intsokt, strngaykybd, strngaykykt,
                strkyhieu, strnguoiky, strnguoisoan, strnguoiduyet,
                strnoinhan, strtrichyeu, strhantraloi, strdonvisoan,
                idkhan, idmat
                );
            //====================================================
            // chon cac truong tra ve view list van ban
            //====================================================
            //====================================================
            // kiem tra nhung van ban user duoc quyen xem/xuly
            //====================================================            
            bool isViewAllvb = _role.IsRole(RoleVanbandi.Xemtatcavb);
            if (isViewAllvb)
            {   // co quyen xem tat ca van ban                
                var listvb = _GetViewVanban(vanban);
                //if (!string.IsNullOrEmpty(xuly))
                //{
                //    listvb = _SearchTinhtrangxuly(xuly, listvb);
                //}
                return listvb;
            }
            else
            {
                var vanbanxuly = _GetQuyenXemVB(vanban, isViewAllvb);
                var listvbxuly = _GetViewVanban(vanbanxuly);

                var listvbpublic = _GetViewVanban_Public(vanban);
                var listvbtonghop = listvbxuly.Union(listvbpublic);

                //if (!string.IsNullOrEmpty(xuly))
                //{
                //    listvbtonghop = _SearchTinhtrangxuly(xuly, listvbtonghop);
                //}
                return listvbtonghop;
            }

        }


        private IQueryable<Vanbandi> _GetVanbandiFromRequest(
            string strngaykycat, int? idloaivb, int? idsovb, string strvbphathanh, string hoibao,
            int? intsobd, int? intsokt, string strngaykybd, string strngaykykt,
            string strkyhieu, string strnguoiky, string strnguoisoan, string strnguoiduyet,
            string strnoinhan, string strtrichyeu, string strhantraloi, string strdonvisoan,
            int? idkhan, int? idmat
            )
        {
            bool isSearch = false;
            string strSearchValues = string.Empty;
            // strSearchValues = "intsobd=1;intsokt=10;idloaivb=2;"

            var vanban = _vanbandiRepo.Vanbandis;

            //====================================================
            // tuy chon category 
            //====================================================
            if (!string.IsNullOrEmpty(strngaykycat))
            {
                DateTime? dtengaykycat = DateServices.FormatDateEn(strngaykycat);
                vanban = vanban.Where(p => p.strngayky == dtengaykycat);
                isSearch = true;
                strSearchValues += "strngaykycat=" + strngaykycat + ";";
            }
            if ((idloaivb != null) && (idloaivb != 0))
            {
                vanban = vanban.Where(p => p.intidphanloaivanbandi == idloaivb);
                //isSearch = true;
                strSearchValues += "idloaivb=" + idloaivb.ToString() + ";";
            }
            if ((idsovb != null) && (idsovb != 0))
            {
                vanban = vanban.Where(p => p.intidsovanban == idsovb);
                //isSearch = true;
                strSearchValues += "idsovb=" + idsovb.ToString() + ";";
            }
            if (!string.IsNullOrEmpty(strvbphathanh))
            {
                vanban = vanban.Where(p => p.intso == null)
                    .Where(p => p.strkyhieu == null);
                //isSearch = true;
                strSearchValues += "strvbphathanh=" + strvbphathanh + ";";
            }
            if (!string.IsNullOrEmpty(hoibao))
            {
                DateTime dteNow = DateTime.Now;
                switch (hoibao)
                {
                    case "quahan":
                        vanban = vanban.Where(p => p.strhanxuly < dteNow);
                        break;
                    case "tronghan":
                        vanban = vanban.Where(p => p.strhanxuly >= dteNow);
                        break;
                    case "dahoibao":

                        break;
                }
                //isSearch = true;
                strSearchValues += "hoibao=" + hoibao + ";";
            }
            //=======================================================
            // Search van ban            
            //======================================================= 
            if ((intsokt != null) && (intsokt != 0))
            {
                if ((intsobd != null) && (intsobd != 0))
                {
                    vanban = vanban.Where(p => p.intso >= intsobd)
                            .Where(p => p.intso <= intsokt);
                    isSearch = true;
                    strSearchValues += "intsobd=" + intsobd.ToString() + ";intsokt=" + intsokt.ToString() + ";";
                }
            }
            else
            {
                if ((intsobd != null) && (intsobd != 0))
                {
                    vanban = vanban.Where(p => p.intso == intsobd);
                    isSearch = true;
                    strSearchValues += "intsobd=" + intsobd + ";";
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

            if (!string.IsNullOrEmpty(strkyhieu))
            {
                vanban = vanban.Where(p => p.strkyhieu.Contains(strkyhieu));
                isSearch = true;
                strSearchValues += "strkyhieu=" + strkyhieu + ";";
            }

            if (!string.IsNullOrEmpty(strnguoiky))
            {
                vanban = vanban.Where(p => p.strnguoiky.Contains(strnguoiky));
                isSearch = true;
                strSearchValues += "strnguoiky=" + strnguoiky + ";";
            }
            if (!string.IsNullOrEmpty(strnguoisoan))
            {
                vanban = vanban.Where(p => p.strnguoisoan.Contains(strnguoisoan));
                isSearch = true;
                strSearchValues += "strnguoisoan=" + strnguoisoan + ";";
            }
            if (!string.IsNullOrEmpty(strnguoiduyet))
            {
                vanban = vanban.Where(p => p.strnguoiduyet.Contains(strnguoiduyet));
                isSearch = true;
                strSearchValues += "strnguoiduyet=" + strnguoiduyet + ";";
            }
            if (!string.IsNullOrEmpty(strnoinhan))
            {
                vanban = vanban.Where(p => p.strnoinhan.Contains(strnoinhan));
                isSearch = true;
                strSearchValues += "strnoinhan=" + strnoinhan + ";";
            }
            if (!string.IsNullOrEmpty(strtrichyeu))
            {
                vanban = vanban.Where(p => p.strtrichyeu.Contains(strtrichyeu));
                isSearch = true;
                strSearchValues += "strtrichyeu=" + strtrichyeu + ";";
            }
            if (!string.IsNullOrEmpty(strhantraloi))
            {
                DateTime? dthantraloi = DateServices.FormatDateEn(strhantraloi);
                vanban = vanban.Where(p => p.strhanxuly == dthantraloi);
                isSearch = true;
                strSearchValues += "strhantraloi=" + strhantraloi + ";";
            }
            if (!string.IsNullOrEmpty(strdonvisoan))
            {
                vanban = vanban.Where(p => p.strdonvisoan.Contains(strdonvisoan));
                isSearch = true;
                strSearchValues += "strdonvisoan=" + strdonvisoan + ";";
            }
            if ((idkhan != null) && (idkhan != 0))
            {
                vanban = vanban.Where(p => p.intidkhan == idkhan);
                isSearch = true;
                strSearchValues += "idkhan=" + idkhan.ToString() + ";";
            }
            if ((idmat != null) && (idmat != 0))
            {
                vanban = vanban.Where(p => p.intidmat == idmat);
                isSearch = true;
                strSearchValues += "idmat=" + idmat.ToString() + ";";
            }
            //========================================================
            // end search
            //========================================================
            if (!isSearch)
            {   // khong phai la search thi gioi han ngay hien thi
                int intngay = _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);
                //vanban = vanban.Where(p => System.Data.Entity.DbFunctions.DiffDays(p.strngayky, DateTime.Now) < intngay);
                DateTime? dtengaybd = DateTime.Now.AddDays(-intngay);
                vanban = vanban.Where(p => p.strngayky >= dtengaybd);
                // reset session
                _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.NoSearch);
            }
            else
            {   // luu cac gia tri search vao session
                _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.SearchVBDi);
                _session.InsertObject(AppConts.SessionSearchTypeValues, strSearchValues);

            }
            return vanban;
        }


        /// <summary>
        /// tra ve list view van ban di
        /// </summary>
        /// <param name="vanban"></param>
        /// <returns></returns>
        private IEnumerable<ListVanbandiViewModel> _GetViewVanban(IQueryable<Vanbandi> vanban)
        {
            IEnumerable<ListVanbandiViewModel> listvb = vanban
                    .GroupJoin(
                        _fileRepo.AttachVanbans
                            .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                            .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbandi),
                        v => 1,
                        f => 1,
                        (v, f) => new { v, f }
                    )
                    .Select(p => new ListVanbandiViewModel
                    {
                        intid = p.v.intid,
                        intso = p.v.intso,
                        strsophu = !string.IsNullOrEmpty(p.v.strmorong) ? p.v.strmorong : "",
                        dtengayky = p.v.strngayky,
                        strkyhieu = p.v.strkyhieu,
                        strnoinhan = p.v.strnoinhan,
                        strtrichyeu = p.v.strtrichyeu,
                        inttrangthai = p.v.inttrangthai,
                        dtehanxuly = p.v.strhanxuly,
                        IsAttach = p.f.Any(a => a.intidvanban == p.v.intid)

                    });
            return listvb;
        }

        private IEnumerable<ListVanbandiViewModel> _GetViewVanban_Public(IQueryable<Vanbandi> vanban)
        {
            IEnumerable<ListVanbandiViewModel> listvb = vanban.Where(p => p.intpublic == (int)enumVanbandi.intpublic.Public)
                    .GroupJoin(
                        _fileRepo.AttachVanbans
                            .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                            .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbandi),
                        v => 1,
                        f => 1,
                        (v, f) => new { v, f }
                    )
                    .Select(p => new ListVanbandiViewModel
                    {
                        intid = p.v.intid,
                        intso = p.v.intso,
                        strsophu = !string.IsNullOrEmpty(p.v.strmorong) ? p.v.strmorong : "",
                        dtengayky = p.v.strngayky,
                        strkyhieu = p.v.strkyhieu,
                        strnoinhan = p.v.strnoinhan,
                        strtrichyeu = p.v.strtrichyeu,
                        inttrangthai = p.v.inttrangthai,
                        dtehanxuly = p.v.strhanxuly,
                        IsAttach = p.f.Any(a => a.intidvanban == p.v.intid)

                    });
            return listvb;
        }


        /// <summary>
        /// lay nhung van ban ma user co quyen xem 
        /// </summary>
        /// <param name="vanban"></param>
        /// <returns></returns>
        private IQueryable<Vanbandi> _GetQuyenXemVB(IQueryable<Vanbandi> vanban, bool isViewAllvb)
        {
            var vbduocxem = vanban;

            int idcanbo = _session.GetUserId();
            //bool isViewAllvb = _role.IsRole(RoleVanbandi.Xemtatcavb);
            try
            {
                if (isViewAllvb == false)
                {   // lấy các văn bản được cấp quyền xem 
                    // và khong phai văn bản public

                    var vbxem = _vanbandicanboRepo.VanbandiCanbos
                            .Where(p => p.intidcanbo == idcanbo);
                    //.Select(p => p.intidvanban);

                    vbduocxem = vanban
                        .Where(p =>
                               vbxem.Any(c => c.intidvanban == p.intid)
                        //    || p.intpublic == (int)enumVanbandi.intpublic.Public
                        )
                        .Where(p => p.intpublic == (int)enumVanbandi.intpublic.Private)
                        ;

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return vbduocxem;
        }

        public SearchVBViewModel GetViewSearch()
        {
            SearchVBViewModel model = new SearchVBViewModel();
            model.Loaivanban = _plvanbanRepo.GetActivePhanloaiVanbans
                .Where(p => p.intloai == (int)enumPhanloaiVanban.intloai.vanbandi)
                .OrderBy(p => p.strtenvanban);
            model.Sovanban = _sovbRepo.GetActiveSoVanbans
                .Where(p => p.intloai == (int)enumSovanban.intloai.Vanbanphathanh)
                .OrderBy(p => p.strten);
            model.Nguoixuly = _canboRepo.GetActiveCanbo
                .Select(p => new CanboViewModel
                {
                    strhoten = p.strhoten
                });

            model.Vanbankhan = _tinhchatvbRepo.GetActiveTinhchatvanbans
                                    .Where(p => p.intloai == (int)enumTinhchatvanban.intloai.Khan)
                                    .OrderBy(p => p.strtentinhchatvb);
            model.Vanbanmat = _tinhchatvbRepo.GetActiveTinhchatvanbans
                                    .Where(p => p.intloai == (int)enumTinhchatvanban.intloai.Mat)
                                    .OrderBy(p => p.strtentinhchatvb);

            return model;
        }

        #endregion ListVanban

        #region Listvanbanlienquan

        public IEnumerable<ListVanbandilienquanViewModel> GetListVanbandilienquan(
            int idhoso,
            string strngaykycat, int? idloaivb, int? idsovb,
            int? intsobd, int? intsokt, string strngaykybd, string strngaykykt,
            string strkyhieu, string strnguoiky, string strnguoisoan, string strnguoiduyet,
            string strnoinhan, string strtrichyeu, string strhantraloi, string strdonvisoan
            )
        {
            var vanban = _GetVanbandiFromRequest(
                strngaykycat, idloaivb, idsovb, "", "",
                intsobd, intsokt, strngaykybd, strngaykykt,
                strkyhieu, strnguoiky, strnguoisoan, strnguoiduyet,
                strnoinhan, strtrichyeu, strhantraloi, strdonvisoan,
                null, null
                );
            //====================================================
            // chon cac truong tra ve view list van ban
            //====================================================
            var listvb = _GetListViewVanbanlienquan(vanban, idhoso);

            return listvb;
        }

        /// <summary>
        /// tra ve list view van ban di lien quan
        /// </summary>
        /// <param name="vanban"></param>
        /// <returns></returns>
        private IEnumerable<ListVanbandilienquanViewModel> _GetListViewVanbanlienquan(IQueryable<Vanbandi> vanban, int idhoso)
        {
            IEnumerable<ListVanbandilienquanViewModel> listvb = vanban
                    .GroupJoin(
                        _fileRepo.AttachVanbans
                            .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                            .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbandi),
                        v => 1,
                        f => 1,
                        (v, f) => new { v, f }
                    )
                    .GroupJoin(
                        _hsvblqRepo.Hosovanbanlienquans
                            .Where(p => p.intidhosocongviec == idhoso)
                            .Where(p => p.intloai == (int)enumHosovanbanlienquan.intloai.Vanbandi),
                        v2 => 1,
                        vblq => 1,
                        (v2, vblq) => new { v2, vblq }
                    )
                    .Select(p => new ListVanbandilienquanViewModel
                    {
                        intid = p.v2.v.intid,
                        intso = p.v2.v.intso,
                        strsophu = !string.IsNullOrEmpty(p.v2.v.strmorong) ? p.v2.v.strmorong : "",
                        dtengayky = p.v2.v.strngayky,
                        strkyhieu = p.v2.v.strkyhieu,
                        strnoinhan = p.v2.v.strnoinhan,
                        strtrichyeu = p.v2.v.strtrichyeu,
                        inttrangthai = p.v2.v.inttrangthai,
                        dtehanxuly = p.v2.v.strhanxuly,
                        IsAttach = p.v2.f.Any(a => a.intidvanban == p.v2.v.intid),

                        isCheck = p.vblq.Any(a => a.intidvanban == p.v2.v.intid)
                    });

            return listvb;
        }

        public int SaveVBDiLienquan(List<int> listidvanban, int idhosocongviec)
        {
            try
            {
                // kiem tra cac van ban lien quan da co trong ho so
                var listvblq = _hsvblqRepo.Hosovanbanlienquans
                    .Where(p => p.intidhosocongviec == idhosocongviec)
                    .Where(p => p.intloai == (int)enumHosovanbanlienquan.intloai.Vanbandi)
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
                    vblq.intloai = (int)enumHosovanbanlienquan.intloai.Vanbandi;
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
        public IEnumerable<ListVanbandiViewModel> GetHosoVBDiLQ(int idhoso)
        {
            var vanban = _vanbandiRepo.Vanbandis
                .Join(
                        _hsvblqRepo.Hosovanbanlienquans
                            .Where(p => p.intidhosocongviec == idhoso)
                            .Where(p => p.intloai == (int)enumHosovanbanlienquan.intloai.Vanbandi),
                        v => v.intid,
                        vblq => vblq.intidvanban,
                        (v, vblq) => v
                    )
                .GroupJoin(
                        _fileRepo.AttachVanbans
                            .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                            .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbandi),
                        v => 1,
                        f => 1,
                        (v, f) => new { v, f }
                    )
                    .Select(p => new ListVanbandiViewModel
                    {
                        intid = p.v.intid,
                        intso = p.v.intso,
                        strsophu = !string.IsNullOrEmpty(p.v.strmorong) ? p.v.strmorong : "",
                        dtengayky = p.v.strngayky,
                        strkyhieu = p.v.strkyhieu,
                        strnoinhan = p.v.strnoinhan,
                        strtrichyeu = p.v.strtrichyeu,
                        inttrangthai = p.v.inttrangthai,
                        dtehanxuly = p.v.strhanxuly,
                        IsAttach = p.f.Any(a => a.intidvanban == p.v.intid)

                    });

            return vanban;
        }

        #endregion Listvanbanlienquan


        #region ViewDetail

        public DetailVBDiViewModel GetViewDetail(int idvanban)
        {
            int idcanbo = _session.GetUserId();
            bool isView = _role.IsViewVanbandi(idvanban, idcanbo);
            if (isView == false)
            {
                _logger.Warn("không có quyền xem văn bản: " + idvanban.ToString());
                return new DetailVBDiViewModel();
            }

            var cv = _vanbandiRepo.Vanbandis.FirstOrDefault(p => p.intid == idvanban);

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

            //============================================================
            int idvanbanden = 0;
            try
            {
                if (!string.IsNullOrEmpty(cv.strtraloivanbanso))
                {
                    idvanbanden = _hoibaovanbanRepo.Hoibaovanbans
                                    .Where(p => p.intTransID == cv.intid)
                                    .Where(p => p.intloai == (int)enumHoibaovanban.intloai.Vanbandi)
                                    .FirstOrDefault().intRecID;
                }
            }
            catch { }          
            //=====================================================

            IEnumerable<DownloadFileViewModel> downloadFiles = _fileRepo.AttachVanbans
                        .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbandi)
                        .Where(p => p.intidvanban == idvanban)
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
                f.intloai = (int)enumDownloadFileViewModel.intloai.Vanbandi;
            }
            //=====================================================
            List<int> listidvbhoibao = _hoibaovanbanRepo.Hoibaovanbans
                        .Where(p => p.intloai == (int)enumHoibaovanban.intloai.Vanbanden)
                        .Where(p => p.intTransID == cv.intid)
                        .Select(p => p.intRecID).ToList();

            IEnumerable<VanbanhoibaoViewModel> Vanbanhoibaos = _vanbandenRepo.Vanbandens
                        .Where(p => listidvbhoibao.Contains(p.intid))
                        .Select(p => new VanbanhoibaoViewModel
                        {
                            idvanban = p.intid,
                            intsoden = (int)p.intsoden,
                            so_kyhieu = p.strkyhieu,
                            strichyeu = p.strtrichyeu,
                            dtengayden = p.strngayden,
                            dtengayky = p.strngayky,
                            Donvigui = p.strnoiphathanh
                        }).ToList();
            foreach (var v in Vanbanhoibaos)
            {
                v.strngayden = DateServices.FormatDateVN(v.dtengayden);
                v.strngayky = DateServices.FormatDateVN(v.dtengayky);
            }

            //====================================================
            var vanban = new DetailVBDiViewModel();

            vanban.intid = cv.intid;
            vanban.strnguoisoan = cv.strnguoisoan;
            vanban.strdonvisoan = cv.strdonvisoan;
            vanban.intso = (cv.intso > 0) ? (int)cv.intso : 0;
            vanban.strsophu = (!string.IsNullOrEmpty(cv.strmorong)) ? cv.strmorong : string.Empty;
            vanban.strkyhieu = cv.strkyhieu;
            try
            {
                vanban.strngayky = DateServices.FormatDateVN(cv.strngayky);
                if (cv.intidsovanban > 0)
                {
                    vanban.strsovanban = _sovbRepo.GetAllSoVanbans.Where(p => p.intloai == (int)enumSovanban.intloai.Vanbanphathanh)
                           .FirstOrDefault(p => p.intid == cv.intidsovanban).strten;
                }
                if (cv.intidphanloaivanbandi > 0)
                {
                    vanban.strloaivanban = _plvanbanRepo.GetAllPhanloaiVanbans.Where(p => p.intloai == (int)enumPhanloaiVanban.intloai.vanbandi)
                            .FirstOrDefault(p => p.intid == cv.intidphanloaivanbandi).strtenvanban;
                }
            }
            catch { }


            vanban.strtrichyeu = cv.strtrichyeu;
            vanban.strnguoiduyet = cv.strnguoiduyet;
            vanban.strnguoiky = cv.strnguoiky;

            vanban.strvbmat = strvbmat;
            vanban.strvbkhan = strvbkhan;

            vanban.strhantraloi = DateServices.FormatDateVN(cv.strhanxuly);
            vanban.strtraloivanban = cv.strtraloivanbanso;
            vanban.strnoinhan = cv.strnoinhan;

            vanban.strsoban = (cv.intsoban != null) ? cv.intsoban.ToString() : string.Empty;
            vanban.strsoto = (cv.intsoto != null) ? cv.intsoto.ToString() : string.Empty;

            // tra loi cho van ban den
            vanban.idvanbanden = idvanbanden;

            vanban.isattach = _fileRepo.AttachVanbans
                        .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbandi)
                        .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                        .Where(p => p.intidvanban == idvanban)
                        .Any();

            vanban.DownloadFiles = downloadFiles;

            vanban.Vanbanhoibaos = Vanbanhoibaos;

            //============luu tru ====================
            ThongtinLuutruViewModel luutru = new ThongtinLuutruViewModel();
            var _lt = _luutruVanbanRepo.LuutruVanbans
                    .Where(p => p.intloaivanban == (int)enumLuutruVanban.intloaivanban.vanbandi)
                    .Where(p => p.intidvanban == vanban.intid)
                    .FirstOrDefault();
            if (_lt != null)
            {
                luutru.intid = _lt.intid;
                luutru.intdonvibaoquan = _lt.intdonvibaoquan;
                luutru.inthopso = _lt.inthopso;
                luutru.strthoihanbaoquan = _lt.strthoihanbaoquan;
                luutru.strnoidung = _lt.strnoidung;
                luutru.strnguoicapnhat = _canboRepo.GetAllCanboByID((int)_lt.intidnguoicapnhat).strhoten;
                luutru.strngaycapnhat = DateServices.FormatDateVN(_lt.strngaycapnhat);
            }
            else
            {
                luutru.intid = 0;
            }
            vanban.Luutru = luutru;

            return vanban;
        }

        #endregion ViewDetail

        #region DuyetVanban

        public ResultFunction DuyetVanban(int idvanban, int intduyet)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                _vanbandiRepo.Duyet(idvanban, intduyet);
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

        #region Updatevanban

        /// <summary>
        /// load du lieu cua form them moi van ban
        /// </summary>
        /// <param name="idloaivb"></param>
        /// <param name="idsovb"></param>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        public ThemvanbanViewModel GetLoaitruong(int? idloaivb, int? idsovb, int? idvanban)
        {
            ThemvanbanViewModel vanban = new ThemvanbanViewModel();

            if ((idvanban == 0) || (idvanban == null))
            {   // them moi van ban
                vanban.Vanbandi = new Vanbandi();
                vanban.Vanbandi.strngayky = DateTime.Now;
                vanban.IsSave = false;
            }
            else
            {  // cap nhat van ban
                var vb = _vanbandiRepo.Vanbandis
                        .FirstOrDefault(p => p.intid == idvanban);
                vanban.Vanbandi = vb;
                //  chua thay doi loai van ban thi chon loai van ban cu
                if ((idloaivb == 0) || (idloaivb == null))
                {
                    idloaivb = vb.intidphanloaivanbandi;
                }

                if ((idsovb == 0) || (idsovb == null))
                {
                    idsovb = vb.intidsovanban;
                }
            }

            //int? idsovb = vb.intidsovanban;
            string strquytac = AppSettings.TraloiVB;
            vanban.strmota_traloivanban = _ruleFileName.GetQuytacTenFile(strquytac);

            //=============================================================
            vanban.Sovanban = _sovbRepo.GetActiveSoVanbans
                               .Where(p => p.intloai == (int)enumSovanban.intloai.Vanbanphathanh)
                               .OrderBy(p => p.intorder).ToList();
            var sovb = vanban.Sovanban.FirstOrDefault(p => p.IsDefault == true);
            // kiem tra dieu kien id sovb
            if ((idsovb == 0) || (idsovb == null))
            {
                // khi them moi van ban 
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

            vanban.PhanloaiVanban = _plvanbanRepo.GetActivePhanloaiVanbans
                                        .Where(p => p.intloai == (int)enumPhanloaiVanban.intloai.vanbandi)
                                        .OrderBy(p => p.strtenvanban).ToList();
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
                // thay doi so van di theo loai vbdi
                var sovb_loaivb = vanban.Sovanban
                                    .FirstOrDefault(p => p.intidloaivb == vanban.intidloaivanban);
                vanban.intidsovanban = (sovb_loaivb == null) ? vanban.intidsovanban : sovb_loaivb.intid;

            }

            vanban.PhanloaiTruong = _pltruongRepo.PhanloaiTruongs
                                        .Where(p => p.intidphanloaivanban == vanban.intidloaivanban)
                                        .Where(p => p.intloai == (int)enumPhanloaiTruong.intloai.vanbandi)
                                        .Where(p => p.intloaitruong == (int)enumPhanloaiTruong.intloaitruong.Default)
                                        .OrderBy(p => p.intorder);




            //vanban.Tochucdoitac = _tochucRepo.GetActiveTochucdoitacs
            //                            .OrderBy(p => p.strtentochucdoitac);

            //var strtencoquan = coquan.Select(p => new { strtendonvi = string.Join(" || ", p.strmatochucdoitac, p.strtentochucdoitac) });
            //var donvi = strtencoquan.Select(p => p.strtendonvi);

            //vanban.strtochucdoitacModel = donvi.AsEnumerable();


            //try
            //{
            //    var strnoinhan = vanban.Vanbandi.strnoinhan;
            //    string[] split = strnoinhan.Split(new Char[] { ',' });
            //    List<ListTochucdoitacViewModel> donvi = new List<ListTochucdoitacViewModel>();
            //    foreach (var s in split)
            //    {
            //        if (!string.IsNullOrEmpty(s))
            //        {
            //            ListTochucdoitacViewModel d = new ListTochucdoitacViewModel();
            //            d.strten = s;
            //            donvi.Add(d);
            //        }
            //    }

            //    string output = Newtonsoft.Json.JsonConvert.SerializeObject(donvi);
            //    vanban.jsTochuc = output;
            //}
            //catch (Exception ex)
            //{
            //    _logger.Error(ex.Message);
            //}



            vanban.Linhvuc = _linhvucRepo.GetActiveLinhvucs
                                    .OrderBy(p => p.strtenlinhvuc);

            vanban.Vanbankhan = _tinhchatvbRepo.GetActiveTinhchatvanbans
                                    .Where(p => p.intloai == (int)enumTinhchatvanban.intloai.Khan)
                                    .OrderBy(p => p.strtentinhchatvb);
            vanban.Vanbanmat = _tinhchatvbRepo.GetActiveTinhchatvanbans
                                    .Where(p => p.intloai == (int)enumTinhchatvanban.intloai.Mat)
                                    .OrderBy(p => p.strtentinhchatvb);

            vanban.Nguoiduyet = _canboRepo.GetActiveCanbo
                                .Select(p => new CanboViewModel
                                {
                                    intid = p.intid,
                                    strhoten = p.strhoten,
                                    strkyhieu = p.strkyhieu
                                })
                                .OrderBy(p => p.strkyhieu)
                                .ThenBy(p => p.strhoten);

            vanban.Diachiluutru = _luutruRepo.GetActiveDiachiluutrus
                                        .OrderBy(p => p.strtendonvi);

            vanban.Nguoiky = _canboRepo.GetActiveCanbo
                                .Where(p => p.intkivb == (int)enumcanbo.intkivb.Co)
                                .Select(p => new CanboViewModel
                                {
                                    intid = p.intid,
                                    strhoten = p.strhoten,
                                    strkyhieu = p.strkyhieu
                                })
                                .OrderBy(p => p.strkyhieu)
                                .ThenBy(p => p.strhoten);

            vanban.Nguoisoan = vanban.Nguoiduyet;

            vanban.IsQPPL = vanban.Vanbandi.intquyphamphapluat == (int)enumVanbandi.intquyphamphapluat.Co ? true : false;

            return vanban;
        }

        /// <summary>
        /// lay cac truong thay  doi: so di khi thay doi so van ban
        /// </summary>
        /// <param name="idsovb"></param>
        /// <returns></returns>
        public AjaxSovanban GetSovanban(int idsovb)
        {
            bool IsSovbdi = AppSettings.IsSoVBDi;

            if (IsSovbdi)
            {   // mỗi nhiệm kỳ sẽ reset số đi từ 1
                DateTime? dteNgaybd = AppSettings.NgayBDSoVBDi; // Ngày bắt đầu reset số đi từ 1
                AjaxSovanban sovb = new AjaxSovanban();
                int intsovbdi = 0;
                var _intsodi = _vanbandiRepo.Vanbandis
                                .Where(p => p.intidsovanban == idsovb)
                    //.Where(p => p.strngayky.Year == year)
                                .Where(p => p.strngayky >= dteNgaybd)
                                .Max(p => p.intso);
                if ((_intsodi == 0) || (_intsodi == null))
                {
                    intsovbdi = 1;
                }
                else
                {
                    intsovbdi = (int)_intsodi + 1;
                }
                sovb.intso = intsovbdi;
                return sovb;

            }
            else
            {   // mỗi năm sẽ reset số đi từ 1
                AjaxSovanban sovb = new AjaxSovanban();
                int intsovbdi = 0;
                int year = DateTime.Today.Year;
                var _intsodi = _vanbandiRepo.Vanbandis
                                .Where(p => p.intidsovanban == idsovb)
                                .Where(p => p.strngayky.Year == year)
                                .Max(p => p.intso);
                if ((_intsodi == 0) || (_intsodi == null))
                {
                    intsovbdi = 1;
                }
                else
                {
                    intsovbdi = (int)_intsodi + 1;
                }
                sovb.intso = intsovbdi;
                return sovb;
            }

        }

        /// <summary>
        /// lay ten cac don vi de nhap noi nhan vb
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ListTochucdoitacViewModel> GetTenDonvi(string q)
        {
            var tochuc = _tochucRepo.GetActiveTochucdoitacs
                    .Where(p => p.strmatochucdoitac.Contains(q)
                        || p.strtentochucdoitac.Contains(q)
                        )
                    .Select(p => new ListTochucdoitacViewModel
                    {
                        strkyhieu = p.strmatochucdoitac,
                        strten = p.strtentochucdoitac,
                        strkyhieu_ten = p.strmatochucdoitac + " || " + p.strtentochucdoitac
                    })
                    .OrderBy(p => p.strten);
            return tochuc;
        }
        public IEnumerable<ListTochucdoitacViewModel> GetTenDonvi()
        {
            var tochuc = _tochucRepo.GetActiveTochucdoitacs
                    .Select(p => new ListTochucdoitacViewModel
                    {
                        strkyhieu = p.strmatochucdoitac,
                        strten = p.strtentochucdoitac,
                        strkyhieu_ten = p.strmatochucdoitac + " || " + p.strtentochucdoitac
                    })
                    .OrderBy(p => p.strten);
            return tochuc;
        }
        public IEnumerable<ListTochucdoitacViewModel> GetNoinhan(int? idvanban, string strnoinhantiep)
        {
            try
            {
                if (idvanban > 0)
                {
                    var strnoinhan = _vanbandiRepo.Vanbandis.FirstOrDefault(p => p.intid == idvanban).strnoinhan;
                    string[] split = strnoinhan.Split(new Char[] { ',' });
                    List<ListTochucdoitacViewModel> donvi = new List<ListTochucdoitacViewModel>();
                    foreach (var s in split)
                    {
                        if (!string.IsNullOrWhiteSpace(s))
                        {
                            ListTochucdoitacViewModel d = new ListTochucdoitacViewModel();
                            d.strten = s;
                            d.strkyhieu_ten = s;
                            donvi.Add(d);
                        }
                    }
                    return donvi;
                }
                else
                {
                    string[] split = strnoinhantiep.Split(new Char[] { ',' });
                    List<ListTochucdoitacViewModel> donvi = new List<ListTochucdoitacViewModel>();
                    foreach (var s in split)
                    {
                        if (!string.IsNullOrWhiteSpace(s))
                        {
                            ListTochucdoitacViewModel d = new ListTochucdoitacViewModel();
                            d.strten = s;
                            d.strkyhieu_ten = s;
                            donvi.Add(d);
                        }
                    }
                    return donvi;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// save van ban moi
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// 0 : loi
        /// !=0: id vanban sau khi save
        /// </returns>
        public int Savevanban(ThemvanbanViewModel model)
        {
            int intidvanban = 0;
            try
            {
                Vanbandi vb = model.Vanbandi;

                vb.inttrangthai = (int)enumVanbandi.inttrangthai.Daduyet;

                // thong tin mac dinh cua van ban
                vb.intidnguoitao = _session.GetUserId();

                string strtenvanban = " số : " + vb.intso.ToString() + ", ký hiệu: " + vb.strkyhieu + ", ngày ký: " + vb.strngayky;

                // kiem tra so phu (van ban phat hanh lui)
                if (_CheckSophu(vb.intso, vb.strngayky, vb.intidsovanban))
                {
                    vb.strmorong = _Getsophu(vb.intso, vb.strngayky, vb.intidsovanban);
                }

                intidvanban = _vanbandiRepo.Them(vb);

                // phan quyen xem cho nguoi soan, nguoi duyet, nguoi ky
                _PhanquyenxemVBDi(vb.strnguoisoan, vb.strnguoiduyet, vb.strnguoiky, intidvanban);

                // tao lien ket van ban den/di 
                // va dong ho so xu ly
                if (!string.IsNullOrEmpty(vb.strtraloivanbanso))
                {
                    string strquytac = AppSettings.TraloiVB;
                    int idvanbanden = _ruleFileName.GetIdVanban(strquytac, vb.strtraloivanbanso);
                    if (idvanbanden != 0)
                    {
                        _ruleFileName.LienketVanban((int)enumHoibaovanban.intloai.Vanbandi, idvanbanden, intidvanban);
                        //_LienketVanban(idvanbanden, intidvanban);
                        _KetthucHoso(idvanbanden);
                    }
                }
                _logger.Info("Thêm mới văn bản đi" + strtenvanban);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return intidvanban;
        }

        /// <summary>
        /// cap nhat van ban
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Editvanban(int idvanban, Vanbandi model)
        {
            bool kq = false;
            string strtenvanban = "";
            try
            {
                var vb = _vanbandiRepo.Vanbandis
                        .FirstOrDefault(p => p.intid == idvanban);
                string strtraloivbcu = vb.strtraloivanbanso;

                if (vb != null)
                {
                    vb = model;
                }

                // thong tin mac dinh cua van ban
                vb.intidnguoisua = _session.GetUserId();

                strtenvanban = " số : " + vb.intso + ", ký hiệu: " + vb.strkyhieu + ", ngày ký: " + vb.strngayky;
                _logger.Info("Cập nhật văn bản đi" + strtenvanban);
                kq = true; // cap nhat thanh cong
                _vanbandiRepo.Sua(idvanban, vb);

                try
                {
                    // phan quyen xem cho nguoi soan, nguoi duyet, nguoi ky
                    _PhanquyenxemVBDi(vb.strnguoisoan, vb.strnguoiduyet, vb.strnguoiky, idvanban);

                }
                catch { }

                // tao lien ket van ban den/di 
                // va dong ho so xu ly
                if (!string.IsNullOrEmpty(vb.strtraloivanbanso))
                {
                    string strquytac = AppSettings.TraloiVB;
                    if (!string.IsNullOrEmpty(strtraloivbcu))
                    {
                        if (strtraloivbcu != vb.strtraloivanbanso)
                        {   // neu tra loi van moi khac tra loi van ban cu 

                            // xoa lien ket cu va mo lai ho so                             
                            int idvanbandencu = _ruleFileName.GetIdVanban(strquytac, strtraloivbcu);
                            if (idvanbandencu != 0)
                            {
                                _XoaLienketVanban(idvanbandencu, idvanban);
                                //_MolaiHoso(idvanbandencu);

                            }
                            // them lien ket moi
                            int idvanbanden = _ruleFileName.GetIdVanban(strquytac, vb.strtraloivanbanso);
                            if (idvanbanden != 0)
                            {
                                _ruleFileName.LienketVanban((int)enumHoibaovanban.intloai.Vanbandi, idvanbanden, idvanban);
                                //_LienketVanban(idvanbanden, idvanban);
                                _KetthucHoso(idvanbanden);
                            }
                        }
                        else
                        {
                            // neu tra loi vb cu giong tra loi vb moi thi giu nguyen       

                            //int idvanbanden = _ruleFileName.GetIdVanban(strquytac, vb.strtraloivanbanso);
                            //if (idvanbanden != 0)
                            //{
                            //    // xoa het cac lien ket cu
                            //    _XoaLienketVanban(idvanbanden, idvanban);
                            //    // them lien ket moi
                            //    _ruleFileName.LienketVanban((int)enumHoibaovanban.intloai.Vanbandi, idvanbanden, idvanban);
                            //    _KetthucHoso(idvanbanden);
                            //}
                        }
                    }
                    else
                    {   // neu tra loi van ban cu la rong (khong co)
                        // thi them lien ket moi vao
                        int idvanbanden = _ruleFileName.GetIdVanban(strquytac, vb.strtraloivanbanso);
                        if (idvanbanden != 0)
                        {
                            _ruleFileName.LienketVanban((int)enumHoibaovanban.intloai.Vanbandi, idvanbanden, idvanban);
                            //_LienketVanban(idvanbanden, idvanban);
                            _KetthucHoso(idvanbanden);
                        }
                    }
                }
                else
                {   // neu khong nhap tra loi vb 
                    // xoa lien ket cu (neu co)
                    // neu strtraloivb !=  traloivb cu thi mo lai ho so da dong truoc do 
                    if (!string.IsNullOrEmpty(strtraloivbcu))
                    {
                        string strquytac = AppSettings.TraloiVB;
                        int idvanbandencu = _ruleFileName.GetIdVanban(strquytac, strtraloivbcu);
                        if (idvanbandencu != 0)
                        {
                            _XoaLienketVanban(idvanbandencu, idvanban);
                            //_MolaiHoso(idvanbandencu);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //isSucess = false;
                _logger.Error(ex.Message);
            }

            return kq;
        }
        /// <summary>
        /// kiem tra xem so di nay da ton tai chua
        /// </summary>
        /// <param name="intso"></param>
        /// <param name="dtengayky"></param>
        /// <param name="idsovanban"></param>
        /// <returns>true/fasle</returns>
        public bool _CheckSophu(int? intso, DateTime dtengayky, int? idsovanban)
        {
            var vbphu = _vanbandiRepo.Vanbandis
                .Where(p => p.intso == intso)
                .Where(p => p.intidsovanban == idsovanban)
                .Where(p => p.strngayky.Year == dtengayky.Year);
            if (vbphu.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// them so phu vao van ban di khi phat hanh lui
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns>tra ve so phu can them vao</returns>
        private string _Getsophu(int? intso, DateTime dtengayky, int? idsovanban)
        {
            string[] listsophu = { "a","b","c","d","e","f","g","h","i",
                                   "j","k","l","m","n","o","p","q","r",
                                   "s","t","u","v","w","x","y","z" };

            string strsophu = string.Empty;

            var strmorong = _vanbandiRepo.Vanbandis
                    .Where(p => p.intso == intso)
                    .Where(p => p.strngayky.Year == dtengayky.Year)
                    .Where(p => p.intidsovanban == idsovanban)
                    .OrderByDescending(p => p.strmorong)
                    .FirstOrDefault().strmorong;

            if (!string.IsNullOrEmpty(strmorong))
            {
                for (int i = 0; i < listsophu.Length; i++)
                {
                    if (strmorong == listsophu[i])
                    {
                        strsophu = listsophu[i + 1];
                    }
                }
            }
            else
            {
                //strsophu = "a";
                strsophu = listsophu[0];
            }
            return strsophu;
        }

        /// <summary>
        /// them lien ket van ban den/di
        /// HIEN KHONG SU DUNG. DUNG TRONG RULE_FILENAME_MANAGER
        /// </summary>
        /// <param name="strtenvanban"></param>
        /// <param name="idvanbandi"></param>
        private void _LienketVanban(int idvanbanden, int idvanbandi)
        {
            if (idvanbanden != 0)
            {
                var hoibao = _hoibaovanbanRepo.Hoibaovanbans
                            .Where(p => p.intloai == (int)enumHoibaovanban.intloai.Vanbandi)
                            .Where(p => p.intTransID == idvanbandi);
                if (hoibao.Count() != 0)
                {
                    // xoa va them moi
                    foreach (var p in hoibao)
                    {
                        _hoibaovanbanRepo.Xoa((int)enumHoibaovanban.intloai.Vanbandi, idvanbandi, p.intRecID);
                    }
                    // them moi
                    _hoibaovanbanRepo.Them((int)enumHoibaovanban.intloai.Vanbandi, idvanbandi, idvanbanden);
                }
                else
                {
                    // them moi
                    _hoibaovanbanRepo.Them((int)enumHoibaovanban.intloai.Vanbandi, idvanbandi, idvanbanden);
                }
            }
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
                if (idvanbanden != 0)
                {
                    var hoibao = _hoibaovanbanRepo.Hoibaovanbans
                                .Where(p => p.intloai == (int)enumHoibaovanban.intloai.Vanbandi)
                                .Where(p => p.intTransID == idvanbandi)
                                .ToList();
                    if (hoibao.Count() > 0)
                    {
                        // xoa 
                        foreach (var p in hoibao)
                        {
                            _hoibaovanbanRepo.Xoa((int)enumHoibaovanban.intloai.Vanbandi, idvanbandi, p.intRecID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

        }

        /// <summary>
        /// ket thuc hoso van ban den 
        /// </summary>
        /// <param name="strtenvanban"></param>
        private void _KetthucHoso(int idvanbanden)
        {
            if (idvanbanden != 0)
            {
                var hsvb = _hosovanbanRepo.Hosovanbans.Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden)
                            .Where(p => p.intidvanban == idvanbanden).FirstOrDefault();
                if (hsvb != null)
                {
                    var hscv = _hosocongviecRepo.Hosocongviecs
                            .Where(p => p.intloai == (int)enumHosocongviec.intloai.Vanbanden
                                || p.intloai == (int)enumHosocongviec.intloai.Vanbanden_Quytrinh
                            )
                            .Where(p => p.intid == hsvb.intidhosocongviec).FirstOrDefault();
                    if (hscv != null)
                    {
                        int idcanbo = _session.GetUserId();
                        _hosocongviecRepo.HoanthanhHoso(hscv.intid, idcanbo);
                        // ghi nhat ky
                        ChitietHoso ct = new ChitietHoso();
                        ct.intidcanbo = idcanbo;
                        ct.intidhosocongviec = hscv.intid;
                        ct.intnguoitao = idcanbo;
                        ct.intvaitro = (int)enumDoituongxuly.intvaitro_chitiethoso.Phathanhvanban_dongHS;
                        _chitietHosoRepo.Them(ct);
                    }
                }
            }
        }

        /// <summary>
        /// mo lai ho so van ban den
        /// </summary>
        /// <param name="idvanbanden"></param>
        private void _MolaiHoso(int idvanbanden)
        {
            if (idvanbanden != 0)
            {
                var hsvb = _hosovanbanRepo.Hosovanbans.Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden)
                            .Where(p => p.intidvanban == idvanbanden).FirstOrDefault();
                if (hsvb != null)
                {
                    var hscv = _hosocongviecRepo.Hosocongviecs
                            .Where(p => p.intloai == (int)enumHosocongviec.intloai.Vanbanden
                                || p.intloai == (int)enumHosocongviec.intloai.Vanbanden_Quytrinh
                            )
                            .Where(p => p.intid == hsvb.intidhosocongviec).FirstOrDefault();
                    if (hscv != null)
                    {
                        int idcanbo = _session.GetUserId();
                        _hosocongviecRepo.Sua(hscv.intid, hscv, false);
                        // ghi nhat ky
                        ChitietHoso ct = new ChitietHoso();
                        ct.intidcanbo = idcanbo;
                        ct.intidhosocongviec = hscv.intid;
                        ct.intnguoitao = idcanbo;
                        ct.intvaitro = (int)enumDoituongxuly.intvaitro_chitiethoso.MolaiHoso;
                        //ct.strthaotac = "";
                        _chitietHosoRepo.Them(ct);
                    }
                }
            }
        }

        /// <summary>
        /// phan quyen xem van ban cho nguoi soan, nguoi duyet va nguoi ky
        /// </summary>
        /// <param name="strnguoisoan"></param>
        /// <param name="strnguoiduyet"></param>
        /// <param name="strnguoiky"></param>
        /// <param name="idvanban"></param>
        private void _PhanquyenxemVBDi(string strnguoisoan, string strnguoiduyet, string strnguoiky, int idvanban)
        {
            try
            {
                if (!string.IsNullOrEmpty(strnguoiky))
                {
                    int idnguoiky = _canboRepo.GetActiveCanbo
                    .Where(p => p.strhoten == strnguoiky)
                    .FirstOrDefault().intid;
                    if (idnguoiky != 0) { _Save_Log_VanbandiCanbo(idnguoiky, idvanban); }
                }
            }
            catch { }
            try
            {
                if (!string.IsNullOrEmpty(strnguoisoan))
                {
                    int idnguoisoan = _canboRepo.GetActiveCanbo
                    .Where(p => p.strhoten == strnguoisoan)
                    .FirstOrDefault().intid;
                    if (idnguoisoan != 0) { _Save_Log_VanbandiCanbo(idnguoisoan, idvanban); }
                }
            }
            catch { }
            try
            {
                if (!string.IsNullOrEmpty(strnguoiduyet))
                {
                    int idnguoiduyet = _canboRepo.GetActiveCanbo
                    .Where(p => p.strhoten == strnguoiduyet)
                    .FirstOrDefault().intid;
                    if (idnguoiduyet != 0) { _Save_Log_VanbandiCanbo(idnguoiduyet, idvanban); }
                }
            }
            catch { }
        }

        /// <summary>
        /// kiem tra ho ten co trong danh muc khong
        /// neu co thi them vao vanbandi canbo
        /// </summary>
        /// <param name="strhoten"></param>
        /// <param name="idvanban"></param>
        private void _Save_Log_VanbandiCanbo(int idcanbo, int idvanban)
        {
            try
            {
                if (idcanbo != 0)
                {
                    // kiem tra xem co phan quyen xem chua
                    var vb = _vanbandicanboRepo.VanbandiCanbos.Where(p => p.intidcanbo == idcanbo)
                            .Where(p => p.intidvanban == idvanban);
                    if (vb.Count() == 0)
                    {   // chua co thi them moi
                        _vanbandicanboRepo.Them(idvanban, idcanbo);
                        // ghi them nhat ky 
                        _SaveLogChitietVanban((int)enumChitietVanbandi.intvaitro_chitietvanbandi.Capquyenxem, idcanbo, idvanban);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Phân quyền đọc văn bản đi. " + ex.Message);
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
            ChitietVanbandi ct = new ChitietVanbandi();
            ct.intidcanbo = idcanbo;
            ct.intidvanban = idvanban;
            ct.intnguoitao = idnguoitao;
            ct.intvaitro = intvaitro;
            //ct.strngaytao = DateTime.Now; mac dinh
            _chitietVBDiRepo.Them(ct);
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
                case (int)enumChitietVanbandi.intvaitro_chitietvanbandi.Huyquyenxem:
                    intvaitrocu = (int)enumChitietVanbandi.intvaitro_chitietvanbandi.Capquyenxem;
                    break;
                case (int)enumChitietVanbandi.intvaitro_chitietvanbandi.HuyquyenPublic:
                    intvaitrocu = (int)enumChitietVanbandi.intvaitro_chitietvanbandi.CapquyenPublic;
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
                    var ct = _chitietVBDiRepo.ChitietVanbandis
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
                        _chitietVBDiRepo.Sua(ct.intid, ct);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
            }

        }

        public QLVB.DTO.Vanbandi.DeleteVBViewModel GetDeleteVanban(int id)
        {
            var vb = _vanbandiRepo.Vanbandis
                .FirstOrDefault(p => p.intid == id);
            if (vb != null)
            {
                QLVB.DTO.Vanbandi.DeleteVBViewModel model = new QLVB.DTO.Vanbandi.DeleteVBViewModel
                {
                    intid = id,
                    strtenvanban = vb.intso.ToString() + " / " + vb.strkyhieu
                    + ", ngày ký " + DateServices.FormatDateVN(vb.strngayky)
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
                var vb = _vanbandiRepo.Vanbandis.FirstOrDefault(p => p.intid == id);
                string strtenvanban = vb.intso.ToString() + "/" + vb.strkyhieu
                    + ", ngày ký " + vb.strngayky.ToString() + ", id " + id.ToString();
                // moi xoa van ban
                // chua xoa cac quan he xu ly
                _vanbandiRepo.Xoa(id);
                _logger.Info("Xóa văn bản đi: " + strtenvanban);
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
        /// kiem tra lien ket van ban
        /// </summary>
        /// <param name="idvanbandi">id van ban di</param>
        /// <returns>
        /// id van ban den
        /// 0: khong co
        /// >0: da lien ket
        /// </returns>
        public int CheckLienketVanban(int idvanbandi)
        {
            int idvanbanden = 0;
            try
            {
                idvanbanden = _hoibaovanbanRepo.Hoibaovanbans
                                    .Where(p => p.intTransID == idvanbandi)
                                    .Where(p => p.intloai == (int)enumHoibaovanban.intloai.Vanbandi)
                                    .FirstOrDefault().intRecID;
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
            }
            return idvanbanden;
        }

        /// <summary>
        /// kiem tra tra loi van ban co ton tai chua
        /// </summary>
        /// <param name="strTraloivb"></param>
        /// <param name="idvanbandi"></param>
        /// <returns></returns>
        public TraloiVanbanViewModel CheckTraloiVanban(string strTraloivb, int idvanbandi)
        {
            TraloiVanbanViewModel model = new TraloiVanbanViewModel();
            try
            {
                // tao lien ket van ban den/di                 
                if (!string.IsNullOrEmpty(strTraloivb))
                {
                    string strquytac = AppSettings.TraloiVB;
                    int idvanbanden = _ruleFileName.GetIdVanban(strquytac, strTraloivb);
                    if (idvanbanden > 0)
                    {   // tim thay van ban den
                        // kiem tra xem da co van ban di chua
                        int _idvanbandi = _hoibaovanbanRepo.Hoibaovanbans
                                        .Where(p => p.intRecID == idvanbanden)
                                        .Where(p => p.intloai == (int)enumHoibaovanban.intloai.Vanbandi)
                                        .FirstOrDefault().intTransID;
                        if (_idvanbandi > 0)
                        {
                            if (_idvanbandi == idvanbandi)
                            {
                                // van ban di dang chinh sua thi bo qua
                                model.intloai = (int)TraloiVanbanViewModel.intTrangthai.Khong;
                            }
                            else
                            {
                                var vb = _vanbandiRepo.Vanbandis.FirstOrDefault(p => p.intid == _idvanbandi);

                                model.intloai = (int)TraloiVanbanViewModel.intTrangthai.Co;
                                model.strSoKyhieu = vb.intso.ToString() + "/" + vb.strkyhieu;
                                model.strNgay = DateServices.FormatDateVN(vb.strngayky);
                                model.strNguoisoan = vb.strnguoisoan;
                            }                            
                        }
                        else
                        {
                            model.intloai = (int)TraloiVanbanViewModel.intTrangthai.Khong;
                        }
                    }
                    else
                    {
                        // khong tim thay van ban den
                        model.intloai = (int)TraloiVanbanViewModel.intTrangthai.KhongthayVBDen;
                    }
                }
            }
            catch { }

            return model;
        }

        #endregion Updatevanban

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

                        .GroupJoin(
                            _vanbandicanboRepo.VanbandiCanbos
                                    .Where(p => p.intidvanban == idvanban),
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
                        .OrderBy(p => p.intlevel)
                        .ThenBy(p => p.ParentId)
                        .ThenBy(p => p.strtendonvi)
                        ;

            user.maxLevelDonvi = _donviRepo.Donvitructhuocs
                        .Where(p => p.inttrangthai == (int)enumDonvitructhuoc.inttrangthai.IsActive)
                        .Max(p => p.intlevel);

            var intpublic = _vanbandiRepo.Vanbandis.FirstOrDefault(p => p.intid == idvanban).intpublic;
            string strpublicbtn = string.Empty;
            if (intpublic == (int)enumVanbandi.intpublic.Public)
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
                    var canbo = _vanbandicanboRepo.VanbandiCanbos.Where(p => p.intidvanban == idvanban).Select(p => p.intidcanbo);
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
                    var canbo = _vanbandicanboRepo.VanbandiCanbos.Where(p => p.intidvanban == idvanban).Select(p => p.intidcanbo);

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
                        var canbo2 = _vanbandicanboRepo.VanbandiCanbos
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
                _vanbandicanboRepo.Them(idvanban, idcanbo);
                // ghi nhat ky trong chi tiet van ban di
                _SaveLogChitietVanban((int)enumChitietVanbandi.intvaitro_chitietvanbandi.Capquyenxem, idcanbo, idvanban);
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
                _vanbandicanboRepo.Xoa(idvanban, idcanbo);
                // ghi nhat ky 
                _Log_Huyquyenxem((int)enumChitietVanbandi.intvaitro_chitietvanbandi.Huyquyenxem, idcanbo, idvanban);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
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
            if (intpublic == (int)enumVanbandi.intpublic.Private)
            {
                intcongcong = (int)enumVanbandi.intpublic.Public;
                intvaitro = (int)enumChitietVanbandi.intvaitro_chitietvanbandi.CapquyenPublic;
            }
            if (intpublic == (int)enumVanbandi.intpublic.Public)
            {
                intcongcong = (int)enumVanbandi.intpublic.Private;
                intvaitro = (int)enumChitietVanbandi.intvaitro_chitietvanbandi.HuyquyenPublic;
            }
            try
            {
                _vanbandiRepo.CapquyenxemPublic(idvanban, intcongcong);
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
        public UploadVBDiViewModel GetListFile(int idvanban)
        {
            UploadVBDiViewModel model = new UploadVBDiViewModel();
            model.idvanban = idvanban;

            var fileAttach = _fileRepo.AttachVanbans
                            .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbandi)
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


        #endregion

        #region Email

        /// <summary>
        /// ds cac don vi gui vbdt idvanban
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        public ListEmailDonviViewModel GetListEmailDonvi(int idvanban)
        {
            ListEmailDonviViewModel model = new ListEmailDonviViewModel();
            model.idvanban = idvanban;
            model.IsAutoSend = AppSettings.IsAutoSendMail;

            var vanban = _vanbandiRepo.Vanbandis.FirstOrDefault(p => p.intid == idvanban);
            model.strnoidung = vanban.intso + "/" + vanban.strkyhieu + " ngày " + DateServices.FormatDateVN(vanban.strngayky);
            model.strtieude = vanban.strtrichyeu;
            model.strnoinhan = vanban.strnoinhan;

            model.listfile = _mailFormat.GetFileVanbanToAttach(idvanban, (int)enumAttachVanban.intloai.Vanbandi);


            var alldonvi = _tochucRepo.GetActiveTochucdoitacs
                .Where(p =>
                    p.Isvbdt == (int)enumTochucdoitac.isvbdt.IsActive
                    || (p.stremail.Length > 5) //(p.stremail != null || p.stremail != "")
                )
                .Select(p => new EmailDonviViewModel
                {
                    iddonvi = p.intid,
                    intloai = p.intidkhoi,
                    strtendonvi = p.strtentochucdoitac,
                    IsVbdt = p.Isvbdt == (int)enumTochucdoitac.isvbdt.IsActive ? true : false,
                    stremailvbdt = string.IsNullOrEmpty(p.stremailvbdt) ? string.Empty : p.stremailvbdt,
                    stremail = string.IsNullOrEmpty(p.stremail) ? string.Empty : p.stremail,
                    strmatructinh = p.strmatructinh,
                    strmadinhdanh = p.strmadinhdanh
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
                .Select(p => new EmailDonviViewModel
                {
                    iddonvi = p.tc.iddonvi,
                    intloai = p.tc.intloai,
                    strtendonvi = p.tc.strtendonvi,
                    IsVbdt = p.tc.IsVbdt,
                    stremailvbdt = string.IsNullOrEmpty(p.tc.stremailvbdt) ? string.Empty : p.tc.stremailvbdt,
                    stremail = string.IsNullOrEmpty(p.tc.stremail) ? string.Empty : p.tc.stremail,
                    IsSend = p.g.Any(a => a.intiddonvi == p.tc.iddonvi),
                    strmatructinh = p.tc.strmatructinh,
                    strmadinhdanh = p.tc.strmadinhdanh
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
               .Select(p => new EmailDonviViewModel
               {
                   iddonvi = p.tc.iddonvi,
                   intloai = p.tc.intloai,
                   strtendonvi = p.tc.strtendonvi,
                   IsVbdt = p.tc.IsVbdt,
                   stremailvbdt = string.IsNullOrEmpty(p.tc.stremailvbdt) ? string.Empty : p.tc.stremailvbdt,
                   stremail = string.IsNullOrEmpty(p.tc.stremail) ? string.Empty : p.tc.stremail,
                   IsSend = p.g.Any(a => a.intiddonvi == p.tc.iddonvi),
                   strmatructinh = p.tc.strmatructinh,
                   strmadinhdanh = p.tc.strmadinhdanh
               })
               .OrderBy(p => p.strtendonvi);

            return model;
        }

        /// <summary>
        /// lay id, dia chi cua cac don vi
        /// </summary>
        /// <param name="listiddonvi"></param>
        /// <returns></returns>
        public IEnumerable<ListSendDonviViewModel> GetListSendDonvi(List<int> listiddonvi)
        {
            var alldonvi = _tochucRepo.GetActiveTochucdoitacs
                .Where(p =>
                    p.Isvbdt == (int)enumTochucdoitac.isvbdt.IsActive
                    || (p.stremail.Length > 5)
                )
                .Select(p => new EmailDonviViewModel
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
                    .Select(p => new ListSendDonviViewModel
                    {
                        intid = p.iddonvi,
                        stremailvbdt = p.IsVbdt == true ? p.stremailvbdt : p.stremail,
                        isvbdt = p.IsVbdt
                    });

            return cacdonvi;
        }


        #endregion Email

        #region ListVBDientu

        public IEnumerable<ListVanbandiViewModel> GetListVBDientuDonvi(
            int? iddonvi, string strngaykybd, string strngaykykt, int? intSoNgaygui
            )
        {
            DateTime? dtengaykybd = DateServices.FormatDateEn(strngaykybd);
            DateTime? dtengaykykt = DateServices.FormatDateEn(strngaykykt);
            strngaykybd = DateServices.FormatDateEn(dtengaykybd);
            strngaykykt = DateServices.FormatDateEn(dtengaykykt);

            string sql = string.Empty;

            var vb = _vanbandiRepo.Vanbandis
                    .Where(p => p.strngayky >= dtengaykybd)
                    .Where(p => p.strngayky <= dtengaykykt)
                    .Join(
                        _guivbRepo.GuiVanbans
                            .Where(p => p.intiddonvi == iddonvi)
                            .Where(p => p.intloaivanban == (int)enumGuiVanban.intloaivanban.Vanbandi),
                        v => v.intid,
                        d => d.intidvanban,
                        (v, d) => new { v, d }
                    );
            var vanban = vb.Select(p => p.v);
            if (intSoNgaygui >= 0)
            {
                vanban = vb.Where(p => System.Data.Entity.DbFunctions.DiffDays(p.v.strngayky, p.d.strngaygui) <= intSoNgaygui)
                    .Select(p => p.v);
            }

            var vbdonvi = _GetViewVanban(vanban);


            return vbdonvi;
        }

        public IEnumerable<ListVanbandiViewModel> GetListLoaiVBDientu(
           int? idloai, string strngaykybd, string strngaykykt)
        {
            DateTime? dtengaykybd = DateServices.FormatDateEn(strngaykybd);
            DateTime? dtengaykykt = DateServices.FormatDateEn(strngaykykt);

            var vb = _vanbandiRepo.Vanbandis
                .Where(p => p.strngayky >= dtengaykybd)
                .Where(p => p.strngayky <= dtengaykykt);
            //.Where(p => p.intguivbdt == idloai);

            switch (idloai)
            {
                case (int)enumVanbandi.intguivbdt.Chuagui:
                    vb = vb.Where(p => p.intguivbdt != (int)enumVanbandi.intguivbdt.Dagui);
                    break;
                case (int)enumVanbandi.intguivbdt.Dagui:
                    vb = vb.Where(p => p.intguivbdt == (int)enumVanbandi.intguivbdt.Dagui);
                    break;
                default:
                    break;
            }

            var vanban = _GetViewVanban_VBDT(vb);
            return vanban;
        }

        private IEnumerable<ListVanbandiViewModel> _GetViewVanban_VBDT(IQueryable<Vanbandi> vanban)
        {
            IEnumerable<ListVanbandiViewModel> listvb = vanban
                    .GroupJoin(
                        _fileRepo.AttachVanbans
                            .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                            .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbandi),
                        v => 1,
                        f => 1,
                        (v, f) => new { v, f }
                    )
                    .GroupJoin(
                        _tinhchatvbRepo.GetAllTinhchatvanbans,
                        v => 1,
                        t => 1,
                        (v,t)=> new {v,t}
                    )
                    .Select(p => new ListVanbandiViewModel
                    {
                        intid = p.v.v.intid,
                        intso = p.v.v.intso,
                        strsophu = !string.IsNullOrEmpty(p.v.v.strmorong) ? p.v.v.strmorong : "",
                        dtengayky = p.v.v.strngayky,
                        strkyhieu = p.v.v.strkyhieu,
                        strnoinhan = p.v.v.strnoinhan,
                        strtrichyeu = p.v.v.strtrichyeu,
                        inttrangthai = p.v.v.inttrangthai,
                        dtehanxuly = p.v.v.strhanxuly,
                        IsAttach = p.v.f.Any(a => a.intidvanban == p.v.v.intid),

                        strDomat = p.t.FirstOrDefault(a=>a.intid == p.v.v.intidmat).strtentinhchatvb // them truong do mat de xem vbdt
                    });
            return listvb;
        }

        #endregion ListVBDientu


    }
}
