using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.Domain.Abstract;

namespace QLVB.DAL
{
    public class EFQuytrinhVersionRepository : IQuytrinhVersionRepository
    {
        private QLVBDatabase context;

        public EFQuytrinhVersionRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<QuytrinhVersion> QuytrinhVersions
        {
            get { return context.QuytrinhVersions; }
        }

        public int Them(QuytrinhVersion qt)
        {
            try
            {
                context.QuytrinhVersions.Add(qt);
                context.SaveChanges();
                return qt.intid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Xoa(int idquytrinh, DateTime ngayapdung)
        {
            try
            {
                var qt = context.QuytrinhVersions
                    .Where(p => p.intidquytrinh == idquytrinh)
                    .Where(p => p.strNgayApdung == ngayapdung)
                    .ToList();
                foreach (var q in qt)
                {
                    context.QuytrinhVersions.Remove(q);
                }
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
