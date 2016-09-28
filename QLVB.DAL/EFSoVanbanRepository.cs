using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFSoVanbanRepository : ISoVanbanRepository
    {
        private QLVBDatabase context;

        public EFSoVanbanRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<SoVanban> GetActiveSoVanbans
        {
            get
            {
                return context.SoVanbans
                  .Where(p => p.inttrangthai == (int)enumSovanban.inttrangthai.IsActive);
            }
        }

        public IQueryable<SoVanban> GetAllSoVanbans
        {
            get { return context.SoVanbans; }
        }

        public void Themmoi(SoVanban sovb)
        {
            sovb.inttrangthai = (int)enumSovanban.inttrangthai.IsActive;

            context.SoVanbans.Add(sovb);
            context.SaveChanges();
        }

        public void Capnhat(int intid, SoVanban sovb)
        {
            SoVanban svb = context.SoVanbans.Find(intid);
            svb.intloai = sovb.intloai;
            svb.intorder = sovb.intorder;
            svb.strten = sovb.strten;
            svb.strkyhieu = sovb.strkyhieu;
            svb.strghichu = sovb.strghichu;
            svb.IsDefault = sovb.IsDefault;
            context.SaveChanges();
        }

        public void Xoa(int intid)
        {
            SoVanban svb = context.SoVanbans.Find(intid);
            svb.inttrangthai = (int)enumSovanban.inttrangthai.NotActive;
            context.SaveChanges();
        }

        public void CapnhatKhoiph(int intid, int intidkhoiph)
        {
            SoVanban svb = context.SoVanbans.FirstOrDefault(p => p.intid == intid);
            if (svb.intloai == (int)enumSovanban.intloai.Vanbanden)
            {
                svb.intidkhoiph = intidkhoiph;
                context.SaveChanges();
            }
        }

        public void CapnhatLoaivb(int intid, int intidloaivb)
        {
            SoVanban svb = context.SoVanbans.FirstOrDefault(p => p.intid == intid);
            if (svb.intloai == (int)enumSovanban.intloai.Vanbanphathanh)
            {
                svb.intidloaivb = intidloaivb;
                context.SaveChanges();
            }
        }

        public void UpdateIsDefault(Int32 intloai)
        {
            var sovb = context.SoVanbans.Where(p => p.intloai == intloai);
            foreach (var p in sovb)
            {
                p.IsDefault = false;
            }
            context.SaveChanges();
        }
    }
}
