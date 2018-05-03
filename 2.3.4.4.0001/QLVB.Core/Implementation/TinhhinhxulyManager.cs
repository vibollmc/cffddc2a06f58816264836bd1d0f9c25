using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Core.Contract;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Tinhhinhxuly;
using QLVB.Common.Logging;
using QLVB.Common.Utilities;
using QLVB.Common.Sessions;
using QLVB.Common.Date;
using System.Data.Entity;

namespace QLVB.Core.Implementation
{
    public class TinhhinhxulyManager : ITinhhinhxulyManager
    {
        #region Constructor

        private ILogger _logger;
        private ISessionServices _session;
        private IVanbandenRepository _vanbandenRepo;
        private IPhanloaiVanbanRepository _plvanbanRepo;
        private ISoVanbanRepository _sovbRepo;
        private IKhoiphathanhRepository _khoiphRepo;
        private ILinhvucRepository _linhvucRepo;
        private ICanboRepository _canboRepo;
        private IConfigRepository _configRepo;
        private IHosocongviecRepository _hosocongviecRepo;
        private IHosovanbanRepository _hosovanbanRepo;
        private IDoituongxulyRepository _doituongRepo;
        private IHoibaovanbanRepository _hoibaovanbanRepo;
        private IVanbandiRepository _vanbandiRepo;
        private IDonvitructhuocRepository _donviRepo;
        private ITinhtrangxulyRepository _tinhtrangxlRepo;
        private IAttachVanbanRepository _fileRepo;
        private IPhanloaiQuytrinhRepository _loaiquytrinhRepo;
        private IQuytrinhRepository _quytrinhRepo;
        private IRoleManager _role;
        private ITinhtrangQuytrinhRepository _tinhtrangqtRepo;
        private IHosoQuytrinhXulyRepository _hsqtRepo;
        private IQuytrinhVersionRepository _qtVersionRepo;
        private IGuiVanbanRepository _guiVanbanRepository;

        public TinhhinhxulyManager(ILogger logger,
            IVanbandenRepository vanbandenRepo, IPhanloaiVanbanRepository plvanbanRepo,
            ISoVanbanRepository sovbRepo, IKhoiphathanhRepository khoiphRepo,
            ILinhvucRepository linhvucRepo, ICanboRepository canboRepo,
             IConfigRepository configRepo, IHosocongviecRepository hosocongviecRepo,
            IHosovanbanRepository hosovanbanRepo, IDoituongxulyRepository doituongRepo,
            IHoibaovanbanRepository hoibaovanbanRepo,
            IVanbandiRepository vanbandiRepo, IDonvitructhuocRepository donviRepo,
            ITinhtrangxulyRepository tinhtrangxlRepo,
            IAttachVanbanRepository fileRepo,
            IPhanloaiQuytrinhRepository loaiquytrinhRepo, IQuytrinhRepository quytrinhRepo,
            IRoleManager role, ISessionServices session, ITinhtrangQuytrinhRepository tinhtrangRepo,
            IHosoQuytrinhXulyRepository hsqtRepo, IQuytrinhVersionRepository qtVersionRepo, IGuiVanbanRepository guiVanbanRepository)
        {
            _logger = logger;
            _vanbandenRepo = vanbandenRepo;
            _plvanbanRepo = plvanbanRepo;
            _sovbRepo = sovbRepo;
            _khoiphRepo = khoiphRepo;
            _linhvucRepo = linhvucRepo;
            _canboRepo = canboRepo;
            _configRepo = configRepo;
            _hosocongviecRepo = hosocongviecRepo;
            _hosovanbanRepo = hosovanbanRepo;
            _doituongRepo = doituongRepo;
            _hoibaovanbanRepo = hoibaovanbanRepo;
            _vanbandiRepo = vanbandiRepo;
            _donviRepo = donviRepo;
            _tinhtrangxlRepo = tinhtrangxlRepo;
            _fileRepo = fileRepo;
            _role = role;
            _session = session;
            _loaiquytrinhRepo = loaiquytrinhRepo;
            _quytrinhRepo = quytrinhRepo;
            _tinhtrangqtRepo = tinhtrangRepo;
            _hsqtRepo = hsqtRepo;
            _qtVersionRepo = qtVersionRepo;
            _guiVanbanRepository = guiVanbanRepository;
        }

        #endregion Constructor

        #region Vanbandi

