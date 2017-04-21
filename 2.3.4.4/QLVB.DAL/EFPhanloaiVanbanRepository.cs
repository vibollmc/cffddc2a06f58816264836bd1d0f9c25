using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFPhanloaiVanbanRepository : IPhanloaiVanbanRepository
    {
        private QLVBDatabase context;

        public EFPhanloaiVanbanRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<PhanloaiVanban> GetActivePhanloaiVanbans
        {
            get
            {
                return context.PhanloaiVanbans
                      .Where(p => p.inttrangthai == (int)enumPhanloaiVanban.inttrangthai.IsActive);
            }
        }

        public IQueryable<PhanloaiVanban> GetAllPhanloaiVanbans
        {
            get { return context.PhanloaiVanbans; }
        }
        public PhanloaiVanban GetLoaiVB(Int32 intid)
        {
            return context.PhanloaiVanbans.FirstOrDefault(p => p.intid == intid);
        }

        public int AddLoaiVB(PhanloaiVanban loaivb)
        {
            loaivb.inttrangthai = (int)enumPhanloaiVanban.inttrangthai.IsActive;

            context.PhanloaiVanbans.Add(loaivb);
            context.SaveChanges();
            return loaivb.intid;
        }

        public void EditLoaiVB(Int32 intid, PhanloaiVanban loaivb)
        {
            var _loaivb = context.PhanloaiVanbans.FirstOrDefault(p => p.intid == intid);
            _loaivb.inttrangthai = loaivb.inttrangthai;
            _loaivb.IsDefault = loaivb.IsDefault;
            _loaivb.strghichu = loaivb.strghichu;
            _loaivb.strkyhieu = loaivb.strkyhieu;
            _loaivb.strmavanban = loaivb.strmavanban;
            _loaivb.strtenvanban = loaivb.strtenvanban;
            context.SaveChanges();
        }

        public void DeleteLoaiVB(Int32 intid)
        {
            var _loaivb = context.PhanloaiVanbans.FirstOrDefault(p => p.intid == intid);
            _loaivb.inttrangthai = (int)enumPhanloaiVanban.inttrangthai.NotActive;
            context.SaveChanges();
        }

        public void UpdateIsDefault(Int32 intloai)
        {
            var loaivb = context.PhanloaiVanbans.Where(p => p.intloai == intloai);
            foreach (var p in loaivb)
            {
                p.IsDefault = false;
            }
            context.SaveChanges();
        }
    }
}
