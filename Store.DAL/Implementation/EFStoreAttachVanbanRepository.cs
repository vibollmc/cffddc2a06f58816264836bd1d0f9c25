using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.DAL.Abstract;
using QLVB.Domain.Entities;

namespace Store.DAL.Implementation
{
    public class EFStoreAttachVanbanRepository : IStoreAttachVanbanRepository
    {
        private QLVBStoreDatabase context;

        public EFStoreAttachVanbanRepository(QLVBStoreDatabase _context)
        {
            context = _context;
        }

        public IQueryable<AttachVanban> AttachVanbans
        {
            get { return context.AttachVanbans.AsNoTracking(); }
        }

        public int Them(AttachVanban vb)
        {
            vb.inttrangthai = (int)enumAttachVanban.inttrangthai.IsActive;
            context.AttachVanbans.Add(vb);
            context.SaveChanges();
            return vb.intid;
        }
        /// <summary>
        /// DeActive file trong database, chua xoa file vat ly
        /// </summary>
        /// <param name="idfile"></param>
        /// <returns>ten file</returns>
        public string Xoa(int idfile, int idcanbo)
        {
            var vb = context.AttachVanbans.FirstOrDefault(p => p.intid == idfile);
            vb.intidnguoixoa = idcanbo;
            vb.strngayxoa = DateTime.Now;
            vb.inttrangthai = (int)enumAttachVanban.inttrangthai.NotActive;
            context.SaveChanges();
            return vb.strmota;
        }
    }
}
