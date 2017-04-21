using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.Core.Contract;
using QLVB.Common.Logging;
using QLVB.Common.Crypt;
using QLVB.Common.Utilities;
using QLVB.Common.Sessions;
using QLVB.DTO.Tonghop;
using QLVB.DTO;

namespace QLVB.Core.Implementation
{
    public class TonghopManager : ITonghopManager
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
        private IRoleManager _role;
        private ITonghopCanboRepository _tonghopRepo;

        public TonghopManager(ILogger logger,
            IVanbandenRepository vanbandenRepo, IPhanloaiVanbanRepository plvanbanRepo,
            ISoVanbanRepository sovbRepo, IKhoiphathanhRepository khoiphRepo,
            ILinhvucRepository linhvucRepo, ICanboRepository canboRepo,
             IConfigRepository configRepo, IHosocongviecRepository hosocongviecRepo,
            IHosovanbanRepository hosovanbanRepo, IDoituongxulyRepository doituongRepo,
            IHoibaovanbanRepository hoibaovanbanRepo,
            IVanbandiRepository vanbandiRepo, IDonvitructhuocRepository donviRepo,
            ITinhtrangxulyRepository tinhtrangxlRepo,
            IAttachVanbanRepository fileRepo,
            IRoleManager role, ISessionServices session,
            ITonghopCanboRepository tonghopRepo)
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
            _tonghopRepo = tonghopRepo;
        }

        #endregion Constructor

        public IEnumerable<ListTonghopVBDenViewModel> GetTonghopVanbanden()
        {
            int idcanbo = _session.GetUserId();
            int intngay = _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);

            var thvbden = _tonghopRepo.TonghopCanbos
                .Where(p => p.intidcanbo == idcanbo)
                .Where(p => p.intloaitailieu == (int)enumTonghopCanbo.intloaitailieu.Vanbanden)
                .Where(p => p.intloai == (int)enumTonghopCanbo.intloai.Vanbanmoi
                    || p.intloai == (int)enumTonghopCanbo.intloai.Debiet
                    || p.intloai == (int)enumTonghopCanbo.intloai.HosoXLVBDen
                )
                .Where(p => p.inttrangthai == (int)enumTonghopCanbo.inttrangthai.Chuaxem)
                .Where(p => System.Data.Entity.DbFunctions.DiffDays(p.strngaytao, DateTime.Now) < intngay)
                .Select(p => p.intidtailieu)
                .ToList();
            if (thvbden.Count() > 0)
            {
                var vanbanden =
                    _vanbandenRepo.Vanbandens
                            .Where(p => thvbden.Contains(p.intid))
                    .GroupJoin(
                    _fileRepo.AttachVanbans
                        .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                        .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbanden),
                    v => 1,
                    f => 1,
                    (v, f) => new { v, f }
                    )
                    .Select(p => new ListTonghopVBDenViewModel
                    {
                        intid = p.v.intid,
                        dtengayden = p.v.strngayden,
                        intsoden = p.v.intsoden,
                        strkyhieu = p.v.strkyhieu,
                        strnoinhan = p.v.strnoinhan,
                        strnoiphathanh = p.v.strnoiphathanh,
                        strtrichyeu = p.v.strtrichyeu,
                        inttrangthai = p.v.inttrangthai,
                        IsAttach = p.f.Any(a => a.intidvanban == p.v.intid)
                    });

                return vanbanden;
            }
            else
            {
                // khong co thong tin moi
                return null;
            }

        }

        public ResultFunction UpdateStatusVanbanden(int idtailieu)
        {
            ResultFunction kq = new ResultFunction();

            int idcanbo = _session.GetUserId();
            kq.id = _tonghopRepo.CapnhatTrangthaiVBDen(idcanbo, idtailieu);

            return kq;
        }


        public IEnumerable<ListTonghopVBDenViewModel> GetTonghopHosoXLVBDen()
        {
            int idcanbo = _session.GetUserId();
            int intngay = _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);

            var thvbden = _tonghopRepo.TonghopCanbos
                .Where(p => p.intidcanbo == idcanbo)
                .Where(p => p.intloaitailieu == (int)enumTonghopCanbo.intloaitailieu.Vanbanden)
                .Where(p => p.intloai == (int)enumTonghopCanbo.intloai.Phieutrinh
                    || p.intloai == (int)enumTonghopCanbo.intloai.Ykienxuly
                    || p.intloai == (int)enumTonghopCanbo.intloai.Trinhky
                )
                .Where(p => p.inttrangthai == (int)enumTonghopCanbo.inttrangthai.Chuaxem)
                .Where(p => System.Data.Entity.DbFunctions.DiffDays(p.strngaytao, DateTime.Now) < intngay)
                .Select(p => p.intidtailieu)
                .ToList();
            if (thvbden.Count() > 0)
            {
                var vanbanden =
                    _vanbandenRepo.Vanbandens
                            .Where(p => thvbden.Contains(p.intid))
                    .GroupJoin(
                    _fileRepo.AttachVanbans
                        .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                        .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbanden),
                    v => 1,
                    f => 1,
                    (v, f) => new { v, f }
                    )
                    .Select(p => new ListTonghopVBDenViewModel
                    {
                        intid = p.v.intid,
                        dtengayden = p.v.strngayden,
                        intsoden = p.v.intsoden,
                        strkyhieu = p.v.strkyhieu,
                        strnoinhan = p.v.strnoinhan,
                        strnoiphathanh = p.v.strnoiphathanh,
                        strtrichyeu = p.v.strtrichyeu,
                        inttrangthai = p.v.inttrangthai,
                        IsAttach = p.f.Any(a => a.intidvanban == p.v.intid)
                    });

                return vanbanden;
            }
            else
            {
                // khong co thong tin moi
                //IEnumerable<ListTonghopVBDenViewModel> vanban = new ListTonghopVBDenViewModel();
                //return vanban;


                return null;
            }
        }
    }
}
