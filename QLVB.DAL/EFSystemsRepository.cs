using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFSystemsRepository : ISystems
    {
        private QLVBDatabase context;

        public EFSystemsRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Systems> Systems
        {
            get { return context.Systems; }
        }
    }
}
