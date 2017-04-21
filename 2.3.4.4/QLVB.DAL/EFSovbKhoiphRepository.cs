using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFSovbKhoiphRepository : ISovbKhoiphRepository
    {
        private QLVBDatabase context;

        public EFSovbKhoiphRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<SovbKhoiph> SovbKhoiphs
        {
            get { return context.SovbKhoiphs; }
        }

        public void Them(SovbKhoiph sovb)
        {
            context.SovbKhoiphs.Add(sovb);
            context.SaveChanges();
        }

        public void Xoa(int intid)
        {
            var sovb = context.SovbKhoiphs.FirstOrDefault(p => p.intid == intid);
            context.SovbKhoiphs.Remove(sovb);
            context.SaveChanges();
        }
    }
}
