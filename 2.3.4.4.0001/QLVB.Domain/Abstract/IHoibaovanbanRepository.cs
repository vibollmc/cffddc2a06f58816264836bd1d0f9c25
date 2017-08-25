using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface IHoibaovanbanRepository
    {
        IQueryable<Hoibaovanban> Hoibaovanbans { get; }

        int Them(int intloai, int intTransID, int intRecID);

        void Xoa(int intloai, int intTransID, int intRecID);

    }
}
