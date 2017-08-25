using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;

namespace QLVB.DAL
{
    public class EFTonghopVanbanRepository:ITonghopVanbanRepository
    {
        private QLVBDatabase context;

        public EFTonghopVanbanRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<TonghopVanban> TonghopVanbans {
            get { return context.TonghopVanbans; }
        }
        public int Them(TonghopVanban vb)
        {
            try
            {
                context.TonghopVanbans.Add(vb);
                context.SaveChanges();
                return vb.intid;
            }
            catch ( Exception ex)
            {
                throw ex;
            }            
        }
    }
}
