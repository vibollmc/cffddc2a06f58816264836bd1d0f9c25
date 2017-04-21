using QLVB.Common.Logging;
using QLVB.Core.Contract;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Linhvuc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QLVB.Core.Implementation
{
    public class LinhvucManager : ILinhvucManager
    {
        #region Constructor

        private ILinhvucRepository _linhvucRepo;
        private ILogger _logger;

        public LinhvucManager(ILinhvucRepository linhvucRepo, ILogger logger)
        {
            _linhvucRepo = linhvucRepo;
            _logger = logger;
        }

        #endregion Constructor

        #region Interface Implementation

        public IEnumerable<ListLinhvucViewModel> GetListLinhvuc()
        {
            var linhvuc = _linhvucRepo.GetActiveLinhvucs
                .Select(p => new ListLinhvucViewModel
                {
                    intid = p.intid,
                    strghichu = p.strghichu,
                    strtenlinhvuc = p.strtenlinhvuc,
                    strkyhieu = p.strkyhieu
                    //intloai = (int)p.intloai : null
                });
            return linhvuc;
        }

        public EditLinhvucViewModel GetEditLinhvuc(int id)
        {
            EditLinhvucViewModel lv = _linhvucRepo.GetActiveLinhvucs
                    .Where(p => p.intid == id)
                    .Select(p => new EditLinhvucViewModel
                    {
                        intid = p.intid,
                        strtenlinhvuc = p.strtenlinhvuc,
                        strghichu = p.strghichu,
                        strkyhieu = p.strkyhieu
                    })
                    .FirstOrDefault();
            return lv;
        }

        public ResultFunction Save(EditLinhvucViewModel model)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                Linhvuc lv = new Linhvuc();
                lv.strtenlinhvuc = model.strtenlinhvuc;
                lv.strkyhieu = model.strkyhieu;
                lv.strghichu = model.strghichu;
                if (model.intid == 0)
                {   // them moi
                    _linhvucRepo.AddLinhvuc(lv);
                    _logger.Info("Thêm mới lĩnh vực: " + model.strtenlinhvuc);
                }
                else
                {   //cap nhat
                    _linhvucRepo.EditLinhvuc(model.intid, lv);
                    _logger.Info("Cập nhật lĩnh vực: " + model.strtenlinhvuc + ", id: " + model.intid.ToString());
                }
                kq.id = (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
                kq.message = ex.Message;
            }
            return kq;
        }

        public ResultFunction Delete(int id)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                _linhvucRepo.DeleteLinhvuc(id);
                _logger.Info("Xóa lĩnh vực id: " + id.ToString());
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