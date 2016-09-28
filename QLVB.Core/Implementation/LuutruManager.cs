using QLVB.Common.Logging;
using QLVB.Core.Contract;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Luutru;
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

        public LuutruManager(IDiachiluutruRepository luutruRepo, ILogger logger)
        {
            _luutruRepo = luutruRepo;
            _logger = logger;
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

        #endregion Interface Implementation
    }
}