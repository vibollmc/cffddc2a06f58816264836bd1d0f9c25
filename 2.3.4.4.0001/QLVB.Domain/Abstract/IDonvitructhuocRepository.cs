using QLVB.Domain.Entities;
using System;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface IDonvitructhuocRepository
    {
        /// <summary>
        /// lay cac don vi dang active
        /// </summary>
        IQueryable<Donvitructhuoc> Donvitructhuocs { get; }

        void SetTen(string strtendonvi, Int32 intid);

        void AddTen(string strtendonvi, Int32 ParentId);

        /// <summary>
        /// khong delete thuc, chi doi inttrangthai: notactive
        /// </summary>
        /// <param name="intid"></param>
        void NotActiveDonvi(Int32 intid);
        /// <summary>
        /// delete thuc su trong database
        /// </summary>
        /// <param name="intid"></param>
        void DeleteDonvi(int intid);
    }
}