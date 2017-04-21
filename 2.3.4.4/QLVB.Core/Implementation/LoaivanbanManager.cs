using QLVB.Common.Logging;
using QLVB.Core.Contract;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Loaivanban;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QLVB.Core.Implementation
{
    public class LoaivanbanManager : ILoaivanbanManager
    {
        #region Constructor

        private IPhanloaiVanbanRepository _loaivbRepo;
        private IPhanloaiTruongRepository _truongvbRepo;
        private IMotaTruongRepository _motatruongRepo;
        private ILogger _logger;

        public LoaivanbanManager(IPhanloaiVanbanRepository loaivbRepo, IPhanloaiTruongRepository truongvbRepo,
                                IMotaTruongRepository motatruongRepo, ILogger logger)
        {
            _loaivbRepo = loaivbRepo;
            _truongvbRepo = truongvbRepo;
            _motatruongRepo = motatruongRepo;
            _logger = logger;
        }

        #endregion Constructor

        #region Interface Implementation

        public IEnumerable<PhanloaiVanban> GetLoaivanban(int intloai)
        {
            var loaivb = _loaivbRepo.GetActivePhanloaiVanbans
                        .Where(p => p.intloai == intloai)
                        .OrderBy(p => p.strtenvanban)
                        .ThenBy(p => p.strmavanban);
            return loaivb;
        }

        public PhanloaiVanban GetIdLoaivanban(int id)
        {
            var loaivb = _loaivbRepo.GetActivePhanloaiVanbans
                    .FirstOrDefault(p => p.intid == id);
            return loaivb;
        }

        public ListTruongvbViewModel GetLoaitruongvanban(int idloaivb)
        {
            ListTruongvbViewModel loaitruong = new ListTruongvbViewModel
            {
                idloaivb = idloaivb,
                Phanloaitruong = _truongvbRepo.PhanloaiTruongs
                            .Where(p => p.intidphanloaivanban == idloaivb)
                            .OrderBy(p => p.intorder)
            };
            return loaitruong;
        }

        public int SaveLoaivanban(EditLoaivanbanViewModel model, int intloai)
        {
            try
            {
                PhanloaiVanban loaivb = new PhanloaiVanban
                {
                    intid = model.intid,
                    inttrangthai = (int)enumPhanloaiVanban.inttrangthai.IsActive,
                    intloai = intloai,//(int)enumPhanloaiVanban.intloai.vanbanden,
                    IsDefault = model.IsDefault,
                    strghichu = model.strghichu,
                    //strkyhieu =
                    strmavanban = model.strmavanban,
                    strtenvanban = model.strtenvanban
                };
                // kiem tra gia tri IsDefaut truoc khi cap nhat
                if (model.IsDefault == true)
                {
                    _loaivbRepo.UpdateIsDefault(intloai); //((int)enumPhanloaiVanban.intloai.vanbanden);
                }
                if (model.intid == 0)
                {   // them moi
                    try
                    {
                        var motatruong = _motatruongRepo.MotaTruongs.Where(p => p.intloai == intloai).ToList();
                        int idloaivb = _loaivbRepo.AddLoaiVB(loaivb);
                        foreach (var item in motatruong)
                        {
                            PhanloaiTruong truong = new PhanloaiTruong
                            {
                                intloai = intloai,
                                intidphanloaivanban = idloaivb,
                                intidmotatruong = item.intid,
                                IsDisplay = true,
                                intorder = item.intorder,
                                IsRequire = item.IsRequire,
                                strmota = item.strten,
                                intloaitruong = (int)enumPhanloaiTruong.intloaitruong.Default
                            };
                            _truongvbRepo.AddPhanloaiTruong(truong);
                        }
                        _logger.Info("Thêm mới loại văn bản: " + model.strtenvanban);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message);
                    }

                }
                else
                {   // cap nhat
                    _loaivbRepo.EditLoaiVB(model.intid, loaivb);
                    _logger.Info("Cập nhật loại văn bản: " + model.strtenvanban + ", id: " + model.intid);
                }
                return (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return (int)ResultViewModels.Error;
            }
        }

        public int DeleteLoaivanban(DeleteLoaivanbanViewModel model)
        {
            try
            {
                _loaivbRepo.DeleteLoaiVB(model.intid);
                _logger.Info("Xóa loại văn bản: " + model.strtenvanban + ", id: " + model.intid);
                return (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return (int)ResultViewModels.Error;
            }
        }

        public void EditPhanloaiTruong(int id, bool IsDisplay, int intorder)
        {
            try
            {
                _truongvbRepo.EditPhanloaiTruong(id, IsDisplay, intorder);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        #endregion Interface Implementation
    }
}