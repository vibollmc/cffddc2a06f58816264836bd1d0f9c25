using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;

namespace QLVB.DAL
{
    public class EFUyQuyenRepository : IUyQuyenRepository
    {
        private QLVBDatabase context;

        public EFUyQuyenRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<UyQuyen> UyQuyens
        {
            get { return context.UyQuyens.AsNoTracking(); }
        }

        public int Them(UyQuyen u)
        {
            context.UyQuyens.Add(u);
            context.SaveChanges();
            return u.intid;
        }

        public int Them(int idUserSend, int idUserRec)
        {
            UyQuyen u = new UyQuyen();
            u.intPersRec = idUserRec;
            u.intPersSend = idUserSend;
            context.UyQuyens.Add(u);
            context.SaveChanges();
            return u.intid;
        }

        public void Xoa(int idUserSend)
        {
            var u = context.UyQuyens.Where(p => p.intPersSend == idUserSend);
            foreach (var p in u)
            {
                context.UyQuyens.Remove(p);
            }
            context.SaveChanges();
        }

        public void Xoa(int idUserSend, int idUserRec)
        {
            var u = context.UyQuyens.Where(p => p.intPersSend == idUserSend)
                    .Where(p => p.intPersRec == idUserRec);
            foreach (var p in u)
            {
                context.UyQuyens.Remove(p);
            }
            context.SaveChanges();
        }
    }
}
