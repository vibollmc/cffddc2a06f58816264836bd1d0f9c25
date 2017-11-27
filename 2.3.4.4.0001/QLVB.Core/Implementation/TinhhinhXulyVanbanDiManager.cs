using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.Core.Contract;
using QLVB.Common.Logging;
using QLVB.Common.Crypt;
using QLVB.Common.Utilities;
using QLVB.Common.Sessions;
using QLVB.DTO.TinhhinhXulyVanbanDi;
using QLVB.DTO;

namespace QLVB.Core.Implementation
{
    public class TinhhinhXulyVanbanDiManager : ITinhhinhXulyVanbanDiManager
    {
        #region Constructor
        private ITinhhinhXulyVanBanDiReponsitory _tinhhinhxlRepo;
        #endregion Constructor
        public TinhhinhXulyVanbanDiManager(ITinhhinhXulyVanBanDiReponsitory tinhhinhxlRepo)
        {
            _tinhhinhxlRepo = tinhhinhxlRepo;
        }
        public IEnumerable<TinhhinhXulyVanbanDiViewModel> GetTinhhinhXulyVanbanDi()
        {
            return null;
        }
        public ResultFunction InsertTinhhinhXulyVanbanDi(TinhhinhXulyVanbanDiViewModel data)
        {
            var addObject = new TinhhinhXulyVanBanDi
            {
                intid = data.intid,
                intidguivanban = data.intidguivanban,
                strdiengiai = data.strdiengiai,
                strmaxuly = data.strmaxuly,
                strngayxuly = data.strngayxuly,
                strnguoixuly = data.strnguoixuly,
                strphongban= data.strphongban
            };

            _tinhhinhxlRepo.Them(addObject);

            return new ResultFunction
            {
                id = addObject.intid
            };
        }

        IEnumerable<TinhhinhXulyVanBanDi> ITinhhinhXulyVanbanDiManager.GetTinhhinhXulyVanbanDi(int idguivanban)
        {
            return _tinhhinhxlRepo.TinhhinhXulyVanBanDis.Where(x => x.intidguivanban == idguivanban).OrderBy(x=>x.strngayxuly);
        }
    }
}
