using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFVanbandiCanboRepository : IVanbandiCanboRepository
    {
        private QLVBDatabase context;

        public EFVanbandiCanboRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<VanbandiCanbo> VanbandiCanbos
        {
            get { return context.VanbandiCanbos; }
        }

        public void Them(int idvanban, int idcanbo)
        {
            try
            {
                VanbandiCanbo vb = new VanbandiCanbo();
                vb.intidcanbo = idcanbo;
                vb.intidvanban = idvanban;
                context.VanbandiCanbos.Add(vb);
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
                var vb = context.VanbandiCanbos.Where(p => p.intidvanban == idvanban)
                   .Where(p => p.intidcanbo == idcanbo).FirstOrDefault();
                if (vb != null)
                {
                    context.VanbandiCanbos.Remove(vb);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }


        }


    }
}
