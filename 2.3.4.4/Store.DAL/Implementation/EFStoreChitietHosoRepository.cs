using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.DAL.Abstract;

namespace Store.DAL.Implementation
{
    public class EFStoreChitietHosoRepository : IStoreChitietHosoRepository
    {
        private QLVBStoreDatabase context;

        public EFStoreChitietHosoRepository(QLVBStoreDatabase _context)
        {
            context = _context;
        }

        public IQueryable<ChitietHoso> ChitietHosos
        {
            get { return context.ChitietHosos.AsNoTracking(); }
        }
    }
}
