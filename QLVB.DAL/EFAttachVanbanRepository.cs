using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;

namespace QLVB.DAL
{
    public class EFAttachVanbanRepository : IAttachVanbanRepository
    {
        private QLVBDatabase context;

        public EFAttachVanbanRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<AttachVanban> AttachVanbans
        {
            get { return context.AttachVanbans.AsNoTracking(); }
        }

        public int Them(AttachVanban vb)
        {
            try
            {
                vb.inttrangthai = (int)enumAttachVanban.inttrangthai.IsActive;
                context.AttachVanbans.Add(vb);
                context.SaveChanges();
                return vb.intid;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// DeActive file trong database, chua xoa file vat ly
        /// </summary>
        /// <param name="idfile"></param>
        /// <returns>ten file</returns>
        public string Xoa(int idfile, int idcanbo)
        {
            try
            {
                var vb = context.AttachVanbans.FirstOrDefault(p => p.intid == idfile);
                vb.intidnguoixoa = idcanbo;
                vb.strngayxoa = DateTime.Now;
                vb.inttrangthai = (int)enumAttachVanban.inttrangthai.NotActive;
                context.SaveChanges();
                return vb.strmota;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