        public IEnumerable<XLVanbandi> TonghopVbDi(string strngaybd, string strngaykt, LoaiNgay loaingay)
        {
            var dtengaybd = DateServices.FormatDateEn(strngaybd);
            var dtengaykt = DateServices.FormatDateEn(strngaykt);

            var dteNow = DateTime.Now;
            var sqlQuery = _guiVanbanRepository.GuiVanbans
                .Join(_vanbandiRepo.Vanbandis,
                    g => g.intidvanban,
                    v => v.intid,
                    (g, v) => new
                    {
                        g.intidvanban,
                        g.intiddonvi,
                        g.strtendonvi,
                        g.strngaygui,
                        g.strngaytiepnhan,
                        g.strngaydangxuly,
                        g.strngayhoanthanh,
                        g.intloaivanban,
                        g.intloaigui,
                        v.strngayky,
                        v.strhanxuly
                    })
                .Where(x => x.intloaivanban == (int) enumLuutruVanban.intloaivanban.vanbandi &&
                            x.intloaigui == (int) enumGuiVanban.intloaigui.Tructinh &&
                            ((loaingay == LoaiNgay.NgayGui && dtengaybd <= x.strngaygui &&
                              x.strngaygui <= dtengaykt) ||
                             (loaingay == LoaiNgay.NgayKy && dtengaybd <= x.strngayky &&
                              x.strngayky <= dtengaykt)))
                .GroupBy(x => new {x.intidvanban, x.strtendonvi})
                .ToList()
                .SelectMany(g => g.OrderByDescending(x => x.strngayhoanthanh)
                    .ThenByDescending(x => x.strngaydangxuly)
                    .ThenByDescending(x => x.strngaytiepnhan)
                    .ThenByDescending(x => x.strngaygui)
                    .Select((y, idx) => new
                    {
                        y.intidvanban,
                        y.intiddonvi,
                        y.strtendonvi,
                        y.strngaygui,
                        y.strngaytiepnhan,
                        y.strngaydangxuly,
                        y.strngayhoanthanh,
                        y.strngayky,
                        y.strhanxuly,
                        RowNumber = idx + 1
                    }));

            var data = sqlQuery.Where(x => x.RowNumber == 1);

            var xlvbdi = data
                .GroupBy(g => new {g.intiddonvi, g.strtendonvi})
                .Select(x => new XLVanbandi
                {
                    IdDonvi = x.Key.intiddonvi,
                    Donvi = x.Key.strtendonvi,
                    Dagui = x.Count(y => y.strngaygui.HasValue && !y.strngaytiepnhan.HasValue && !y.strngaydangxuly.HasValue && !y.strngayhoanthanh.HasValue && (!y.strhanxuly.HasValue || (y.strhanxuly.HasValue && y.strhanxuly >= dteNow))),
                    Tiepnhan = x.Count(y => y.strngaytiepnhan.HasValue && !y.strngaydangxuly.HasValue && !y.strngayhoanthanh.HasValue),
                    DangXuly = x.Count(y => y.strngaydangxuly.HasValue && !y.strngayhoanthanh.HasValue),
                    Hoanthanh = x.Count(y => y.strngayhoanthanh.HasValue && !(y.strhanxuly.HasValue && y.strhanxuly < y.strngayhoanthanh)),
                    Quahan = x.Count(y => y.strhanxuly.HasValue && !y.strngayhoanthanh.HasValue && y.strhanxuly < dteNow),
                    Hoanthanhtrehan = x.Count(y => y.strngayhoanthanh.HasValue && y.strhanxuly.HasValue && y.strhanxuly < y.strngayhoanthanh)
                })
            	.OrderBy(x=> x.Donvi);

            return xlvbdi;
        }

        public IEnumerable<DTO.Vanbandi.ListVanbandiViewModel> GetListVanbandi(LoaiXuLyVbDi loaiXuLyVbDi, string donvi, string strngaybd,
            string strngaykt, LoaiNgay loaingay)
        {
            var dtengaybd = DateServices.FormatDateEn(strngaybd);
            var dtengaykt = DateServices.FormatDateEn(strngaykt);
            var dteNow = DateTime.Now;
            var sqlData = _guiVanbanRepository.GuiVanbans
                .Join(_vanbandiRepo.Vanbandis,
                    g => g.intidvanban,
                    v => v.intid,
                    (g, v) => new {g, v})
                .Where(
                    x =>
                         x.g.intloaigui == (int)enumGuiVanban.intloaigui.Tructinh && 
                         x.g.intloaivanban == (int)enumLuutruVanban.intloaivanban.vanbandi &&
                         (
                             (loaingay == LoaiNgay.NgayGui && dtengaybd <= x.g.strngaygui &&
                              x.g.strngaygui <= dtengaykt) ||

                             (loaingay == LoaiNgay.NgayKy && dtengaybd <= x.v.strngayky &&
                              x.v.strngayky <= dtengaykt)
                         )
                         &&
                         (
                             (loaiXuLyVbDi == LoaiXuLyVbDi.Dagui && x.g.strngaygui.HasValue &&
                              !x.g.strngaytiepnhan.HasValue && !x.g.strngaydangxuly.HasValue &&
                              !x.g.strngayhoanthanh.HasValue &&
                              (!x.v.strhanxuly.HasValue ||
                               (x.v.strhanxuly.HasValue && x.v.strhanxuly >= dteNow))) ||

                             (loaiXuLyVbDi == LoaiXuLyVbDi.Datiepnhan && x.g.strngaytiepnhan.HasValue &&
                              !x.g.strngaydangxuly.HasValue && !x.g.strngayhoanthanh.HasValue) ||

                             (loaiXuLyVbDi == LoaiXuLyVbDi.Dangxuly && x.g.strngaydangxuly.HasValue &&
                              !x.g.strngayhoanthanh.HasValue) ||

                             (loaiXuLyVbDi == LoaiXuLyVbDi.Hoanthanh && x.g.strngayhoanthanh.HasValue && 
                             !(x.v.strhanxuly.HasValue && x.v.strhanxuly < x.g.strngayhoanthanh)) ||

                             (loaiXuLyVbDi == LoaiXuLyVbDi.Quahan && !x.g.strngayhoanthanh.HasValue &&
                              x.v.strhanxuly.HasValue && x.v.strhanxuly < dteNow) ||

                             (loaiXuLyVbDi == LoaiXuLyVbDi.Hoanthanhtrehan && x.g.strngayhoanthanh.HasValue &&
                              x.v.strhanxuly.HasValue && x.v.strhanxuly < x.g.strngayhoanthanh)
                         )
                         &&
                         (
                            (donvi.Trim() == "") ||
                            (donvi.Trim() != "" && donvi == x.g.strtendonvi)
                         )
                )
                .Select(x => new
                {
                    x.g.intidvanban,
                    x.g.strtendonvi,
                    x.g.strngaygui,
                    x.g.strngaytiepnhan,
                    x.g.strngaydangxuly,
                    x.g.strngayhoanthanh,

                    x.v.intid,
                    x.v.strngayky,
                    x.v.intso,
                    x.v.strkyhieu,
                    x.v.strtrichyeu,
                    x.v.strnoinhan,
                    x.v.strhanxuly,
                    x.v.inttrangthai,
                    x.v.strmorong
                })
                .GroupBy(x => new { x.intidvanban, x.strtendonvi })
                
                .ToList()
                .SelectMany(g => g.OrderByDescending(x => x.strngayhoanthanh)
                    .ThenByDescending(x => x.strngaydangxuly)
                    .ThenByDescending(x => x.strngaytiepnhan)
                    .ThenByDescending(x => x.strngaygui)
                    .Select((x, idx) => new
                    {
                        intid = x.intid,
                        dtengayky = x.strngayky,
                        intso = x.intso,
                        strkyhieu = x.strkyhieu,
                        strtrichyeu = x.strtrichyeu,
                        strnoinhan = x.strnoinhan,
                        dtehanxuly = x.strhanxuly,
                        inttrangthai = x.inttrangthai,
                        strsophu = !string.IsNullOrEmpty(x.strmorong) ? x.strmorong : "",
                        RowNumber = idx + 1
                    }));

            var result = sqlData
                .Where(x=> x.RowNumber == 1)
                .Select(x => new DTO.Vanbandi.ListVanbandiViewModel
                {
                    intid = x.intid,
                    dtengayky = x.dtengayky,
                    intso = x.intso,
                    strkyhieu = x.strkyhieu,
                    strtrichyeu = x.strtrichyeu,
                    strnoinhan = x.strnoinhan,
                    dtehanxuly = x.dtehanxuly,
                    inttrangthai = x.inttrangthai,
                    IsAttach = _fileRepo.AttachVanbans
                        .Any(a => a.intidvanban == x.intid &&
                                  a.inttrangthai == (int) enumAttachVanban.inttrangthai.IsActive &&
                                  a.intloai == (int) enumAttachVanban.intloai.Vanbandi),
                    strsophu = x.strsophu
                });

            return result;
        }

