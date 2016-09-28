using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.Domain.Abstract;

namespace QLVB.DAL
{
    public class EFTinhtrangQuytrinhRepository : ITinhtrangQuytrinhRepository
    {
        private QLVBDatabase context;

        public EFTinhtrangQuytrinhRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<TinhTrangQuytrinh> TinhtrangQuytrinhs
        {
            get { return context.TinhtrangQuytrinhs.AsNoTracking(); }
        }
    }
}
