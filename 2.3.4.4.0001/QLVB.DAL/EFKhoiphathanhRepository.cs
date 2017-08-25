using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFKhoiphathanhRepository : IKhoiphathanhRepository
    {
        private QLVBDatabase context;

        public EFKhoiphathanhRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Khoiphathanh> GetActiveKhoiphathanhs
        {
            get
            {
                return context.Khoiphathanhs
                    .Where(p => p.inttrangthai == (int)enumKhoiphathanh.inttrangthai.IsActive);
            }
        }

        public IQueryable<Khoiphathanh> GetAllKhoiphathanhs
        {
            get { return context.Khoiphathanhs.AsNoTracking(); }
        }

        public void AddKhoi(Khoiphathanh khoi)
        {
            khoi.inttrangthai = (int)enumKhoiphathanh.inttrangthai.IsActive;
            context.Khoiphathanhs.Add(khoi);
            context.SaveChanges();
        }

        public void EditKhoi(Int32 intid, Khoiphathanh khoi)
        {
            Khoiphathanh _khoiph = context.Khoiphathanhs.FirstOrDefault(p => p.intid == intid);
            _khoiph.IsDefault = khoi.IsDefault;
            _khoiph.strkyhieu = khoi.strkyhieu;
            _khoiph.strtenkhoi = khoi.strtenkhoi;
            context.SaveChanges();
        }

        public void DeleteKhoi(Int32 intid)
        {
            Khoiphathanh khoiph = context.Khoiphathanhs.FirstOrDefault(p => p.intid == intid);
            khoiph.inttrangthai = (int)enumKhoiphathanh.inttrangthai.NotActive;
            context.SaveChanges();
        }
    }
}
