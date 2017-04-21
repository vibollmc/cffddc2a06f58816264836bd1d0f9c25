using QLVB.Common.Logging;
using QLVB.Core.Contract;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Chucdanh;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QLVB.Core.Implementation
{
    public class ChucdanhManager : IChucdanhManager
    {
        #region Constructor

        private ILogger _logger;
        private IChucdanhRepository _chucdanhRepo;
        private ICanboRepository _canboRepo;

        public ChucdanhManager(ILogger logger, IChucdanhRepository chucdanhRepo, ICanboRepository canboRepo)
        {
            _logger = logger;
            _chucdanhRepo = chucdanhRepo;
            _canboRepo = canboRepo;
        }

        #endregion Constructor

        #region Interface Implementation

        public IEnumerable<Chucdanh> GetChucdanh()
        {
            var chucdanh = _chucdanhRepo.Chucdanhs.OrderBy(p => p.strtenchucdanh);
            return chucdanh;
        }

        public IEnumerable<ListChucdanhViewModel> GetListChucdanh()
        {
            var chucdanh = _chucdanhRepo.Chucdanhs.OrderBy(p => p.strtenchucdanh)
                        .Select(p => new ListChucdanhViewModel
                        {
                            intid = p.intid,
                            strmachucdanh = p.strmachucdanh,
                            strghichu = p.strghichu,
                            strtenchucdanh = p.strtenchucdanh,
                            Loaichucdanh = p.intloai == (int)enumchucdanh.intloai.Lanhdao ? "Lãnh đạo" : "Cán bộ"
                        });
            return chucdanh;
            //select new
            //{
            //    FirstName = student.FirstName,
            //    LastName = student.LastName,
            //    Grade = grade.Grade.Value >= 4 ? "A" :
            //                grade.Grade.Value >= 3 ? "B" :
            //                grade.Grade.Value >= 2 ? "C" :
            //                grade.Grade.Value != null ? "D" : "-"
            //};
        }

        public Chucdanh GetChucdanh(int id)
        {
            return _chucdanhRepo.GetChucdanh(id);
        }

        public int Save(EditChucdanhViewModel model)
        {
            try
            {
                Chucdanh cd = new Chucdanh
                {
                    intid = model.intid,
                    intloai = (int)model.Loaichucdanh,
                    strghichu = model.strghichu,
                    strmachucdanh = model.strmachucdanh,
                    strtenchucdanh = model.strtenchucdanh
                };
                if (model.intid == 0)
                {   // them moi
                    _chucdanhRepo.AddChucdanh(cd);
                    _logger.Info("Thêm mới chức danh: " + model.strtenchucdanh);
                }
                else
                {   // cap nhat
                    _chucdanhRepo.EditChucdanh(model.intid, cd);
                    _logger.Info("Cập nhật chức danh: " + model.strtenchucdanh + ", id: " + model.intid);
                }
                return (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return (int)ResultViewModels.Error;
            }
        }

        public int Delete(DeleteChucdanhViewModel model)
        {
            try
            {   // kiem tra xem co quan he voi table canbo khong
                int cb = _canboRepo.GetAllCanbo.Where(p => p.intchucvu == model.intid).Count();
                if (cb == 0)
                {
                    _chucdanhRepo.DeleteChucdanh(model.intid);
                    _logger.Info("Xóa chức danh: " + model.strtenchucdanh + ", id: " + model.intid);
                    return (int)ResultViewModels.Success;
                }
                else
                {
                    return (int)ResultViewModels.Error;
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