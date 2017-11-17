using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
namespace QLVB.Domain.Abstract
{
    public interface ITinhhinhXulyVanBanDiReponsitory
    {
        IQueryable<TinhhinhXulyVanBanDi> TinhhinhXulyVanBanDis { get; }

        int Them(TinhhinhXulyVanBanDi xuly);
    }
}
