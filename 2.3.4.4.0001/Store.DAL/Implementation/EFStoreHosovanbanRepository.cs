using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.DAL.Abstract;
using QLVB.Domain.Entities;

namespace Store.DAL.Implementation
{
    public class EFStoreHosovanbanRepository : IStoreHosovanbanRepository
    {
        private QLVBStoreDatabase context;

        public EFStoreHosovanbanRepository(QLVBStoreDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Hosovanban> Hosovanbans
        {
            get { return context.Hosovanbans; }
        }

        public int Them(Hosovanban hs)
        {
            // cac truong mac dinh khi them moi
            hs.inttrangthai = (int)enumHosovanban.inttrangthai.Dangxuly;

            context.Hosovanbans.Add(hs);
            context.SaveChanges();
            return hs.intid;
        }

        public void Sua(int intid, Hosovanban hs)
        {
            //var _hs = context.Hosovanbans.SingleOrDefault(p => p.intid == intid);
        }

        public void Xoa(int intid)
        {
            var hs = context.Hosovanbans.FirstOrDefault(p => p.intid == intid);
            context.Hosovanbans.Remove(hs);
            context.SaveChanges();
        }

        public int Xoa(int idvanban, int intloai)
        {
            var hs = context.Hosovanbans.Where(p => p.intidvanban == idvanban)
                .Where(p => p.intloai == intloai)
                .FirstOrDefault();
            int idhoso = hs.intidhosocongviec;
            context.Hosovanbans.Remove(hs);
            context.SaveChanges();
            return idhoso;
        }
    }
}
