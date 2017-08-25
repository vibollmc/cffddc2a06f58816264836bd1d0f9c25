using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Core.Contract;
using QLVB.Common.Logging;
using QLVB.Common.Sessions;
using QLVB.Core.Contract;
using QLVB.DTO.Hoso;
using QLVB.Common.Utilities;
using QLVB.Domain.Entities;
using Store.DAL.Abstract;
using QLVB.Domain.Abstract;
using QLVB.Common.Date;
using QLVB.DTO.File;

namespace Store.Core.Implementation
{
    public class TracuuHosoManager : ITracuuHosoManager
    {
        #region Constructor

        private ILogger _logger;
        private ISessionServices _session;


        private IStoreHosocongviecRepository _hosocongviecRepo;
        private IStoreHosovanbanRepository _hosovanbanRepo;
        private IStoreHosoykienxulyRepository _hosoykienRepo;
        private IStoreVanbandenRepository _vanbandenRepo;
        private IStoreDoituongxulyRepository _doituongRepo;
        private IStoreChitietHosoRepository _chitietHosoRepo;
        private IStorePhieutrinhRepository _phieutrinhRepo;
        private IStoreHosovanbanlienquanRepository _hsvblqRepo;
        private IStoreAttachHosoRepository _fileHsRepo;
        private IStoreAttachVanbanRepository _fileVBRepo;
        private IStoreFileManager _fileManager;
        private IStoreVanbandiRepository _vanbandiRepo;

        //======QLVB.Domain==========
        private ILinhvucRepository _linhvucRepo;
        private ICanboRepository _canboRepo;
        private IChucdanhRepository _chucdanhRepo;
        private IDonvitructhuocRepository _donviRepo;
        private IConfigRepository _configRepo;

        private IRoleManager _role;

        private IPhanloaiVanbanRepository _plvanbanRepo;

        private IRuleFileNameManager _ruleFileName;
        private ISoVanbanRepository _sovbRepo;




        public TracuuHosoManager(ILogger logger, ISessionServices session,

            IStoreHosocongviecRepository hosocvRepo,
            IStoreHosovanbanRepository hosovbRepo, IStoreHosoykienxulyRepository hosoykienRepo,
            IStoreVanbandenRepository vanbandenRepo, IStoreDoituongxulyRepository doituongRepo,
            IStorePhieutrinhRepository phieutrinhRepo, IStoreAttachHosoRepository fileHsRepo,
            IStoreHosovanbanlienquanRepository hsvblqRepo, IStoreFileManager fileManager,
            IStoreVanbandiRepository vbdiRepo, IStoreAttachVanbanRepository fileVBRepo,
            IStoreChitietHosoRepository chitietHosoRepo,

            ILinhvucRepository linhvucRepo, ICanboRepository canboRepo, IChucdanhRepository chucdanhRepo,
            IDonvitructhuocRepository donviRepo, IConfigRepository configRepo,
            IRoleManager role, IPhanloaiVanbanRepository plvanbanRepo, IRuleFileNameManager ruleFileName,
            ISoVanbanRepository sovbRepo
            )
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
            _hsvblqRepo = hsvblqRepo;
            _fileManager = fileManager;
            _plvanbanRepo = plvanbanRepo;
            _vanbandiRepo = vbdiRepo;
            _ruleFileName = ruleFileName;
            _sovbRepo = sovbRepo;
            _fileVBRepo = fileVBRepo;
        }

        #endregion Constructor


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
            var allCanbo = _canboRepo.GetAllCanbo.ToList().AsEnumerable();

            var lanhdaophutrach = allCanbo
                            .Join(
                                _doituongRepo.Doituongxulys
                                                .Where(p => p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaophutrach)
                                                .Where(p => p.intidhosocongviec == idhosocongviec),
                                c => c.intid,
                                d => d.intidcanbo,
                                (c, d) => c
                            );
            if (lanhdaophutrach.Count() > 0)
            {
                foreach (var ld in lanhdaophutrach)
                {
                    strldphutrach += ld.strhoten + ", ";
                }
                int len = strldphutrach.Length - 2;
                strldphutrach = strldphutrach.Substring(0, len);
            }
            //=========phoi hop xu ly ===================================
            var phoihopxuly = allCanbo
                    .Join(
                        _doituongRepo.Doituongxulys
                                        .Where(p => p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Phoihopxuly)
                                        .Where(p => p.intidhosocongviec == idhosocongviec),
                        c => c.intid,
                        d => d.intidcanbo,
                        (c, d) => c
                    );
            if (phoihopxuly.Count() > 0)
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
                var lanhdaogiaoviec = allCanbo
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

                var xulychinh = allCanbo
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
            var allCanbo = _canboRepo.GetAllCanbo.ToList().AsEnumerable();
            var donvi = _donviRepo.Donvitructhuocs.ToList().AsEnumerable();
            //=====================================================
            // doi tuong xu ly ho so
            //=====================================================
            var doituongxuly = _doituongRepo.Doituongxulys
                        .Where(p => p.intidhosocongviec == idhosocongviec)
                        .ToList().AsEnumerable();

