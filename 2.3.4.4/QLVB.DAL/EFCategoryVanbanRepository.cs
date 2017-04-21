using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;

namespace QLVB.DAL
{
    public class EFCategoryVanbanRepository : ICategoryRepository
    {
        private QLVBDatabase context;

        public EFCategoryVanbanRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<CategoryVanban> CategoryVanbans
        {
            get { return context.CategoryVanban; }
        }
    }
}
