using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.Domain.Abstract;

namespace QLVB.DAL
{
    public class EFQuytrinhXulyRepository : IQuytrinhXulyRepository
    {
        private QLVBDatabase context;

        public EFQuytrinhXulyRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<QuytrinhXuly> QuytrinhXulys
        {
            get { return context.QuytrinhXulys; }
        }

        public int Them(QuytrinhXuly xuly)
        {
            context.QuytrinhXulys.Add(xuly);
            context.SaveChanges();
            return xuly.intid;
        }

        public int Xoa(int intidNode)
        {
            try
            {
                var xuly = context.QuytrinhXulys.FirstOrDefault(p => p.intidNode == intidNode);
                context.QuytrinhXulys.Remove(xuly);
                context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
    }
}
