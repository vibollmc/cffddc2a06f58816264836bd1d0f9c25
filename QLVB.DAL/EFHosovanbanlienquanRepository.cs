using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFHosovanbanlienquanRepository : IHosovanbanlienquanRepository
    {
        private QLVBDatabase context;

        public EFHosovanbanlienquanRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Hosovanbanlienquan> Hosovanbanlienquans
        {
            get { return context.Hosovanbanlienquans; }
        }

        public int Them(Hosovanbanlienquan hs)
        {
            // cac truong mac dinh khi them moi
            hs.inttrangthai = (int)enumHosovanbanlienquan.inttrangthai.Dangxuly;

            context.Hosovanbanlienquans.Add(hs);
            context.SaveChanges();
            return hs.intid;
        }

        public void Sua(int intid, Hosovanbanlienquan hs)
        {
            //var _hs = context.Hosovanbans.SingleOrDefault(p => p.intid == intid);
        }

        public void Xoa(int intid)
        {
            var hs = context.Hosovanbanlienquans.FirstOrDefault(p => p.intid == intid);
            context.Hosovanbanlienquans.Remove(hs);
            context.SaveChanges();
        }
    }
}
