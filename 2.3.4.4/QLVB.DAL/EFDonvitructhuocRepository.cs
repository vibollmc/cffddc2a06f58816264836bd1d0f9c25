using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFDonvitructhuocRepository : IDonvitructhuocRepository
    {
        private QLVBDatabase context;

        public EFDonvitructhuocRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Donvitructhuoc> Donvitructhuocs
        {
            get
            {
                return context.Donvitructhuocs
                      .Where(p => p.inttrangthai == (int)enumDonvitructhuoc.inttrangthai.IsActive)
                      .OrderBy(p => p.strtendonvi);
            }
        }

        public void SetTen(string strtendonvi, Int32 intid)
        {
            var donvi = context.Donvitructhuocs
                .FirstOrDefault(p => p.Id == intid);

            donvi.strtendonvi = strtendonvi;
            context.SaveChanges();
        }

        public void AddTen(string strtendonvi, Int32 ParentId)
        {
            Donvitructhuoc donvi = new Donvitructhuoc();
            donvi.strtendonvi = strtendonvi;
            donvi.ParentId = ParentId;
            donvi.inttrangthai = (int)enumDonvitructhuoc.inttrangthai.IsActive;
            // tang intlevel len dua vao intlevel cua parentid + 1
            var intlevelParent = context.Donvitructhuocs.FirstOrDefault(p => p.Id == ParentId).intlevel;
            donvi.intlevel = intlevelParent + 1;

            context.Donvitructhuocs.Add(donvi);
            context.SaveChanges();
        }

        public void NotActiveDonvi(Int32 intid)
        {
            Donvitructhuoc donvi = context.Donvitructhuocs.FirstOrDefault(p => p.Id == intid);
            //context.Donvitructhuocs.Remove(donvi);
            donvi.inttrangthai = (int)enumDonvitructhuoc.inttrangthai.NotActive;
            context.SaveChanges();
        }

        public void DeleteDonvi(int intid)
        {
            Donvitructhuoc donvi = context.Donvitructhuocs.FirstOrDefault(p => p.Id == intid);
            context.Donvitructhuocs.Remove(donvi);
            context.SaveChanges();
        }
    }
}
