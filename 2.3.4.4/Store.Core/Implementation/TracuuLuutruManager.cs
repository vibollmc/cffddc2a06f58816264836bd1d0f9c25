using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Core.Contract;
using QLVB.DTO.TracuuLuutru;
using QLVB.Common.Logging;
using QLVB.Common.Sessions;
using Store.DAL.Abstract;
using QLVB.Domain.Entities;
using QLVB.Common.Date;
using QLVB.Domain.Abstract;
using QLVB.DTO;

namespace Store.Core.Implementation
{
    public class TracuuLuutruManager : ITracuuLuutruManager
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
        private IStoreLuutruVanbanRepository _luutruRepo;

        private ICanboRepository _canboRepo;

        public TracuuLuutruManager(IStoreVanbandiRepository vanbandiRepo, IStoreFileManager fileManager,
                IStoreAttachVanbanRepository fileRepo, IStoreHosovanbanlienquanRepository hsvblqRepo,
                IStoreVanbandenRepository vanbandenRepo, IStoreHosovanbanRepository hosovanbanRepo,
                IStoreHosocongviecRepository hosocongviecRepo, IStoreHoibaovanbanRepository hoibaovanbanRepo,
                IStoreVanbandiCanboRepository vanbandicanboRepo,
                ILogger logger, ISessionServices session,
                IStoreLuutruVanbanRepository luutruRepo,
                ICanboRepository canboRepo
                )
        {
            _vanbandiRepo = vanbandiRepo;
            _fileManager = fileManager;
            _fileRepo = fileRepo;
            _logger = logger;
            _vanbandenRepo = vanbandenRepo;
            _hosovanbanRepo = hosovanbanRepo;
            _hosocongviecRepo = hosocongviecRepo;
            _hoibaovanbanRepo = hoibaovanbanRepo;
            _vanbandicanboRepo = vanbandicanboRepo;
            _session = session;
            _hsvblqRepo = hsvblqRepo;
            _luutruRepo = luutruRepo;
            _canboRepo = canboRepo;
        }
        #endregion Constructor
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
                var lt = _luutruRepo.LuutruVanbans
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
                    _luutruRepo.Them(luutru);
                    return luutru.intid;
                }
                else
                {
                    _luutruRepo.Sua(luutru.intid, luutru);
                    return luutru.intid;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return (int)ResultViewModels.Error;
            }
        }
    }
}
