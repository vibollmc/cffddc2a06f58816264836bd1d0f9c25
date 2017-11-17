using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace QLVB.DAL
{
   public class EFTinhhinhXulyVanBanDiRepository:ITinhhinhXulyVanBanDiReponsitory
    {
        private QLVBDatabase context;

        public EFTinhhinhXulyVanBanDiRepository(QLVBDatabase _context)
        {
            context = _context;
        }
        public IQueryable<TinhhinhXulyVanBanDi> TinhhinhXulyVanBanDis
        {
            get { return context.TinhhinhXulyVanBanDis; }
        }
        public int Them(TinhhinhXulyVanBanDi xuly)
        {

            context.TinhhinhXulyVanBanDis.Add(xuly);
            context.SaveChanges();
            return xuly.intid;
        }

    }
}
