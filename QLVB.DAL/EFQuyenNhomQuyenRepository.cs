using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.Domain.Abstract;

namespace QLVB.DAL
{
    public class EFQuyenNhomQuyenRepository : IQuyenNhomQuyenRepository
    {
        private QLVBDatabase context;

        public EFQuyenNhomQuyenRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<QuyenNhomQuyen> QuyenNhomQuyens
        {
            get { return context.QuyenNhomQuyens; }
        }

        public void _DeleteRoleGroup(QuyenNhomQuyen group)
        {
            context.QuyenNhomQuyens.Remove(group);
            context.SaveChanges();
        }

        public void _DeleteRoleGroup(int intidnhomquyen)
        {
            var groups = context.QuyenNhomQuyens.Where(p => p.intidnhomquyen == intidnhomquyen);
            foreach (var g in groups)
            {
                context.QuyenNhomQuyens.Remove(g);
            }
            context.SaveChanges();
        }

        public void _AddRoleGroup(QuyenNhomQuyen group)
        {
            context.QuyenNhomQuyens.Add(group);
            //context.SaveChanges();
        }

        public void _AddRoleGroup(List<int> lstidquyen, int idgroup)
        {
            foreach (int p in lstidquyen)
            {
                var addquyen = new QuyenNhomQuyen
                {
                    intidquyen = p,
                    intidnhomquyen = idgroup
                };
                context.QuyenNhomQuyens.Add(addquyen);
            }
            context.SaveChanges();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
