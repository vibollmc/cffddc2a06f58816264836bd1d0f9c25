using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.DAL.Abstract;

namespace Store.DAL.Implementation
{
    public class EFStoreVanbandenCanboRepository : IStoreVanbandenCanboRepository
    {
        private QLVBStoreDatabase context;

        public EFStoreVanbandenCanboRepository(QLVBStoreDatabase _context)
        {
            context = _context;
        }

        public IQueryable<VanbandenCanbo> VanbandenCanbos
        {
            get { return context.VanbandenCanbos; }
        }

        public void Them(int idvanban, int idcanbo)
        {
            VanbandenCanbo vb = new VanbandenCanbo();
            vb.intidcanbo = idcanbo;
            vb.intidvanban = idvanban;
            context.VanbandenCanbos.Add(vb);
            context.SaveChanges();
        }

        public void Xoa(int idvanban, int idcanbo)
        {
            var vb = context.VanbandenCanbos.Where(p => p.intidvanban == idvanban)
                    .Where(p => p.intidcanbo == idcanbo).FirstOrDefault();

            context.VanbandenCanbos.Remove(vb);
            context.SaveChanges();

        }
    }
}
