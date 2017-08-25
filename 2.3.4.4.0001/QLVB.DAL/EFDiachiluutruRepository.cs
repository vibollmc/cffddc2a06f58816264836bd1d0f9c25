using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFDiachiluutruRepository : IDiachiluutruRepository
    {
        private QLVBDatabase context;

        public EFDiachiluutruRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Diachiluutru> GetActiveDiachiluutrus
        {
            get
            {
                return context.Diachiluutrus
                    .Where(p => p.inttrangthai == (int)enumDiachiluutru.inttrangthai.IsActive);
            }
        }

        public IQueryable<Diachiluutru> GetAllDiachiluutrus
        {
            get { return context.Diachiluutrus; }
        }

        public void SetTen(string strtendonvi, Int32 intid)
        {
            var donvi = context.Diachiluutrus
                .FirstOrDefault(p => p.Id == intid);

            donvi.strtendonvi = strtendonvi;
            context.SaveChanges();
        }

        public void AddTen(string strtendonvi, Int32 ParentId)
        {
            Diachiluutru donvi = new Diachiluutru();
            donvi.strtendonvi = strtendonvi;
            donvi.ParentId = ParentId;
            donvi.inttrangthai = (int)enumDiachiluutru.inttrangthai.IsActive;
            context.Diachiluutrus.Add(donvi);
            context.SaveChanges();
        }

        public void DeleteDonvi(Int32 intid)
        {
            Diachiluutru donvi = context.Diachiluutrus.FirstOrDefault(p => p.Id == intid);
            //context.Donvitructhuocs.Remove(donvi);
            donvi.inttrangthai = (int)enumDiachiluutru.inttrangthai.NotActive;
            context.SaveChanges();
        }
    }
}