        #endregion

        #region Vanbanden

        /// <summary>
        /// lay ds cac phong ban 
        /// </summary>
        /// <returns></returns>
        public ListdonviViewModel GetListDonvi(int? iddonvi, string strngaybd, string strngaykt, int? idloaingay, int? idsovb)
        {
            ListdonviViewModel model = new ListdonviViewModel();

            // lay danh sach cac phong ban truc thuoc
            var Donvi = _donviRepo.Donvitructhuocs
                        .Where(p => p.inttrangthai == (int)enumDonvitructhuoc.inttrangthai.IsActive)
                        .OrderBy(p => p.intlevel)
                        .ThenBy(p => p.ParentId)
                        .ThenBy(p => p.strtendonvi);

            int maxLevelDonvi = 1;
            int intiddonvi = 0;

            DateTime? dteNgaybd = DateTime.Now; //new DateTime(2013, 1, 1);
            DateTime? dteNgaykt = DateTime.Now;

            dteNgaybd = DateServices.GetDauTuan(DateTime.Now);
            dteNgaykt = DateServices.GetCuoiTuan(DateTime.Now);

            if (!string.IsNullOrEmpty(strngaybd))
            {
                dteNgaybd = DateServices.FormatDateEn(strngaybd);
            }
            if (!string.IsNullOrEmpty(strngaykt))
            {
                dteNgaykt = DateServices.FormatDateEn(strngaykt);
            }
            model.dteNgaybd = (DateTime)dteNgaybd;
            model.dteNgaykt = (DateTime)dteNgaykt;

            if (_role.IsRole(RoleTinhhinhxulyVBDen.Xemtatca))
            {
                maxLevelDonvi = _donviRepo.Donvitructhuocs
                        .Where(p => p.inttrangthai == (int)enumDonvitructhuoc.inttrangthai.IsActive)
                        .Max(p => p.intlevel);

                if ((iddonvi != null) && (iddonvi != 0))
                {
                    intiddonvi = (int)iddonvi;
                }
                else
                {
                    intiddonvi = 1; // don vi duoc chon
                }
            }
            else
            {
                // chi co quyen xem trong phong minh
                int iduser = _session.GetUserId();
                intiddonvi = (int)_canboRepo.GetActiveCanboByID(iduser).intdonvi;

                Donvi = _donviRepo.Donvitructhuocs
                        .Where(p => p.inttrangthai == (int)enumDonvitructhuoc.inttrangthai.IsActive)
                        .Where(p => p.Id == intiddonvi)
                        .OrderBy(p => p.intlevel)
                        .ThenBy(p => p.ParentId)
                        .ThenBy(p => p.strtendonvi);


                maxLevelDonvi = 0;

                if ((iddonvi != null) && (iddonvi != 0))
                {
                    if (intiddonvi != iddonvi) { _logger.Warn("không có quyền xem tình hình xử lý văn bản đến của iddonvi: " + iddonvi.ToString()); }
                }
            }

            model.Donvi = Donvi;
            model.iddonvi = intiddonvi;
            model.maxLevelDonvi = maxLevelDonvi;

            model.idloaingay = idloaingay > 0 ? (int)idloaingay : (int)QLVB.DTO.Tinhhinhxuly.enumidloaingay.Vanbanden;

            model.idsovb = idsovb >= 0 ? (int)idsovb : 0; // =0 : tat ca cac so vb
            model.Sovanban = _sovbRepo.GetActiveSoVanbans
                        .Where(p => p.intloai == (int)enumSovanban.intloai.Vanbanden)
                        .OrderBy(p => p.strten);

            return model;
        }

