using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Core.Contract;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Sovb;
using QLVB.Common.Logging;


namespace QLVB.Core.Implementation
{
    public class SovanbanManager : ISovanbanManager
    {
        #region Constructor
        private ILogger _logger;

        private ISoVanbanRepository _sovbRepo;
        private IMenuRepository _menuRepo;
        private IKhoiphathanhRepository _khoiphRepo;
        private IPhanloaiVanbanRepository _phanloaiVBRepo;
        private ISovbKhoiphRepository _sovbKhoiphRepo;

        public SovanbanManager(ISoVanbanRepository sovbRepo, IMenuRepository menuRepo,
            IKhoiphathanhRepository khoiphRepo, IPhanloaiVanbanRepository phanloaiVBRepo,
            ISovbKhoiphRepository sovbkhoiphRepo, ILogger logger)
        {
            _sovbRepo = sovbRepo;
            _menuRepo = menuRepo;
            _khoiphRepo = khoiphRepo;
            _phanloaiVBRepo = phanloaiVBRepo;
            _sovbKhoiphRepo = sovbkhoiphRepo;
            _logger = logger;
        }
        #endregion Constructor

        #region Interface Implementation

        public ListSovanbanViewModel GetListSovb()
        {
            ListSovanbanViewModel sovanban = new ListSovanbanViewModel
            {
                Sovbden = _sovbRepo.GetActiveSoVanbans
                            .Where(p => p.intloai == (int)enumSovanban.intloai.Vanbanden)
                            .OrderBy(p => p.intorder)
                            .ThenBy(p => p.strten),
                Sovbdi = _sovbRepo.GetActiveSoVanbans
                            .Where(p => p.intloai == (int)enumSovanban.intloai.Vanbanphathanh)
                            .OrderBy(p => p.intorder)
                            .ThenBy(p => p.strten)
            };
            return sovanban;
        }

        public int GetLoaiSovb(int idsovb)
        {
            int? intloai = _sovbRepo.GetActiveSoVanbans
                .FirstOrDefault(p => p.intid == idsovb).intloai;
            return (int)intloai;
        }

        public ListKhoiphathanhViewModel GetListKhoiphathanh(int idsovb)
        {
            var idkhoi = _sovbRepo.GetActiveSoVanbans
                    .Where(p => p.intloai == (int)enumSovanban.intloai.Vanbanden)
                    .Where(p => p.intid == idsovb)
                    .FirstOrDefault().intidkhoiph;
            var khoiph = _khoiphRepo.GetActiveKhoiphathanhs
                .Where(p => p.inttrangthai == (int)enumKhoiphathanh.inttrangthai.IsActive)
                .Select(p => new KhoiphathanhViewModel
                {
                    intid = p.intid,
                    strtenkhoi = p.strtenkhoi,
                    IsDefault = p.intid == idkhoi ? true : false
                })
                .OrderBy(p => p.strtenkhoi);
            ListKhoiphathanhViewModel listkhoiph = new ListKhoiphathanhViewModel
            {
                idsovb = idsovb,
                Khoiphathanh = khoiph
            };

            return listkhoiph;
        }

        public ResultFunction SaveKhoiphathanh(int idsovb, int idkhoiph)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                _sovbRepo.CapnhatKhoiph(idsovb, idkhoiph);
                //_logger.Info("Cập nhật khối phát hành của sổ văn bản đến");
                kq.id = (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
            }
            return kq;
        }

        public ListLoaivanbanViewModel GetListLoaivanban(int idsovb)
        {
            var idloaivb = _sovbRepo.GetActiveSoVanbans
                    .Where(p => p.intloai == (int)enumSovanban.intloai.Vanbanphathanh)
                    .Where(p => p.intid == idsovb)
                    .FirstOrDefault().intidloaivb;
            var loaivb = _phanloaiVBRepo.GetActivePhanloaiVanbans
                    .Where(p => p.intloai == (int)enumPhanloaiVanban.intloai.vanbandi)
                    .Select(p => new LoaivanbanViewModel
                    {
                        intid = p.intid,
                        strtenloaivanban = p.strtenvanban,
                        IsDefault = p.intid == idloaivb ? true : false
                    })
                    .OrderBy(p => p.strtenloaivanban);
            ListLoaivanbanViewModel listloaivb = new ListLoaivanbanViewModel
            {
                idsovb = idsovb,
                Loaivanban = loaivb
            };
            return listloaivb;
        }

        public ResultFunction SaveLoaivanban(int idsovb, int idloaivb)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                _sovbRepo.CapnhatLoaivb(idsovb, idloaivb);
                //_logger.Info("Cập nhật loại văn bản của sổ văn bản đi");
                kq.id = (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
            }
            return kq;
        }

        public EditSovanbanViewModel GetEditSovanban(int id)
        {
            var sovb = _sovbRepo.GetActiveSoVanbans
                .Where(p => p.intid == id)
                .Select(p => new EditSovanbanViewModel
                {
                    intid = p.intid,
                    strkyhieu = p.strkyhieu,
                    strten = p.strten,
                    strghichu = p.strghichu,
                    intloai = p.intloai,
                    intorder = p.intorder,
                    IsDefault = p.IsDefault,
                    Loaisovanban = p.intloai == (int)enumSovanban.intloai.Vanbanden ? enumSovanban.intloai.Vanbanden : enumSovanban.intloai.Vanbanphathanh
                })
                .FirstOrDefault();
            return sovb;
        }

        public ResultFunction SaveSovanban(EditSovanbanViewModel model)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                SoVanban sovb = new SoVanban();
                sovb.intloai = model.intloai;
                sovb.strghichu = model.strghichu;
                sovb.strkyhieu = model.strkyhieu;
                sovb.strten = model.strten;
                sovb.IsDefault = model.IsDefault;
                if (model.IsDefault == true)
                {
                    _sovbRepo.UpdateIsDefault((int)model.intloai);
                }
                if (model.intid == 0)
                {
                    // them moi
                    _sovbRepo.Themmoi(sovb);
                    _logger.Info("Thêm mới sổ văn bản: " + model.strten + ", intloai: " + model.intloai.ToString());
                }
                else
                {
                    // cap nhat
                    _sovbRepo.Capnhat(model.intid, sovb);
                    _logger.Info("Cập nhật sổ văn bản: " + model.strten + ", id: " + model.intid.ToString());
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

        public ResultFunction DeleteSovanban(EditSovanbanViewModel model)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                _sovbRepo.Xoa(model.intid);
                _logger.Info("Xóa sổ văn bản: " + model.strten + ", id: " + model.intid.ToString());
                kq.id = (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
            }
            return kq;
        }

        #endregion Interface Implementation

    }
}
