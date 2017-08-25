using QLVB.Domain.Entities;
using System;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface INhomQuyenRepository
    {
        /// <summary>
        /// lay cac nhom quyen dang active
        /// </summary>
        IQueryable<NhomQuyen> GetActiveNhomQuyens { get; }
        /// <summary>
        /// lay tat ca cac nhom quyen
        /// </summary>
        IQueryable<NhomQuyen> GetAllNhomQuyens { get; }

        void SaveGroup(NhomQuyen group);

        void SaveGroup(string strtennhom);

        void EditGroup(Int32 intidgroup, string strtennhom);

        void DeleteGroup(Int32 intidgroup);

        string GetTenNhom(Int32 intidgroup);
    }
}