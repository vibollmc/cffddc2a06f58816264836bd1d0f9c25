using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DAL.Abstract
{
    public interface IStoreHosovanbanlienquanRepository
    {
        IQueryable<Hosovanbanlienquan> Hosovanbanlienquans { get; }

        int Them(Hosovanbanlienquan hoso);

        void Sua(int intid, Hosovanbanlienquan hoso);

        void Xoa(int intid);
    }
}
