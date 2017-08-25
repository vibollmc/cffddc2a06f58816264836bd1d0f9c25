using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface IHosoQuytrinhRepository
    {
        IQueryable<HosoQuytrinh> HosoQuytrinhs { get; }
        HosoQuytrinh GetHosoQuytrinhByID(int id);
        int Them(HosoQuytrinh hoso);

        int Sua();

        int CapnhatTrangthai(int idhoso, int inttrangthai);

    }
}
