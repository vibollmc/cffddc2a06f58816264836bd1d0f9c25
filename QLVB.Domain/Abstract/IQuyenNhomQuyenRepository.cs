using QLVB.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface IQuyenNhomQuyenRepository
    {
        IQueryable<QuyenNhomQuyen> QuyenNhomQuyens { get; }

        void _DeleteRoleGroup(QuyenNhomQuyen group);

        void _DeleteRoleGroup(int intidnhomquyen);

        void _AddRoleGroup(QuyenNhomQuyen group);

        void _AddRoleGroup(List<int> lstidquyen, int idgroup);

        void SaveChanges();
    }
}