        /// <summary>
        /// thong tin tong hop tinh hinh xu ly cua iddonvi
        /// </summary>
        /// <param name="iddonvi"></param>
        /// <param name="strngaybd"></param>
        /// <param name="strngaykt"></param>
        /// <returns></returns>
        public IEnumerable<XLVanbanden> TonghopVBDen(int iddonvi, string strngaybd, string strngaykt, int idloaingay, int idsovb)
        {
            //string strSearchValues = string.Empty;
            //strSearchValues += "strngaybd=" + strngaybd + ";strngaykt=" + strngaykt + ";";
            //strSearchValues += "iddonvi=" + iddonvi.ToString() + ";";
            //strSearchValues += "idloaingay=" + idloaingay.ToString() + ";";
            //strSearchValues += "idsovb=" + idsovb.ToString() + ";";

            //// luu cac gia tri vao session
            //_session.InsertObject(AppConts.SessionTonghopXuly, EnumSession.TonghopXuly.Vanbanden);
            //_session.InsertObject(AppConts.SessionTonghopValues, strSearchValues);

            List<int> listiddonvi = _GetListIdDonvi(iddonvi);

            DateTime? dtengaybd = DateServices.FormatDateEn(strngaybd);
            DateTime? dtengaykt = DateServices.FormatDateEn(strngaykt);

            DateTime dteNow = DateTime.Now;

            var vanbancanbo = _canboRepo.GetActiveCanbo
                        .Where(p => p.intdonvi == iddonvi)
                        .Where(p => listiddonvi.Contains((int)p.intdonvi))
                        .GroupJoin(
                            _tinhtrangxlRepo.Tinhtrangxulys
                                .Where(p => p.intloaihosocongviec == (int)enumHosocongviec.intloai.Vanbanden)
                                .Where(p => p.strngayden >= dtengaybd)
                                .Where(p => p.strngayden <= dtengaykt),
                            c => 1,
                            t => 1,
                            (c, t) => new { c, t }
                        );

            switch (idloaingay)
            {
                case (int)QLVB.DTO.Tinhhinhxuly.enumidloaingay.Vanbanden:
                    if (idsovb > 0)
                    {
                        vanbancanbo = _canboRepo.GetActiveCanbo
                       .Where(p => p.intdonvi == iddonvi)
                       .Where(p => listiddonvi.Contains((int)p.intdonvi))
                       .GroupJoin(
                           _tinhtrangxlRepo.Tinhtrangxulys
                               .Where(p => p.intloaihosocongviec == (int)enumHosocongviec.intloai.Vanbanden)
                               .Where(p => p.intidsovanban == idsovb) // tim theo so vb
                               .Where(p => p.strngayden >= dtengaybd)
                               .Where(p => p.strngayden <= dtengaykt),
                           c => 1,
                           t => 1,
                           (c, t) => new { c, t }
                       );
                    }
                    else
                    {
                        vanbancanbo = _canboRepo.GetActiveCanbo
                       .Where(p => p.intdonvi == iddonvi)
                       .Where(p => listiddonvi.Contains((int)p.intdonvi))
                       .GroupJoin(
                           _tinhtrangxlRepo.Tinhtrangxulys
                               .Where(p => p.intloaihosocongviec == (int)enumHosocongviec.intloai.Vanbanden)
                               .Where(p => p.strngayden >= dtengaybd)
                               .Where(p => p.strngayden <= dtengaykt),
                           c => 1,
                           t => 1,
                           (c, t) => new { c, t }
                       );
                    }

                    break;
                case (int)QLVB.DTO.Tinhhinhxuly.enumidloaingay.Thoihanxuly:
                    if (idsovb > 0)
                    {
                        vanbancanbo = _canboRepo.GetActiveCanbo
                        .Where(p => p.intdonvi == iddonvi)
                        .Where(p => listiddonvi.Contains((int)p.intdonvi))
                        .GroupJoin(
                            _tinhtrangxlRepo.Tinhtrangxulys
                                .Where(p => p.intloaihosocongviec == (int)enumHosocongviec.intloai.Vanbanden)
                                .Where(p => p.intidsovanban == idsovb) // tim theo so vb
                                .Where(p => p.strthoihanxuly >= dtengaybd)
                                .Where(p => p.strthoihanxuly <= dtengaykt),
                            c => 1,
                            t => 1,
                            (c, t) => new { c, t }
                        );
                    }
                    else
                    {
                        vanbancanbo = _canboRepo.GetActiveCanbo
                        .Where(p => p.intdonvi == iddonvi)
                        .Where(p => listiddonvi.Contains((int)p.intdonvi))
                        .GroupJoin(
                            _tinhtrangxlRepo.Tinhtrangxulys
                                .Where(p => p.intloaihosocongviec == (int)enumHosocongviec.intloai.Vanbanden)
                                .Where(p => p.strthoihanxuly >= dtengaybd)
                                .Where(p => p.strthoihanxuly <= dtengaykt),
                            c => 1,
                            t => 1,
                            (c, t) => new { c, t }
                        );
                    }

                    break;
            }


            var model = new XLVanbanden();

            //var cb = _canboRepo.GetActiveCanbo
            //            .Where(p => p.intdonvi == iddonvi)
            //            .Where(p => listiddonvi.Contains((int)p.intdonvi))
            //            .GroupJoin(
            //                _tinhtrangxlRepo.Tinhtrangxulys
            //                    .Where(p => p.intloaihosocongviec == (int)enumHosocongviec.intloai.Vanbanden)
            //                    .Where(p => p.strngayden >= dtengaybd)
            //                    .Where(p => p.strngayden <= dtengaykt),
            //                c => 1,
            //                t => 1,
            //                (c, t) => new { c, t }
            //            )
            var cb = vanbancanbo
                        .Select(p => new XLVanbanden
                        {
                            idcanbo = p.c.intid,
                            strkyhieu = p.c.strkyhieu,
                            strhoten = p.c.strhoten,
                            intVBLuu = p.t.Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dahoanthanh)
                                        .Where(a => a.intluuhoso == (int)enumHosocongviec.intluuhoso.Co)
                                        .Count(a => a.intidcanbo == p.c.intid),

                            intVBDaXuly = p.t.Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dahoanthanh)
                                        .Where(a => a.intluuhoso == (int)enumHosocongviec.intluuhoso.Khong)
                                        .Count(a => a.intidcanbo == p.c.intid),

                            intVBDangXuly = p.t.Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dangxuly)
                                        .Where(a => a.strthoihanxuly >= dteNow)
                                        .Count(a => a.intidcanbo == p.c.intid),

