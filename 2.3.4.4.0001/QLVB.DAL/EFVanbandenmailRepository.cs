using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFVanbandenmailRepository : IVanbandenmailRepository
    {
        private QLVBDatabase context;

        public EFVanbandenmailRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Vanbandenmail> Vanbandenmails
        {
            get { return context.Vanbandenmails.AsNoTracking(); }
        }

        public int Them(Vanbandenmail vb)
        {
            try
            {
                if (vb.intsoban == 0) vb.intsoban = null;
                if (vb.intsoto == 0) vb.intsoto = null;
                vb.strngaynhanvb = DateTime.Now;
                vb.inttrangthai = (int)enumVanbandenmail.inttrangthai.Chuacapnhat;
                if (vb.strngayky.HasValue)
                    vb.strngayky = vb.strngayky.Value.Date;
                context.Vanbandenmails.Add(vb);
                context.SaveChanges();
                return vb.intid;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void UpdateIntAttach(int id, enumVanbandenmail.intattach attach)
        {
            var vb = context.Vanbandenmails.FirstOrDefault(x => x.intid == id);
            if (vb != null)
            {
                vb.intattach = (int)attach;
                context.SaveChanges();
            }
        }


        public void Xoa(int intid)
        {
            try
            {
                var vb = context.Vanbandenmails.FirstOrDefault(p => p.intid == intid);
                context.Vanbandenmails.Remove(vb);
                context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void UpdateTrangthai(int intid)
        {
            try
            {
                var vb = context.Vanbandenmails.FirstOrDefault(p => p.intid == intid);
                vb.inttrangthai = (int)enumVanbandenmail.inttrangthai.Dacapnhat;
                context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}
