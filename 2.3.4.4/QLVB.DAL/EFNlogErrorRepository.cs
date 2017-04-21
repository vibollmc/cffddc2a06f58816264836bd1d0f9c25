using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFNlogErrorRepository : INlogErrorRepository
    {
        private QLVBDatabase context;

        public EFNlogErrorRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<NLogError> Nlogger
        {
            get { return context.NlogErrors; }
        }
    }
}
