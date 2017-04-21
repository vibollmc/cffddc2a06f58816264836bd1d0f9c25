using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.DAL.Abstract;
using QLVB.Domain.Entities;

namespace Store.DAL.Implementation
{
    public class EFStoreVanbandiCanboRepository : IStoreVanbandiCanboRepository
    {
        private QLVBStoreDatabase context;

        public EFStoreVanbandiCanboRepository(QLVBStoreDatabase _context)
        {
            context = _context;
        }

        public IQueryable<VanbandiCanbo> VanbandiCanbos
        {
            get { return context.VanbandiCanbos; }
        }

        public void Them(int idvanban, int idcanbo)
        {
            VanbandiCanbo vb = new VanbandiCanbo();
            vb.intidcanbo = idcanbo;
            vb.intidvanban = idvanban;
            context.VanbandiCanbos.Add(vb);
            context.SaveChanges();
        }

        public void Xoa(int idvanban, int idcanbo)
        {
            var vb = context.VanbandiCanbos.Where(p => p.intidvanban == idvanban)
                    .Where(p => p.intidcanbo == idcanbo).FirstOrDefault();
            if (vb != null)
            {
                context.VanbandiCanbos.Remove(vb);
                context.SaveChanges();
            }
        }
    }
}
