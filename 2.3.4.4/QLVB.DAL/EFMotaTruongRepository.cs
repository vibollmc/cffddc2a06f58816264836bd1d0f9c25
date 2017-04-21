using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFMotaTruongRepository : IMotaTruongRepository
    {
        private QLVBDatabase context;

        public EFMotaTruongRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<MotaTruong> MotaTruongs
        {
            get { return context.MotaTruongs; }
        }
    }
}
