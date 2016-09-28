using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Core.Contract;
using QLVB.DTO.Vanbandi;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.Common.Logging;
using QLVB.Common.Sessions;
using QLVB.Domain.Abstract;
using Store.DAL.Abstract;
using QLVB.Common.Date;
using QLVB.Common.Utilities;
using QLVB.DTO.File;

namespace Store.Core.Implementation
{
    public class TracuuVanbandiManager : ITracuuVanbandiManager
    {
        #region Constructor

        private ILogger _logger;
        private ISessionServices _session;
        //========Store=======================
        private IStoreVanbandiRepository _vanbandiRepo;
        private IStoreAttachVanbanRepository _fileRepo;
        private IStoreVanbandenRepository _vanbandenRepo;
        private IStoreHosovanbanRepository _hosovanbanRepo;
        private IStoreHosocongviecRepository _hosocongviecRepo;
        private IStoreHoibaovanbanRepository _hoibaovanbanRepo;
        //private IChitietHosoRepository _chitietHosoRepo;
        private IStoreVanbandiCanboRepository _vanbandicanboRepo;
        private IStoreHosovanbanlienquanRepository _hsvblqRepo;

        private IStoreFileManager _fileManager;

        //===========QLVB======================
        private IPhanloaiVanbanRepository _plvanbanRepo;
        private ISoVanbanRepository _sovbRepo;
        private IKhoiphathanhRepository _khoiphRepo;
        private ILinhvucRepository _linhvucRepo;
        private ITinhchatvanbanRepository _tinhchatvbRepo;
        private ICanboRepository _canboRepo;
        private IDiachiluutruRepository _luutruRepo;
        private ITochucdoitacRepository _tochucRepo;

        private IConfigRepository _configRepo;

        private IDonvitructhuocRepository _donviRepo;
        private IChitietVanbandiRepository _chitietVBDiRepo;
        private QLVB.Core.Contract.IRoleManager _role;


        public TracuuVanbandiManager(IStoreVanbandiRepository vanbandiRepo, IStoreFileManager fileManager,
                IStoreAttachVanbanRepository fileRepo, IStoreHosovanbanlienquanRepository hsvblqRepo,
                IStoreVanbandenRepository vanbandenRepo, IStoreHosovanbanRepository hosovanbanRepo,
                IStoreHosocongviecRepository hosocongviecRepo, IStoreHoibaovanbanRepository hoibaovanbanRepo,
                IStoreVanbandiCanboRepository vanbandicanboRepo,

                IPhanloaiVanbanRepository plvanbanRepo, ISoVanbanRepository sovbRepo,
                IKhoiphathanhRepository khoiphRepo, ILinhvucRepository linhvucRepo,
                ITinhchatvanbanRepository tinhchatvbRepo, ICanboRepository canboRepo,
                IDiachiluutruRepository luutruRepo, ITochucdoitacRepository tochucRepo,
                ICategoryRepository categoryRepo, IConfigRepository configRepo,
                ILogger logger, IChitietHosoRepository chitietHosoRepo,
                IDonvitructhuocRepository donviRepo, IChitietVanbandiRepository chitietVBDiRepo,
                IGuiVanbanRepository guivbRepo, ISessionServices session,
                QLVB.Core.Contract.IRoleManager role
                )
        {
            _vanbandiRepo = vanbandiRepo;
            _fileManager = fileManager;
            _plvanbanRepo = plvanbanRepo;
            _sovbRepo = sovbRepo;
            _khoiphRepo = khoiphRepo;
            _linhvucRepo = linhvucRepo;
            _tinhchatvbRepo = tinhchatvbRepo;
            _canboRepo = canboRepo;
            _luutruRepo = luutruRepo;
            _tochucRepo = tochucRepo;
            _configRepo = configRepo;
            _fileRepo = fileRepo;
            _logger = logger;
            _vanbandenRepo = vanbandenRepo;
            _hosovanbanRepo = hosovanbanRepo;
            _hosocongviecRepo = hosocongviecRepo;
            _hoibaovanbanRepo = hoibaovanbanRepo;
            _vanbandicanboRepo = vanbandicanboRepo;
            _donviRepo = donviRepo;

            _chitietVBDiRepo = chitietVBDiRepo;
            _session = session;
            //_fileManager = fileManager;
            _hsvblqRepo = hsvblqRepo;
            _role = role;
        }
        #endregion Constructor

        #region ListVanban


        public IEnumerable<ListVanbandiViewModel> GetListVanbandi(
            string strngaykycat, int? idloaivb, int? idsovb, string strvbphathanh,
            int? intsobd, int? intsokt, string strngaykybd, string strngaykykt,
            string strkyhieu, string strnguoiky, string strnguoisoan, string strnguoiduyet,
            string strnoinhan, string strtrichyeu, string strhantraloi, string strdonvisoan,
            int? idkhan, int? idmat
            )
        {
            var vanban = _GetVanbandiFromRequest(
                strngaykycat, idloaivb, idsovb, strvbphathanh,
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
            bool isViewAllvb = _role.IsRole(RoleTracuuVanbandi.Xemtatcavb);
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
            string strngaykycat, int? idloaivb, int? idsovb, string strvbphathanh,
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
                isSearch = true;
                strSearchValues += "idloaivb=" + idloaivb.ToString() + ";";
            }
            if ((idsovb != null) && (idsovb != 0))
            {
                vanban = vanban.Where(p => p.intidsovanban == idsovb);
                isSearch = true;
                strSearchValues += "idsovb=" + idsovb.ToString() + ";";
            }
            if (!string.IsNullOrEmpty(strvbphathanh))
            {
                vanban = vanban.Where(p => p.intso == null)
                    .Where(p => p.strkyhieu == null);
                isSearch = true;
                strSearchValues += "strvbphathanh=" + strvbphathanh + ";";
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
                var ngaybd = _vanbandiRepo.Vanbandis.OrderByDescending(p => p.strngayky).FirstOrDefault();
                if (ngaybd != null)
                {
                    int intngay = _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);

                    DateTime? dtengaybd = ngaybd.strngayky.AddDays(-intngay);
                    vanban = vanban.Where(p => p.strngayky >= dtengaybd);
                }

                // reset session
                _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.NoSearch);
            }
            else
            {   // luu cac gia tri search vao session
                _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.SearchTracuuVBDi);
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

        #region ViewDetail

        public DetailVBDiViewModel GetViewDetail(int idvanban)
        {
            int idcanbo = _session.GetUserId();
            //bool isView = _role.IsViewVanbandi(idvanban, idcanbo);
            //if (isView == false)
            //{
            //    _logger.Warn("không có quyền xem văn bản: " + idvanban.ToString());
            //    return new DetailVBDiViewModel();
            //}

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


            return vanban;
        }

        #endregion ViewDetail

    }
}
