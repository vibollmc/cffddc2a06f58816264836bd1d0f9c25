using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.Domain.Abstract;

namespace QLVB.DAL
{
    public class EFTinhtrangxulyRepository : ITinhtrangxulyRepository
    {
        private QLVBDatabase context;

        public EFTinhtrangxulyRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Tinhtrangxuly> Tinhtrangxulys
        {
            get { return context.Tinhtrangxulys.AsNoTracking(); }
        }

    }
}
