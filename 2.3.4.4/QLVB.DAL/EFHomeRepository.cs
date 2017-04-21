using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;

namespace QLVB.DAL
{
    public class EFHomeRepository : IHomeRepository
    {
        private QLVBDatabase context;

        public EFHomeRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Home> Homes
        {
            get { return context.Homes; }
        }
    }
}
