using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.Domain.Abstract;

namespace QLVB.DAL
{
    public class EFChitietHosoRepository : IChitietHosoRepository
    {
        private QLVBDatabase context;

        public EFChitietHosoRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<ChitietHoso> ChitietHosos
        {
            get { return context.ChitietHosos; }
        }

        public int Them(ChitietHoso hs)
        {
            try
            {
                hs.strngaytao = DateTime.Now;
                context.ChitietHosos.Add(hs);
                context.SaveChanges();
                return hs.intid;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
