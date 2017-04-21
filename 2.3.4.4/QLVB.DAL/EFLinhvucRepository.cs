using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFLinhvucRepository : ILinhvucRepository
    {
        private QLVBDatabase context;

        public EFLinhvucRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Linhvuc> GetActiveLinhvucs
        {
            get
            {
                return context.Linhvucs
                    .Where(p => p.inttrangthai == (int)enumLinhvuc.inttrangthai.IsActive);
            }
        }

        public IQueryable<Linhvuc> GetAllLinhvucs
        {
            get
            {
                return context.Linhvucs;
            }
        }

        public void AddLinhvuc(Linhvuc linhvuc)
        {
            linhvuc.inttrangthai = (int)enumLinhvuc.inttrangthai.IsActive;
            context.Linhvucs.Add(linhvuc);
            context.SaveChanges();
        }

        public void EditLinhvuc(Int32 intid, Linhvuc linhvuc)
        {
            Linhvuc lv = context.Linhvucs.FirstOrDefault(p => p.intid == intid);
            lv.strghichu = linhvuc.strghichu;
            lv.strkyhieu = linhvuc.strkyhieu;
            lv.strtenlinhvuc = linhvuc.strtenlinhvuc;

            lv.intloai = linhvuc.intloai;
            lv.strthoihanxuly = linhvuc.strthoihanxuly;

            context.SaveChanges();
        }

        public void DeleteLinhvuc(Int32 intid)
        {
            Linhvuc lv = context.Linhvucs.FirstOrDefault(p => p.intid == intid);
            lv.inttrangthai = (int)enumLinhvuc.inttrangthai.NotActive;
            //context.Linhvucs.Remove(lv);
            context.SaveChanges();
        }
    }
}