                            intVBQuahan = p.t.Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dangxuly)
                                        .Where(a => a.intluuhoso != (int)enumHosocongviec.intluuhoso.DangTrinhky)
                                        .Where(a => a.strthoihanxuly < dteNow)
                                        .Count(a => a.intidcanbo == p.c.intid),

                            intTrinhky = p.t.Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dangxuly)
                                        .Where(a => a.intluuhoso == (int)enumHosocongviec.intluuhoso.DangTrinhky)
                                        .Where(a => a.strthoihanxuly < dteNow)
                                        .Count(a => a.intidcanbo == p.c.intid),

                            intTong = 0

                        })
                        .OrderBy(p => p.strkyhieu)
                        .ThenBy(p => p.strhoten)
                        ;
            return cb;

        }

        /// <summary>
        /// lấy tất cả các id phòng ban và id phòng ban trực thuộc
        /// </summary>
        /// <param name="iddonvi"></param>
        /// <returns></returns>
        private List<int> _GetListIdDonvi(int iddonvi)
        {
            var alldonvi = _donviRepo.Donvitructhuocs
                .Where(p => p.inttrangthai == (int)enumDonvitructhuoc.inttrangthai.IsActive)
                            .ToList();
            var donvi = alldonvi
                        .Where(p => p.Id == iddonvi)// || p.ParentId == iddonvi)
                        .Select(p => p.Id)
                        .ToList();
            int maxLevel = alldonvi.Max(p => p.intlevel);
            int minLevel = alldonvi.Where(p => p.Id == iddonvi)
                            .FirstOrDefault().intlevel;
            List<int> listiddonvi = new List<int>();
            foreach (var d in donvi)
            {
                listiddonvi.Add(d);
            }
            for (var i = minLevel + 1; i <= maxLevel; i++)
            {
                var dv2 = alldonvi
                            .Where(p => p.intlevel == i)
                            .Where(p => p.ParentId != null)
                            .Where(p => listiddonvi.Contains((int)p.ParentId))
                            .Select(p => p.Id).ToList();
                if (dv2.Count() != 0)
                {
                    foreach (var m in dv2)
                    {
                        listiddonvi.Add(m);
                    }
                }
                else
                {
                    i = maxLevel + 1; // ngung lai
                }
            }
            return listiddonvi;
        }

        public IEnumerable<QLVB.DTO.Vanbanden.ListVanbandenViewModel> GetListVanbanden(
            int intloai, int idcanbo, int iddonvi, string strngaybd, string strngaykt, int idloaingay, int idsovb)
        {

            DateTime? dtengaybd = DateServices.FormatDateEn(strngaybd);
            DateTime? dtengaykt = DateServices.FormatDateEn(strngaykt);
            DateTime dteNow = DateTime.Now;

            var xuly = _tinhtrangxlRepo.Tinhtrangxulys
                        .Where(p => p.intloaihosocongviec == (int)enumHosocongviec.intloai.Vanbanden)
                        .Where(p => p.strngayden >= dtengaybd)
                        .Where(p => p.strngayden <= dtengaykt);

            switch (idloaingay)
            {
                case (int)QLVB.DTO.Tinhhinhxuly.enumidloaingay.Vanbanden:
                    xuly = _tinhtrangxlRepo.Tinhtrangxulys
                        .Where(p => p.intloaihosocongviec == (int)enumHosocongviec.intloai.Vanbanden)
                        .Where(p => p.strngayden >= dtengaybd)
                        .Where(p => p.strngayden <= dtengaykt);
                    break;
                case (int)QLVB.DTO.Tinhhinhxuly.enumidloaingay.Thoihanxuly:
                    xuly = _tinhtrangxlRepo.Tinhtrangxulys
                        .Where(p => p.intloaihosocongviec == (int)enumHosocongviec.intloai.Vanbanden)
                        .Where(p => p.strthoihanxuly >= dtengaybd)
                        .Where(p => p.strthoihanxuly <= dtengaykt);
                    break;
            }

            if (idsovb > 0)
            {
                xuly = xuly.Where(p => p.intidsovanban == idsovb);
            }

            List<int> listidcanbo = _canboRepo.GetAllCanbo
                        .Where(p => p.intdonvi == iddonvi)
                        .Select(p => p.intid).ToList();

            switch (intloai)
            {
                case (int)enumtinhtrangxuly.intloai.LuuHS:
                    xuly = xuly.Where(p => p.intidcanbo == idcanbo)
                                .Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dahoanthanh)
                                .Where(a => a.intluuhoso == (int)enumHosocongviec.intluuhoso.Co);
                    break;
                case (int)enumtinhtrangxuly.intloai.DaXL:
                    xuly = xuly.Where(p => p.intidcanbo == idcanbo)
                                .Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dahoanthanh)
                                .Where(a => a.intluuhoso == (int)enumHosocongviec.intluuhoso.Khong);
                    break;
                case (int)enumtinhtrangxuly.intloai.DangXL:
                    xuly = xuly.Where(p => p.intidcanbo == idcanbo)
                                .Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dangxuly)
                                .Where(a => a.strthoihanxuly >= dteNow);
                    break;
                case (int)enumtinhtrangxuly.intloai.QuahanXL:
                    xuly = xuly.Where(p => p.intidcanbo == idcanbo)
                                .Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dangxuly)
                                .Where(a => a.intluuhoso != (int)enumHosocongviec.intluuhoso.DangTrinhky)
                                .Where(a => a.strthoihanxuly < dteNow);
                    break;
                case (int)enumtinhtrangxuly.intloai.Trinhky:
                    xuly = xuly.Where(p => p.intidcanbo == idcanbo)
                                .Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dangxuly)
                                .Where(a => a.strthoihanxuly < dteNow)
                                .Where(a => a.intluuhoso == (int)enumHosocongviec.intluuhoso.DangTrinhky);
                    break;

                //================tong cong  ================================
                case (int)enumtinhtrangxuly.intloai.TongLuuHS:
                    xuly = xuly.Where(p => listidcanbo.Contains(p.intidcanbo))
                                .Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dahoanthanh)
                                .Where(a => a.intluuhoso == (int)enumHosocongviec.intluuhoso.Co);
                    break;
                case (int)enumtinhtrangxuly.intloai.TongDaXL:
                    xuly = xuly.Where(p => listidcanbo.Contains(p.intidcanbo))
                                .Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dahoanthanh)
                                .Where(a => a.intluuhoso == (int)enumHosocongviec.intluuhoso.Khong);
                    break;
                case (int)enumtinhtrangxuly.intloai.TongDangXL:
                    xuly = xuly.Where(p => listidcanbo.Contains(p.intidcanbo))
                                .Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dangxuly)
                                .Where(a => a.strthoihanxuly >= dteNow);
                    break;
                case (int)enumtinhtrangxuly.intloai.TongQuahanXL:
                    xuly = xuly.Where(p => listidcanbo.Contains(p.intidcanbo))
                                .Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dangxuly)
                                .Where(a => a.intluuhoso != (int)enumHosocongviec.intluuhoso.DangTrinhky)
                                .Where(a => a.strthoihanxuly < dteNow);
                    break;
                case (int)enumtinhtrangxuly.intloai.TongTrinhKy:
                    xuly = xuly.Where(p => listidcanbo.Contains(p.intidcanbo))
                                .Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dangxuly)
                                .Where(a => a.strthoihanxuly < dteNow)
                                .Where(a => a.intluuhoso == (int)enumHosocongviec.intluuhoso.DangTrinhky);
                    break;
            }

            var vanban = _vanbandenRepo.Vanbandens
                 .Join(
                     xuly,
                     v => v.intid,
                     t => t.intidvanban,
                     (v, t) => v
                 );
            IEnumerable<QLVB.DTO.Vanbanden.ListVanbandenViewModel> listvb = _GetListViewVanban(vanban);

            return listvb;
        }

        private IEnumerable<QLVB.DTO.Vanbanden.ListVanbandenViewModel> _GetListViewVanban(IQueryable<Vanbanden> vanban)
        {
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
                .Select(p => new QLVB.DTO.Vanbanden.ListVanbandenViewModel
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
                    IsAttach = p.v3.v2.f.Any(a => a.intidvanban == p.v3.v2.v.intid),

                    inttinhtrangxuly = p.hscv.FirstOrDefault(a =>
                            a.intid == p.v3.hsvb
                                .FirstOrDefault(b => b.intidvanban == p.v3.v2.v.intid)
                                .intidhosocongviec).inttrangthai

                    //isykien=
                })
                //.Distinct() 
                // dung distinct o day chay lau hon
                ;

            return listvb;

        }

        #endregion Vanbanden

        #region Quytrinh

        public ListQuytrinhViewModel GetListQuytrinh(
            int? intidloaiquytrinh, int? idquytrinh,
            string strngaybd, string strngaykt)
        {
            ListQuytrinhViewModel model = new ListQuytrinhViewModel();

            DateTime? dteNgaybd = DateTime.Now; //new DateTime(2013, 1, 1);
            DateTime? dteNgaykt = DateTime.Now;

            dteNgaybd = DateServices.GetDauTuan(DateTime.Now);
            dteNgaykt = DateServices.GetCuoiTuan(DateTime.Now);

            if (!string.IsNullOrEmpty(strngaybd))
            {
                dteNgaybd = DateServices.FormatDateEn(strngaybd);
            }
            if (!string.IsNullOrEmpty(strngaykt))
            {
                dteNgaykt = DateServices.FormatDateEn(strngaykt);
            }
            model.dteNgaybd = (DateTime)dteNgaybd;
            model.dteNgaykt = (DateTime)dteNgaykt;

            model.Loaiquytrinh = _loaiquytrinhRepo.PhanloaiQuytrinhs
               .Select(p => new QLVB.DTO.Quytrinh.EditLoaiQuytrinhViewModel
               {
                   intid = p.intid,
                   strtenloaiquytrinh = p.strtenloai
               });

            model.intidloaiquytrinh = (intidloaiquytrinh != null) ? (int)intidloaiquytrinh : 0;

            return model;
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

            return quytrinh;
        }


        /// <summary>
        /// thong tin tong hop tinh hinh xu ly cua iddonvi, idquytrinh
        /// </summary>
        /// <param name="idquytrinh"></param>
        /// <param name="iddonvi"></param>
        /// <param name="strngaybd"></param>
        /// <param name="strngaykt"></param>
        /// <returns></returns>
        public IEnumerable<QuytrinhVBDenViewModel> TonghopQuytrinh(
            int? idloaiquytrinh, int? idquytrinh, int? iddonvi,
            string strngaybd, string strngaykt)
        {
            string strSearchValues = string.Empty;
            strSearchValues += "strngaybd=" + strngaybd + ";strngaykt=" + strngaykt + ";";
            strSearchValues += "iddonvi=" + iddonvi.ToString() + ";";
            strSearchValues += "idquytrinh=" + idquytrinh.ToString() + ";";
            strSearchValues += "idloaiquytrinh=" + idloaiquytrinh.ToString() + ";";

            // luu cac gia tri vao session
            _session.InsertObject(AppConts.SessionTonghopXuly, EnumSession.TonghopXuly.Quytrinh);
            _session.InsertObject(AppConts.SessionTonghopValues, strSearchValues);


            DateTime? dtengaybd = DateServices.FormatDateEn(strngaybd);
            DateTime? dtengaykt = DateServices.FormatDateEn(strngaykt);

            ;

            var listidquytrinh = _quytrinhRepo.AllQuytrinhs
                .Where(p => p.intidloai == idloaiquytrinh)
                .Select(p => p.intid).ToList();

            var hosoquytrinh = _tinhtrangqtRepo.TinhtrangQuytrinhs
                //.Where(p => p.intidquytrinh == idquytrinh)
                .Where(p => listidquytrinh.Contains(p.intidquytrinh))
                .ToList();


            DateTime dtCurrent = DateTime.Now;

            var quytrinh = _quytrinhRepo.AllQuytrinhs
                    .Where(p => p.intidloai == idloaiquytrinh)
                .GroupJoin(
                    _tinhtrangqtRepo.TinhtrangQuytrinhs
                                .Where(p => p.intloaihosocongviec == (int)enumHosocongviec.intloai.Vanbanden_Quytrinh)
                                .Where(p => p.strngayden >= dtengaybd)
                                .Where(p => p.strngayden <= dtengaykt),
                    q => 1,
                    t => 1,
                    (q, t) => new { q, t }
                )
                .Select(p => new QuytrinhVBDenViewModel
                {
                    strtenquytrinh = p.q.strten,
                    idquytrinh = p.q.intid,
                    // idvanban = p.t.f
                    intVBDaXuly_Dunghan = p.t.Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dahoanthanh)
                        //.Where(a => DbFunctions.DiffDays(a.strthoihanxuly, a.strngayketthuc) >= 0)
                            .Where(a => a.strngayketthuc <= a.strthoihanxuly)
                            .Count(a => a.intidquytrinh == p.q.intid),

                    intVBDaXuly_Trehan = p.t.Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dahoanthanh)
                           .Where(a => a.strngayketthuc > a.strthoihanxuly)
                           .Count(a => a.intidquytrinh == p.q.intid),

                    intVBDangXuly = p.t.Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dangxuly)
                            .Where(a => dtCurrent <= a.strthoihanxuly)
                            .Count(a => a.intidquytrinh == p.q.intid),

                    intVBQuahan = p.t.Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dangxuly)
                            .Where(a => dtCurrent > a.strthoihanxuly)
                            .Count(a => a.intidquytrinh == p.q.intid)


                });

            return quytrinh;
        }

        public IEnumerable<QLVB.DTO.Vanbanden.ListVanbandenViewModel> GetListQuytrinhVBDen
           (int? idquytrinh, int intloai, int? idloaiquytrinh,
            string strngaybd, string strngaykt, string NodeId)
        {
            DateTime? dtengaybd = DateServices.FormatDateEn(strngaybd);
            DateTime? dtengaykt = DateServices.FormatDateEn(strngaykt);

            var xuly = _tinhtrangqtRepo.TinhtrangQuytrinhs
                        .Where(p => p.intloaihosocongviec == (int)enumHosocongviec.intloai.Vanbanden_Quytrinh)
                        .Where(p => p.strngayden >= dtengaybd)
                        .Where(p => p.strngayden <= dtengaykt)
                        .Where(p => p.intidquytrinh == idquytrinh);

            DateTime dtCurrent = DateTime.Now;

            switch (intloai)
            {
                case (int)enumtinhtrangquytrinh.intloai.DaXL_Dunghan:
                    xuly = xuly.Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dahoanthanh)
                                .Where(a => a.strngayketthuc <= a.strthoihanxuly);

                    break;
                case (int)enumtinhtrangquytrinh.intloai.DaXL_Trehan:
                    xuly = xuly.Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dahoanthanh)
                                .Where(a => a.strngayketthuc > a.strthoihanxuly);
                    break;
                case (int)enumtinhtrangquytrinh.intloai.DangXL:
                    xuly = xuly.Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dangxuly)
                                .Where(a => dtCurrent <= a.strthoihanxuly);
                    break;
                case (int)enumtinhtrangquytrinh.intloai.QuahanXL:
                    xuly = xuly.Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dangxuly)
                               .Where(a => dtCurrent > a.strthoihanxuly);
                    break;

            }

            if (!string.IsNullOrEmpty(NodeId))
            {
                var _listidhoso = _hsqtRepo.HosoQuytrinhxulys
                        .Where(p => p.nodeidFrom == NodeId)
                        .Where(p => p.inttrangthai == (int)enumHosoQuytrinhXuly.inttrangthai.DangXuly)
                        .Join(
                            xuly,
                            q => q.intidhoso,
                            x => x.intidhoso,
                            (q, x) => q.intidhoso
                        )
                        .Distinct();

                xuly = xuly.Where(p => _listidhoso.Contains(p.intidhoso));
            }

            var vanban = _vanbandenRepo.Vanbandens
                 .Join(
                     xuly,
                     v => v.intid,
                     t => t.intidvanban,
                     (v, t) => v
                 );
            IEnumerable<QLVB.DTO.Vanbanden.ListVanbandenViewModel> listvb = _GetListViewVanban(vanban);

            return listvb;
        }

        #endregion Quytrinh

        #region ViewTonghopQuytrinh


        public string XemTonghopQuytrinhFlowchart(int idquytrinh, int intloai, string strngaybd, string strngaykt)
        {
            try
            {
                List<int> listidhoso = _GetListidHoso(idquytrinh, intloai, strngaybd, strngaykt);

                var quytrinh = _hsqtRepo.HosoQuytrinhxulys
                    .Where(p => p.intidquytrinh == idquytrinh)
                    .Where(p => listidhoso.Contains(p.intidhoso))
                    .ToList();

                var dteNgayApdung = quytrinh.FirstOrDefault().strNgayapdung;

                var quytrinhversion = _qtVersionRepo.QuytrinhVersions
                    .Where(p => p.intidquytrinh == idquytrinh)
                    .Where(p => p.strNgayApdung == dteNgayApdung)
                    .ToList();
                //=======================================================

                List<NodeViewModel> nodeView = new List<NodeViewModel>();
                List<ConnectionViewModel> connectionView = new List<ConnectionViewModel>();
                List<NodeXulyTonghopViewModel> Xulys = new List<NodeXulyTonghopViewModel>();
                string strNgayApdung = DateServices.FormatDateVN(dteNgayApdung);

                foreach (var q in quytrinhversion)
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
                            nodeTo = quytrinhversion.FirstOrDefault(p => p.intidFrom == q.intidTo).nodeidFrom;
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
                        NodeXulyTonghopViewModel xuly = new NodeXulyTonghopViewModel();
                        xuly.Id = q.nodeidFrom;
                        var canbo = _canboRepo.GetAllCanboByID((int)q.intidCanbo);
                        xuly.strhotencanbo = canbo.strhoten;
                        xuly.intSongay = q.intSongay;
                        string strVaitro = "";
                        switch ((int)q.intVaitro)
                        {
                            case (int)QLVB.DTO.Quytrinh.enumEditThongtinXulyViewModel.Khongthamgia:
                                strVaitro = "Không tham gia xử lý";
                                break;
                            case (int)QLVB.DTO.Quytrinh.enumEditThongtinXulyViewModel.Lanhdaogiaoviec:
                                strVaitro = "Lãnh đạo giao việc";
                                break;
                            case (int)QLVB.DTO.Quytrinh.enumEditThongtinXulyViewModel.Lanhdaophutrach:
                                strVaitro = "Lãnh đạo phụ trách";
                                break;
                            case (int)QLVB.DTO.Quytrinh.enumEditThongtinXulyViewModel.Phoihopxuly:
                                strVaitro = "Phối hợp xử lý";
                                break;
                            case (int)QLVB.DTO.Quytrinh.enumEditThongtinXulyViewModel.Xulychinh:
                                strVaitro = "Xử lý chính";
                                break;
                        }
                        xuly.strVaitro = strVaitro;
                        xuly.intXulyDongthoi = q.intXulyDongthoi;

                        //chi tinh so ho so tai node dang xu ly (cua van ban dang xu ly tre han)
                        var trehan = quytrinh
                            //.Where(p => listidhoso.Contains(p.intidhoso))
                            .Where(p => p.intidFrom == q.intidFrom)
                            .Where(p => p.inttrangthai == (int)enumHosoQuytrinhXuly.inttrangthai.DangXuly)
                            .Select(p => p.intidhoso)
                            .Distinct()
                            //.Count();
                            ;

                        xuly.intSoHoso = trehan.Count();

                        Xulys.Add(xuly);
                    }
                }

                // them vao flowchart
                FlowchartTonghopViewModel flowchart = new FlowchartTonghopViewModel();
                flowchart.nodes = nodeView;
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
        private string WriteJson(FlowchartTonghopViewModel flowchart)
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

        private List<int> _GetListidHoso(int idquytrinh, int intloai, string strngaybd, string strngaykt)
        {
            DateTime? dtengaybd = DateServices.FormatDateEn(strngaybd);
            DateTime? dtengaykt = DateServices.FormatDateEn(strngaykt);

            var xuly = _tinhtrangqtRepo.TinhtrangQuytrinhs
                        .Where(p => p.intloaihosocongviec == (int)enumHosocongviec.intloai.Vanbanden_Quytrinh)
                        .Where(p => p.strngayden > dtengaybd)
                        .Where(p => p.strngayden < dtengaykt)
                        .Where(p => p.intidquytrinh == idquytrinh);

            DateTime dtCurrent = DateTime.Now;

            switch (intloai)
            {
                case (int)enumtinhtrangquytrinh.intloai.DaXL_Dunghan:
                    xuly = xuly.Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dahoanthanh)
                                .Where(a => a.strngayketthuc <= a.strthoihanxuly);

                    break;
                case (int)enumtinhtrangquytrinh.intloai.DaXL_Trehan:
                    xuly = xuly.Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dahoanthanh)
                                .Where(a => a.strngayketthuc > a.strthoihanxuly);
                    break;
                case (int)enumtinhtrangquytrinh.intloai.DangXL:
                    xuly = xuly.Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dangxuly)
                                .Where(a => dtCurrent < a.strthoihanxuly);
                    break;
                case (int)enumtinhtrangquytrinh.intloai.QuahanXL:
                    xuly = xuly.Where(a => a.inttrangthai == (int)enumHosocongviec.inttrangthai.Dangxuly)
                               .Where(a => dtCurrent > a.strthoihanxuly);
                    break;

            }
            List<int> listidhoso = new List<int>();
            foreach (var x in xuly)
            {
                listidhoso.Add(x.intidhoso);
            }

            return listidhoso;
        }

        #endregion ViewTonghopQuytrinh

    }
}
