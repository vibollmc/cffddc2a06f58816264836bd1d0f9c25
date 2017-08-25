using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFVanbandenCanboRepository : IVanbandenCanboRepository
    {
        private QLVBDatabase context;

        public EFVanbandenCanboRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<VanbandenCanbo> VanbandenCanbos
        {
            get { return context.VanbandenCanbos; }
        }

        public void Them(int idvanban, int idcanbo)
        {
            try
            {
                VanbandenCanbo vb = new VanbandenCanbo();
                vb.intidcanbo = idcanbo;
                vb.intidvanban = idvanban;
                context.VanbandenCanbos.Add(vb);
                context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void Xoa(int idvanban, int idcanbo)
        {
            try
            {
                var vb = context.VanbandenCanbos.Where(p => p.intidvanban == idvanban)
                    .Where(p => p.intidcanbo == idcanbo).FirstOrDefault();

                context.VanbandenCanbos.Remove(vb);
                context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }


        }


    }
}
