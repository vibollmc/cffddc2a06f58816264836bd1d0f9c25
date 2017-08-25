using QLVB.Domain.Entities;
using System;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface IChucdanhRepository
    {
        IQueryable<Chucdanh> Chucdanhs { get; }

        Chucdanh GetChucdanh(Int32 intid);

        void AddChucdanh(Chucdanh chucdanh);

        void EditChucdanh(Int32 intid, Chucdanh chucdanh);

        void DeleteChucdanh(Int32 intid);
    }
}