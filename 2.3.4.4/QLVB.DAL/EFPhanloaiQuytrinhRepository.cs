using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.Domain.Abstract;

namespace QLVB.DAL
{
    public class EFPhanloaiQuytrinhRepository : IPhanloaiQuytrinhRepository
    {
        private QLVBDatabase context;

        public EFPhanloaiQuytrinhRepository(QLVBDatabase _context)
        {
            context = _context;
        }
        public IQueryable<PhanloaiQuytrinh> PhanloaiQuytrinhs
        {
            get
            {
                return context.PhanloaiQuytrinhs
                    .Where(p => p.inttrangthai == (int)enumPhanloaiQuytrinh.inttrangthai.IsActive);
            }
        }

        public int Them(PhanloaiQuytrinh hs)
        {
            hs.inttrangthai = (int)enumPhanloaiQuytrinh.inttrangthai.IsActive;
            context.PhanloaiQuytrinhs.Add(hs);
            context.SaveChanges();
            return hs.intid;
        }

        public int Them(string strtenloaiquytrinh)
        {
            if (!string.IsNullOrEmpty(strtenloaiquytrinh))
            {
                PhanloaiQuytrinh hs = new PhanloaiQuytrinh
                {
                    strtenloai = strtenloaiquytrinh
                };
                return Them(hs);
            }
            else
            {
                return 0;
            }
        }
        public int Sua(int intid, string strten)
        {
            var hs = context.PhanloaiQuytrinhs.FirstOrDefault(p => p.intid == intid);
            hs.strtenloai = strten;
            context.SaveChanges();
            return hs.intid;
        }

        public void Xoa(int intid)
        {
            var hs = context.PhanloaiQuytrinhs.FirstOrDefault(p => p.intid == intid);
            hs.inttrangthai = (int)enumPhanloaiQuytrinh.inttrangthai.NotActive;
            context.SaveChanges();
        }

    }
}
