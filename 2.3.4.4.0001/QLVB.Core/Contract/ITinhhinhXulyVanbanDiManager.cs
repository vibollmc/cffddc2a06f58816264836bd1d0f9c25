using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.DTO.TinhhinhXulyVanbanDi;
using QLVB.DTO;

namespace QLVB.Core.Contract
{
    public interface ITinhhinhXulyVanbanDiManager
    {
        IEnumerable<TinhhinhXulyVanBanDi> GetTinhhinhXulyVanbanDi(int idguivanban);
        ResultFunction InsertTinhhinhXulyVanbanDi(TinhhinhXulyVanbanDiViewModel data);
    }
}
