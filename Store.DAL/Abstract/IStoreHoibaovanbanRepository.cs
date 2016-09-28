using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DAL.Abstract
{
    public interface IStoreHoibaovanbanRepository
    {
        IQueryable<Hoibaovanban> Hoibaovanbans { get; }

        int Them(int intloai, int intTransID, int intRecID);

        void Xoa(int intloai, int intTransID, int intRecID);
    }
}
