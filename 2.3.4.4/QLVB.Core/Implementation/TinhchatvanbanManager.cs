using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Core.Contract;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Tinhchatvb;
using QLVB.Common.Logging;
using QLVB.Common.Utilities;

namespace QLVB.Core.Implementation
{
    public class TinhchatvanbanManager : ITinhchatvanbanManager
    {
        #region Constructor

        private ILogger _logger;
        private ITinhchatvanbanRepository _tinhchatvbRepo;

        public TinhchatvanbanManager(ITinhchatvanbanRepository tinhchatvbRepo, ILogger logger)
        {
            _tinhchatvbRepo = tinhchatvbRepo;
            _logger = logger;
        }

        #endregion Constructor

        #region Interface Implementation

        public IEnumerable<Tinhchatvanban> GetTinhchatvb(int intloai)
        {
            var vb = _tinhchatvbRepo.GetActiveTinhchatvanbans.Where(p => p.intloai == intloai);
            return vb;
        }

        public EditTinhchatViewModel GetEdit(int id)
        {
            var vb = _tinhchatvbRepo.GetActiveTinhchatvanbans
                .Where(p => p.intid == id)
                .Select(p => new EditTinhchatViewModel
                {
                    intid = p.intid,
                    strtentinhchatvb = p.strtentinhchatvb,
                    strmota = p.strmota,
                    strkyhieu = p.strkyhieu,
                    intloai = p.intloai
                })
                .FirstOrDefault();
            return vb;
        }

        public ResultFunction Save(EditTinhchatViewModel model)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                Tinhchatvanban vb = new Tinhchatvanban();
                vb.intloai = model.intloai;
                vb.strkyhieu = model.strkyhieu;
                vb.strmota = model.strmota;
                vb.strtentinhchatvb = model.strtentinhchatvb;
                if (model.intid == 0)
                {
                    // them moi
                    _tinhchatvbRepo.AddTinhchatvb(vb);
                    _logger.Info("Thêm mới tính chất văn bản: " + model.strtentinhchatvb);
                }
                else
                {
                    // cap nhat
                    _tinhchatvbRepo.EditTinhchatvb(model.intid, vb);
                    _logger.Info("Cập nhật tính chất văn bản: " + model.strtentinhchatvb + ",id: " + model.intid.ToString());

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
        public ResultFunction Delete(EditTinhchatViewModel model)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                _tinhchatvbRepo.DeleteTinhchatvb(model.intid);
                _logger.Info("Xóa tính chất văn bản: " + model.strtentinhchatvb + ",id: " + model.intid.ToString());
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
