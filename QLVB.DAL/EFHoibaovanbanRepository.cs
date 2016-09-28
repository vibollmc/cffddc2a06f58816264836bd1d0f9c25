using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFHoibaovanbanRepository : IHoibaovanbanRepository
    {
        private QLVBDatabase context;

        public EFHoibaovanbanRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Hoibaovanban> Hoibaovanbans
        {
            get { return context.Hoibaovanbans; }
        }

        public int Them(int intloai, int intTransID, int intRecID)
        {
            Hoibaovanban hs = new Hoibaovanban();
            hs.intloai = intloai;
            hs.intTransID = intTransID;
            hs.intRecID = intRecID;
            context.Hoibaovanbans.Add(hs);
            context.SaveChanges();
            return hs.intid;
        }

        public void Xoa(int intloai, int intTransID, int intRecID)
        {
            var hs = context.Hoibaovanbans.Where(p => p.intloai == intloai)
                    .Where(p => p.intTransID == intTransID)
                    .Where(p => p.intRecID == intRecID)
                    .FirstOrDefault();
            context.Hoibaovanbans.Remove(hs);
            context.SaveChanges();
        }
    }
}
