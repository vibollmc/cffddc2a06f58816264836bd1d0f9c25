using QLVB.Domain.Entities;
using System;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface IKhoiphathanhRepository
    {
        IQueryable<Khoiphathanh> GetActiveKhoiphathanhs { get; }

        IQueryable<Khoiphathanh> GetAllKhoiphathanhs { get; }

        void AddKhoi(Khoiphathanh khoi);

        void EditKhoi(Int32 intid, Khoiphathanh khoi);

        /// <summary>
        /// cap nhat trang thai notactive
        /// </summary>
        /// <param name="intid"></param>
        void DeleteKhoi(Int32 intid);
    }
}