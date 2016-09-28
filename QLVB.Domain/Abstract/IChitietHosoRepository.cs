using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface IChitietHosoRepository
    {
        IQueryable<ChitietHoso> ChitietHosos { get; }

        int Them(ChitietHoso hs);

        // tam thoi khong duoc xoa. sua 
        //void Sua(int intid, Doituongxuly dt);

        //void Xoa(int intid);

    }
}