            var dtxl = doituongxuly
                        .Join(
                            allCanbo, //_canboRepo.GetAllCanbo,
                            d => d.intidcanbo,
                            c => c.intid,
                            (d, c) => new { d, c }
                        )
                        .Join(
                            donvi, //_donviRepo.Donvitructhuocs,
                            xl => xl.c.intdonvi,
                            pb => pb.Id,
                            (xl, pb) => new { xl, pb }
                        )
                        .Join(
                            allCanbo, // _canboRepo.GetAllCanbo,
                            nguoixuly => nguoixuly.xl.d.intnguoitao,
                            nguoitao => nguoitao.intid,
                            (nguoixuly, nguoitao) => new { nguoixuly, nguoitao }
                        )
                        .Select(p => new LuanchuyenvanbanViewModel
                        {
                            strtendonvi = p.nguoixuly.pb.strtendonvi,
                            strtencanbo = p.nguoixuly.xl.c.strhoten,
                            intvaitro = p.nguoixuly.xl.d.intvaitro,
                            strtennguoitao = p.nguoitao.strhoten,
                            dtengaytao = p.nguoixuly.xl.d.strngaytao,
                            intvaitrocu = p.nguoixuly.xl.d.intvaitrocu,
                            strthaotac = p.nguoixuly.xl.d.strthaotac,
                            dtengaychuyen = p.nguoixuly.xl.d.strngaychuyen,
                            //strtennguoichuyen = p.nguoichuyen.strhoten,
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
            var hoso = _chitietHosoRepo.ChitietHosos
                        .Where(p => p.intidhosocongviec == idhosocongviec)
                        .ToList().AsEnumerable();

            var chitietHS = hoso
                        .Join(
                            allCanbo,   //_canboRepo.GetAllCanbo,
                            d => d.intidcanbo,
                            c => c.intid,
                            (d, c) => new { d, c }
                        )
                        .Join(
                           donvi,//_donviRepo.Donvitructhuocs,
                            xl => xl.c.intdonvi,
                            pb => pb.Id,
                            (xl, pb) => new { xl, pb }
                        )
                        .Join(
                            allCanbo, // _canboRepo.GetAllCanbo,
                            nguoixuly => nguoixuly.xl.d.intnguoitao,
                            nguoitao => nguoitao.intid,
                            (nguoixuly, nguoitao) => new { nguoixuly, nguoitao }
                        )
                        .Select(p => new LuanchuyenvanbanViewModel
                        {
                            strtendonvi = p.nguoixuly.pb.strtendonvi,
                            strtencanbo = p.nguoixuly.xl.c.strhoten,
                            intvaitro = p.nguoixuly.xl.d.intvaitro,
                            strtennguoitao = p.nguoitao.strhoten,
                            dtengaytao = p.nguoixuly.xl.d.strngaytao,
                            //intvaitrocu = p.nguoixuly2.nguoixuly.xl.d.intvaitrocu,
                            strthaotac = p.nguoixuly.xl.d.strthaotac,
                            //dtengaychuyen = p.nguoixuly2.nguoixuly.xl.d.strngaychuyen,
                            //strtennguoichuyen = p.nguoichuyen.strhoten,
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
            var allCanbo = _canboRepo.GetAllCanbo.ToList().AsEnumerable();
            var donvi = _donviRepo.Donvitructhuocs.ToList().AsEnumerable();

            var ykien = _hosoykienRepo.Hosoykienxulys
                        .Join(
                            _doituongRepo.Doituongxulys.Where(p => p.intidhosocongviec == idhosocongviec),
                            yk => yk.intiddoituongxuly,
                            dt => dt.intid,
                            (yk, dt) => new { yk, dt }
                        ).ToList().AsEnumerable();

            //var hosoykien = _hosoykienRepo.Hosoykienxulys
            //            .Join(
            //                _doituongRepo.Doituongxulys.Where(p => p.intidhosocongviec == idhosocongviec),
            //                yk => yk.intiddoituongxuly,
            //                dt => dt.intid,
            //                (yk, dt) => new { yk, dt }
            //            )
            var hosoykien = ykien
                        .Join(
                            allCanbo, //_canboRepo.GetAllCanbo,
                            yk2 => yk2.dt.intidcanbo,
                            cb => cb.intid,
                            (yk2, cb) => new { yk2, cb }
                        )
                        .Join(
                            donvi, //_donviRepo.Donvitructhuocs,
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
                var allCanbo = _canboRepo.GetAllCanbo.ToList().AsEnumerable();

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

                var doituongxuly = _doituongRepo.GetAllCanboxulys
                    .Where(p => p.intidhosocongviec == idhoso)
                    .ToList();

                var dtxl = doituongxuly
                    .Join(
                        allCanbo, //_canboRepo.GetAllCanbo,
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

        /// <summary>
        ///  tra ve intiddoituongxuly cua idcanbo dang xem/xu ly ho so
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="idcanbo"></param>
        /// <returns>0:khong phai user dang trong luong xu ly</returns>
        private int GetIdDoituongxuly(int idhoso, int idcanbo)
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

        #endregion ViewDetailHosocongviec
    }
}
