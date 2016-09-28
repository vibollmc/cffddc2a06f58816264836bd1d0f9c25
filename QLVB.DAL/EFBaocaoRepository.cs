using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;

namespace QLVB.DAL
{
    public class EFBaocaoRepository : IBaocaoRepository
    {
        private QLVBDatabase context;

        public EFBaocaoRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Baocao> Baocaos
        {
            get { return context.Baocaos.AsNoTracking(); }
        }
    }
}
