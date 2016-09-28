using QLVB.Common.Date;
using QLVB.Common.Logging;
using QLVB.Common.Sessions;
using QLVB.Core.Contract;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO.Baocao;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QLVB.Core.Implementation
{
    public class BaocaoManager : IBaocaoManager
    {
        #region Constructor

        private ILogger _logger;
        private ISessionServices _session;
        private IBaocaoRepository _baocaoRepo;
        private ISoVanbanRepository _sovbRepo;
        private IPhanloaiVanbanRepository _plvanbanRepo;
        private IVanbandenRepository _vbdenRepo;
        private IVanbandiRepository _vbdiRepo;
        private IDonvitructhuocRepository _donviRepo;
        private IHosovanbanRepository _hosovanbanRepo;
        private IDoituongxulyRepository _dtxulyRepo;
        private ICanboRepository _canboRepo;
        private IConfigRepository _configRepo;

        private IAccountManager _accountMan;

        public BaocaoManager(ILogger logger, IBaocaoRepository baocaoRepo, ISoVanbanRepository sovbRepo,
                                IPhanloaiVanbanRepository plvanbanRepo, ISessionServices session,
                            IVanbandenRepository vbdenRepo, IVanbandiRepository vbdiRepo,
                            IAccountManager accountMan, IDonvitructhuocRepository donviRepo,
                            IHosovanbanRepository hosovanbanRepo, IDoituongxulyRepository dtxlRepo,
                            ICanboRepository canboRepo, IConfigRepository configRepo)
        {
            _logger = logger;
            _baocaoRepo = baocaoRepo;
            _sovbRepo = sovbRepo;
            _plvanbanRepo = plvanbanRepo;
            _session = session;
            _vbdenRepo = vbdenRepo;
            _vbdiRepo = vbdiRepo;
            _accountMan = accountMan;
            _donviRepo = donviRepo;
            _hosovanbanRepo = hosovanbanRepo;
            _dtxulyRepo = dtxlRepo;
            _canboRepo = canboRepo;
            _configRepo = configRepo;
        }

        #endregion Constructor

        public CategoryBaocaoViewModel GetCategory()
        {
            CategoryBaocaoViewModel model = new CategoryBaocaoViewModel();
            model.Vanbanden = _baocaoRepo.Baocaos
                            .Where(p => p.intloai == (int)enumBaocao.intloai.Vanbanden)
                            .Where(p => p.inttrangthai == (int)enumBaocao.inttrangthai.IsActive)
                            .OrderBy(p => p.intorder);
            model.Vanbandi = _baocaoRepo.Baocaos
                            .Where(p => p.intloai == (int)enumBaocao.intloai.Vanbandi)
                            .Where(p => p.inttrangthai == (int)enumBaocao.inttrangthai.IsActive)
                            .OrderBy(p => p.intorder);
            return model;
        }

        public IEnumerable<SoVanban> GetSovanban(int idloai)
        {
            var sovb = _sovbRepo.GetActiveSoVanbans
                        .Where(p => p.intloai == idloai);

            return sovb;
        }

        public IEnumerable<PhanloaiVanban> GetLoaivanban(int idloai)
        {
            var loaivb = _plvanbanRepo.GetActivePhanloaiVanbans
                .Where(p => p.intloai == idloai);
            return loaivb;
        }

        public ListdonviViewModel GetListDonvi()
        {
            ListdonviViewModel model = new ListdonviViewModel();

            // lay danh sach cac phong ban truc thuoc
            var Donvi = _donviRepo.Donvitructhuocs
                        .Where(p => p.inttrangthai == (int)enumDonvitructhuoc.inttrangthai.IsActive)
                        .OrderBy(p => p.intlevel)
                        .ThenBy(p => p.ParentId)
                        .ThenBy(p => p.strtendonvi);

            int maxLevelDonvi = 1;
            maxLevelDonvi = _donviRepo.Donvitructhuocs
                        .Where(p => p.inttrangthai == (int)enumDonvitructhuoc.inttrangthai.IsActive)
                        .Max(p => p.intlevel);

            int intiddonvi = 0;

            model.Donvi = Donvi;
            model.iddonvi = intiddonvi;
            model.maxLevelDonvi = maxLevelDonvi;
            model.IsXuly = true;

            return model;
        }

        #region Report

        public string GetTenDonvi()
        {
            string strten = _accountMan.GetTenDonvi();
            return strten;
        }

        public string GetTenPhong(int idphong, bool isxuly)
        {
            if (isxuly)
            {
                return _donviRepo.Donvitructhuocs.FirstOrDefault(p => p.Id == idphong).strtendonvi;
            }
            else
            {
                return string.Empty;
            }

        }

        public List<Vanbanden> GetSovanbanden(SoVanbandenViewModel model)
        {
            try
            {
                DateTime dtngaydenbd = (DateTime)DateServices.FormatDateEn(model.strTungay);
                DateTime dtngaydenkt = (DateTime)DateServices.FormatDateEn(model.strDenngay);

                if (model.IsXuly)
                {   // in van ban da phan xu ly theo don vi
                    List<int> listidcanbo = _canboRepo.GetAllCanbo.Where(p => p.intdonvi == model.iddonvi)
                            .Select(p => p.intid).ToList();

                    var vanban = _vbdenRepo.Vanbandens
                        //.Where(p => p.intidsovanban == model.idsovanban)
                        .Where(p => model.listidso.Contains(p.intidsovanban))
                        .Where(p => p.strngayden >= dtngaydenbd)
                        .Where(p => p.strngayden <= dtngaydenkt)
                        .Join(
                            _hosovanbanRepo.Hosovanbans.Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden),
                            v => v.intid,
                            hs => hs.intidvanban,
                            (v, hs) => new { v, hs.intidhosocongviec }
                        )
                        .Join(
                            _dtxulyRepo.Doituongxulys
                                .Where(p => p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Xulychinh)
                                .Where(p => listidcanbo.Contains((int)p.intidcanbo)),
                            v2 => v2.intidhosocongviec,
                            dt => dt.intidhosocongviec,
                            (v2, dt) => v2
                        )
                        .Select(p => p.v)
                        .OrderBy(p => p.strngayden)
                        .ThenBy(p => p.intsoden)
                        .ToList();
                    return vanban;
                }
                else
                {   // in tat ca van ban
                    var vanban = _vbdenRepo.Vanbandens
                        //.Where(p => p.intidsovanban == model.idsovanban)
                        .Where(p => model.listidso.Contains(p.intidsovanban))
                        .Where(p => p.strngayden >= dtngaydenbd)
                        .Where(p => p.strngayden <= dtngaydenkt)
                        .OrderBy(p => p.strngayden)
                        .ThenBy(p => p.intsoden)
                        .ToList();
                    return vanban;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }

        public string GetTensovanban(int idsovb)
        {
            try
            {
                return _sovbRepo.GetAllSoVanbans.FirstOrDefault(p => p.intid == idsovb).strten;
            }
            catch
            {
                return null;
            }
        }
        public string GetTensovanban(string listidsovb)
        {
            try
            {
                List<int> listid = new List<int>();
                string[] split = listidsovb.Split(',');
                foreach (var s in split)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            int id = Convert.ToInt32(s);
                            listid.Add(id);
                        }
                    }
                    catch { }
                }
                var sovb = _sovbRepo.GetAllSoVanbans.Where(p => listid.Contains(p.intid));
                string strtenso = string.Empty;
                foreach (var p in sovb)
                {
                    strtenso += p.strten + ", ";
                }
                return strtenso;
            }
            catch
            {
                return null;
            }
        }
        public List<Vanbandi> GetSovanbandi(SovanbandiViewModel model)
        {
            try
            {
                DateTime dtngaykybd = (DateTime)DateServices.FormatDateEn(model.strTungay);
                DateTime dtngaykykt = (DateTime)DateServices.FormatDateEn(model.strDenngay);
                var vanban = _vbdiRepo.Vanbandis
                        .Where(p => p.intidsovanban == model.idsovanban)
                        .Where(p => p.strngayky >= dtngaykybd)
                        .Where(p => p.strngayky <= dtngaykykt)
                        .ToList();
                return vanban;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }

        public List<Vanbanden> GetLoaivanbanden(LoaivanbandenViewModel model)
        {
            try
            {
                DateTime dtngaydenbd = (DateTime)DateServices.FormatDateEn(model.strTungay);
                DateTime dtngaydenkt = (DateTime)DateServices.FormatDateEn(model.strDenngay);
                var vanban = _vbdenRepo.Vanbandens
                        .Where(p => p.intidphanloaivanbanden == model.idloaivanban)
                        .Where(p => p.strngayden >= dtngaydenbd)
                        .Where(p => p.strngayden <= dtngaydenkt)
                        .ToList();
                return vanban;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }

        public string GetTenLoaivanban(int idloai)
        {
            var loaivb = _plvanbanRepo.GetAllPhanloaiVanbans
                .FirstOrDefault(p => p.intid == idloai).strtenvanban;
            return loaivb;
        }

        public List<Vanbandi> GetLoaivanbandi(LoaivanbandiViewModel model)
        {
            try
            {
                DateTime dtngaykybd = (DateTime)DateServices.FormatDateEn(model.strTungay);
                DateTime dtngaykykt = (DateTime)DateServices.FormatDateEn(model.strDenngay);
                var vanban = _vbdiRepo.Vanbandis
                        .Where(p => p.intidphanloaivanbandi == model.idloaivanban)
                        .Where(p => p.strngayky >= dtngaykybd)
                        .Where(p => p.strngayky <= dtngaykykt)
                        .ToList();
                return vanban;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }

        }
        #endregion Report

        #region PhieuNhanVBDen

        public PhieunhanVBDenViewModels GetNoidungPhieuVBDen(int idvanban)
        {
            PhieunhanVBDenViewModels model = new PhieunhanVBDenViewModels();

            try
            {
                var vb = _vbdenRepo.GetVanbandenById(idvanban);
                model.strngaybd = "ngày " + vb.strngayden.Value.Day + " tháng " + vb.strngayden.Value.Month + " năm " + vb.strngayden.Value.Year;
                int thoihanxuly = 10;
                thoihanxuly = _configRepo.GetConfigToInt(QLVB.Domain.Entities.ThamsoHethong.ThoihanXLVB);
                DateTime dtngaykt = DateServices.AddThoihanxuly((DateTime)vb.strngayden, thoihanxuly);
                model.strngaykt = "ngày " + dtngaykt.Day + " tháng " + dtngaykt.Month + " năm " + dtngaykt.Year;
                DateTime dteNow = DateTime.Now;

                model.strngayhientai = "Đồng Nai, ngày " + dteNow.Day + " tháng " + dteNow.Month + " năm " + dteNow.Year;
                model.strtendonvi = vb.strnoiphathanh;
                model.strtrichyeu = vb.strtrichyeu;

                int iduser = _session.GetUserId();
                model.strnguoitiepnhan = _canboRepo.GetActiveCanboByID(iduser).strhoten;
                model.strtensovanban = vb.intsoden.ToString();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return model;
        }

        #endregion PhieuNhanVBDen
    }
}