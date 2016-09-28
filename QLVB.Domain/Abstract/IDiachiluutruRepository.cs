using QLVB.Domain.Entities;
using System;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface IDiachiluutruRepository
    {
        IQueryable<Diachiluutru> GetActiveDiachiluutrus { get; }

        IQueryable<Diachiluutru> GetAllDiachiluutrus { get; }

        void SetTen(string strtendonvi, Int32 intid);

        void AddTen(string strtendonvi, Int32 ParentId);

        /// <summary>
        /// khong xoa, cap nhat trang thai notactive
        /// </summary>
        /// <param name="intid"></param>
        void DeleteDonvi(Int32 intid);
    }
}