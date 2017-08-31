using QLVB.Common.Date;
using QLVB.Common.Logging;
using QLVB.Common.Sessions;
using QLVB.Common.Utilities;
using QLVB.Core.Contract;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.File;
using QLVB.DTO.Hoso;
using QLVB.DTO.Quytrinh;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QLVB.Core.Implementation
{
    public class HosoManager : IHosoManager
    {
        #region Constructor

        private ILogger _logger;
        private ISessionServices _session;
        private IHosocongviecRepository _hosocongviecRepo;
        private IHosovanbanRepository _hosovanbanRepo;
        private IHosoykienxulyRepository _hosoykienRepo;
        private IVanbandenRepository _vanbandenRepo;
        private ILinhvucRepository _linhvucRepo;
        private ICanboRepository _canboRepo;
        private IChucdanhRepository _chucdanhRepo;
        private IDoituongxulyRepository _doituongRepo;

        private IDonvitructhuocRepository _donviRepo;
        private IConfigRepository _configRepo;
        private IChitietHosoRepository _chitietHosoRepo;
        private IRoleManager _role;
        private IPhieutrinhRepository _phieutrinhRepo;
        private IAttachHosoRepository _fileHsRepo;
        private ITonghopCanboRepository _tonghopRepo;
        private IHosovanbanlienquanRepository _hsvblqRepo;

        private IPhanloaiQuytrinhRepository _loaiquytrinhRepo;
        private IQuytrinhRepository _quytrinhRepo;
        private IQuytrinhNodeRepository _qtNodeRepo;
        private IQuytrinhConnectionRepository _qtConnectionRepo;
        private IQuytrinhXulyRepository _qtXulyRepo;
        private IHosoQuytrinhXulyRepository _hsqtRepo;
        private IQuytrinhVersionRepository _quytrinhVersionRepo;
        private IHosoQuytrinhRepository _hosoQuytrinhRepo;
        private IPhanloaiVanbanRepository _plvanbanRepo;
        private IVanbandiRepository _vanbandiRepo;
        private IRuleFileNameManager _ruleFileName;
        private ISoVanbanRepository _sovbRepo;
        private IAttachVanbanRepository _fileVBRepo;

        private IFileManager _fileManager;

        public HosoManager(ILogger logger, IHosocongviecRepository hosocvRepo,
            IHosovanbanRepository hosovbRepo, IHosoykienxulyRepository hosoykienRepo,
            IVanbandenRepository vanbandenRepo, ILinhvucRepository linhvucRepo,
            ICanboRepository canboRepo, IChucdanhRepository chucdanhRepo,
            IDoituongxulyRepository doituongRepo, IDonvitructhuocRepository donviRepo,
            IConfigRepository configRepo, IChitietHosoRepository chitietHosoRepo,
            IRoleManager role, IPhieutrinhRepository phieutrinhRepo,
            IAttachHosoRepository fileHsRepo, ISessionServices session,
            ITonghopCanboRepository tonghopRepo, IHosovanbanlienquanRepository hsvblqRepo,
            IPhanloaiQuytrinhRepository loaiquytrinhRepo, IQuytrinhRepository quytrinhRepo,
            IQuytrinhNodeRepository qtNodeRepo, IQuytrinhConnectionRepository qtConnectionRepo,
            IQuytrinhXulyRepository qtXulyRepo, IHosoQuytrinhXulyRepository hsqtRepo,
            IFileManager fileManager, IQuytrinhVersionRepository qtVersionRepo,
            IHosoQuytrinhRepository hosoquytrinhRepo, IPhanloaiVanbanRepository plvanbanRepo,
            IVanbandiRepository vbdiRepo, IRuleFileNameManager ruleFileName,
            ISoVanbanRepository sovbRepo, IAttachVanbanRepository fileVBRepo)
        {
            _logger = logger;
            _hosocongviecRepo = hosocvRepo;
            _hosovanbanRepo = hosovbRepo;
            _hosoykienRepo = hosoykienRepo;
            _vanbandenRepo = vanbandenRepo;
            _linhvucRepo = linhvucRepo;
            _canboRepo = canboRepo;
            _chucdanhRepo = chucdanhRepo;
            _doituongRepo = doituongRepo;
            _donviRepo = donviRepo;
            _configRepo = configRepo;
            _chitietHosoRepo = chitietHosoRepo;
            _role = role;
            _phieutrinhRepo = phieutrinhRepo;
            _fileHsRepo = fileHsRepo;
            _session = session;
            _tonghopRepo = tonghopRepo;
            _hsvblqRepo = hsvblqRepo;
            _loaiquytrinhRepo = loaiquytrinhRepo;
            _quytrinhRepo = quytrinhRepo;
            _qtNodeRepo = qtNodeRepo;
            _qtConnectionRepo = qtConnectionRepo;
            _qtXulyRepo = qtXulyRepo;
            _hsqtRepo = hsqtRepo;
            _fileManager = fileManager;
            _quytrinhVersionRepo = qtVersionRepo;
            _hosoQuytrinhRepo = hosoquytrinhRepo;
            _plvanbanRepo = plvanbanRepo;
            _vanbandiRepo = vbdiRepo;
            _ruleFileName = ruleFileName;
            _sovbRepo = sovbRepo;
            _fileVBRepo = fileVBRepo;
        }

        #endregion Constructor

        #region Index

        public CategoryCongviecViewModel GetCategoryCongviec()
        {
            CategoryCongviecViewModel model = new CategoryCongviecViewModel();
            model.VaitroXuly = new VaitroXuly();
            model.Songayhienthi = _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);
            return model;
        }

        public IEnumerable<ListCongviecViewModel> GetListCongviec(
            string strngayhscat, string strxuly, int? intsobd, int? intsokt, string strngayhsbd, string strngayhskt,
            string strtieude, string strhantraloi, int? idlinhvuc
            )
        {
            var hoso = _GetHosoFromRequest(
                strngayhscat, strxuly, intsobd, intsokt, strngayhsbd, strngayhskt,
                    strtieude, strhantraloi, idlinhvuc);

            var listcongviec = _GetListViewCongviec(hoso);

            return listcongviec;
        }


        private IQueryable<Hosocongviec> _GetHosoFromRequest(
            string strngayhscat, string strxuly, int? intsobd, int? intsokt, string strngayhsbd, string strngayhskt,
            string strtieude, string strhantraloi, int? idlinhvuc
            )
        {
            bool isSearch = false;
            bool isCategory = false;
            string strSearchValues = string.Empty;
            // strSearchValues = "intsodenbd=1;intsodenkt=10;idloaivb=2;"
            //===========================================================
            var hoso = _hosocongviecRepo.Hosocongviecs;
            //====================================================
            // tuy chon category 
            //====================================================
            if (!string.IsNullOrEmpty(strngayhscat))
            {
                DateTime? dtengayden = DateServices.FormatDateEn(strngayhscat);
                hoso = hoso.Where(p => p.strngaymohoso == dtengayden);
                isSearch = true;
                isCategory = true;
                strSearchValues += "strngayhscat=" + strngayhscat + ";";
            }
            // tinh trang xu ly
            if (!string.IsNullOrEmpty(strxuly))
            {
                switch (strxuly)
                {
                    case "xulychinh":
                        //hoso = hoso.
                        break;
                    case "phoihopxuly":

                        break;
                    case "canxuly":
                        hoso = hoso.Where(p => p.inttrangthai == (int)enumHosocongviec.inttrangthai.Dangxuly);
                        break;
                    case "hoanthanh":
                        hoso = hoso.Where(p => p.inttrangthai == (int)enumHosocongviec.inttrangthai.Dahoanthanh);
                        break;
                }
                isSearch = true;
                isCategory = true;
                strSearchValues += "strxuly=" + strxuly + ";";
            }
            //====================================================
            // Search van ban
            //====================================================
            //if ((intsokt != null) && (intsokt != 0))
            //{
            //    if ((intsobd != null) && (intsobd != 0))
            //    {
            //        vanban = vanban.Where(p => p.intsoden >= intsobd)
            //                .Where(p => p.intsoden <= intsokt);
            //        isSearch = true;
            //        strSearchValues += "intsobd=" + intsobd.ToString() + ";intsokt=" + intsokt.ToString() + ";";
            //    }
            //}
            //else
            //{
            //    if ((intsobd != null) && (intsobd != 0))
            //    {
            //        vanban = vanban.Where(p => p.intsoden == intsobd);
            //        isSearch = true;
            //        strSearchValues += "intsbd=" + intsobd.ToString() + ";";
            //    }
            //}

            if (!string.IsNullOrEmpty(strngayhskt))
            {
                if (!string.IsNullOrEmpty(strngayhsbd))
                {
                    DateTime? dtngayhsbd = DateServices.FormatDateEn(strngayhsbd);
                    DateTime? dtngayhskt = DateServices.FormatDateEn(strngayhskt);
                    hoso = hoso.Where(p => p.strngaymohoso >= dtngayhsbd)
                            .Where(p => p.strngaymohoso <= dtngayhskt);
                    isSearch = true;
                    strSearchValues += "strngayhsbd=" + strngayhsbd + ";strngayhskt=" + strngayhskt + ";";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(strngayhsbd))
                {
                    DateTime? dtngayhsbd = DateServices.FormatDateEn(strngayhsbd);
                    hoso = hoso.Where(p => p.strngaymohoso == dtngayhsbd);
                    isSearch = true;
                    strSearchValues += "strngayhsbd=" + strngayhsbd + ";";
                }
            }

            if (!string.IsNullOrEmpty(strtieude))
            {
                hoso = hoso.Where(p => p.strtieude.Contains(strtieude));
                isSearch = true;
                strSearchValues += "strtieude=" + strtieude + ";";
            }
            if (!string.IsNullOrEmpty(strhantraloi))
            {
                DateTime? dtngaytraloi = DateServices.FormatDateEn(strhantraloi);
                hoso = hoso.Where(p => p.strthoihanxuly == dtngaytraloi);
                //.Where(p => p.strthoihanxuly <= dtngaytraloi);
                isSearch = true;
                strSearchValues += "strhantraloi=" + strhantraloi + ";";
            }
            if (idlinhvuc > 0)
            {
                hoso = hoso.Where(p => p.intlinhvuc == idlinhvuc);
                isSearch = true;
                strSearchValues += "idlinhvuc=" + idlinhvuc.ToString() + ";";
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
                hoso = hoso.Where(p => p.strngaymohoso >= dtengaybd);

                // reset session
                _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.NoSearch);
            }
            else
            {   // luu cac gia tri search vao session

                _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.SearchHSCV);
                _session.InsertObject(AppConts.SessionSearchTypeValues, strSearchValues);


                // tim kiem thi hien thi tat ca

                // Category thi gioi han ngay hien thi
                if (isCategory)
                {
                    int intngay = _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);
                    DateTime? dtengaybd = DateTime.Now.AddDays(-intngay);
                    hoso = hoso.Where(p => p.strngaymohoso >= dtengaybd);
                }
            }
            return hoso;
        }

        private IEnumerable<ListCongviecViewModel> _GetListViewCongviec(IQueryable<Hosocongviec> hoso)
        {
            int idcanbo = _session.GetUserId();

            var hso = hoso
                .Where(p => p.intloai == (int)enumHosocongviec.intloai.Giaiquyetcongviec)
                .Join(
                    _doituongRepo.Doituongxulys.Where(p => p.intidcanbo == idcanbo),
                    cv => cv.intid,
                    dt => dt.intidhosocongviec,
                    (cv, dt) => cv
                )
                .GroupJoin(
                    _canboRepo.GetAllCanbo,
                    hs => hs.intidnguoinhap,
                    cb => cb.intid,
                    (hs, cb) => new { hs, cb.FirstOrDefault().strhoten }
                )
                .Select(p => new ListCongviecViewModel
                {
                    intid = p.hs.intid,
                    dtehanxuly = p.hs.strthoihanxuly,
                    dtengayhs = p.hs.strngaymohoso,
                    intso = p.hs.intsoden,
                    inttrangthai = p.hs.inttrangthai,
                    strtieude = p.hs.strtieude,
                    idnguoinhap = (int)p.hs.intidnguoinhap,
                    strnguoinhap = p.strhoten
                })
                ;

            return hso;
        }

        public SearchCongviecViewModel GetViewSearch()
        {
            SearchCongviecViewModel model = new SearchCongviecViewModel();

            model.ListLinhvuc = _linhvucRepo.GetActiveLinhvucs
                .Select(p => new QLVB.DTO.Linhvuc.EditLinhvucViewModel
                {
                    intid = p.intid,
                    strtenlinhvuc = p.strtenlinhvuc
                })
                .OrderBy(p => p.strtenlinhvuc)
                ;

            return model;
        }

        public ResultFunction DeleteHoso(int idhoso)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                string strtieude = _hosocongviecRepo.Xoa(idhoso);
                _logger.Info("Xóa hồ sơ : " + strtieude + ". id: " + idhoso);
                kq.id = (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
            }
            return kq;
        }
        #endregion Index

        #region ViewDetailHosocongviec

        /// <summary>
        /// hien thi thong thi chi tiet va qua trinh xu ly cua ho so
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <returns></returns>
        public DetailHosoViewModel GetDetailHoso(int idhosocongviec)
        {
            // kiem tra user co quyen xem ?
            int idcanbo = _session.GetUserId();
            if (!_role.IsViewHosocongviec(idhosocongviec, idcanbo))
            {
                _logger.Warn(AppConts.ErrLog + " hồ sơ: " + idhosocongviec.ToString());
                var kq = new DetailHosoViewModel();
                kq.idhosocongviec = 0;
                return kq;
            }

            //==============================================
            // phai su dung GetAll cac table de view cac
            // truong co the da bi xoa
            //==============================================

            string strtennguoihoanthanh = "";
            string strtenlinhvuc = "";
            string strmucquantrong = "";
            string strldphutrach = "";
            string strphoihopxuly = "";
            string strtrangthai = "";
            //int intiddonvi = (int)_session.GetObject(AppConts.SessionDonviId);

            var hs = _hosocongviecRepo.Hosocongviecs
                    .FirstOrDefault(p => p.intid == idhosocongviec);

            if ((hs.inttrangthai == (int)enumHosocongviec.inttrangthai.Dahoanthanh)
                    && (hs.intidnguoihoanthanh != null) && (hs.intidnguoihoanthanh != 0))
            {
                //strtennguoihoanthanh = _canboRepo.GetAllCanbo.FirstOrDefault(p => p.intid == hs.intidnguoihoanthanh).strhoten;
                strtennguoihoanthanh = _canboRepo.GetAllCanboByID((int)hs.intidnguoihoanthanh).strhoten;
            }
            if ((hs.intlinhvuc != null) && (hs.intlinhvuc != 0))
            {
                strtenlinhvuc = _linhvucRepo.GetAllLinhvucs.FirstOrDefault(p => p.intid == hs.intlinhvuc).strtenlinhvuc;
            }
            if ((hs.intmucdo != null) && (hs.intmucdo != 0))
            {
                //strmucquantrong =
            }

            //========lanh dao phu trach=======================================
            var lanhdaophutrach = _canboRepo.GetAllCanbo
                            .Join(
                                _doituongRepo.Doituongxulys
                                                .Where(p => p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaophutrach)
                                                .Where(p => p.intidhosocongviec == idhosocongviec),
                                c => c.intid,
                                d => d.intidcanbo,
                                (c, d) => c
                            );
            if (lanhdaophutrach.Count() != 0)
            {
                foreach (var ld in lanhdaophutrach)
                {
                    strldphutrach += ld.strhoten + ", ";
                }
                int len = strldphutrach.Length - 2;
                strldphutrach = strldphutrach.Substring(0, len);
            }
            //=========phoi hop xu ly ===================================
            var phoihopxuly = _canboRepo.GetAllCanbo
                    .Join(
                        _doituongRepo.Doituongxulys
                                        .Where(p => p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Phoihopxuly)
                                        .Where(p => p.intidhosocongviec == idhosocongviec),
                        c => c.intid,
                        d => d.intidcanbo,
                        (c, d) => c
                    );
            if (phoihopxuly.Count() != 0)
            {
                foreach (var ph in phoihopxuly)
                {
                    strphoihopxuly += ph.strhoten + ", ";
                }
                int len = strphoihopxuly.Length - 2;
                strphoihopxuly = strphoihopxuly.Substring(0, len);
            }
            //=========luan chuyen van ban========================================
            var dtxl = _GetLuanchuyenvanbanModel(idhosocongviec);

            //========ho so y kien xu ly ===================================
            var hosoykien = _GetHosoykienxulyModel(idhosocongviec);

            //======================================================
            DetailHosoViewModel detail = new DetailHosoViewModel();
            try
            {
                detail.idhosocongviec = idhosocongviec;
                detail.strnguoihoanthanh = strtennguoihoanthanh;
                detail.intloai = hs.intloai;
                detail.strsohieuht = hs.strsohieuht;
                detail.strtieude = hs.strtieude;
                //strtrangthai = strtrangthai,
                detail.strthoihanxuly = DateServices.FormatDateVN(hs.strthoihanxuly);
                detail.strngaymohoso = DateServices.FormatDateVN(hs.strngaymohoso);
                //strngayketthuc = DateServices.FormatDateTimeVN(hs.strngayketthuc),
                detail.strketqua = hs.strketqua;
                detail.strnoidung = hs.strnoidung;
                //strtendonvi = _donviRepo.Donvitructhuocs
                //        .FirstOrDefault(p => p.Id == intiddonvi)
                //        .strtendonvi,
                detail.strtenlinhvuc = strtenlinhvuc;
                detail.strmucquantrong = strmucquantrong;

                if ((hs.intloai == (int)enumHosocongviec.intloai.Vanbanden)
                    || hs.intloai == (int)enumHosocongviec.intloai.Vanbanden_Quytrinh)
                {
                    var vanban = _vanbandenRepo.Vanbandens
                        .Join(
                            _hosovanbanRepo.Hosovanbans.Where(p => p.intidhosocongviec == idhosocongviec),
                            v => v.intid,
                            h => h.intidvanban,
                            (v, h) => v
                        ).FirstOrDefault();

                    detail.strtrichyeuvanbanden = vanban.strtrichyeu;
                    detail.idvanbanden = vanban.intid;
                }
                else
                {
                    detail.idvanbanden = 0;
                }

                //=================================
                var lanhdaogiaoviec = _canboRepo.GetAllCanbo
                            .Join(
                                _doituongRepo.Doituongxulys
                                                .Where(p => p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaogiaoviec)
                                                .Where(p => p.intidhosocongviec == idhosocongviec),
                                c => c.intid,
                                d => d.intidcanbo,
                                (c, d) => c
                            ).FirstOrDefault();
                if (lanhdaogiaoviec != null)
                {
                    detail.strlanhdaogiaoviec = lanhdaogiaoviec.strhoten;
                }

                var xulychinh = _canboRepo.GetAllCanbo
                            .Join(
                                _doituongRepo.Doituongxulys
                                                .Where(p => p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Xulychinh)
                                                .Where(p => p.intidhosocongviec == idhosocongviec),
                                c => c.intid,
                                d => d.intidcanbo,
                                (c, d) => c
                            ).FirstOrDefault();
                if (xulychinh != null)
                {
                    detail.strxulychinh = xulychinh.strhoten;
                }

                detail.strlanhdaophutrach = strldphutrach;

                detail.strphoihopxuly = strphoihopxuly;
                //=============================================
                // luan chuyen van ban
                //intsodoituongxuly = dtxl.Count(),

                detail.doituongxuly = dtxl;

                detail.hosoykien = hosoykien;

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            //========================================
            if (hs.inttrangthai == (int)enumHosocongviec.inttrangthai.Dahoanthanh)
            {
                strtrangthai = "Đã hoàn thành";
                detail.strngayketthuc = DateServices.FormatDateTimeVN(hs.strngayketthuc);
            }
            if (hs.inttrangthai == (int)enumHosocongviec.inttrangthai.Dangxuly)
            { strtrangthai = "Đang xử lý"; }

            detail.strtrangthai = strtrangthai;


            //===========phieu trinh =================
            detail.phieutrinh = _GetPhieutrinhViewModel(idhosocongviec);

            return detail;
        }

        /// <summary>
        /// lay thong tin luan chuyen van ban
        /// va chi tiet ho so
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <returns></returns>
        private IEnumerable<LuanchuyenvanbanViewModel> _GetLuanchuyenvanbanModel(int idhosocongviec)
        {
            //=====================================================
            // doi tuong xu ly ho so
            //=====================================================
            var dtxl = _doituongRepo.Doituongxulys
                        .Where(p => p.intidhosocongviec == idhosocongviec)
                        .Join(
                            _canboRepo.GetAllCanbo,
                            d => d.intidcanbo,
                            c => c.intid,
                            (d, c) => new { d, c }
                        )
                //.Join(
                //    _canbodonviRepo.CanboDonvis,
                //    dt => dt.d.intidcanbo,
                //    dv => dv.intidcanbo,
                //    (dt, dv) => new { dt, dv }
                //)
                        .Join(
                            _donviRepo.Donvitructhuocs,
                            xl => xl.c.intdonvi,
                            pb => pb.Id,
                            (xl, pb) => new { xl, pb }
                        )
                        .GroupJoin(
                            _canboRepo.GetAllCanbo,
                            nguoixuly => 1,
                            u => 1,
                            (nguoixuly, u) => new { nguoixuly, u }
                        )
                        .Select(p => new LuanchuyenvanbanViewModel
                        {
                            strtendonvi = p.nguoixuly.pb.strtendonvi,
                            strtencanbo = p.nguoixuly.xl.c.strhoten,
                            intvaitro = p.nguoixuly.xl.d.intvaitro,
                            strtennguoitao = p.u.FirstOrDefault(x => x.intid == p.nguoixuly.xl.d.intnguoitao).strhoten,
                            dtengaytao = p.nguoixuly.xl.d.strngaytao,
                            intvaitrocu = p.nguoixuly.xl.d.intvaitrocu,
                            strthaotac = p.nguoixuly.xl.d.strthaotac,
                            dtengaychuyen = p.nguoixuly.xl.d.strngaychuyen,
                            strtennguoichuyen = p.u.FirstOrDefault(x => x.intid == p.nguoixuly.xl.d.intnguoichuyen).strhoten,
                        })
                        .OrderBy(p => p.dtengaytao)
                        .ToList();
            foreach (var p in dtxl)
            {
                p.strvaitro = _SetVaitroxuly(p.intvaitro, p.intvaitrocu);
                p.strngaytao = DateServices.FormatDateTimeVN(p.dtengaytao);
                p.strngaychuyen = DateServices.FormatDateTimeVN(p.dtengaychuyen);
            }
            //=================================================
            // thong tin chi tiet ho so
            //=================================================
            var chitietHS = _chitietHosoRepo.ChitietHosos
                        .Where(p => p.intidhosocongviec == idhosocongviec)
                        .Join(
                            _canboRepo.GetAllCanbo,
                            d => d.intidcanbo,
                            c => c.intid,
                            (d, c) => new { d, c }
                        )
                //.Join(
                //    _canbodonviRepo.CanboDonvis,
                //    dt => dt.d.intidcanbo,
                //    dv => dv.intidcanbo,
                //    (dt, dv) => new { dt, dv }
                //)
                        .Join(
                            _donviRepo.Donvitructhuocs,
                            xl => xl.c.intdonvi,
                            pb => pb.Id,
                            (xl, pb) => new { xl, pb }
                        )
                        .GroupJoin(
                            _canboRepo.GetAllCanbo,
                            nguoixuly => 1,
                            u => 1,
                            (nguoixuly, u) => new { nguoixuly, u }
                        )
                        .Select(p => new LuanchuyenvanbanViewModel
                        {
                            strtendonvi = p.nguoixuly.pb.strtendonvi,
                            strtencanbo = p.nguoixuly.xl.c.strhoten,
                            intvaitro = p.nguoixuly.xl.d.intvaitro,
                            strtennguoitao = p.u.FirstOrDefault(x => x.intid == p.nguoixuly.xl.d.intnguoitao).strhoten,
                            dtengaytao = p.nguoixuly.xl.d.strngaytao,
                            //intvaitrocu = p.nguoixuly.xl.d.intvaitrocu,
                            strthaotac = p.nguoixuly.xl.d.strthaotac,
                            //dtengaychuyen = p.nguoixuly.xl.d.strngaychuyen,
                            //strtennguoichuyen = p.u.FirstOrDefault(x => x.intid == p.nguoixuly.xl.d.intnguoichuyen).strhoten,
                        })
                        .OrderBy(p => p.dtengaytao)
                        .ToList();
            foreach (var p in chitietHS)
            {
                p.strvaitro = _SetVaitroChitietHoso(p.intvaitro);
                p.strngaytao = DateServices.FormatDateTimeVN(p.dtengaytao);
                p.strngaychuyen = DateServices.FormatDateTimeVN(p.dtengaychuyen);
            }

            var tonghop = dtxl.Union(chitietHS)
                            .OrderBy(p => p.dtengaytao);

            return tonghop;
        }

        /// <summary>
        /// lay thong tin y kien xu ly
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <returns></returns>
        private IEnumerable<HosoykienxulyViewModel> _GetHosoykienxulyModel(int idhosocongviec)
        {
            var hosoykien = _hosoykienRepo.Hosoykienxulys
                        .Join(
                            _doituongRepo.Doituongxulys.Where(p => p.intidhosocongviec == idhosocongviec),
                            yk => yk.intiddoituongxuly,
                            dt => dt.intid,
                            (yk, dt) => new { yk, dt }
                        )
                        .Join(
                            _canboRepo.GetAllCanbo,
                            yk2 => yk2.dt.intidcanbo,
                            cb => cb.intid,
                            (yk2, cb) => new { yk2, cb }
                        )
                        .Join(
                            _donviRepo.Donvitructhuocs,
                            yk4 => yk4.cb.intdonvi,
                            dv => dv.Id,
                            (yk4, dv) => new { yk4, dv }
                        )
                        .Select(p => new HosoykienxulyViewModel
                        {
                            idykien = p.yk4.yk2.yk.intid,
                            strtendonvi = p.dv.strtendonvi,
                            strtencanbo = p.yk4.cb.strhoten,
                            strkykien = p.yk4.yk2.yk.strykien,
                            dtethoigian = p.yk4.yk2.yk.strthoigian
                            //files =
                        })
                        .OrderBy(p => p.dtethoigian)
                        .ToList()
                        ;
            foreach (var p in hosoykien)
            {
                p.strthoigian = DateServices.FormatDateTimeVN(p.dtethoigian);

                var downloadFiles = _fileHsRepo.AttachHosos
                        .Where(f => f.inttrangthai == (int)enumAttachHoso.inttrangthai.IsActive)
                        .Where(f => f.intloai == (int)enumAttachHoso.intloai.Ykien)
                        .Where(f => f.intidhoso == idhosocongviec)
                        .Where(f => f.intidtailieu == p.idykien)
                        .Select(f => new QLVB.DTO.File.DownloadFileViewModel
                        {
                            intid = f.intid,
                            strtenfile = f.strmota,
                            IsPhathanh = (f.intphathanh == 1) ? true : false
                        }).ToList();

                foreach (var f in downloadFiles)
                {
                    f.fileExt = _fileManager.GetFileExtention(f.strtenfile);
                    f.strfiletypeimages = _fileManager.GetFileTypeImages(f.strtenfile);
                    //f.fileIcon = _fileManager.getfi
                    f.intloai = (int)enumDownloadFileViewModel.intloai.HSCV_Ykien;

                }
                p.DownloadFiles = downloadFiles;
            }

            return hosoykien;
        }

        /// <summary>
        /// xac dinh vai tro xu ly cua can bo dang xet
        /// </summary>
        /// <param name="intvaitro"></param>
        /// <param name="intvaitrocu"></param>
        /// <returns></returns>
        private string _SetVaitroxuly(int? intvaitro, int? intvaitrocu)
        {
            string strvaitro = "";
            switch (intvaitro)
            {
                case 1:
                    strvaitro = "Lãnh đạo giao việc";
                    break;

                case 2:
                    strvaitro = "Lãnh đạo phụ trách";
                    break;

                case 3:
                    strvaitro = "Xử lý chính";
                    break;

                case 4:
                    strvaitro = "Phối hợp xử lý";
                    break;

                case 5:
                    strvaitro = _SetVaitroChuyenxuly(intvaitrocu);
                    break;
                //case 6:
                //    strvaitro = "Tiếp nhận văn bản giấy";
                //    break;
                //case 7:
                //    strvaitro = "Cập nhật văn bản điện tử";
                //    break;
                //case 8:
                //    strvaitro = "Kết thúc hồ sơ";
                //    break;
                //case 9:
                //    strvaitro = "Mở lại hồ sơ";
                //    break;
            }
            return strvaitro;
        }

        /// <summary>
        /// xac dinh vai tro da chuyen xu ly cua can bo dang xet
        /// </summary>
        /// <param name="intvaitrocu"></param>
        /// <returns></returns>
        private string _SetVaitroChuyenxuly(int? intvaitrocu)
        {
            string strvaitro = "";
            switch (intvaitrocu)
            {
                case 1:
                    strvaitro = "Lãnh đạo giao việc<br>(Đã chuyển xử lý)";
                    break;

                case 2:
                    strvaitro = "Lãnh đạo phụ trách<br>(Đã chuyển xử lý)";
                    break;

                case 3:
                    strvaitro = "Xử lý chính<br>(Đã chuyển xử lý)";
                    break;

                case 4:
                    strvaitro = "Phối hợp xử lý<br>(Đã chuyển xử lý)";
                    break;
            }
            return strvaitro;
        }

        /// <summary>
        /// xac dinh vai tro cua can bo trong chi tiet ho so
        /// </summary>
        /// <param name="intvaitro"></param>
        /// <returns></returns>
        private string _SetVaitroChitietHoso(int? intvaitro)
        {
            string strvaitro = "";
            switch (intvaitro)
            {
                case 1:
                    strvaitro = "Tiếp nhận văn bản giấy";
                    break;

                case 2:
                    strvaitro = "Cập nhật văn bản điện tử";
                    break;

                case 3:
                    strvaitro = "Kết thúc hồ sơ";
                    break;

                case 4:
                    strvaitro = "Mở lại hồ sơ";
                    break;

                case 5:
                    strvaitro = "Phát hành văn bản, Kết thúc Hồ sơ";
                    break;
            }
            return strvaitro;
        }

        private IEnumerable<PhieutrinhViewModel> _GetPhieutrinhViewModel(int idhoso)
        {
            try
            {
                var phieutrinh = _phieutrinhRepo.Phieutrinhs
                .Where(p => p.intidhosocongviec == idhoso)
                .Select(p => new PhieutrinhViewModel
                {
                    idphieutrinh = p.intid,
                    idhoso = (int)p.intidhosocongviec,
                    idcanbotrinh = (int)p.intidcanbotrinh,

                    strnoidungtrinh = p.strnoidungtrinh,
                    //strngaytrinh =
                    dtengaytrinh = p.strngaytrinh,
                    idlanhdaotrinh = (int)p.intidlanhdao,
                    strykienchidao = p.strykienchidao,
                    //strngaychidao =
                    dtengaychidao = p.strngaychidao,
                    IsChoykienchidao = p.inttrangthaichidao == (int)enumphieutrinh.inttrangthaichidao.Chuachidao
                                        ? true : false
                })
                .OrderBy(p => p.dtengaytrinh)
                .ToList();

                var dtxl = _doituongRepo.GetAllCanboxulys
                    .Where(p => p.intidhosocongviec == idhoso)
                    .Join(
                        _canboRepo.GetAllCanbo,
                        dt => dt.intidcanbo,
                        cb => cb.intid,
                        (dt, cb) => new { dt, cb.strhoten }
                    )
                    .Select(p => new { p.dt.intid, p.strhoten })
                    .ToList();

                int iduser = _session.GetUserId();
                int iddtxl = GetIdDoituongxuly(idhoso, iduser);

                if (iddtxl == 0)
                {   // khong phai la lanh dao cho y kien chi dao
                    foreach (var p in phieutrinh)
                    {
                        p.strngaytrinh = DateServices.FormatDateTimeVN(p.dtengaytrinh);
                        p.strngaychidao = DateServices.FormatDateTimeVN(p.dtengaychidao);
                        foreach (var u in dtxl)
                        {
                            if (u.intid == p.idcanbotrinh)
                            {
                                p.strcanbotrinh = u.strhoten;
                            }
                            if (u.intid == p.idlanhdaotrinh)
                            {
                                p.strlanhdaotrinh = u.strhoten;
                            }
                        }
                        p.IsChoykienchidao = false;
                    }
                }
                else
                {
                    // la lanh dao cho y kien chi dao
                    foreach (var p in phieutrinh)
                    {
                        p.strngaytrinh = DateServices.FormatDateTimeVN(p.dtengaytrinh);
                        p.strngaychidao = DateServices.FormatDateTimeVN(p.dtengaychidao);
                        foreach (var u in dtxl)
                        {
                            if (u.intid == p.idcanbotrinh)
                            {
                                p.strcanbotrinh = u.strhoten;
                            }
                            if (u.intid == p.idlanhdaotrinh)
                            {
                                p.strlanhdaotrinh = u.strhoten;
                            }
                        }
                        if (p.IsChoykienchidao)
                        {
                            p.IsChoykienchidao = p.idlanhdaotrinh == iddtxl ? true : false;
                        }
                    }
                }

                return phieutrinh;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }

        #endregion ViewDetailHosocongviec

        #region PhanXLVB

        /// <summary>
        /// load du lieu form phan xu ly van ban
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        public PhanXLVBViewModel GetFormPhanXLVB(int idvanban)
        {
            PhanXLVBViewModel hoso = new PhanXLVBViewModel();

            var hsvb = _hosovanbanRepo.Hosovanbans
                                .Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden)
                                .FirstOrDefault(p => p.intidvanban == idvanban);

            // idhosocongviec = 0:  van ban den chua phan xu ly
            // idhosocongviec !=0: van ban den da phan xu ly, load du lieu do vao form

            int? idhosocongviec = (hsvb != null) ? hsvb.intidhosocongviec : 0;

            //DateTime ngayhientai = DateTime.Now;
            if ((idhosocongviec == null) || (idhosocongviec == 0))
            {   // ho so moi
                try
                {
                    Vanbanden vanban = _vanbandenRepo.Vanbandens.FirstOrDefault(p => p.intid == idvanban);

                    hoso.HosocongviecModel = new Hosocongviec();
                    hoso.HosocongviecModel.intid = 0;
                    hoso.HosocongviecModel.strngaymohoso = DateTime.Now;
                    hoso.HosocongviecModel.strtieude = vanban.strtrichyeu;
                    hoso.HosocongviecModel.intlinhvuc = 0;
                    hoso.intidlanhdaogiaoviec = vanban.intidnguoiduyet;
                    hoso.intidxulychinh = 0;
                    hoso.stridlanhdaophutrach = string.Empty;

                    int intthoihanxuly = _configRepo.GetConfigToInt(ThamsoHethong.ThoihanXLVB);
                    hoso.HosocongviecModel.strthoihanxuly = DateServices.AddThoihanxuly(DateTime.Now, intthoihanxuly);

                    hoso.IsDonghoso = false;

                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
            }
            else
            {   // ho so da co
                try
                {
                    Hosocongviec hscv = _hosocongviecRepo.Hosocongviecs.FirstOrDefault(p => p.intid == idhosocongviec);
                    hoso.HosocongviecModel = hscv;

                    hoso.IsQuytrinh = IsHosoQuytrinhxuly((int)idhosocongviec);

                    // load du lieu tu doituongxuly
                    var lanhdaogiaoviec = _doituongRepo.Doituongxulys
                                                .Where(p => p.intidhosocongviec == idhosocongviec)
                                                .Where(p => p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaogiaoviec)
                                                .FirstOrDefault();

                    hoso.intidlanhdaogiaoviec = (lanhdaogiaoviec != null) ? lanhdaogiaoviec.intidcanbo : 0;

                    var ldpt = _doituongRepo.Doituongxulys
                                .Where(p => p.intidhosocongviec == idhosocongviec)
                                .Where(p => p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaophutrach);

                    string stridlanhdaophutrach = "";
                    List<int> intidlanhdaophutrach = new List<int>();
                    if (ldpt.Count() != 0)
                    {
                        foreach (var ld in ldpt)
                        {
                            stridlanhdaophutrach += ld.intidcanbo.ToString() + ",";
                            intidlanhdaophutrach.Add((int)ld.intidcanbo);
                        }
                        int len = stridlanhdaophutrach.Length - 1;
                        stridlanhdaophutrach = stridlanhdaophutrach.Substring(0, len);
                    }
                    hoso.stridlanhdaophutrach = stridlanhdaophutrach;
                    hoso.listidlanhdaophutrach = intidlanhdaophutrach;

                    var xulychinh = _doituongRepo.Doituongxulys
                                                .Where(p => p.intidhosocongviec == idhosocongviec)
                                                .Where(p => p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Xulychinh)
                                                .FirstOrDefault();

                    hoso.intidxulychinh = (xulychinh != null) ? xulychinh.intidcanbo : 0;

                    if (hoso.intidxulychinh > 0)
                    {
                        hoso.intDonvi = _canboRepo.GetAllCanboByID((int)hoso.intidxulychinh).intdonvi;
                    }

                    hoso.IsDonghoso = hscv.intdonghoso == (int)enumHosocongviec.intdonghoso.Co ? true : false;

                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }

            }

            hoso.intidvanban = idvanban;

            //hoso.VanbandenModel = vanban;

            hoso.LinhvucModel = _linhvucRepo.GetActiveLinhvucs
                            .Where(p => p.inttrangthai == (int)enumLinhvuc.inttrangthai.IsActive)
                            .OrderBy(p => p.strtenlinhvuc);

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

            //hoso.DoituongxulyModel = _doituongRepo.Doituongxulys.Where(p => p.intidhosocongviec == idhosocongviec);

            return hoso;
        }

        /// <summary>
        /// luu phan xu ly van ban
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="model"></param>
        /// <returns>
        /// </returns>
        public ResultFunction SavePhanXLVB(int idvanban, PhanXLVBViewModel model)
        {
            //====================================================================
            //  duyet van ban dang phan xl
            //  cap nhat ten nguoi xu ly chinh(strnoinhan) cua van ban
            //  insert vao table hosocongviec
            //  insert vao table doituongxuly: lanh dao  giao viec, lanh dao phu trach , xu ly chinh
            //  insert vao table hosovanban
            //  copy file dinh kem cua van ban vao folder hoso
            //  nhan tin SMS (neu co)
            //=====================================================================

            ResultFunction kq = new ResultFunction();
            kq.id = (int)ResultViewModels.Error;

            int idhosocongviec = model.HosocongviecModel.intid;
            int idlanhdaogiaoviec = (int)model.intidlanhdaogiaoviec;
            int idxulychinh = (int)model.intidxulychinh;

            //List<int> listidlanhdaophutrach = model.listidlanhdaophutrach;
            string stridlanhdaophutrach = model.stridlanhdaophutrach;

            Hosocongviec hscv = new Hosocongviec();
            hscv = model.HosocongviecModel;


            //=======================================================
            // cap nhat du lieu
            //=======================================================
            try
            {
                //  duyet van ban dang phan xl
                _vanbandenRepo.Duyet(idvanban, (int)enumVanbanden.inttrangthai.Daduyet);
            }
            catch //(Exception ex)
            {
                //_logger.Error(ex.Message); 
            }


            // THEM MOI HO SO CONG VIEC
            if (idhosocongviec == 0)
            {
                // KIEM TRA PHAN 2 LAN 1 VAN BAN
                if (_role.CheckPhanHosocongviec(idvanban, (int)enumHosovanban.intloai.Vanbanden))
                {
                    try
                    {
                        //  cap nhat ten nguoi xu ly chinh(strnoinhan) cua van ban
                        _CapnhatXulychinhVanbanden(idvanban, (int)model.intidxulychinh, true);

                        //  insert vao table hosocongviec
                        hscv.intloai = (int)enumHosocongviec.intloai.Vanbanden;
                        hscv.intidnguoinhap = _session.GetUserId();
                        idhosocongviec = _hosocongviecRepo.Them(hscv);

                        //  insert vao table doituongxuly: lanh dao  giao viec, lanh dao phu trach , xu ly chinh
                        _ThemDoituongxuly(idhosocongviec, idlanhdaogiaoviec, (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaogiaoviec);
                        _ThemDoituongxuly(idhosocongviec, idxulychinh, (int)enumDoituongxuly.intvaitro_doituongxuly.Xulychinh);

                        // insert vao tonghopcanbo                        
                        int intloaitonghop = (int)enumTonghopCanbo.intloai.HosoXLVBDen;
                        _ThemTonghopCanbo(idhosocongviec, idlanhdaogiaoviec, intloaitonghop);
                        _ThemTonghopCanbo(idhosocongviec, idxulychinh, intloaitonghop);

                        // kiem tra chuoi stridlanhdaophutrach khac rong
                        // va tach chuoi lay id ra de them vao doituongxuly
                        if (!string.IsNullOrEmpty(stridlanhdaophutrach))
                        {
                            string[] strlanhdaophutrach = stridlanhdaophutrach.Split(new Char[] { ',' });
                            foreach (var p in strlanhdaophutrach)
                            {
                                if (!string.IsNullOrEmpty(p))
                                {
                                    int idlanhdaophutrach = Convert.ToInt32(p);
                                    _ThemDoituongxuly(idhosocongviec, idlanhdaophutrach, (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaophutrach);
                                    _ThemTonghopCanbo(idhosocongviec, idlanhdaophutrach, intloaitonghop);
                                }
                            }
                        }

                        //  insert vao table hosovanban
                        _ThemHosovanban(idhosocongviec, idvanban, (int)enumHosovanban.intloai.Vanbanden);

                        // chua lam
                        //  copy file dinh kem cua van ban vao folder hoso
                        //  nhan tin SMS (neu co)

                        kq.id = (int)ResultViewModels.Success;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message);
                    }
                }
            }
            else
            {   // CAP NHAT HO SO DA CO
                try
                {
                    //  cap nhat ten nguoi xu ly chinh(strnoinhan) cua van ban
                    _CapnhatXulychinhVanbanden(idvanban, (int)model.intidxulychinh, false);


                    // kiem tra ho so da ket thuc chua
                    // truoc khi mo ho so lai thi ghi nhat ky
                    //if (RoleViewVanbanServices.CheckHoanthanhHoso(idhosocongviec))
                    if (_role.CheckHoanthanhHoso(idhosocongviec))
                    {
                        int idcanbo = _session.GetUserId();
                        ChitietHoso ct = new ChitietHoso();
                        ct.intidcanbo = idcanbo;
                        ct.intidhosocongviec = idhosocongviec;
                        ct.intnguoitao = idcanbo;
                        ct.intvaitro = (int)enumDoituongxuly.intvaitro_chitiethoso.MolaiHoso;
                        _chitietHosoRepo.Them(ct);
                        //_ThemDoituongxuly(idhosocongviec, idcanbo, (int)EnumVanban.intvaitro_doituongxuly.MolaiHoso);
                    }

                    //  cap nhat hosocongviec
                    //  CHU Y: van thu thi cap nhat strnoidung , chuyen vien thi ghi y kien xu ly
                    _hosocongviecRepo.Sua(idhosocongviec, hscv, true);

                    //  insert vao table doituongxuly: lanh dao  giao viec, lanh dao phu trach , xu ly chinh
                    // kiểm tra xem canbo xuly có bị thay đổi sang người mới không
                    // nếu có thì chuyển vai trò của canbo trong doituongxuly sang vai trò cũ
                    // và thêm mới
                    _CapnhatMotDoituongxuly(idhosocongviec, idlanhdaogiaoviec, (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaogiaoviec);
                    _CapnhatMotDoituongxuly(idhosocongviec, idxulychinh, (int)enumDoituongxuly.intvaitro_doituongxuly.Xulychinh);

                    _CapnhatLanhdaophutrach(idhosocongviec, stridlanhdaophutrach);

                    //  nhan tin SMS (neu co)

                    kq.id = (int)ResultViewModels.Success;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
            }

            return kq;
        }

        /// <summary>
        /// cap nhat ten nguoi xu ly chinh(strnoinhan) cua van ban sau khi phan xu ly
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="idcanbo"></param>
        private void _CapnhatXulychinhVanbanden(int idvanban, int idcanbo, bool IsNew)
        {
            try
            {
                string strhoten = _canboRepo.GetActiveCanboByID(idcanbo).strhoten;
                if (!string.IsNullOrEmpty(strhoten))
                {
                    if (IsNew)
                    {
                        _vanbandenRepo.CapnhatNguoixulychinh(idvanban, strhoten);
                    }
                    else
                    {
                        _vanbandenRepo.CapnhatNguoixulychinh(idvanban, strhoten + ",");
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
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
                //dt.intvaitrocu
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
                // truong mac dinh them vao khi them moi
                //hsvb.inttrangthai = (int)EnumVanban.inttrangthai_hosovanban.Dangxuly;
                _hosovanbanRepo.Them(hsvb);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// dung cho 1 nguoi :lanh dao giao viec va xu ly chinh
        /// them moi can bo xu ly va cap nhat lai vai tro cua can bo xu ly cu truoc do        ///
        /// bo sung tonghopcanbo: xoa va them moi thong tin tong hop
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <param name="idcanboxuly"></param>
        /// <param name="intvaitro">lanh dao giao viec; xu ly chinh</param>
        private void _CapnhatMotDoituongxuly(int idhosocongviec, int idcanboxuly, int intvaitro)
        {
            try
            {
                // kiem tra xem can bo nay co dang tham gia xu ly khong
                var cb = _doituongRepo.Doituongxulys
                        .Where(p => p.intidhosocongviec == idhosocongviec)
                        .Where(p => p.intvaitro == intvaitro)
                    //.Where(p => p.intidcanbo == idcanboxuly)
                        .FirstOrDefault();

                // lanh dao giao viec, va xu ly chinh chi co 1 nguoi
                if (cb != null)
                {   // neu dang tham gia thi chuyen vai tro xu ly
                    if (cb.intidcanbo != idcanboxuly)
                    {   // canbo trong doituongxuly khác với canbo của form submit
                        // thì đổi vai trò của canbo trong doituongxuly
                        int idcanbo = _session.GetUserId();
                        _doituongRepo.CapnhatVaitrocu(cb.intid, idcanbo);
                        // va them can bo xu ly moi vao
                        _ThemDoituongxuly(idhosocongviec, idcanboxuly, intvaitro);

                        // xoa can bo cu trong tonghopcanbo(neu co)
                        // va them can bo xu ly moi vao
                        _XoaTonghopCanboVBDen(idhosocongviec, (int)cb.intidcanbo);
                        _ThemTonghopCanbo(idhosocongviec, idcanboxuly, (int)enumTonghopCanbo.intloai.HosoXLVBDen);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// cap nhat lanh dao phu trach vao bang doi tuong xu ly
        /// bo sung tonghopcanbo
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <param name="stridlanhdaophutrach"></param>
        private void _CapnhatLanhdaophutrach(int idhosocongviec, string stridlanhdaophutrach)
        {
            try
            {
                var dt = _doituongRepo.Doituongxulys
                       .Where(p => p.intidhosocongviec == idhosocongviec)
                       .Where(p => p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaophutrach)
                       .ToList();
                // do lãnh đạo phụ trách là tùy chọn nên có thể có hoặc không

                // nếu tồn tại ldpt trong doituongxuly
                if (dt.Count() != 0)
                {   // nếu submit không có lanh dao phu trach thì
                    // chuyển tất cả ldpt trong doituongxuly sang vai trò cũ
                    if (string.IsNullOrEmpty(stridlanhdaophutrach))
                    {
                        foreach (var d in dt)
                        {   // chuyen vai tro sang vaitrocu
                            int idcanbo = _session.GetUserId();
                            _doituongRepo.CapnhatVaitrocu(d.intid, idcanbo);

                            // xoa trong tonghopcanbo
                            _XoaTonghopCanboVBDen(idhosocongviec, (int)d.intidcanbo);
                        }
                    }
                    else
                    {   // nếu submit có lanh dao phu trach
                        // kiểm tra xem ldpt có bị thay đổi sang người mới không
                        //======================================================
                        // ld: 2,5,17
                        // sub (submit) : 2,4
                        // đổ phần tử mảng sub vào mảng ld: 2,5,17,4
                        // so sánh với mảng sub để tìm những phần tử không có
                        // và đổi vai trò, ld: 5, 17
                        //======================================================
                        string[] strlanhdaophutrach = stridlanhdaophutrach.Split(new Char[] { ',' });
                        // insert cac ldpt chua co vao bang doituongxuly
                        foreach (var p in strlanhdaophutrach)
                        {
                            bool isfound = false;
                            int idlanhdaophutrach = Convert.ToInt32(p);
                            foreach (var d in dt)
                            {
                                if (idlanhdaophutrach == d.intidcanbo)
                                {
                                    isfound = true;
                                }
                            }
                            if (isfound == false)
                            {   // neu khong tim thay thi them moi
                                // do phan tu mang sub vao mang ld
                                _ThemDoituongxuly(idhosocongviec, idlanhdaophutrach, (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaophutrach);

                                // them moi tonghopcanbo
                                _ThemTonghopCanbo(idhosocongviec, idlanhdaophutrach, (int)enumTonghopCanbo.intloai.HosoXLVBDen);
                            }
                        }
                        // doi vai tro cua cac ldpt khong co trong mang submit
                        var cb = _doituongRepo.Doituongxulys
                                   .Where(p => p.intidhosocongviec == idhosocongviec)
                                   .Where(p => p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaophutrach)
                                   .ToList();
                        foreach (var c in cb)
                        {
                            bool isfound = false;
                            foreach (var p in strlanhdaophutrach)
                            {
                                int idlanhdaophutrach = Convert.ToInt32(p);
                                if (idlanhdaophutrach == c.intidcanbo)
                                {
                                    isfound = true;
                                }
                            }
                            if (isfound == false)
                            {   // neu khong tim thay thi chuyen vai tro sang vai tro cu
                                int idcanbo = _session.GetUserId();
                                _doituongRepo.CapnhatVaitrocu(c.intid, idcanbo);

                                // xoa trong tonghopcanbo
                                _XoaTonghopCanboVBDen(idhosocongviec, (int)c.intidcanbo);
                            }
                        }
                    }
                }
                // nếu không tồn tại ldpt trong bảng doituongxuly
                // thì thêm mới ldpt vào trong bảng doituongxuly
                else
                {
                    if (!string.IsNullOrEmpty(stridlanhdaophutrach))
                    {
                        string[] strlanhdaophutrach = stridlanhdaophutrach.Split(new Char[] { ',' });
                        foreach (var p in strlanhdaophutrach)
                        {
                            int idlanhdaophutrach = Convert.ToInt32(p);
                            _ThemDoituongxuly(idhosocongviec, idlanhdaophutrach, (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaophutrach);

                            // them moi tonghopcanbo
                            _ThemTonghopCanbo(idhosocongviec, idlanhdaophutrach, (int)enumTonghopCanbo.intloai.HosoXLVBDen);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        //======================================================
        // luu phan xu ly nhieu van ban cho 1 nguoi xu ly chinh
        //======================================================
        public int SavePhanXLNhieuVB(List<int> listidvanban, string strykienxuly, PhanXLVBViewModel model)
        {
            foreach (int id in listidvanban)
            {   // duyet tung van ban
                int idvanban = id;

                var vanban = _vanbandenRepo.Vanbandens
                    .Where(p => p.intid == idvanban).FirstOrDefault();

                if (vanban != null)
                {
                    model.HosocongviecModel.strtieude = vanban.strtrichyeu;

                    var hoso = _hosovanbanRepo.Hosovanbans
                        .Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden)
                        .Where(p => p.intidvanban == idvanban)
                        .FirstOrDefault();

                    if (hoso != null)
                    {   // chỉ phân xl vb đã có hosocongviec roi
                        model.HosocongviecModel.intid = hoso.intidhosocongviec;

                        // phan xu ly 1 van ban
                        SavePhanXLVB(idvanban, model);

                        // ghi y kien xu ly
                        _ThemYkienxulyNhieuVB(strykienxuly, model.HosocongviecModel.intid);
                    }
                    else
                    {
                        //model.HosocongviecModel.intid = 0;
                    }
                }
            }
            return (int)ResultViewModels.Success;
        }

        private void _ThemYkienxulyNhieuVB(string strykien, int idhosocongviec)
        {
            if (!string.IsNullOrWhiteSpace(strykien))
            {
                int iduser = _session.GetUserId();
                int iddoituongxl = 0;

                var doituongxl = _doituongRepo.GetCanboDangXulys
                    .Where(p => p.intidcanbo == iduser)
                    .Where(p => p.intidhosocongviec == idhosocongviec)
                    .OrderBy(p => p.intvaitro)
                    .ToList();
                if (doituongxl.Count() > 0)
                {
                    iddoituongxl = doituongxl.FirstOrDefault().intid;
                }
                else
                {
                    Doituongxuly dtxl = new Doituongxuly();
                    dtxl.intidcanbo = iduser;
                    dtxl.intvaitro = (int)enumDoituongxuly.intvaitro_doituongxuly.Phoihopxuly;
                    dtxl.intidhosocongviec = idhosocongviec;
                    dtxl.intnguoitao = iduser;
                    try
                    {
                        iddoituongxl = _doituongRepo.Them(dtxl);
                    }
                    catch (Exception ex) { _logger.Error(ex.Message); }
                }
                Hosoykienxuly yk = new Hosoykienxuly
                {
                    intiddoituongxuly = iddoituongxl,
                    intidnguoilap = iduser,
                    //strthoigian = DateTime.Now, da co trong gia tri mac dinh
                    strykien = strykien,
                    inttrangthai = (int)enumHosoykienxuly.inttrangthai.Dachoykien
                };
                try
                {
                    _hosoykienRepo.Them(yk);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
            }

        }

        #endregion PhanXLVB

        #region ThemHoso
        /// <summary>
        /// load du lieu form them hoso
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        public PhanXLVBViewModel GetFormThemHoso(int idhoso)
        {
            PhanXLVBViewModel hoso = new PhanXLVBViewModel();

            // idhosocongviec = 0:  hoso moi
            // idhosocongviec !=0: da co hoso, load du lieu vao form

            int? idhosocongviec = (idhoso > 0) ? idhoso : 0;

            //DateTime ngayhientai = DateTime.Now;
            if ((idhosocongviec == null) || (idhosocongviec == 0))
            {   // ho so moi
                try
                {
                    hoso.HosocongviecModel = new Hosocongviec();
                    hoso.HosocongviecModel.intid = 0;
                    hoso.HosocongviecModel.strngaymohoso = DateTime.Now;
                    hoso.HosocongviecModel.strtieude = null;
                    hoso.HosocongviecModel.intlinhvuc = 0;
                    hoso.intidlanhdaogiaoviec = 0;
                    hoso.intidxulychinh = _session.GetUserId();
                    hoso.stridlanhdaophutrach = string.Empty;

                    int intthoihanxuly = _configRepo.GetConfigToInt(ThamsoHethong.ThoihanXLVB);
                    hoso.HosocongviecModel.strthoihanxuly = DateServices.AddThoihanxuly(DateTime.Now, intthoihanxuly);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
            }
            else
            {   // ho so da co
                try
                {
                    Hosocongviec hscv = _hosocongviecRepo.Hosocongviecs.FirstOrDefault(p => p.intid == idhosocongviec);
                    hoso.HosocongviecModel = hscv;

                    hoso.IsQuytrinh = IsHosoQuytrinhxuly((int)idhosocongviec);

                    // load du lieu tu doituongxuly
                    var lanhdaogiaoviec = _doituongRepo.Doituongxulys
                                                .Where(p => p.intidhosocongviec == idhosocongviec)
                                                .Where(p => p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaogiaoviec)
                                                .FirstOrDefault();

                    hoso.intidlanhdaogiaoviec = (lanhdaogiaoviec != null) ? lanhdaogiaoviec.intidcanbo : 0;

                    var ldpt = _doituongRepo.Doituongxulys
                                .Where(p => p.intidhosocongviec == idhosocongviec)
                                .Where(p => p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaophutrach);

                    string stridlanhdaophutrach = "";
                    List<int> intidlanhdaophutrach = new List<int>();
                    if (ldpt.Count() != 0)
                    {
                        foreach (var ld in ldpt)
                        {
                            stridlanhdaophutrach += ld.intidcanbo.ToString() + ",";
                            intidlanhdaophutrach.Add((int)ld.intidcanbo);
                        }
                        int len = stridlanhdaophutrach.Length - 1;
                        stridlanhdaophutrach = stridlanhdaophutrach.Substring(0, len);
                    }
                    hoso.stridlanhdaophutrach = stridlanhdaophutrach;
                    hoso.listidlanhdaophutrach = intidlanhdaophutrach;

                    var xulychinh = _doituongRepo.Doituongxulys
                                                .Where(p => p.intidhosocongviec == idhosocongviec)
                                                .Where(p => p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Xulychinh)
                                                .FirstOrDefault();

                    hoso.intidxulychinh = (xulychinh != null) ? xulychinh.intidcanbo : 0;


                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }

            }

            hoso.LinhvucModel = _linhvucRepo.GetActiveLinhvucs
                            .Where(p => p.inttrangthai == (int)enumLinhvuc.inttrangthai.IsActive)
                            .OrderBy(p => p.strtenlinhvuc);

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

            if (hoso.intidxulychinh > 0)
            {
                hoso.intDonvi = _canboRepo.GetAllCanboByID((int)hoso.intidxulychinh).intdonvi;
            }

            hoso.listDonvi = _donviRepo.Donvitructhuocs
                   .Select(p => new QLVB.DTO.Donvi.EditDonviViewModel
                   {
                       intid = p.Id,
                       strtendonvi = p.strtendonvi
                   })
                   .OrderBy(p => p.strtendonvi);

            return hoso;
        }

        /// <summary>
        /// luu them hoso
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="model"></param>
        /// <returns>        
        /// </returns>
        public ResultFunction SaveHoso(PhanXLVBViewModel model)
        {
            //====================================================================            
            //  insert vao table hosocongviec
            //  insert vao table doituongxuly: lanh dao  giao viec, lanh dao phu trach , xu ly chinh            
            //  copy file dinh kem cua van ban vao folder hoso
            //  nhan tin SMS (neu co)
            //=====================================================================

            ResultFunction kq = new ResultFunction();
            kq.id = (int)ResultViewModels.Error;

            int idhosocongviec = model.HosocongviecModel.intid;
            int idlanhdaogiaoviec = (int)model.intidlanhdaogiaoviec;
            int idxulychinh = (int)model.intidxulychinh;

            //List<int> listidlanhdaophutrach = model.listidlanhdaophutrach;
            string stridlanhdaophutrach = model.stridlanhdaophutrach;

            Hosocongviec hscv = model.HosocongviecModel;

            //=======================================================
            // cap nhat du lieu
            //=======================================================

            // THEM MOI HO SO CONG VIEC
            if (idhosocongviec == 0)
            {
                try
                {
                    //  insert vao table hosocongviec
                    hscv.intloai = (int)enumHosocongviec.intloai.Giaiquyetcongviec;
                    hscv.intidnguoinhap = _session.GetUserId();
                    idhosocongviec = _hosocongviecRepo.Them(hscv);

                    //  insert vao table doituongxuly: lanh dao  giao viec, lanh dao phu trach , xu ly chinh
                    _ThemDoituongxuly(idhosocongviec, idlanhdaogiaoviec, (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaogiaoviec);
                    _ThemDoituongxuly(idhosocongviec, idxulychinh, (int)enumDoituongxuly.intvaitro_doituongxuly.Xulychinh);

                    // insert vao tonghopcanbo                        
                    //int intloaitonghop = (int)enumTonghopCanbo.intloai.HosoXLVBDen;
                    //_ThemTonghopCanbo(idhosocongviec, idlanhdaogiaoviec, intloaitonghop);
                    //_ThemTonghopCanbo(idhosocongviec, idxulychinh, intloaitonghop);

                    // kiem tra chuoi stridlanhdaophutrach khac rong
                    // va tach chuoi lay id ra de them vao doituongxuly
                    if (!string.IsNullOrEmpty(stridlanhdaophutrach))
                    {
                        string[] strlanhdaophutrach = stridlanhdaophutrach.Split(new Char[] { ',' });
                        foreach (var p in strlanhdaophutrach)
                        {
                            if (!string.IsNullOrEmpty(p))
                            {
                                int idlanhdaophutrach = Convert.ToInt32(p);
                                _ThemDoituongxuly(idhosocongviec, idlanhdaophutrach, (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaophutrach);
                                //_ThemTonghopCanbo(idhosocongviec, idlanhdaophutrach, intloaitonghop);
                            }
                        }
                    }

                    // chua lam
                    //  copy file dinh kem cua van ban vao folder hoso
                    //  nhan tin SMS (neu co)

                    kq.id = (int)ResultViewModels.Success;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }

            }
            else
            {   // CAP NHAT HO SO DA CO
                try
                {
                    // kiem tra ho so da ket thuc chua
                    // truoc khi mo ho so lai thi ghi nhat ky                    
                    if (_role.CheckHoanthanhHoso(idhosocongviec))
                    {
                        int idcanbo = _session.GetUserId();
                        ChitietHoso ct = new ChitietHoso();
                        ct.intidcanbo = idcanbo;
                        ct.intidhosocongviec = idhosocongviec;
                        ct.intnguoitao = idcanbo;
                        ct.intvaitro = (int)enumDoituongxuly.intvaitro_chitiethoso.MolaiHoso;
                        _chitietHosoRepo.Them(ct);
                        //_ThemDoituongxuly(idhosocongviec, idcanbo, (int)EnumVanban.intvaitro_doituongxuly.MolaiHoso);
                    }

                    //  cap nhat hosocongviec
                    //  CHU Y: van thu thi cap nhat strnoidung , chuyen vien thi ghi y kien xu ly
                    _hosocongviecRepo.Sua(idhosocongviec, hscv, true);

                    //  insert vao table doituongxuly: lanh dao  giao viec, lanh dao phu trach , xu ly chinh
                    // kiểm tra xem canbo xuly có bị thay đổi sang người mới không
                    // nếu có thì chuyển vai trò của canbo trong doituongxuly sang vai trò cũ
                    // và thêm mới
                    _CapnhatMotDoituongxuly(idhosocongviec, idlanhdaogiaoviec, (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaogiaoviec);
                    _CapnhatMotDoituongxuly(idhosocongviec, idxulychinh, (int)enumDoituongxuly.intvaitro_doituongxuly.Xulychinh);

                    _CapnhatLanhdaophutrach(idhosocongviec, stridlanhdaophutrach);

                    //  nhan tin SMS (neu co)

                    kq.id = (int)ResultViewModels.Success;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
            }

            return kq;
        }

        /// <summary>
        /// lay thong tin cac user thuoc don vi : iddonvi
        /// </summary>
        /// <param name="iddonvi"></param>
        /// <returns></returns>
        public IEnumerable<QLVB.DTO.Donvi.EditUserViewModel> GetListCanbo(int iddonvi)
        {
            try
            {
                var canbo = _canboRepo.GetActiveCanbo
                    .Where(p => p.intdonvi == iddonvi)
                    .OrderBy(p => p.strkyhieu)
                    .ThenBy(p => p.strhoten)
                    .Select(p => new QLVB.DTO.Donvi.EditUserViewModel
                    {
                        intid = p.intid,
                        strhoten = p.strhoten
                    })
                    ;
                return canbo;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new List<QLVB.DTO.Donvi.EditUserViewModel>();
            }
        }

        #endregion ThemHoso

        #region Tonghopcanbo

        /// <summary>
        /// them thong tin tong hop chuyen toi user
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <param name="idcanbo"></param>
        /// <param name="intloai"></param>
        private void _ThemTonghopCanbo(int idhosocongviec, int idcanbo, int intloai)
        {
            try
            {
                // trong hosocongviec nay thi chi co vanbanden
                // nen ghi nhan idvanban vao tonghopcanbo
                int idvanban = _hosovanbanRepo.Hosovanbans
                    .Where(p => p.intidhosocongviec == idhosocongviec)
                    .Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden)
                    .FirstOrDefault().intidvanban;

                TonghopCanbo th = new TonghopCanbo();
                th.intidcanbo = idcanbo;
                th.intloaitailieu = (int)enumTonghopCanbo.intloaitailieu.Vanbanden;
                th.intidtailieu = idvanban; //idhosocongviec;
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
        /// <param name="idhosocongviec"></param>
        /// <param name="idcanbo"></param>
        private void _XoaTonghopCanboVBDen(int idhosocongviec, int idcanbo)
        {
            // trong hosocongviec nay thi chi co vanbanden            
            int idvanban = _hosovanbanRepo.Hosovanbans
                .Where(p => p.intidhosocongviec == idhosocongviec)
                .Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden)
                .FirstOrDefault().intidvanban;

            //int intloaitailieu = (int)enumTonghopCanbo.intloaitailieu.Vanbanden;
            _tonghopRepo.CapnhatTrangthaiVBDen(idcanbo, idvanban);

        }

        private void _XoaTonghopCanboHSXLVBDen(int idhosocongviec, int idcanbo)
        {
            // trong hosocongviec nay thi chi co vanbanden            
            int idvanban = _hosovanbanRepo.Hosovanbans
                .Where(p => p.intidhosocongviec == idhosocongviec)
                .Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden)
                .FirstOrDefault().intidvanban;

            _tonghopRepo.CapnhatTrangthaiHosoVBDen(idcanbo, idvanban);

        }

        #endregion Tonghopcanbo

        #region XulyHosocongviec

        #region Thongtinchung

        /// <summary>
        /// kiem tra xem user co quyen xu ly ho so khong
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        public bool IsXulyHoso(int idhoso)
        {
            int iduser = _session.GetUserId();
            // kiem tra can bo da chuyen xu ly co quyen xem ho so khong
            bool isXuly = _configRepo.GetConfigToBool(ThamsoHethong.IsXulyHoso);
            if (isXuly == true)
            {
                return _role.IsXulyHosocongviec(idhoso, iduser);
            }
            else
            {   // chi co can bo dang xu ly moi duoc phep xem ho so
                return _role.IsDangXulyHosocongviec(idhoso, iduser);
            }
        }

        public ToolbarXulyViewModel GetToolbarXuly(int idhoso, int? intBack)
        {
            ToolbarXulyViewModel tbar = new ToolbarXulyViewModel();

            tbar.intBack = intBack;
            tbar.IsDisable = false;
            int idcanbo = _session.GetUserId();

            // lay iddoituongxuly cua user dang xem
            tbar.IdCurrentUser = GetIdDoituongxuly(idhoso, idcanbo);

            // kiem tra user co dang xu ly hs
            if (_role.IsDangXulyHosocongviec(idhoso, idcanbo) == false)
            {
                // hide cac nut chuc nang cua toolbar di
                tbar.IsDisable = true;
            }
            // kiem tra xem ho so da dong chua
            if (_role.CheckHoanthanhHoso(idhoso))
            {
                // hide cac nut chuc nang cua toolbar di
                tbar.IsDisable = true;
            }
                        
            tbar.IsXulychinhDongHoso = _configRepo.GetConfigToBool(ThamsoHethong.IsXulychinhKetthucHoso);
            // chi cho xu ly chinh dong hs thi kiem tra xem co phai la xu ly chinh khong
            tbar.IsXulychinh = (tbar.IsXulychinhDongHoso) ? _CheckXulychinhHoso(idhoso, idcanbo): false;
           

            // lay so phieu trinh ma user dang xem(lanh dao) can cho y kien chi dao
            tbar.intsophieutrinh = 0;
            if (!tbar.IsDisable)
            {   // toolbar khong bi disable, user dc phep xu ly ho so
                var phieutrinh = _phieutrinhRepo.Phieutrinhs
                    .Where(p => p.intidhosocongviec == idhoso)
                    .Where(p => p.intidlanhdao == tbar.IdCurrentUser)
                    .Where(p => p.inttrangthaichidao == (int)enumphieutrinh.inttrangthaichidao.Chuachidao);

                tbar.intsophieutrinh = phieutrinh.Count();
            }

            var hosoquytrinh = _hsqtRepo.HosoQuytrinhxulys
                .Where(p => p.intidhoso == idhoso)
                .ToList();

            tbar.IsQuytrinh = IsHosoQuytrinhxuly(idhoso);

            tbar.IsXulyQuytrinh = IsXulyQuytrinh(idhoso, hosoquytrinh);

            //tbar.IsDieukienXuly = _KiemtraDieukienBuocXuly(idhoso);

            var hoso = _hosocongviecRepo.GetHSCVById((int)idhoso);

            tbar.IsXLVB = (hoso.intloai == (int)enumHosocongviec.intloai.Vanbanden) ? true : false;

            tbar.IsTamngungQuytrinh = (hoso.intluuhoso == (int)enumHosocongviec.intluuhoso.TamngungXL) ? true : false;

            tbar.IsPhoihopXLQuytrinh = _IsPhoihopXulyQuytrinh(idhoso);

            tbar.IsChonBuocXLQuytrinh = _IsChonBuocXuly(idhoso, hosoquytrinh);

            tbar.IsDongHoso = hoso.intdonghoso == (int)enumHosocongviec.intdonghoso.Khong ? false : true;

            return tbar;
        }

        /// <summary>
        /// danh sach tat ca nguoi dang xu ly cua ho so :
        /// Lanh dao giao viec, lanh dao phu trach, xu ly chinh, phoi hop xu ly
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        public IEnumerable<DanhsachNguoixulyViewModel> GetDanhsachNguoixuly(int idhoso)
        {
            var canbo = _doituongRepo.GetCanboDangXulys
                        .Where(p => p.intidhosocongviec == idhoso)
                        .Join(
                            _canboRepo.GetAllCanbo,
                            d => d.intidcanbo,
                            c => c.intid,
                            (d, c) => new { d, c }
                        )
                        .Select(p => new DanhsachNguoixulyViewModel
                        {
                            idcanbo = p.d.intid,
                            strkyhieu = p.c.strkyhieu,
                            strhoten = p.c.strhoten,
                            intvaitro = (int)p.d.intvaitro,
                            strvaitro = ""
                        })
                        .OrderBy(p => p.intvaitro)
                        .ToList();

            foreach (var p in canbo)
            {
                if (p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaogiaoviec) { p.strvaitro = "Lãnh đạo giao việc"; }
                if (p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaophutrach) { p.strvaitro = "Lãnh đạo phụ trách"; }
                if (p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Xulychinh) { p.strvaitro = "Xử lý chính"; }
                if (p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Phoihopxuly) { p.strvaitro = "Phối hợp xử lý"; }
            }
            return canbo;
        }

        /// <summary>
        /// cac thong tin chung cua hoso
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        public ThongtinHosoViewModel GetThongtinHoso(int idhoso)
        {
            try
            {
                int idcanbo = _session.GetUserId();
                if (_role.IsXulyHosocongviec(idhoso, idcanbo) == false)
                {
                    _logger.Warn(AppConts.ErrLog + " hồ sơ: " + idhoso.ToString());
                    return new ThongtinHosoViewModel();
                }

                string strtennguoihoanthanh = string.Empty;
                string strtenlinhvuc = string.Empty;
                string strmucquantrong = string.Empty;
                string strtrangthai = string.Empty;

                var hs = _hosocongviecRepo.Hosocongviecs
                        .FirstOrDefault(p => p.intid == idhoso);

                if ((hs.intidnguoihoanthanh != null) && (hs.intidnguoihoanthanh != 0))
                {
                    strtennguoihoanthanh = _canboRepo.GetAllCanboByID((int)hs.intidnguoihoanthanh).strhoten;
                    //_canboRepo.Canbos.FirstOrDefault(p => p.intid == hs.intidnguoihoanthanh).strhoten;
                }
                if ((hs.intlinhvuc != null) && (hs.intlinhvuc != 0))
                {
                    strtenlinhvuc = _linhvucRepo.GetAllLinhvucs.FirstOrDefault(p => p.intid == hs.intlinhvuc).strtenlinhvuc;
                }
                if ((hs.intmucdo != null) && (hs.intmucdo != 0))
                {
                    //strmucquantrong =
                }
                //=========================

                var detail = new ThongtinHosoViewModel();

                detail.idhosocongviec = hs.intid;
                //detail.intloai = (int)hs.intloai;
                detail.strsohieuht = hs.strsohieuht;
                //strloaihoso =
                detail.strtenlinhvuc = strtenlinhvuc;
                detail.strmucquantrong = strmucquantrong;
                detail.strngayketthuc = DateServices.FormatDateVN(hs.strngayketthuc);
                detail.strngaymohoso = DateServices.FormatDateVN(hs.strngaymohoso);
                detail.strthoihanxuly = DateServices.FormatDateVN(hs.strthoihanxuly);
                detail.strnoidung = hs.strnoidung;
                detail.strtieude = hs.strtieude;
                //strtrangthai =

                if ((hs.intloai == (int)enumHosocongviec.intloai.Vanbanden)
                    || hs.intloai == (int)enumHosocongviec.intloai.Vanbanden_Quytrinh)
                {
                    var vanban = _vanbandenRepo.Vanbandens
                            .Join(
                                _hosovanbanRepo.Hosovanbans.Where(p => p.intidhosocongviec == idhoso),
                                v => v.intid,
                                h => h.intidvanban,
                                (v, h) => v
                            ).FirstOrDefault();

                    detail.idvanban = vanban.intid;
                    detail.strtrichyeuvanban = vanban.strtrichyeu;
                }
                if (hs.intloai == (int)enumHosocongviec.intloai.Giaiquyetcongviec)
                {
                    detail.idvanban = 0;
                }

                if (hs.inttrangthai == (int)enumHosocongviec.inttrangthai.Dahoanthanh)
                {
                    strtrangthai = "Đã hoàn thành";
                    detail.strngayketthuc = DateServices.FormatDateTimeVN(hs.strngayketthuc);
                }
                if (hs.inttrangthai == (int)enumHosocongviec.inttrangthai.Dangxuly)
                { strtrangthai = "Đang xử lý"; }

                detail.strtrangthai = strtrangthai;

                // ====== van ban lien quan =============
                var vblqden = _hsvblqRepo.Hosovanbanlienquans
                    .Where(p => p.intloai == (int)enumHosovanbanlienquan.intloai.Vanbanden)
                    .Where(p => p.intidhosocongviec == idhoso);

                detail.isVBDenLQ = vblqden.Count() > 0 ? true : false;


                var vblqdi = _hsvblqRepo.Hosovanbanlienquans
                    .Where(p => p.intloai == (int)enumHosovanbanlienquan.intloai.Vanbandi)
                    .Where(p => p.intidhosocongviec == idhoso);

                detail.isVBDiLQ = vblqdi.Count() > 0 ? true : false;


                detail.isVBAttachLQ = false;


                return detail;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw (ex);
            }

        }

        /// <summary>
        ///  tra ve intiddoituongxuly cua idcanbo dang xem/xu ly ho so
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="idcanbo"></param>
        /// <returns>0:khong phai user dang trong luong xu ly</returns>
        public int GetIdDoituongxuly(int idhoso, int idcanbo)
        {
            int IdCurrentUser = 0;
            var dtxl = _doituongRepo.GetCanboDangXulys
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
        /// kiem tra xem user co dang la xu ly chinh ho so khong
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="idcanbo"></param>
        /// <returns></returns>
        private bool _CheckXulychinhHoso(int idhoso, int idcanbo)
        {
            var dtxl = _doituongRepo.GetCanboDangXulys
                .Where(p => p.intidhosocongviec == idhoso)
                .Where(p => p.intidcanbo == idcanbo)
                .Where(p=>p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Xulychinh)
                .FirstOrDefault();
            if (dtxl != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion Thongtinchung

        #region Phoihopxuly

        /// <summary>
        /// lay danh sach cac tat cac user trong don vi
        /// neu user da tham gia xu ly roi thi isCheck = true
        /// neu user la nguoi phoi hop xu ly thi isphoihopxuly =true
        /// de chi cho phep them/bot nguoi phoi hop xu ly thoi
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <returns></returns>
        public PhoihopXulyViewModel GetUserPhoihopxuly(int idhosocongviec)
        {
            PhoihopXulyViewModel user = new PhoihopXulyViewModel();

            user.idhoso = idhosocongviec;

            user.canbo = _canboRepo.GetActiveCanbo
                        .GroupJoin(
                            _doituongRepo.GetCanboDangXulys
                                .Where(p => p.intidhosocongviec == idhosocongviec),
                            cb => 1,
                            dt => 1,
                            (cb, dt) => new { cb, dt }
                        )
                        .Select(p => new UserPhoihopxuly
                        {
                            intid = p.cb.intid,
                            IsCheck = p.dt.Any(x => x.intidcanbo == p.cb.intid),
                            IsPhoihopxuly = p.dt
                                    .Where(x => x.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Phoihopxuly)
                                    .Any(x => x.intidcanbo == p.cb.intid),
                            strkyhieu = p.cb.strkyhieu,
                            strhoten = p.cb.strhoten,
                            intiddonvi = (int)p.cb.intdonvi
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

            return user;
        }

        /// <summary>
        /// them can bo phoi hop xu ly vao ho so
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultFunction SavePhoihopxuly(int idhosocongviec, List<int> listidphoihopxuly)
        {
            //===============================================
            // tạo 1 mảng lưu các giá trị idcanbo đã chọn
            // kiểm tra xem trong list idcanbo, chọn ra những idcanbo
            // là phối hợp xử lý
            // so sánh vói ds phxl trong ho so
            // nếu chưa có: thêm vào
            // và kiểm tra xem những ds phxl mà không có trong ds chọn thì
            // chuyển vai trò xử lý
            // (do có thể thêm/bớt người phối hợp xử lý)
            // (xử lý giống như lãnh đạo phụ trách)
            //===============================================
            ResultFunction kq = new ResultFunction();
            kq.id = (int)ResultViewModels.Error;

            var dtxl = _doituongRepo.GetCanboDangXulys
                            .Where(p => p.intidhosocongviec == idhosocongviec)
                            .ToList();

            var phoihopxuly = dtxl.Where(p => p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Phoihopxuly);

            if (listidphoihopxuly.Count() == 0)
            {   // neu submit khong chon nguoi phoi hop xu ly
                // thi chuyen vai tro xu ly cua tat ca nguoi phxl dang co
                if (phoihopxuly.Count() != 0)
                {
                    try
                    {
                        foreach (var p in phoihopxuly)
                        {
                            int idcanbo = _session.GetUserId();
                            _doituongRepo.CapnhatVaitrocu(p.intid, idcanbo);

                            // xoa trong tonghopcanbo
                            _XoaTonghopCanboVBDen(idhosocongviec, (int)p.intidcanbo);
                        }
                        kq.id = (int)ResultViewModels.Success;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message);
                    }
                }
            }
            else
            {   // neu submit co nguoi phoi hop xu ly
                if (phoihopxuly.Count() == 0)
                {   // ho so chua co nguoi phoi hop xu ly
                    // them moi vao
                    try
                    {
                        foreach (int p in listidphoihopxuly)
                        {
                            int idcanbo = p;
                            // chua co ten trong doi tuong xu ly thi them vao
                            _ThemDoituongxuly(idhosocongviec, idcanbo, (int)enumDoituongxuly.intvaitro_doituongxuly.Phoihopxuly);

                            // them trong tonghopcanbo
                            _ThemTonghopCanbo(idhosocongviec, idcanbo, (int)enumTonghopCanbo.intloai.HosoXLVBDen);
                        }
                        kq.id = (int)ResultViewModels.Success;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message);
                    }
                }
                else
                {   // ho so da co nguoi phoi hop xu ly
                    // nếu submit có phoi hop xu ly
                    // kiểm tra xem phxl có bị thay đổi sang người mới không
                    // (giong nhu lanh dao phu trach)
                    //======================================================
                    // phxl: 2,5,17
                    // sub (submit) : 2,4
                    // đổ phần tử mảng sub vào mảng phxl: 2,5,17,4
                    // so sánh với mảng sub để tìm những phần tử không có
                    // và đổi vai trò, phxl: 5, 17
                    //======================================================
                    try
                    {
                        foreach (int p in listidphoihopxuly)
                        {
                            bool isfound = false;
                            int idcanbo = p;
                            foreach (var d in phoihopxuly)
                            {
                                if (d.intidcanbo == idcanbo)
                                {
                                    isfound = true;
                                }
                            }
                            if (isfound == false)
                            {   // neu khong tim thay thi them moi
                                // do phan tu mang sub vao mang phxl
                                _ThemDoituongxuly(idhosocongviec, idcanbo, (int)enumDoituongxuly.intvaitro_doituongxuly.Phoihopxuly);

                                // them trong tonghopcanbo
                                _ThemTonghopCanbo(idhosocongviec, idcanbo, (int)enumTonghopCanbo.intloai.HosoXLVBDen);
                            }
                        }

                        var nguoiphxl = _doituongRepo.Doituongxulys
                                        .Where(p => p.intidhosocongviec == idhosocongviec)
                                        .Where(p => p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Phoihopxuly)
                                        .ToList();
                        // doi vai tro xu ly cua nhung nguoi phoi hop xu ly khong co trong danh sach submit
                        foreach (var p in nguoiphxl)
                        {
                            bool isfound = false;
                            foreach (int c in listidphoihopxuly)
                            {
                                int idcanbo = Convert.ToInt32(c);
                                if (p.intidcanbo == idcanbo)
                                {
                                    isfound = true;
                                }
                            }
                            if (isfound == false)
                            {   // neu khong tim thay thi chuyen vai tro sang vai tro cu
                                int _idcanbo = _session.GetUserId();
                                _doituongRepo.CapnhatVaitrocu(p.intid, _idcanbo);

                                _XoaTonghopCanboVBDen(idhosocongviec, (int)p.intidcanbo);
                            }
                        }
                        kq.id = (int)ResultViewModels.Success;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message);
                    }
                }
            }
            return kq;
        }

        #endregion Phoihopxuly

        #region Ykien

        /// <summary>
        /// ghi nhan y kien xu ly cua can bo
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="strykien"></param>
        /// <returns></returns>
        public ResultFunction SaveYkienxuly(int idhosocongviec, string strykien)
        {
            ResultFunction kq = new ResultFunction();
            kq.id = (int)ResultViewModels.Error;

            // kiem tra user co quyen xu ly hs
            int idcanbo = _session.GetUserId();
            if (!_role.IsDangXulyHosocongviec(idhosocongviec, idcanbo))
            {
                string error = AppConts.ErrLog + " hồ sơ: " + idhosocongviec.ToString();
                _logger.Warn(error);
                kq.id = (int)ResultViewModels.Error;
                kq.message = error;
            }
            else
            {   // kiem tra xem ho so da dong chua
                if (_role.CheckHoanthanhHoso(idhosocongviec))
                {
                    string error = "Hồ sơ " + idhosocongviec.ToString() + " đã hoàn thành, không thể cho ý kiến xử lý";
                    _logger.Warn(error);
                    kq.id = (int)ResultViewModels.Error;
                    kq.message = error;
                }

                var dtxl = _doituongRepo.GetCanboDangXulys
                        .Where(p => p.intidhosocongviec == idhosocongviec)
                        .Where(p => p.intidcanbo == idcanbo)
                        .OrderBy(p => p.intvaitro)
                        .Select(p => p.intid);
                // 1 user co the co nhieu vai tro trong xu ly ho so
                // chon ghi y kien o vai tro cao nhat

                if (dtxl.Count() != 0)
                {
                    Hosoykienxuly yk = new Hosoykienxuly
                    {
                        intiddoituongxuly = dtxl.FirstOrDefault(),
                        intidnguoilap = idcanbo,
                        //strthoigian = DateTime.Now, da co trong gia tri mac dinh
                        strykien = strykien,
                        inttrangthai = (int)enumHosoykienxuly.inttrangthai.Dachoykien
                    };
                    try
                    {
                        kq.id = _hosoykienRepo.Them(yk);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message);
                        kq.message = ex.Message;
                    }
                }
            }
            return kq;
        }

        /// <summary>
        /// HIEN KHONG SU DUNG
        /// lay id y kien de attach file, inttrangthai=0
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        public ResultFunction GetIdYkien(int idhosocongviec)
        {
            ResultFunction kq = new ResultFunction();
            kq.id = (int)ResultViewModels.Error;

            // kiem tra user co quyen xu ly hs
            int idcanbo = _session.GetUserId();
            if (!_role.IsDangXulyHosocongviec(idhosocongviec, idcanbo))
            {
                string error = AppConts.ErrLog + " hồ sơ: " + idhosocongviec.ToString();
                _logger.Warn(error);
                kq.id = (int)ResultViewModels.Error;
                kq.message = error;
            }
            else
            {   // kiem tra xem ho so da dong chua
                if (_role.CheckHoanthanhHoso(idhosocongviec))
                {
                    string error = "Hồ sơ " + idhosocongviec.ToString() + " đã hoàn thành, không thể cho ý kiến xử lý";
                    _logger.Warn(error);
                    kq.id = (int)ResultViewModels.Error;
                    kq.message = error;
                }

                int iduser = _session.GetUserId();
                var hsykien = _hosoykienRepo.Hosoykienxulys
                    .Where(p => p.intidnguoilap == iduser);

                var dtxl = _doituongRepo.GetCanboDangXulys
                        .Where(p => p.intidhosocongviec == idhosocongviec)
                        .Where(p => p.intidcanbo == idcanbo)
                        .OrderBy(p => p.intvaitro)
                        .Select(p => p.intid);
                // 1 user co the co nhieu vai tro trong xu ly ho so
                // chon ghi y kien o vai tro cao nhat

                if (dtxl.Count() != 0)
                {
                    Hosoykienxuly yk = new Hosoykienxuly
                    {
                        intiddoituongxuly = dtxl.FirstOrDefault(),
                        intidnguoilap = idcanbo,
                        //strthoigian = DateTime.Now, da co trong gia tri mac dinh
                        strykien = string.Empty,
                        inttrangthai = (int)enumHosoykienxuly.inttrangthai.DangchoYkien
                    };
                    try
                    {
                        kq.id = _hosoykienRepo.Them(yk);
                        kq.message = "idykien";
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message);
                        kq.message = ex.Message;
                    }
                }
            }
            return kq;
        }

        /// <summary>
        /// HIEN KHONG SU DUNG
        /// update y kien xu ly cua can bo sau khi da attach file
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="strykien"></param>
        /// <returns></returns>
        public ResultFunction UpdateYkienxuly(int idhosocongviec, int idykien, string strykien)
        {
            ResultFunction kq = new ResultFunction();
            kq.id = (int)ResultViewModels.Error;

            // kiem tra user co quyen xu ly hs
            int idcanbo = _session.GetUserId();
            if (!_role.IsDangXulyHosocongviec(idhosocongviec, idcanbo))
            {
                string error = AppConts.ErrLog + " hồ sơ: " + idhosocongviec.ToString();
                _logger.Warn(error);
                kq.id = (int)ResultViewModels.Error;
                kq.message = error;
            }
            else
            {   // kiem tra xem ho so da dong chua
                if (_role.CheckHoanthanhHoso(idhosocongviec))
                {
                    string error = "Hồ sơ " + idhosocongviec.ToString() + " đã hoàn thành, không thể cho ý kiến xử lý";
                    _logger.Warn(error);
                    kq.id = (int)ResultViewModels.Error;
                    kq.message = error;
                }

                var hsykien = _hosoykienRepo.Hosoykienxulys.FirstOrDefault(p => p.intid == idykien);
                if (hsykien != null)
                {
                    hsykien.strykien = strykien;
                    hsykien.strthoigian = DateTime.Now;
                    hsykien.inttrangthai = (int)enumHosoykienxuly.inttrangthai.Dachoykien;

                    _hosoykienRepo.Sua(idykien, hsykien);
                    kq.id = (int)ResultViewModels.Success;
                }
            }
            return kq;
        }

        #endregion Ykien

        #region KetthucHoso

        public ResultFunction LuuHoso(int idhosocongviec)
        {
            ResultFunction kq = new ResultFunction();

            // kiem tra user co quyen xu ly hs
            int idcanbo = _session.GetUserId();
            if (!_role.IsDangXulyHosocongviec(idhosocongviec, idcanbo))
            {
                string error = AppConts.ErrLog + " hồ sơ: " + idhosocongviec.ToString();
                _logger.Warn(error);
                kq.id = (int)ResultViewModels.Error;
                kq.message = error;
            }
            else
            {
                // kiem tra xem ho so da dong chua
                if (_role.CheckHoanthanhHoso(idhosocongviec))
                {
                    string error = "Hồ sơ " + idhosocongviec.ToString() + " đã hoàn thành, không thể cho ý kiến xử lý";
                    //_logger.Warn(error);
                    kq.id = (int)ResultViewModels.Error;
                    kq.message = error;
                }

                if (_role.CheckQuyenDongHoso(idhosocongviec))
                {
                    // tu dong luu y kien xu ly
                    SaveYkienxuly(idhosocongviec, "Lưu theo dõi");
                    // dong ho so
                    _hosocongviecRepo.LuuHoso(idhosocongviec, idcanbo);

                    // ghi nhat ky nguoi ket thuc
                    ChitietHoso ct = new ChitietHoso();
                    ct.intidcanbo = idcanbo;
                    ct.intidhosocongviec = idhosocongviec;
                    ct.intnguoitao = idcanbo;
                    ct.intvaitro = (int)enumDoituongxuly.intvaitro_chitiethoso.KetthucHoso;
                    _chitietHosoRepo.Them(ct);
                    // _ThemDoituongxuly(idhosocongviec, idcanbo, (int)EnumVanban.intvaitro_doituongxuly.KetthucHoso);

                    kq.id = (int)ResultViewModels.Success;
                }

            }
            return kq;
        }

        public ResultFunction Trinhky(int idhosocongviec)
        {
            ResultFunction kq = new ResultFunction();

            // kiem tra user co quyen xu ly hs
            int idcanbo = _session.GetUserId();
            if (!_role.IsDangXulyHosocongviec(idhosocongviec, idcanbo))
            {
                string error = AppConts.ErrLog + " hồ sơ: " + idhosocongviec.ToString();
                _logger.Warn(error);
                kq.id = (int)ResultViewModels.Error;
                kq.message = error;
            }
            else
            {
                // kiem tra xem ho so da dong chua
                if (_role.CheckHoanthanhHoso(idhosocongviec))
                {
                    string error = "Hồ sơ " + idhosocongviec.ToString() + " đã hoàn thành, không thể cho ý kiến xử lý";
                    //_logger.Warn(error);
                    kq.id = (int)ResultViewModels.Error;
                    kq.message = error;
                }
                // tu dong them y kien xu ly
                SaveYkienxuly(idhosocongviec, "Đang trình ký");
                // dang trinh ky
                _hosocongviecRepo.Trinhky(idhosocongviec, idcanbo);


                kq.id = (int)ResultViewModels.Success;
            }
            return kq;
        }

        public ResultFunction HoanthanhHoso(int idhosocongviec)
        {
            ResultFunction kq = new ResultFunction();

            // kiem tra user co quyen xu ly hs
            int idcanbo = _session.GetUserId();
            if (!_role.IsDangXulyHosocongviec(idhosocongviec, idcanbo))
            {
                string error = AppConts.ErrLog + " hồ sơ: " + idhosocongviec.ToString();
                _logger.Warn(error);
                kq.id = (int)ResultViewModels.Error;
                kq.message = error;
            }
            else
            {
                // kiem tra xem ho so da dong chua
                if (_role.CheckHoanthanhHoso(idhosocongviec))
                {
                    string error = "Hồ sơ " + idhosocongviec.ToString() + " đã hoàn thành, không thể cho ý kiến xử lý";
                    //_logger.Warn(error);
                    kq.id = (int)ResultViewModels.Error;
                    kq.message = error;
                }
                if (_role.CheckQuyenDongHoso(idhosocongviec))
                {
                    _hosocongviecRepo.HoanthanhHoso(idhosocongviec, idcanbo);
                    // ghi nhat ky nguoi ket thuc

                    ChitietHoso ct = new ChitietHoso();
                    ct.intidcanbo = idcanbo;
                    ct.intidhosocongviec = idhosocongviec;
                    ct.intnguoitao = idcanbo;
                    ct.intvaitro = (int)enumDoituongxuly.intvaitro_chitiethoso.KetthucHoso;
                    _chitietHosoRepo.Them(ct);
                    // _ThemDoituongxuly(idhosocongviec, idcanbo, (int)EnumVanban.intvaitro_doituongxuly.KetthucHoso);

                    kq.id = (int)ResultViewModels.Success;
                }

            }
            return kq;
        }

        public ResultFunction LuuNhieuHoso(List<int> listidvanban)
        {
            ResultFunction kq = new ResultFunction();

            foreach (int idvanban in listidvanban)
            {
                var hoso = _hosovanbanRepo.Hosovanbans
                        .Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden)
                        .Where(p => p.intidvanban == idvanban)
                        .FirstOrDefault();

                if (hoso != null)
                {
                    kq = LuuHoso(hoso.intidhosocongviec);
                }
            }
            return kq;
        }

        public int? GetIdVanbanden(int idhoso)
        {
            var hosovb =
                _hosovanbanRepo.Hosovanbans.FirstOrDefault(
                    x => x.intidhosocongviec == idhoso && x.intloai == (int) enumHosovanban.intloai.Vanbanden);

            if (hosovb == null) return null;

            return hosovb.intidvanban;
        }
        public ResultFunction HoanthanhNhieuHoso(List<int> listidvanban)
        {
            ResultFunction kq = new ResultFunction();

            foreach (int idvanban in listidvanban)
            {
                var hoso = _hosovanbanRepo.Hosovanbans
                        .Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden)
                        .Where(p => p.intidvanban == idvanban)
                        .FirstOrDefault();

                if (hoso != null)
                {
                    kq = HoanthanhHoso(hoso.intidhosocongviec);
                }
            }
            return kq;
        }

        #endregion KetthucHoso

        #region Phieutrinh

        /// <summary>
        /// thong tin phieu trinh
        /// </summary>
        /// <param name="idphieutrinh"></param>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        public PhieutrinhViewModel GetPhieutrinh(int idphieutrinh, int idhoso)
        {
            PhieutrinhViewModel model = new PhieutrinhViewModel();
            model.idphieutrinh = idphieutrinh;
            model.idhoso = idhoso;
            model.lanhdao = GetDanhsachNguoixuly(idhoso)
                .Where(p => p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaogiaoviec
                    || p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaophutrach
                );

            if (idphieutrinh == 0)
            {
                // them moi
                model.strnoidungtrinh = null;
                return model;
            }
            else
            {
                // load phieu trinh
                var phieutrinh = _phieutrinhRepo.Phieutrinhs
                    .FirstOrDefault(p => p.intid == idphieutrinh);

                model.idcanbotrinh = (int)phieutrinh.intidcanbotrinh;
                model.strnoidungtrinh = phieutrinh.strnoidungtrinh;
                model.strngaytrinh = DateServices.FormatDateTimeVN(phieutrinh.strngaytrinh);

                model.idlanhdaotrinh = (int)phieutrinh.intidlanhdao;
                model.strlanhdaotrinh = model.lanhdao.FirstOrDefault(p => p.idcanbo == model.idlanhdaotrinh).strhoten;

                model.strykienchidao = phieutrinh.strykienchidao;
                model.strngaychidao = DateServices.FormatDateTimeVN(phieutrinh.strngaychidao);

                return model;
            }
        }

        /// <summary>
        /// them moi noi dung trinh lanh dao
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="idlanhdaotrinh"></param>
        /// <param name="strnoidungtrinh"></param>
        /// <returns></returns>
        public int SaveNoidungtrinh(int idhoso, int idlanhdaotrinh, string strnoidungtrinh)
        {
            int iduser = _session.GetUserId();
            int idcanbotrinh = _doituongRepo.GetCanboDangXulys
                .Where(p => p.intidhosocongviec == idhoso)
                .Where(p => p.intidcanbo == iduser)
                .OrderBy(p => p.intvaitro)
                .FirstOrDefault().intid;

            int idphieutrinh = _phieutrinhRepo.ThemNoidungtrinh(idhoso, idcanbotrinh, idlanhdaotrinh, strnoidungtrinh);

            // them tong hop can bo
            int? idlanhdaotonghop = _doituongRepo.GetCanboDangXulys
                .Where(p => p.intidhosocongviec == idhoso)
                .Where(p => p.intid == idlanhdaotrinh)
                .FirstOrDefault().intidcanbo;

            _ThemTonghopCanbo(idhoso, (int)idlanhdaotonghop, (int)enumTonghopCanbo.intloai.Phieutrinh);

            return idphieutrinh;
        }

        /// <summary>
        /// lanh dao cho y kien
        /// </summary>
        /// <param name="idphieutrinh"></param>
        /// <param name="idlanhdao"></param>
        /// <param name="strykienchidao"></param>
        /// <returns></returns>
        public int SaveYkienchidao(int idphieutrinh, int idlanhdao, string strykienchidao)
        {
            int iduser = _session.GetUserId();
            int? idhosocongviec = _phieutrinhRepo.Phieutrinhs
                    .Where(p => p.intid == idphieutrinh)
                    .FirstOrDefault().intidhosocongviec;
            // chi sau khi cho y kien phieu trinh thi moi xoa 
            _XoaTonghopCanboHSXLVBDen((int)idhosocongviec, iduser);

            return _phieutrinhRepo.ThemYkienchidao(idphieutrinh, idlanhdao, strykienchidao);
        }

        #endregion Phieutrinh

        #region Quytrinh

        /// <summary>
        /// dung thuat toan BFS duyet quy trinh.tra ve ds cac node da duyet
        /// </summary>
        /// <param name="quytrinh"></param>
        /// <param name="idnodeStart"></param>
        /// <param name="idnodeEnd"></param>
        /// <returns></returns>
        private List<int> _DuyetQuytrinh(List<HosoQuytrinhXuly> quytrinh, int idnodeStart, int idnodeEnd)
        {
            List<int> listNodeDaDuyet = new List<int>();
            List<int> listAllNode = new List<int>();

            foreach (var p in quytrinh)
            {
                if (!listAllNode.Contains(p.intidFrom))
                {
                    listAllNode.Add(p.intidFrom);
                }
            }
            int intidNodeStart = idnodeStart; //quytrinh.Where(p => p.nodeidFrom == "node_begin").FirstOrDefault().intidFrom; ;
            int intidNodeEnd = idnodeEnd;//quytrinh.Where(p => p.nodeidFrom == "node_end").FirstOrDefault().intidFrom;

            List<int> listNodeOld = new List<int>();
            List<int> listNodeNew = new List<int>();

            listNodeOld.Add(intidNodeStart);
            if (!listAllNode.Contains(intidNodeEnd)) { listAllNode.Add(intidNodeEnd); }

            // danh dau nhung node yeu cau xu ly dong thoi
            // (nhung node ket thuc xu ly song song)
            List<int> listNodeDongthoi = new List<int>();
            var xulydongthoi = quytrinh.Where(p => p.intXulyDongthoi == (int)enumQuytrinhXuly.intXulyDongthoi.Co);
            if (xulydongthoi != null)
            {
                foreach (var x in xulydongthoi)
                {
                    listNodeDongthoi.Add(x.intidFrom);
                }
            }
            while ((listNodeDaDuyet.Count < listAllNode.Count) && (intidNodeStart != intidNodeEnd))
            {
                listNodeNew = new List<int>();
                foreach (var node in listNodeOld)
                {
                    intidNodeStart = node;
                    if (intidNodeStart == intidNodeEnd) { break; }
                    listNodeDaDuyet.Add(node);
                    var duongdi = quytrinh.Where(d => d.intidFrom == node);
                    foreach (var d in duongdi)
                    {
                        if (!listNodeDaDuyet.Contains((int)d.intidTo))
                        {   // tap moi gom nhung node chua duyet va gan ke 
                            if (!listNodeNew.Contains((int)d.intidTo))
                            {
                                listNodeNew.Add((int)d.intidTo);
                            }
                        }
                    }

                }
                listNodeOld = listNodeNew;
            }

            return listNodeDaDuyet;
        }

        /// <summary>
        /// ket thuc 1 buoc xu ly trong quy trinh va chuyen sang buoc tiep theo
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        public ResultFunction KetthucBuocXuly(int idhoso)
        {
            ResultFunction kq = new ResultFunction();
            int idNextNode = 0;
            if (!_KiemtraDieukienBuocXuly(idhoso))
            {
                idNextNode = _UpdateTinhtrangBuocXuly(idhoso);
            }
            else
            {   // co dieu kien re nhanh
                //idNextNode = _UpdateTinhtrangBuocXuly_ReNhanh(idhoso);
            }
            if (idNextNode > 0)
            {
                // cap nhat tinh trang xu ly xong thi them buoc xu ly
                var hosoquytrinh = _hsqtRepo.HosoQuytrinhxulys
                            .Where(p => p.intidhoso == idhoso)
                            .OrderBy(p => p.intid)
                            .ToList();

                //_AddBuocXuly(idhoso, hosoquytrinh, idNextNode);
                int count = 0;
                while ((idNextNode > 0) && (count < 5))
                {   // chi cho phep chay 5 lan 
                    idNextNode = _AddBuocXuly(idhoso, hosoquytrinh, idNextNode);
                    count++;
                }
                kq.id = (int)ResultViewModels.Success;
            }
            return kq;
        }

        /// <summary>
        ///  kiểm tra tại bước này có điều kiện rẽ nhánh không?
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        private bool _KiemtraDieukienBuocXuly(int idhoso)
        {
            try
            {
                bool result = false;
                int idcanbo = _session.GetUserId();
                var hs = _hsqtRepo.HosoQuytrinhxulys
                    .Where(p => p.intidhoso == idhoso)
                    .Where(p => p.intidCanbo == idcanbo);
                if (hs.Count() > 1)
                {
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// cap nhat tinh trang dang xu ly ==> da xu ly.Tra ve idNextNode
        /// </summary>
        /// <param name="idhoso"></param>
        private int _UpdateTinhtrangBuocXuly(int idhoso)
        {
            int result = 0;
            try
            {
                int idcanbo = _session.GetUserId();
                var hs = _hsqtRepo.HosoQuytrinhxulys
                    .Where(p => p.intidhoso == idhoso)
                    .Where(p => p.intidCanbo == idcanbo)
                    .Where(p => p.inttrangthai == (int)enumHosoQuytrinhXuly.inttrangthai.DangXuly)
                    .ToList();
                foreach (var p in hs)
                {
                    _hsqtRepo.CapnhatTrangthai_DaXuly(p.intid, (int)enumHosoQuytrinhXuly.inttrangthai.DaXuly);
                    result = (int)p.intidTo;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);

            }
            return result;
        }

        /// <summary>
        /// cap nhat tinh trang dang xu ly ==> da xu ly tai buoc xu ly re nhanh. Tra ve idNextNode
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns>idNextNode</returns>
        private int _UpdateTinhtrangBuocXuly_ReNhanh(int idhoso)
        {
            int result = 0;
            try
            {
                int idcanbo = _session.GetUserId();
                List<int> listIdNodeFromTo = new List<int>();
                var hsFrom = _hsqtRepo.HosoQuytrinhxulys
                    .Where(p => p.intidhoso == idhoso)
                    .Where(p => p.intidCanbo == idcanbo)
                    //.Where(p => p.inttrangthai == (int)enumHosoQuytrinhXuly.inttrangthai.DangXuly)
                    //.FirstOrDefault();
                    .ToList();
                // lấy ds các node có thể đi từ nodeFrom
                foreach (var h in hsFrom)
                {
                    listIdNodeFromTo.Add((int)h.intidTo);
                }

                // lấy ds các nodeTo khác có thể đi tới nodeFrom
                int idNodeFrom = hsFrom.FirstOrDefault().intidFrom;
                var hsTo = _hsqtRepo.HosoQuytrinhxulys
                   .Where(p => p.intidhoso == idhoso)
                    //.Where(p => listIdNodeFrom.Contains((int)p.intidTo))
                    .Where(p => p.intidTo == idNodeFrom)
                   .ToList();
                foreach (var h in hsTo)
                {
                    if (listIdNodeFromTo.Contains(h.intidFrom))
                    {
                        listIdNodeFromTo.Remove(h.intidFrom);
                    }
                }
                int IdNextNode = 0;
                IdNextNode = listIdNodeFromTo.FirstOrDefault();
                int intidNodeFrom = hsFrom.FirstOrDefault(p => p.intidTo == IdNextNode).intid;

                foreach (var p in hsFrom)
                {   // cap nhat tat ca cac re nhanh thanh ==> da xu ly
                    _hsqtRepo.CapnhatTrangthai_DaXuly(p.intid, (int)enumHosoQuytrinhXuly.inttrangthai.DaXuly);
                }
                //_SaveYkienQuytrinh(idhoso, (int)hs.FirstOrDefault().intidCanbo);
                result = IdNextNode;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// them 1 buoc xu ly vao ho so. Tra ve 0: ket thuc; >0: idNextNode de them tiep khi buoc nay mac dinh hoan thanh
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <param name="hosoquytrinh"></param>
        /// <param name="idNextNode"></param>
        /// <returns></returns>
        private int _AddBuocXuly(int idhosocongviec, List<HosoQuytrinhXuly> hosoquytrinh, int idNextNode)
        {
            int result = 0;
            try
            {
                var hoso = hosoquytrinh.Where(x => x.intidhoso == idhosocongviec)
                    .Where(x => x.intidFrom == idNextNode);
                //.FirstOrDefault();

                // dang xu ly tai buoc re nhanh
                // nen tu 1 node co the di ra nhieu hon 1 node
                // duyet tung node cu the
                foreach (var hs in hoso)
                {
                    if ((hs.intVaitro > (int)enumQuytrinhXuly.intVaitro.Khongthamgia)
                            && (hs.intidCanbo > 0)
                            && (hs.inttrangthai != (int)enumHosoQuytrinhXuly.inttrangthai.DaXuly))
                    {
                        // them vao table doituongxuly: cán bộ xử lý tại bước này
                        // nếu tại bước này, có tự hoàn thành thì trả về idNextNode
                        //  insert vao table doituongxuly: lanh dao  giao viec, lanh dao phu trach , xu ly chinh 
                        //=============================================
                        // kiem tra dieu kien yeu cau xu ly dong thoi
                        bool isHoanthanh = false;
                        if (hs.intXulyDongthoi == (int)enumHosoQuytrinhXuly.intXulyDongthoi.Co)
                        {
                            // ds cac node tro toi node dang xet
                            var hsFrom = hosoquytrinh.Where(p => p.intidTo == idNextNode);
                            int countHoanthanh = 0;
                            foreach (var from in hsFrom)
                            {
                                if (from.inttrangthai == (int)enumHosoQuytrinhXuly.inttrangthai.DaXuly)
                                {
                                    countHoanthanh++;
                                }
                            }
                            if (countHoanthanh == hsFrom.Count())
                            {   // neu tat cac cac node deu da hoan thanh thi them node next vao doituongxuly
                                isHoanthanh = true;
                            }
                        }
                        else
                        {
                            isHoanthanh = true;
                        }
                        if (!isHoanthanh)
                        {   // neu co node chua hoan thanh thi thoat ra
                            break;
                        }
                        //==========================================
                        var dtxl = _doituongRepo.GetCanboDangXulys
                                    .Where(x => x.intidhosocongviec == idhosocongviec)
                                    .Where(p => p.intidcanbo == hs.intidCanbo)
                                    .Where(p => p.intvaitro == hs.intVaitro)
                                    .ToList();

                        if (dtxl.Count == 0)
                        {   // chua co trong doituongxuly, them moi
                            _ThemDoituongxuly(idhosocongviec, (int)hs.intidCanbo, (int)hs.intVaitro);
                        }

                        int idvanban = _hosovanbanRepo.Hosovanbans.FirstOrDefault(x => x.intidhosocongviec == idhosocongviec).intidvanban;

                        if (hs.intVaitro == (int)enumQuytrinhXuly.intVaitro.Xulychinh)
                        {   // chi cap nhat ten nguoi xu ly chinh(strnoinhan) cua van ban khi la xu ly chinh
                            _CapnhatXulychinhVanbanden(idvanban, (int)hs.intidCanbo, true);
                        }
                        else
                        {   // chi cap nhat ten nguoi xu ly khi van ban chua co 
                            var vanban = _vanbandenRepo.GetVanbandenById(idvanban);
                            if (string.IsNullOrEmpty(vanban.strnoinhan))
                            {
                                _CapnhatXulychinhVanbanden(idvanban, (int)hs.intidCanbo, true);
                            }
                        }

                        if (hs.intHoanthanh == (int)enumHosoQuytrinhXuly.intHoanthanh.Co)
                        {
                            _hsqtRepo.CapnhatTrangthai_DaXuly(hs.intid, (int)enumHosoQuytrinhXuly.inttrangthai.DaXuly);
                            result = (int)hs.intidTo;
                        }
                        else
                        {
                            _hsqtRepo.CapnhatTrangthai_DangXuly(hs.intid, (int)enumHosoQuytrinhXuly.inttrangthai.DangXuly);
                        }
                    }

                    if (hs.intVaitro == (int)enumQuytrinhXuly.intVaitro.Khongthamgia)
                    {   // khong thoa dieu kien
                        // intvaitro =0 (khong tham gia xu ly)
                        // van xet inttrangthai de cap nhat
                        if (hs.intHoanthanh == (int)enumHosoQuytrinhXuly.intHoanthanh.Co)
                        {
                            _hsqtRepo.CapnhatTrangthai_DaXuly(hs.intid, (int)enumHosoQuytrinhXuly.inttrangthai.DaXuly);
                            result = (int)hs.intidTo;
                        }
                        else
                        {
                            _hsqtRepo.CapnhatTrangthai_DangXuly(hs.intid, (int)enumHosoQuytrinhXuly.inttrangthai.DangXuly);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return result;
        }


        /// <summary>
        /// load flowchart
        /// HIEN KHONG SU DUNG
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns>json</returns>
        public string ReadFlowChart__(int idhoso)
        {
            try
            {
                var quytrinhs = _hsqtRepo.HosoQuytrinhxulys
                    .Where(p => p.intidhoso == idhoso)
                    .GroupJoin(
                        _canboRepo.GetAllCanbo,
                        hs => hs.intidCanbo,
                        cb => cb.intid,
                        (hs, cb) => new { hs, cb.FirstOrDefault().strhoten }
                    )
                    .ToList();

                int? numberOfElements = 1;

                Dictionary<int, string> nodeConnect = new Dictionary<int, string>();

                List<int> listidnode = new List<int>();

                List<NodeViewModel> nodeView = new List<NodeViewModel>();

                List<NodeXulyViewModel> Xulys = new List<NodeXulyViewModel>();

                var nodes = quytrinhs.GroupBy(p => p.hs.nodeidFrom)
                                .Select(p => p.First()).ToList();
                foreach (var p in nodes)
                {
                    nodeConnect.Add(p.hs.intidFrom, p.hs.nodeidFrom);
                    var node = new NodeViewModel
                    {
                        Id = p.hs.nodeidFrom,
                        text = p.hs.strTenNodeFrom,
                        left = (int)p.hs.intLeft,
                        top = (int)p.hs.intTop
                    };
                    nodeView.Add(node);

                    NodeXulyViewModel xl = new NodeXulyViewModel();
                    xl.Id = p.hs.nodeidFrom;
                    xl.strhotencanbo = p.strhoten;
                    // neu khong tham gia xu ly (vaitro=0) thi mac dinh da xu ly xong
                    //xl.inttrangthai = p.hs.intVaitro == 0 ? (int)enumHosoQuytrinhXuly.inttrangthai.DaXuly : p.hs.inttrangthai;
                    xl.inttrangthai = p.hs.inttrangthai;
                    Xulys.Add(xl);
                }
                List<ConnectionViewModel> connectionView = new List<ConnectionViewModel>();

                foreach (var conn in quytrinhs)
                {
                    ConnectionViewModel connect = new ConnectionViewModel();
                    connect.label = conn.hs.strlabel;
                    connect.from = conn.hs.nodeidFrom;
                    try
                    {
                        connect.to = nodeConnect[(int)conn.hs.intidTo];
                        connectionView.Add(connect);
                    }
                    catch
                    {
                        // node_end 
                        connect.to = null;
                        //connectionView.Add(connect);
                    }
                }

                // them vao flowchart
                FlowchartViewModel flowchart = new FlowchartViewModel();
                flowchart.nodes = nodeView;
                flowchart.connections = connectionView;
                flowchart.numberOfElements = (int)numberOfElements;
                flowchart.xulys = Xulys;

                string jsFlowchart = WriteJson(flowchart);
                return jsFlowchart;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }

        }
        /// <summary>
        /// load flowchart tu hosoquytrinhxuly
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        public string ReadFlowChart(int idhoso, int? intUpdate)
        {
            QuytrinhXulyViewModels model = new QuytrinhXulyViewModels();
            try
            {
                var quytrinh = _hsqtRepo.HosoQuytrinhxulys
                    .Where(p => p.intidhoso == idhoso)
                    .ToList();

                List<NodeViewModel> nodeView = new List<NodeViewModel>();
                List<ConnectionViewModel> connectionView = new List<ConnectionViewModel>();
                List<NodeXulyViewModel> Xulys = new List<NodeXulyViewModel>();
                string strNgayApdung = DateServices.FormatDateVN(quytrinh.FirstOrDefault().strNgayapdung);
                int idquytrinh = quytrinh.FirstOrDefault().intidquytrinh;

                var NodeDangXuly = quytrinh
                    .Where(p => p.inttrangthai == (int)enumHosoQuytrinhXuly.inttrangthai.DangXuly)
                    .FirstOrDefault();

                List<int?> listNodeNext = new List<int?>();
                if (NodeDangXuly != null)
                {
                    listNodeNext = quytrinh.Where(p => p.intidFrom == NodeDangXuly.intidFrom)
                            .Select(p => p.intidTo).ToList();
                }

                foreach (var q in quytrinh)
                {
                    var node = new NodeViewModel
                    {
                        Id = q.nodeidFrom,
                        text = q.strTenNodeFrom,
                        left = (int)q.intLeft,
                        top = (int)q.intTop
                    };

                    var findnode = nodeView.FirstOrDefault(p => p.Id == node.Id);
                    if (findnode == null)
                    {
                        nodeView.Add(node);
                    }

                    //===============================================
                    ConnectionViewModel connect = new ConnectionViewModel();
                    connect.label = q.strlabel;
                    connect.from = q.nodeidFrom;

                    string nodeTo = null;
                    if (q.intidTo != null)
                    {   // bo node_end
                        try
                        {
                            nodeTo = quytrinh.FirstOrDefault(p => p.intidFrom == q.intidTo).nodeidFrom;
                        }
                        catch
                        {
                            _logger.Error("Lỗi view quytrinh. intidTo :" + q.intidTo);
                        }
                        connect.to = nodeTo;
                        connectionView.Add(connect);
                    }
                    //==============================================
                    if (q.intidCanbo > 0)
                    {   // khong add node_begin va node_end (khong co idcanbo)
                        NodeXulyViewModel xuly = new NodeXulyViewModel();
                        xuly.Id = q.nodeidFrom;
                        var canbo = _canboRepo.GetAllCanboByID((int)q.intidCanbo);
                        xuly.strhotencanbo = canbo.strhoten;
                        xuly.intSongay = q.intSongay;
                        string strVaitro = "";
                        switch ((int)q.intVaitro)
                        {
                            case (int)enumEditThongtinXulyViewModel.Khongthamgia:
                                strVaitro = "Không tham gia xử lý";
                                break;
                            case (int)enumEditThongtinXulyViewModel.Lanhdaogiaoviec:
                                strVaitro = "Lãnh đạo giao việc";
                                break;
                            case (int)enumEditThongtinXulyViewModel.Lanhdaophutrach:
                                strVaitro = "Lãnh đạo phụ trách";
                                break;
                            case (int)enumEditThongtinXulyViewModel.Phoihopxuly:
                                strVaitro = "Phối hợp xử lý";
                                break;
                            case (int)enumEditThongtinXulyViewModel.Xulychinh:
                                strVaitro = "Xử lý chính";
                                break;
                        }
                        xuly.strVaitro = strVaitro;
                        xuly.intXulyDongthoi = q.intXulyDongthoi;

                        xuly.strNgaybd = DateServices.FormatDateVN(q.strNgaybd);
                        xuly.strNgaykt = DateServices.FormatDateVN(q.strNgaykt);

                        switch (q.inttrangthai)
                        {   // so sanh ngay bat dau va ngay ket thuc de tinh dunghan/trehan                           
                            case (int)enumHosoQuytrinhXuly.inttrangthai.DangXuly:
                                if (q.strNgaybd != null)
                                {
                                    DateTime dteNgayxuly = (DateTime)q.strNgaybd;
                                    dteNgayxuly = dteNgayxuly.AddDays((int)q.intSongay);
                                    if (dteNgayxuly >= DateTime.Now)
                                    {   // dang xu ly dung han : 1
                                        xuly.inttrangthai = q.inttrangthai;
                                    }
                                    else
                                    {   // dang xu ly tre han: 11
                                        xuly.inttrangthai = 11;
                                    }
                                }
                                else
                                {
                                    xuly.inttrangthai = q.inttrangthai;
                                }
                                break;
                            case (int)enumHosoQuytrinhXuly.inttrangthai.DaXuly:
                                if ((q.strNgaykt != null) && (q.strNgaybd != null))
                                {
                                    DateTime dteNgayxuly = (DateTime)q.strNgaybd;
                                    dteNgayxuly = dteNgayxuly.AddDays((int)q.intSongay);
                                    if (dteNgayxuly >= q.strNgaykt)
                                    {   // da xu ly dung han : 2
                                        xuly.inttrangthai = q.inttrangthai;
                                    }
                                    else
                                    {   // da xu ly tre han: 22
                                        xuly.inttrangthai = 22;
                                    }
                                }
                                else
                                {
                                    xuly.inttrangthai = q.inttrangthai;
                                }
                                break;
                            case (int)enumHosoQuytrinhXuly.inttrangthai.ChuaXuly:
                                xuly.inttrangthai = q.inttrangthai;
                                break;
                        }

                        xuly.intChon = 0;
                        if (intUpdate == 2)
                        {   // chon node tiep theo node dang xu ly
                            foreach (var n in listNodeNext)
                            {
                                if (n == q.intidFrom)
                                {
                                    xuly.intChon = 1;
                                }
                            }
                        }

                        Xulys.Add(xuly);
                    }
                }

                //var nodeDistinct = nodeView.Distinct();

                // them vao flowchart
                FlowchartViewModel flowchart = new FlowchartViewModel();
                flowchart.nodes = nodeView; //nodeDistinct; //nodeView;
                flowchart.connections = connectionView;
                flowchart.numberOfElements = 0;
                flowchart.xulys = Xulys;

                string strtenquytrinh = _quytrinhRepo.AllQuytrinhs
                        .FirstOrDefault(p => p.intid == idquytrinh).strten;
                flowchart.strNgayApdung = " Quy trình: " + strtenquytrinh + " . Ngày áp dụng: " + strNgayApdung;

                string jsFlowchart = WriteJson(flowchart);
                return jsFlowchart;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// convert flowchart to json
        /// </summary>
        /// <param name="flowchart"></param>
        /// <returns></returns>
        private string WriteJson(FlowchartViewModel flowchart)
        {
            try
            {
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(flowchart);
                return output;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Tại bước rẽ nhánh, quay trở lại bước xử lý trước đó (được ĐN trong quy trình)
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        public ResultFunction ReturnBuocXuly(int idhoso)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                int idcanbo = _session.GetUserId();
                var hsFrom = _hsqtRepo.HosoQuytrinhxulys
                    .Where(p => p.intidhoso == idhoso)
                    .Where(p => p.intidCanbo == idcanbo)
                    .Where(p => p.inttrangthai == (int)enumHosoQuytrinhXuly.inttrangthai.DangXuly)
                    .ToList();
                //.FirstOrDefault();
                int intidFrom = hsFrom.FirstOrDefault().intidFrom;

                var hs = _hsqtRepo.HosoQuytrinhxulys
                   .Where(p => p.intidhoso == idhoso)
                   .Where(p => p.intidTo == intidFrom)
                   .ToList();
                foreach (var p in hs)
                {
                    // cap nhat tinh trang xu ly cua buoc xu ly truoc do
                    _hsqtRepo.CapnhatTrangthai_DangXuly(p.intid, (int)enumHosoQuytrinhXuly.inttrangthai.DangXuly);
                }

                foreach (var p in hsFrom)
                {
                    // cap nhat tinh trang xu ly cua node hien tai 
                    _hsqtRepo.CapnhatTrangthai_DangXuly(p.intid, (int)enumHosoQuytrinhXuly.inttrangthai.ChuaXuly);
                }

                _SaveYkienQuytrinh(idhoso, (int)hs.FirstOrDefault().intidCanbo);
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
        /// ghi nhan y kien khi chuyen buoc xu ly
        /// </summary>
        /// <param name="idhoso"></param>
        private void _SaveYkienQuytrinh(int idhoso, int ToIdCanbo)
        {
            string strhoten = _canboRepo.GetActiveCanboByID(ToIdCanbo).strhoten;
            int idcanbo = _session.GetUserId();
            var dtxl = _doituongRepo.GetCanboDangXulys
                        .Where(p => p.intidhosocongviec == idhoso)
                        .Where(p => p.intidcanbo == idcanbo)
                        .OrderBy(p => p.intvaitro)
                        .Select(p => p.intid);
            Hosoykienxuly yk = new Hosoykienxuly
            {
                intiddoituongxuly = dtxl.FirstOrDefault(),
                intidnguoilap = idcanbo,
                //strthoigian = DateTime.Now, da co trong gia tri mac dinh
                strykien = "Trả hồ sơ cho " + strhoten,
                inttrangthai = (int)enumHosoykienxuly.inttrangthai.Dachoykien
            };
            try
            {
                _hosoykienRepo.Them(yk);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
        /// <summary>
        /// kiem tra ho so quy trinh co tam ngung khong?
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        private bool _IsTamngungQuytrinh(int idhoso)
        {
            var hoso = _hosocongviecRepo.GetHSCVById(idhoso);
            if (hoso.intluuhoso == (int)enumHosocongviec.intluuhoso.TamngungXL)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// cap nhat tinh trang tam ngung ho so quy trinh
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="valid"></param>
        /// <returns></returns>
        public ResultFunction TamngungQuytrinh(int idhoso, bool valid, string strngay)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                if (valid)
                {   // tam ngung xu ly                    
                    if (!string.IsNullOrEmpty(strngay))
                    {
                        int intNgay = Convert.ToInt32(strngay);

                        //int intluuhoso = (int)enumHosocongviec.intluuhoso.TamngungXL;
                        //_hosocongviecRepo.TamngungQuytrinh(idhoso, intluuhoso);
                        _TinhThemSoNgayTamngungQuytrinh(idhoso, intNgay);

                        // tu dong luu y kien xu ly
                        SaveYkienxuly(idhoso, "Tạm ngừng xử lý quy trình trong " + strngay + " ngày");

                    }
                }
                else
                {   // tiep tuc xu ly
                    //int intluuhoso = (int)enumHosocongviec.intluuhoso.Khong;
                    //_hosocongviecRepo.TamngungQuytrinh(idhoso, intluuhoso);
                    _TinhSongayTieptucXLQuytrinh(idhoso, DateTime.Now);

                    // tu dong luu y kien xu ly
                    SaveYkienxuly(idhoso, "Tiếp tục xử lý quy trình");

                    //_hosoQuytrinhRepo.CapnhatTrangthai(idhoso, (int)enumHosoQuytrinh.inttrangthai.TieptucXL);
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
        /// tinh so ngay da xu ly, so ngay con lai, va cong them thoi gian ngung
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="intSongay"></param>
        /// <returns></returns>
        private int _TinhThemSoNgayTamngungQuytrinh(int idhoso, int intSongay)
        {
            try
            {
                DateTime dteNow = DateTime.Now;
                DateTime dteHanxuly = DateServices.AddThoihanxuly(dteNow, intSongay);

                // cap nhat trang thai va thoi gian xu ly cua hosocongviec
                int intluuhoso = (int)enumHosocongviec.intluuhoso.TamngungXL;
                _hosocongviecRepo.TamngungQuytrinh(idhoso, intluuhoso, dteHanxuly);

                int idcanbo = _session.GetUserId();
                var quytrinh = _hsqtRepo.HosoQuytrinhxulys
                    .Where(p => p.intidCanbo == idcanbo)
                    .Where(p => p.inttrangthai == (int)enumHosoQuytrinhXuly.inttrangthai.DangXuly)
                    .FirstOrDefault();

                // ghi nhan tam ngung xl tai node nao
                HosoQuytrinh hoso = new HosoQuytrinh();
                hoso.intidcanbo = idcanbo;
                hoso.intidhoso = idhoso;
                //hoso.intidquytrinh = 
                hoso.intidFrom = quytrinh.intidFrom;
                hoso.intSongayNgungXuly = intSongay;
                hoso.strNgayNgungXuly = DateTime.Now;
                hoso.inttrangthai = (int)enumHosoQuytrinh.inttrangthai.Dangtamngung;
                _hosoQuytrinhRepo.Them(hoso);

                // cap nhat thoi gian xu ly tai node 
                int intSongayNgungXL = (int)quytrinh.intSongay + intSongay;
                _hsqtRepo.CapnhatThoigianXuly(idhoso, quytrinh.intidFrom, intSongayNgungXL);

                return (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return (int)ResultViewModels.Error;
            }
        }

        /// <summary>
        /// tinh so ngay con lai cua quy trinh khi tiep tuc xu ly
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="dteNgayTieptuc"></param>
        /// <returns></returns>
        private int _TinhSongayTieptucXLQuytrinh(int idhoso, DateTime dteNgayTieptuc)
        {
            try
            {
                // so ngay thuc te duoc tam ngung xl
                var hsquytrinh = _hosoQuytrinhRepo.HosoQuytrinhs
                    .Where(p => p.intidhoso == idhoso)
                    .Where(p => p.inttrangthai == (int)enumHosoQuytrinh.inttrangthai.Dangtamngung)
                    .FirstOrDefault();
                //.ToList();

                // tinh so ngay thuc te ngung xu ly (khong tinh t7,cn, ngay le)
                int intSongayThucteNgungXL = DateServices.DemSongayThemvao((DateTime)hsquytrinh.strNgayNgungXuly, DateTime.Now);
                int intSongayNgungXL = (int)hsquytrinh.intSongayNgungXuly;

                if (intSongayThucteNgungXL < intSongayNgungXL)
                {
                    DateTime dteHanxuly = DateServices.AddThoihanxuly((DateTime)hsquytrinh.strNgayNgungXuly, intSongayThucteNgungXL);

                    // cap nhat han xu ly cua hosocongviec
                    // cap nhat trang thai va thoi gian xu ly cua hosocongviec
                    int intluuhoso = (int)enumHosocongviec.intluuhoso.Khong;
                    _hosocongviecRepo.TamngungQuytrinh(idhoso, intluuhoso, dteHanxuly);

                    //===============================================
                    int idcanbo = _session.GetUserId();
                    var quytrinh = _hsqtRepo.HosoQuytrinhxulys
                        .Where(p => p.intidCanbo == idcanbo)
                        .Where(p => p.inttrangthai == (int)enumHosoQuytrinhXuly.inttrangthai.DangXuly)
                        .FirstOrDefault();

                    int intSongayXL = 0;
                    // so ngay khai bao trong quy trinh
                    intSongayXL = (int)quytrinh.intSongay - intSongayNgungXL;
                    // so ngay xu ly = so ngay thuc te ngung xl + so ngay theo quy trinh
                    intSongayXL += intSongayThucteNgungXL;

                    // cap nhat so ngay xu ly tai node
                    _hsqtRepo.CapnhatThoigianXuly(idhoso, quytrinh.intidFrom, intSongayXL);

                }
                else if (intSongayThucteNgungXL > intSongayNgungXL)
                {   // giu nguyen thoi gian tiep tuc xu ly

                }

                _hosoQuytrinhRepo.CapnhatTrangthai(idhoso, (int)enumHosoQuytrinh.inttrangthai.TieptucXL);

                return (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return (int)ResultViewModels.Error;
            }
        }
        /// <summary>
        /// kiem tra xem co phai la nguoi phoi hop xu ly khong? de cho phep ghi y kien tai buoc xu ly nay
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        private bool _IsPhoihopXulyQuytrinh(int idhoso)
        {
            try
            {
                // lay can bo dang xu ly quy trinh
                var idcanboQuytrinh = _hsqtRepo.HosoQuytrinhxulys
                        .Where(p => p.intidhoso == idhoso)
                    //.Where(p => p.intidCanbo == idcanbo)
                        .Where(p => p.inttrangthai == (int)enumHosoQuytrinhXuly.inttrangthai.DangXuly)
                        .FirstOrDefault().intidCanbo;

                int idcanbo = _session.GetUserId();
                // lay can bo duoc them phoi hop xu ly boi canboquytrinh
                var doituong = _doituongRepo.GetCanboDangXulys
                        .Where(p => p.intidhosocongviec == idhoso)
                        .Where(p => p.intnguoitao == idcanboQuytrinh)
                        .Where(p => p.intidcanbo == idcanbo)
                        .Count();

                return (doituong > 0) ? true : false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// lay cac thong tin xu ly cua idhoso tai nodeid
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="NodeId"></param>
        /// <returns></returns>
        public EditHosoQuytrinhXulyViewModel GetEditHosoQuytrinhXuly(int idhoso, string NodeId)
        {
            EditHosoQuytrinhXulyViewModel model = new EditHosoQuytrinhXulyViewModel();

            model.listDonvi = _donviRepo.Donvitructhuocs
                   .Select(p => new QLVB.DTO.Donvi.EditDonviViewModel
                   {
                       intid = p.Id,
                       strtendonvi = p.strtendonvi
                   })
                   .OrderBy(p => p.strtendonvi);
            model.idhoso = idhoso;
            model.strNodeId = NodeId;

            var hoso = _hsqtRepo.HosoQuytrinhxulys
                .Where(p => p.intidhoso == idhoso)
                .Where(p => p.nodeidFrom == NodeId)
                .FirstOrDefault();
            if (hoso != null)
            {
                var xuly = _qtXulyRepo.QuytrinhXulys
                   .Where(p => p.intidNode == hoso.intidFrom)
                   .FirstOrDefault();

                model.intDonvi = xuly.intidDonvi;
                model.strTenNode = hoso.strTenNodeFrom;
                model.intCanbo = (int)hoso.intidCanbo;

                switch (hoso.intVaitro)
                {
                    case (int)enumQuytrinhXuly.intVaitro.Khongthamgia:
                        model.LoaiVaitro = enumEditThongtinXulyViewModel.Khongthamgia;
                        break;
                    case (int)enumQuytrinhXuly.intVaitro.Lanhdaogiaoviec:
                        model.LoaiVaitro = enumEditThongtinXulyViewModel.Lanhdaogiaoviec;
                        break;
                    case (int)enumQuytrinhXuly.intVaitro.Lanhdaophutrach:
                        model.LoaiVaitro = enumEditThongtinXulyViewModel.Lanhdaophutrach;
                        break;
                    case (int)enumQuytrinhXuly.intVaitro.Xulychinh:
                        model.LoaiVaitro = enumEditThongtinXulyViewModel.Xulychinh;
                        break;
                    case (int)enumQuytrinhXuly.intVaitro.Phoihopxuly:
                        model.LoaiVaitro = enumEditThongtinXulyViewModel.Phoihopxuly;
                        break;
                    default:
                        model.LoaiVaitro = enumEditThongtinXulyViewModel.Khongthamgia;
                        break;
                }
            }

            return model;
        }

        /// <summary>
        /// cap nhat thay doi hoso quy trinh xu ly: thay doi can bo 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultFunction UpdateHosoQuytrinhXuly(EditHosoQuytrinhXulyViewModel model)
        {
            ResultFunction kq = new ResultFunction();
            var result = _hsqtRepo.CapnhatCanbo(model.idhoso, model.strNodeId, model.intCanbo);
            if (result)
            {
                // tu dong luu y kien xu ly
                SaveYkienxuly(model.idhoso, "Thay đổi cán bộ xử lý tại bước " + model.strTenNode);

                kq.id = (int)ResultViewModels.Success;
            }
            else
            {
                kq.id = (int)ResultViewModels.Error;
                kq.message = "Lỗi! Không cập nhật thành công";
            }

            return kq;
        }

        /// <summary>
        /// tuy chon buoc xu ly tiep theo khi co >2 buoc xu ly tiep
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="NodeId"></param>
        /// <returns></returns>
        public ResultFunction ChonBuocXuly(int idhoso, string jsNodes)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                _UpdateBuocXuly(idhoso, jsNodes);
                kq.id = (int)ResultViewModels.Success;
                kq.message = "1";//ReadFlowChart(idhoso, 0);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
            }
            return kq;
        }

        private bool _UpdateBuocXuly(int idhoso, string jsNodes)
        {
            try
            {
                var quytrinh = _hsqtRepo.HosoQuytrinhxulys
                    .Where(p => p.intidhoso == idhoso)
                    .ToList();

                var NodeDangxuly = quytrinh
                    .Where(p => p.inttrangthai == (int)enumHosoQuytrinhXuly.inttrangthai.DangXuly)
                    .FirstOrDefault();

                _hsqtRepo.CapnhatTrangthai_DaXuly(idhoso, NodeDangxuly.nodeidFrom, (int)enumHosoQuytrinhXuly.inttrangthai.DaXuly);

                //_hsqtRepo.CapnhatTrangthai(idhoso, NodeId, (int)enumHosoQuytrinhXuly.inttrangthai.DangXuly);

                int idNodeStart = quytrinh.FirstOrDefault(p => p.nodeidFrom == "node_begin").intidFrom;

                string[] strNodes = jsNodes.Split(new Char[] { ';' });
                foreach (var NodeId in strNodes)
                {
                    if (!string.IsNullOrEmpty(NodeId))
                    {
                        var nodeNext = quytrinh
                            .Where(p => p.nodeidFrom == NodeId)
                            .FirstOrDefault();
                        if (nodeNext.inttrangthai == (int)enumHosoQuytrinhXuly.inttrangthai.DaXuly)
                        {   // neu node da xu ly 
                            // tra ho so ve buoc truoc do
                            // va kiem tra lai quy trinh xu ly 

                            // lay ds cac node da duyet tu dau den node nay
                            List<int> listNodeDaduyet = _DuyetQuytrinh(quytrinh, idNodeStart, nodeNext.intidFrom);

                            // cap nhat toan bo cac node con lai thanh chua xu ly
                            _hsqtRepo.CapnhatTrangthai(idhoso, listNodeDaduyet, (int)enumHosoQuytrinhXuly.inttrangthai.ChuaXuly);

                            // cap nhat trang thai cua node dang xet
                            _hsqtRepo.CapnhatTrangthai_DangXuly(idhoso, NodeId, (int)enumHosoQuytrinhXuly.inttrangthai.DangXuly);
                            break;
                        }
                        else
                        {
                            // neu node chua xu ly thi chuyen xu ly buoc tiep theo 
                            var idNodeNext = nodeNext.intidFrom;

                            // cap nhat tinh trang xu ly xong thi them buoc xu ly
                            var hosoquytrinh = _hsqtRepo.HosoQuytrinhxulys
                                        .Where(p => p.intidhoso == idhoso)
                                        .OrderBy(p => p.intid)
                                        .ToList();

                            int count = 0;

                            while ((idNodeNext > 0) && (count < 5))
                            {   // chi cho phep chay 5 lan 
                                idNodeNext = _AddBuocXuly(idhoso, hosoquytrinh, idNodeNext);
                                count++;
                            }
                        }

                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// kiem tra xem co cho phep user chon buoc xu ly tiep theo khong
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="hosoquytrinh"></param>
        /// <returns></returns>
        private bool _IsChonBuocXuly(int idhoso, List<HosoQuytrinhXuly> hosoquytrinh)
        {
            int idcanbo = _session.GetUserId();
            var hoso = hosoquytrinh.Where(p => p.intidCanbo == idcanbo)
                .Where(p => p.inttrangthai == (int)enumHosoQuytrinhXuly.inttrangthai.DangXuly)
                ;

            return (hoso.Count() > 1) ? true : false;
        }

        #endregion Quytrinh

        #region PhathanhVB

        public PhathanhVBViewModel GetPhathanhVanban(int idhoso, string listfile)
        {
            PhathanhVBViewModel model = new PhathanhVBViewModel();
            model.idhoso = idhoso;

            model.idvanbanden = _hosovanbanRepo.Hosovanbans
                    .Where(p => p.intidhosocongviec == idhoso)
                    .FirstOrDefault().intidvanban;

            model.PhanloaiVanban = _plvanbanRepo.GetActivePhanloaiVanbans
                                       .Where(p => p.intloai == (int)enumPhanloaiVanban.intloai.vanbandi)
                                       .OrderBy(p => p.strtenvanban);

            model.Nguoiky = _canboRepo.GetActiveCanbo
                                .Where(p => p.intkivb == (int)enumcanbo.intkivb.Co)
                                .Select(p => new CanboViewModel
                                {
                                    intid = p.intid,
                                    strhoten = p.strhoten,
                                    strkyhieu = p.strkyhieu
                                })
                                .OrderBy(p => p.strkyhieu)
                                .ThenBy(p => p.strhoten);

            model.strtrichyeu = _hosocongviecRepo.GetHSCVById(idhoso).strtieude;

            model.dteHantraloi = DateTime.Now;

            model.strlistfile = listfile;

            return model;
        }

        public ResultFunction SaveVanbanPhathanh(PhathanhVBViewModel model)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                Vanbandi vb = new Vanbandi();
                int idcanbo = _session.GetUserId();

                vb.intidphanloaivanbandi = model.intidloaivanban;
                vb.strtrichyeu = model.strtrichyeu;
                vb.strnguoisoan = _canboRepo.GetActiveCanboByID(idcanbo).strhoten;
                vb.intidnguoiduyet = model.idnguoiduyet;
                vb.strngayky = DateTime.Now;

                vb.inttrangthai = (int)enumVanbandi.inttrangthai.Chuaduyet;

                // thong tin mac dinh cua van ban
                vb.intidnguoitao = idcanbo; //_session.GetUserId();

                var vbden = _vanbandenRepo.GetVanbandenById(model.idvanbanden);
                var sovb = _sovbRepo.GetActiveSoVanbans
                    .FirstOrDefault(p => p.intid == vbden.intidsovanban).strkyhieu;
                string strtraloivanban = string.Empty;
                if (!string.IsNullOrEmpty(sovb))
                {
                    strtraloivanban = sovb + "-" + vbden.intsoden.ToString() + "-" + vbden.strngayden.Value.Year;
                }

                vb.strtraloivanbanso = strtraloivanban;

                var intidvanban = _vanbandiRepo.Them(vb);

                _CopyFileToPhathanh(intidvanban, model.strlistfile, model.idhoso);

                // tao lien ket van ban den/di 
                // va khong dong ho so xu ly
                if (!string.IsNullOrEmpty(vb.strtraloivanbanso))
                {
                    //string strquytac = AppSettings.TraloiVB;
                    if (model.idvanbanden > 0)
                    {
                        _ruleFileName.LienketVanban((int)enumHoibaovanban.intloai.Vanbandi, model.idvanbanden, intidvanban);

                    }
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

        private void _CopyFileToPhathanh(int idvanban, string strlistfile, int idhoso)
        {
            try
            {
                string[] split = strlistfile.Split(';');
                List<int> listidfile = new List<int>();
                foreach (var p in split)
                {
                    if (!string.IsNullOrEmpty(p))
                    {
                        int idfile = Convert.ToInt32(p);
                        listidfile.Add(idfile);
                    }
                }

                var downloadFiles = _fileHsRepo.AttachHosos
                        .Where(f => f.inttrangthai == (int)enumAttachHoso.inttrangthai.IsActive)
                        .Where(f => f.intidhoso == idhoso)
                        .Where(f => listidfile.Contains(f.intid))
                        .ToList();
                foreach (var f in downloadFiles)
                {
                    FileDownloadResult file = _fileManager.DownloadHoso(f.intid, 1);

                    int intsttfile;
                    var vb = _fileVBRepo.AttachVanbans
                            .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbandi)
                            .Where(p => p.intidvanban == idvanban);
                    intsttfile = (vb.Count() == 0) ? 1 : vb.Count() + 1;

                    string strmota = f.strmota;

                    string fileExt = _fileManager.GetFileExtention(f.strtenfile);

                    //  dinh dang file : idvanban_intsttfile.*
                    string fileName = idvanban.ToString() + "_" + intsttfile.ToString() + "." + fileExt;

                    string strLoaiFile = AppConts.FileCongvanphathanh;

                    string folderSavepath = _fileManager.SetPathUpload(strLoaiFile);

                    string fileSavepath = System.IO.Path.Combine(folderSavepath, fileName);


                    string sourceFile = file.physicalFilePath;

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
                        // To copy a file to another location and  
                        // overwrite the destination file if it already exists. 
                        System.IO.File.Copy(sourceFile, fileSavepath, true);
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
                            filevb.intloai = (int)enumAttachVanban.intloai.Vanbandi;
                            filevb.inttrangthai = (int)enumAttachVanban.inttrangthai.IsActive;
                            filevb.strmota = strmota;
                            filevb.strngaycapnhat = DateTime.Now;
                            filevb.strtenfile = fileName;
                            int intid = _fileVBRepo.Them(filevb);
                            _logger.Info("Chuyển phát hành file : " + strmota + " vào văn bản đi : " + idvanban);

                            _fileHsRepo.CapnhatPhathanh(f.intid);

                        }

                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);

                    }

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        #endregion PhathanhVB


        #endregion XulyHosocongviec

        #region PhanQuytrinh
        /// <summary>
        /// load du lieu form phan quy trinh xu ly
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        public PhanQuytrinhViewModel GetFormPhanQuytrinh(int idvanban)
        {
            PhanQuytrinhViewModel hoso = new PhanQuytrinhViewModel();

            var hsvb = _hosovanbanRepo.Hosovanbans
                                .Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden)
                                .FirstOrDefault(p => p.intidvanban == idvanban);

            // idhosocongviec = 0:  van ban den chua phan xu ly
            // idhosocongviec !=0: van ban den da phan xu ly, load du lieu do vao form

            int? idhosocongviec = (hsvb != null) ? hsvb.intidhosocongviec : 0;

            //DateTime ngayhientai = DateTime.Now;
            if ((idhosocongviec == null) || (idhosocongviec == 0))
            {   // ho so moi
                Vanbanden vanban = _vanbandenRepo.Vanbandens.FirstOrDefault(p => p.intid == idvanban);

                hoso.HosocongviecModel = new Hosocongviec();
                hoso.HosocongviecModel.intid = 0;
                hoso.HosocongviecModel.strngaymohoso = DateTime.Now;
                hoso.HosocongviecModel.strtieude = vanban.strtrichyeu;
                hoso.HosocongviecModel.intlinhvuc = 0;
                hoso.intidlanhdaogiaoviec = vanban.intidnguoiduyet;
                hoso.intidxulychinh = 0;
                hoso.stridlanhdaophutrach = string.Empty;

                int intthoihanxuly = _configRepo.GetConfigToInt(ThamsoHethong.ThoihanXLVB);
                hoso.HosocongviecModel.strthoihanxuly = DateServices.AddThoihanxuly(DateTime.Now, intthoihanxuly);

                hoso.intidloaiquytrinh = 0;
                hoso.intidquytrinh = 0;
            }
            else
            {   // ho so da co
                Hosocongviec hscv = _hosocongviecRepo.Hosocongviecs.FirstOrDefault(p => p.intid == idhosocongviec);
                hoso.HosocongviecModel = hscv;

                var quytrinh = _hsqtRepo.HosoQuytrinhxulys
                    .FirstOrDefault(p => p.intidhoso == idhosocongviec);
                if (quytrinh != null)
                {
                    hoso.intidquytrinh = quytrinh.intidquytrinh;
                    hoso.intidloaiquytrinh = _quytrinhRepo.AllQuytrinhs
                        .FirstOrDefault(p => p.intid == quytrinh.intidquytrinh)
                        .intidloai;
                }
                else
                {
                    hoso.intidloaiquytrinh = 0;
                    hoso.intidquytrinh = 0;
                }
            }

            hoso.intidvanban = idvanban;

            //hoso.VanbandenModel = vanban;

            hoso.LinhvucModel = _linhvucRepo.GetActiveLinhvucs
                            .Where(p => p.inttrangthai == (int)enumLinhvuc.inttrangthai.IsActive)
                            .OrderBy(p => p.strtenlinhvuc);

            //=================================================
            // quy trinh
            // ================================================
            hoso.Loaiquytrinh = _loaiquytrinhRepo.PhanloaiQuytrinhs
                .Select(p => new QLVB.DTO.Quytrinh.EditLoaiQuytrinhViewModel
                {
                    intid = p.intid,
                    strtenloaiquytrinh = p.strtenloai
                });

            return hoso;
        }

        /// <summary>
        /// lay ten cac quy trinh thuoc loai idloaiquytrinh
        /// </summary>
        /// <param name="idloaiquytrinh"></param>
        /// <returns></returns>
        public IEnumerable<QLVB.DTO.Quytrinh.EditQuytrinhViewModel> GetQuytrinh(int idloaiquytrinh)
        {
            var quytrinh = _quytrinhRepo.ActiveQuytrinhs
                .Where(p => p.intidloai == idloaiquytrinh)
                .Select(p => new QLVB.DTO.Quytrinh.EditQuytrinhViewModel
                {
                    intid = p.intid,
                    strtenquytrinh = p.strten,
                    dteThoigianApdung = p.strNgayApdung
                })
                .ToList();
            List<EditQuytrinhViewModel> listRemove = new List<EditQuytrinhViewModel>();

            foreach (var p in quytrinh)
            {
                DateTime? dteNgay = _GetNgayApdungQuytrinh((DateTime)p.dteThoigianApdung, p.intid);
                if (dteNgay == null)
                {   // khong tim thay ngay phien ban
                    // do ngay ap dung > ngay hien tai
                    // thi go bo quy trinh nay ra khoi danh sach
                    listRemove.Add(p);
                }
                string strngay = DateServices.FormatDateVN(dteNgay);
                if (!string.IsNullOrEmpty(strngay))
                {
                    p.strtenquytrinh = p.strtenquytrinh + " (" + strngay + ")";
                }
            }

            foreach (var p in listRemove)
            {
                try
                {
                    quytrinh.Remove(p);
                }
                catch { }
            }

            return quytrinh;
        }

        /// <summary>
        /// phan quy trinh xu ly cho van ban
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultFunction SavePhanQuytrinh(int idvanban, PhanQuytrinhViewModel model)
        {
            //====================================================================
            //  duyet van ban dang phan xl
            //  cap nhat ten nguoi xu ly chinh(strnoinhan) cua van ban
            //  insert vao table hosocongviec
            //  insert vao table hosovanban
            //  ====================================
            //  phan xu ly theo quy trinh
            //  insert vao table doituongxuly: lanh dao  giao viec, lanh dao phu trach , xu ly chinh
            //  ====================================
            //
            //  copy file dinh kem cua van ban vao folder hoso
            //  nhan tin SMS (neu co)
            //=====================================================================

            ResultFunction kq = new ResultFunction();
            kq.id = (int)ResultViewModels.Error;

            int idhosocongviec = model.HosocongviecModel.intid;
            Hosocongviec hscv = model.HosocongviecModel;

            //=======================================================
            // cap nhat du lieu
            //=======================================================

            //  duyet van ban dang phan xl
            _vanbandenRepo.Duyet(idvanban, (int)enumVanbanden.inttrangthai.Daduyet);

            // THEM MOI HO SO CONG VIEC
            if (idhosocongviec == 0)
            {
                // KIEM TRA PHAN 2 LAN 1 VAN BAN
                if (_role.CheckPhanHosocongviec(idvanban, (int)enumHosovanban.intloai.Vanbanden))
                {
                    try
                    {
                        int songayxuly = _quytrinhRepo.ActiveQuytrinhs
                            .FirstOrDefault(p => p.intid == model.intidquytrinh).intSoNgay;
                        //  insert vao table hosocongviec
                        //hscv.strthoihanxuly = DateServices.AddThoihanxuly(DateTime.Now, songayxuly);
                        DateTime dtNgaybd = (DateTime)model.HosocongviecModel.strngaymohoso;
                        hscv.strthoihanxuly = DateServices.AddThoihanxuly(dtNgaybd, songayxuly);
                        // trong table hosocongviec: de intloai la vbden_quytrinh 
                        hscv.intloai = (int)enumHosocongviec.intloai.Vanbanden_Quytrinh;
                        idhosocongviec = _hosocongviecRepo.Them(hscv);
                        //  insert vao table hosovanban
                        // van de inloai la vbden de co the nhin thay o menu/vanbanden
                        _ThemHosovanban(idhosocongviec, idvanban, (int)enumHosovanban.intloai.Vanbanden);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message);
                    }
                }   // end tao ho so van ban                
            }
            //=================================================
            // them quy trinh vao ho so da co
            _AddHosoQuytrinhXuly(idhosocongviec, model.intidquytrinh);

            var hosoquytrinh = _hsqtRepo.HosoQuytrinhxulys
                .Where(p => p.intidhoso == idhosocongviec).ToList();

            AddDoituongxuly_Quytrinh(idhosocongviec, hosoquytrinh);

            // chua lam
            //  copy file dinh kem cua van ban vao folder hoso
            //  nhan tin SMS (neu co)

            kq.id = (int)ResultViewModels.Success;

            return kq;
        }

        /// <summary>
        /// lay toan bo quy trinh xu ly cong viec 
        /// HIEN KHONG SU DUNG DO UPDATE QUYTRINH VERSION
        /// </summary>
        /// <param name="idquytrinh"></param>
        /// <returns></returns>
        private QuytrinhXulyViewModels GetQuytrinhXuly(int idquytrinh)
        {
            QuytrinhXulyViewModels model = new QuytrinhXulyViewModels();
            var quytrinh = _quytrinhRepo.AllQuytrinhs.FirstOrDefault(p => p.intid == idquytrinh);
            if (quytrinh != null)
            {
                model.idquytrinh = idquytrinh;
                model.strtenquytrinh = quytrinh.strten;
                model.intTongSongayxuly = quytrinh.intSoNgay;
                try
                {
                    var congviecs = _qtNodeRepo.QuytrinhNodes.Where(p => p.intidquytrinh == idquytrinh)
                    .GroupJoin(
                        _qtXulyRepo.QuytrinhXulys,
                        cv => cv.intid,
                        xl => xl.intidNode,
                        (cv, xl) => new { congviec = cv, xuly = xl.FirstOrDefault() }
                    )
                    .Select(p => new CongviecXulyViewModel
                    {
                        idcongviec = p.congviec.intid,
                        strtencongviec = p.congviec.strten,
                        nodeid = p.congviec.NodeId,
                        intLeft = p.congviec.intleft,
                        intTop = p.congviec.inttop,
                        intidCanbo = p.xuly.intidCanbo,
                        intVaitro = p.xuly.intVaitro,
                        intidDonvi = p.xuly.intidDonvi,
                        intNext = p.xuly.intNext,
                        intSoNgay = p.xuly.intSoNgay,
                        Hoanthanh = p.xuly.intHoanthanh,
                        intXulyDongthoi = p.xuly.intXulyDongthoi
                    });

                    model.congviecs = congviecs;

                    var xulys = _qtConnectionRepo.QuytrinhConnections
                    .Join(
                        _qtNodeRepo.QuytrinhNodes.Where(p => p.intidquytrinh == idquytrinh),
                        con => con.intidFrom,
                        node => node.intid,
                        (con, node) => new { con, node }
                    )
                    .OrderBy(p => p.node.NodeId)
                    .Select(p => new ThutuXulyViewModel
                    {
                        idFrom = p.con.intidFrom,
                        idTo = p.con.intidTo,
                        label = p.con.strlabel
                    })
                    .ToList();

                    // lay danh sach theo thu tu cac buoc xu ly tu begin --- node ---- end
                    List<ThutuXulyViewModel> Buocxuly = new List<ThutuXulyViewModel>();
                    var node_begin = xulys.Last();
                    Buocxuly.Add(node_begin);
                    foreach (var p in xulys)
                    {
                        if (p != node_begin)
                        {
                            Buocxuly.Add(p);
                        }
                    }

                    model.BuocXuly = Buocxuly;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }



            }

            return model;
        }

        /// <summary>        
        /// them vao table doituongxuly: cán bộ xử lý tại bước này
        /// nếu tại bước này, có tự hoàn thành thì thêm cán bộ xử lý bước tiếp vào
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="quytrinh"></param>
        private void AddDoituongxuly_Quytrinh(int idhosocongviec, List<HosoQuytrinhXuly> hosoquytrinh)
        {
            //=============================================
            // kiem tra tuy chon 
            // Tự động chuyển xử lý  : hết thời gian cho phép tại bước này thi tự động chuyển sang bước tiếp
            // Mặc định hoàn thành : tự động hoàn thành bước này
            //==============================================
            try
            {
                //bool flag = false;
                // bool findXulychinh = false;
                Dictionary<int, int> dicidcanbo = new Dictionary<int, int>();
                // list cac node da duyet
                Dictionary<int, string> listNode = new Dictionary<int, string>();
                // duyet hosoquytrinh tu node begin
                var NodeBegin = hosoquytrinh.FirstOrDefault(p => p.nodeidFrom == "node_begin");
                listNode.Add(NodeBegin.intidFrom, NodeBegin.nodeidFrom);

                var xulys = hosoquytrinh.Where(p => p.intidFrom == NodeBegin.intidTo).ToList();

                foreach (var xl in xulys)
                {
                    var xuly = xl;
                    int count = 0;
                    bool isStop = false;
                    //while ((xuly.intidTo != null) && (count < 100))
                    while ((!isStop) && (count < 100))
                    {   // xuly != node_end
                        // can than lap vo han???. Toi da lap 100 buoc
                        count++;
                        if ((xuly.intidCanbo > 0)
                            && (xuly.intVaitro > (int)enumQuytrinhXuly.intVaitro.Khongthamgia)
                            && (xuly.inttrangthai != (int)enumHosoQuytrinhXuly.inttrangthai.DaXuly))
                        {
                            // them vao table doituongxuly: cán bộ xử lý tại bước này
                            // nếu tại bước này, có tự hoàn thành thì thêm cán bộ xử lý bước tiếp vào

                            //  insert vao table doituongxuly: lanh dao  giao viec, lanh dao phu trach , xu ly chinh

                            // kiem tra ds doi tuong xu ly da co ten can bo nay chua de tranh truong hop them nhieu idcanbo  
                            // vao table doituongxuly
                            if (!dicidcanbo.ContainsKey((int)xuly.intidCanbo))
                            {   // chua co trong doituongxuly(doituongxuly), them moi
                                _ThemDoituongxuly(idhosocongviec, (int)xuly.intidCanbo, (int)xuly.intVaitro);
                                dicidcanbo.Add((int)xuly.intidCanbo, (int)xuly.intVaitro);
                                //  cap nhat ten nguoi xu ly chinh(strnoinhan) cua van ban
                                int idvanban = _hosovanbanRepo.Hosovanbans.FirstOrDefault(x => x.intidhosocongviec == idhosocongviec).intidvanban;
                                _CapnhatXulychinhVanbanden(idvanban, (int)xuly.intidCanbo, true);
                            }
                            else
                            {   // da co trong doituongxuly (listidcanbo)
                                int vaitro = dicidcanbo[(int)xuly.intidCanbo];
                                if (vaitro != xuly.intVaitro)
                                {   // so sanh vai tro xu ly
                                    bool dtxl = _doituongRepo.KiemtraVaitroxuly(idhosocongviec, (int)xuly.intidCanbo, (int)xuly.intVaitro);
                                    if (!dtxl)
                                    {
                                        _ThemDoituongxuly(idhosocongviec, (int)xuly.intidCanbo, (int)xuly.intVaitro);
                                    }
                                }
                            }

                            if (xuly.intHoanthanh == (int)enumHosoQuytrinhXuly.intHoanthanh.Khong)
                            {
                                //_hsqtRepo.CapnhatTrangthai(xuly.intid, (int)enumHosoQuytrinhXuly.inttrangthai.DangXuly);                                
                                _hsqtRepo.CapnhatTrangthai_DangXuly(idhosocongviec, xuly.nodeidFrom, (int)enumHosoQuytrinhXuly.inttrangthai.DangXuly);

                                // khong tu dong hoan thanh thi dung tai buoc nay
                                //xuly.intidTo = null;
                                isStop = true;
                            }
                            else
                            {
                                //_hsqtRepo.CapnhatTrangthai(xuly.intid, (int)enumHosoQuytrinhXuly.inttrangthai.DaXuly);
                                _hsqtRepo.CapnhatTrangthai_DaXuly(idhosocongviec, xuly.nodeidFrom, (int)enumHosoQuytrinhXuly.inttrangthai.DaXuly);
                                // tu dong hoan thanh thi them buoc ke tiep
                                int? idNodeTo = xuly.intidTo;
                                xuly = hosoquytrinh.FirstOrDefault(p => p.intidFrom == idNodeTo);
                            }
                        }

                        if (xuly.intVaitro == (int)enumQuytrinhXuly.intVaitro.Khongthamgia)
                        {   // khong thoa dieu kien
                            // intvaitro =0 (khong tham gia xu ly)
                            // van xet inttrangthai de cap nhat

                            if (xuly.intHoanthanh == (int)enumHosoQuytrinhXuly.intHoanthanh.Khong)
                            {
                                //_hsqtRepo.CapnhatTrangthai(xuly.intid, (int)enumHosoQuytrinhXuly.inttrangthai.DangXuly);
                                _hsqtRepo.CapnhatTrangthai_DangXuly(idhosocongviec, xuly.nodeidFrom, (int)enumHosoQuytrinhXuly.inttrangthai.DangXuly);
                                // khong tu dong hoan thanh thi dung tai buoc nay
                                // xuly.intidTo = null;
                                isStop = true;
                            }
                            else
                            {
                                //_hsqtRepo.CapnhatTrangthai(xuly.intid, (int)enumHosoQuytrinhXuly.inttrangthai.DaXuly);
                                _hsqtRepo.CapnhatTrangthai_DaXuly(idhosocongviec, xuly.nodeidFrom, (int)enumHosoQuytrinhXuly.inttrangthai.DaXuly);
                                // tu dong hoan thanh thi them buoc ke tiep
                                int? idNodeTo = xuly.intidTo;
                                xuly = hosoquytrinh.FirstOrDefault(p => p.intidFrom == idNodeTo);
                            }
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
        /// luu toan bo cac buoc xu ly cong viec theo quy trinh , ap dung vao hosocongviec
        /// HIEN KHONG SU DUNG
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="model"></param>
        private void _AddHosoQuytrinhXuly__(int idhosocongviec, QuytrinhXulyViewModels quytrinh)
        {
            try
            {
                int idnode_end = 0;
                foreach (var p in quytrinh.BuocXuly)
                {
                    // bat dau voi node_begin va ket thuc voi node_end
                    HosoQuytrinhXuly _hosoquytrinh = new HosoQuytrinhXuly();
                    _hosoquytrinh.intidquytrinh = quytrinh.idquytrinh;
                    _hosoquytrinh.intidhoso = idhosocongviec;
                    _hosoquytrinh.intidFrom = p.idFrom;
                    _hosoquytrinh.intidTo = p.idTo;
                    _hosoquytrinh.strlabel = p.label;

                    var congviec = quytrinh.congviecs
                        .FirstOrDefault(x => x.idcongviec == p.idFrom);
                    if (congviec != null)
                    {
                        _hosoquytrinh.intidCanbo = congviec.intidCanbo;
                        _hosoquytrinh.intVaitro = congviec.intVaitro;
                        _hosoquytrinh.intSongay = congviec.intSoNgay;
                        _hosoquytrinh.intNext = congviec.intNext;
                        _hosoquytrinh.intHoanthanh = congviec.Hoanthanh;

                        _hosoquytrinh.nodeidFrom = congviec.nodeid;
                        _hosoquytrinh.strTenNodeFrom = congviec.strtencongviec;
                        _hosoquytrinh.intLeft = congviec.intLeft;
                        _hosoquytrinh.intTop = congviec.intTop;

                        if (congviec.Hoanthanh == (int)enumHosoQuytrinhXuly.intHoanthanh.Khong)
                        {
                            _hosoquytrinh.inttrangthai = (int)enumHosoQuytrinhXuly.inttrangthai.ChuaXuly;
                        }
                        else
                        {
                            _hosoquytrinh.inttrangthai = (int)enumHosoQuytrinhXuly.inttrangthai.DaXuly;
                        }

                    }

                    var id = _hsqtRepo.Them(_hosoquytrinh);
                    // node_end cuoi cung cua buoc xu ly
                    idnode_end = p.idTo;
                }

                var node_end = quytrinh.congviecs.FirstOrDefault(p => p.idcongviec == idnode_end);
                HosoQuytrinhXuly _hoso = new HosoQuytrinhXuly();
                _hoso.intidquytrinh = quytrinh.idquytrinh;
                _hoso.intidhoso = idhosocongviec;
                _hoso.intidFrom = node_end.idcongviec;
                _hoso.intidTo = null;
                _hoso.strlabel = null;


                _hoso.intidCanbo = node_end.intidCanbo;
                _hoso.intVaitro = node_end.intVaitro;
                _hoso.intSongay = node_end.intSoNgay;
                _hoso.intNext = node_end.intNext;
                _hoso.intHoanthanh = node_end.Hoanthanh;

                _hoso.nodeidFrom = node_end.nodeid;
                _hoso.strTenNodeFrom = node_end.strtencongviec;
                _hoso.intLeft = node_end.intLeft;
                _hoso.intTop = node_end.intTop;

                _hoso.inttrangthai = null;

                _hsqtRepo.Them(_hoso);

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// luu toan bo cac buoc xu ly cong viec theo quy trinh Version , ap dung vao hosocongviec
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="idquytrinh"></param>
        /// <returns></returns>
        private bool _AddHosoQuytrinhXuly(int idhoso, int idquytrinh)
        {
            _DeleteHosoQuytrinhXuly(idhoso);
            _DeleteHosoDoituongxuly(idhoso);
            //===============================================
            var _qt = _quytrinhRepo.ActiveQuytrinhs.FirstOrDefault(p => p.intid == idquytrinh);
            DateTime? dteNgayVersion;
            if (_qt != null)
            {
                //dteNgayVersion = (DateTime)_qt.strNgayApdung;
                dteNgayVersion = _GetNgayApdungQuytrinh((DateTime)_qt.strNgayApdung, idquytrinh);
                if (dteNgayVersion == null)
                {
                    return false;
                }

                var quytrinh = _quytrinhVersionRepo.QuytrinhVersions
                .Where(p => p.intidquytrinh == idquytrinh)
                .Where(p => p.strNgayApdung == dteNgayVersion)
                .ToList();

                foreach (var p in quytrinh)
                {
                    HosoQuytrinhXuly _hosoquytrinh = new HosoQuytrinhXuly();
                    _hosoquytrinh.intidquytrinh = idquytrinh;
                    _hosoquytrinh.strNgayapdung = dteNgayVersion;
                    _hosoquytrinh.intidhoso = idhoso;
                    _hosoquytrinh.intidFrom = p.intidFrom;
                    _hosoquytrinh.intidTo = p.intidTo;
                    _hosoquytrinh.strlabel = p.strlabel;

                    _hosoquytrinh.intidCanbo = p.intidCanbo;
                    _hosoquytrinh.intVaitro = p.intVaitro;
                    _hosoquytrinh.intSongay = p.intSongay;
                    _hosoquytrinh.intNext = p.intNext;
                    _hosoquytrinh.intHoanthanh = p.intHoanthanh;
                    _hosoquytrinh.intXulyDongthoi = p.intXulyDongthoi;

                    _hosoquytrinh.nodeidFrom = p.nodeidFrom;
                    _hosoquytrinh.strTenNodeFrom = p.strTenNodeFrom;
                    _hosoquytrinh.intLeft = p.intLeft;
                    _hosoquytrinh.intTop = p.intTop;

                    //_hosoquytrinh.inttrangthai = (p.intHoanthanh == (int)enumHosoQuytrinhXuly.intHoanthanh.Khong) ?
                    //                            (int)enumHosoQuytrinhXuly.inttrangthai.ChuaXuly :
                    //                            (int)enumHosoQuytrinhXuly.inttrangthai.DaXuly;
                    _hosoquytrinh.inttrangthai = (int)enumHosoQuytrinhXuly.inttrangthai.ChuaXuly;

                    var id = _hsqtRepo.Them(_hosoquytrinh);
                }
                return true;
            }
            else
            {
                _logger.Error("Không tìm thấy quy trình id: " + idquytrinh);
                return false;
            }
        }

        /// <summary>
        /// so sanh cac ngay ap dung phien ban quy trinh de lay ngay moi nhat
        /// </summary>
        /// <param name="dteNgayVersion"></param>
        /// <returns></returns>
        private DateTime? _GetNgayApdungQuytrinh(DateTime dteNgayVersion, int idquytrinh)
        {
            //var _qt = _quytrinhRepo.ActiveQuytrinhs.FirstOrDefault(p => p.intid == idquytrinh);
            //DateTime dteNgayVersion = (DateTime)_qt.strNgayApdung;
            DateTime dtenow = DateTime.Now;
            if (dteNgayVersion < dtenow)
            {   // ngay phien ban < ngay hien tai thi lay theo ngay phien ban
                return dteNgayVersion;
            }
            else
            {
                // ngay phien ban >= ngay hien tai (ngay phien ban chua duoc ap dung)
                // lay theo phien ban cu hon

                // sap ngay theo thu tu giam dan
                var ngay = _quytrinhVersionRepo.QuytrinhVersions
                           .Where(p => p.intidquytrinh == idquytrinh)
                           .Select(p => p.strNgayApdung)
                           .Distinct()
                           .OrderByDescending(p => p.Year)
                           .ThenByDescending(p => p.Month)
                           .ThenByDescending(p => p.Day)
                           .ToList();

                bool isfound = false;
                foreach (var n in ngay)
                {
                    if (n < dtenow)
                    {   // ngay phien ban < ngay hien tai 
                        // 20/4 ; 10/4 < now = 15/4 ==> kq: 10/4
                        dteNgayVersion = n;
                        isfound = true;
                        break;
                    }
                }
                if (isfound)
                {
                    return dteNgayVersion;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// xoa hosoquytrinhxuly neu (neu co) de thay doi theo quy trinh moi
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        private bool _DeleteHosoQuytrinhXuly(int idhoso)
        {
            try
            {
                bool isfound = IsHosoQuytrinhxuly(idhoso);
                bool isDelete = true;
                if (isfound)
                {
                    isDelete = _hsqtRepo.Xoa(idhoso);
                }
                return isDelete;
            }
            catch (Exception ex)
            {
                _logger.Error("Lỗi! Không xóa hồ sơ quy trình đã có được. Idhoso:" + idhoso.ToString() + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// xoa toan bo doi tuong xu ly trong hoso
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        private bool _DeleteHosoDoituongxuly(int idhoso)
        {
            try
            {
                _doituongRepo.XoaCacCanbo(idhoso);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// tao cac truong can thiet cho hoso congviec
        /// </summary>
        /// <param name="hoso"></param>
        /// <returns></returns>
        private Hosocongviec SaveHosocongviec(Hosocongviec hoso)
        {
            Hosocongviec hs = hoso;
            hs.intloai = (int)enumHosocongviec.intloai.Vanbanden;
            //hs.intsotudong = 0;
            //hs.strsohieuht = 
            //hs.strthoihanxuly
            return hs;
        }

        /// <summary>
        /// kiem tra xem hoso nay da duoc phan quy trinh xu ly khong? true: co, false:khong
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <returns>
        /// true: co
        /// false:khong
        /// </returns>
        private bool IsHosoQuytrinhxuly(int idhosocongviec)
        {
            var quytrinh = _hsqtRepo.HosoQuytrinhxulys
                .FirstOrDefault(p => p.intidhoso == idhosocongviec);
            if (quytrinh == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// kiem tra xem user con trong buoc xu ly quy trinh khong? true: co, false:khong
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <returns></returns>
        private bool IsXulyQuytrinh(int idhosocongviec, List<HosoQuytrinhXuly> quytrinh)
        {
            int idcanbo = _session.GetUserId();
            var _quytrinh = quytrinh
                .Where(p => p.intidCanbo == idcanbo)
                .Where(p => p.inttrangthai == (int)enumHosoQuytrinhXuly.inttrangthai.DangXuly)
                .Count();
            if (_quytrinh > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        #endregion PhanQuytrinh

    }
}