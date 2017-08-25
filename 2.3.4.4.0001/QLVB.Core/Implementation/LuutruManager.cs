using QLVB.Common.Date;
using QLVB.Common.Logging;
using QLVB.Common.Sessions;
using QLVB.Core.Contract;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Luutru;
using QLVB.DTO.TracuuLuutru;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QLVB.Core.Implementation
{
    public class LuutruManager : ILuutruManager
    {
        #region Constructor

        private ILogger _logger;
        private IDiachiluutruRepository _luutruRepo;
        private IVanbandiRepository _vanbandiRepo;
        private ISessionServices _session;
        private ILuutruVanbanRepository _luutruVanbanRepo;
        private ICanboRepository _canboRepo;

        public LuutruManager(IDiachiluutruRepository luutruRepo, ILogger logger,
            IVanbandiRepository vanbandiRepo, ISessionServices session,
            ILuutruVanbanRepository luutruvbRepo, ICanboRepository canboRepo)
        {
            _luutruRepo = luutruRepo;
            _logger = logger;
            _vanbandiRepo = vanbandiRepo;
            _session = session;
            _luutruVanbanRepo = luutruvbRepo;
            _canboRepo = canboRepo;
        }

        #endregion Constructor

        #region Interface Implementation

        public IList<Diachiluutru> GetRootDonvi()
        {
            var donvicon = _luutruRepo.GetActiveDiachiluutrus
                        .ToList();
            var donvi = donvicon.Where(p => p.ParentId == null).OrderBy(p => p.strtendonvi).ToList();

            return donvi;
        }

        public EditDonviViewModel GetDonvi(int id)
        {
            var dv = _luutruRepo.GetActiveDiachiluutrus.Where(p => p.Id == id)
               .Select(p => new EditDonviViewModel
               {
                   intid = p.Id,
                   strtendonvi = p.strtendonvi
               }).First();
            return dv;
        }

        public int SaveDonvi(EditDonviViewModel model)
        {
            try
            {
                if (model.intType == 0)
                {   // add new
                    // them node con vao id don vi dang chon
                    _luutruRepo.AddTen(model.strtendonvi, model.intid);
                    _logger.Info("Thêm mới đơn vị lưu trữ: " + model.strtendonvi);
                }
                else
                {   // cap nhat
                    _luutruRepo.SetTen(model.strtendonvi, model.intid);
                    _logger.Info("Cập nhật đơn vị lưu trữ: " + model.strtendonvi + ", id: " + model.intid);
                }
                return (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return (int)ResultViewModels.Error;
            }
        }

        public int DeleteDonvi(DeleteDonviViewModel model)
        {
            try
            {
                // kiem tra dieu kien xem co xoa don vi duoc khong
                // co vanban va don vi con truc thuoc khong???
                var donvicon = _luutruRepo.GetActiveDiachiluutrus
                                .Where(p => p.ParentId == model.intid);
                if (donvicon.Count() == 0)
                {
                    _luutruRepo.DeleteDonvi(model.intid);
                    _logger.Info("Xóa đơn vị lưu trữ: " + model.strtendonvi);
                    return (int)ResultViewModels.Success;

                    //var vanban = _luutruRepo.GetActiveDiachiluutrus.Where(p => p.Id == model.intid);
                    //if (vanban.Count() == 0)
                    //{
                    //    _luutruRepo.DeleteDonvi(model.intid);
                    //    _logger.Info("Xóa đơn vị lưu trữ: " + model.strtendonvi);
                    //    return (int)ResultViewModels.Success;
                    //}
                    //else
                    //{
                    //    _logger.Info("Không thể xóa đơn vị: " + model.strtendonvi);
                    //    return (int)ResultViewModels.ErrorForeignKey;
                    //}
                }
                else
                {
                    _logger.Info("Không thể xóa đơn vị: " + model.strtendonvi);
                    return (int)ResultViewModels.ErrorForeignKey;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return (int)ResultViewModels.Error;
            }
        }

        public CapnhatLuutruViewModel GetFormCapnhatLuutru(int idvanban, int intloai)
        {
            CapnhatLuutruViewModel model = new CapnhatLuutruViewModel();
            if (intloai == (int)enumLuutruVanban.intloaivanban.vanbandi)
            {
                var vb = _vanbandiRepo.Vanbandis.FirstOrDefault(p => p.intid == idvanban);
                if (vb != null)
                {
                    model.intidvanban = vb.intid;
                    model.intso = vb.intso;
                    model.strsophu = vb.strmorong;
                    model.strkyhieu = vb.strkyhieu;
                    model.strtrichyeu = vb.strtrichyeu;
                    model.dtengayvanban = vb.strngayky;
                    model.strngayvanban = DateServices.FormatDateVN(vb.strngayky);
                }
                var lt = _luutruVanbanRepo.LuutruVanbans
                        .Where(p => p.intidvanban == idvanban)
                        .Where(p => p.intloaivanban == intloai)
                        .FirstOrDefault();
                if (lt != null)
                {
                    model.intid = lt.intid;
                    model.inthopso = lt.inthopso;
                    model.intdonvibaoquan = lt.intdonvibaoquan;
                    model.strthoihanbaoquan = lt.strthoihanbaoquan;
                    model.strnoidung = lt.strnoidung;
                    if (lt.intidnguoicapnhat > 0)
                    {
                        model.strnguoicapnhat = _canboRepo.GetAllCanboByID((int)lt.intidnguoicapnhat).strhoten;
                    }
                    if (lt.strngaycapnhat != null)
                    {
                        model.strngaycapnhat = DateServices.FormatDateVN(lt.strngaycapnhat);
                    }
                }
                else
                {   // them moi
                    model.strthoihanbaoquan = "vĩnh viễn";
                }
            }

            return model;
        }

        public int SaveLuutruVanban(LuutruVanban luutru)
        {
            try
            {
                luutru.intidnguoicapnhat = _session.GetUserId();
                if (luutru.intid == 0)
                {   // them moi
                    _luutruVanbanRepo.Them(luutru);
                    return luutru.intid;
                }
                else
                {
                    _luutruVanbanRepo.Sua(luutru.intid, luutru);
                    return luutru.intid;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return (int)ResultViewModels.Error;
            }
        }

        #endregion Interface Implementation
    }
}