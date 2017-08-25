using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using QLVB.Domain.Entities;
using QLVB.Domain.Abstract;

namespace QLVB.DAL
{
    public class EFQuytrinhRepository : IQuytrinhRepository
    {
        private QLVBDatabase context;

        public EFQuytrinhRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Quytrinh> AllQuytrinhs
        {
            get
            {
                return context.Quytrinhs;
            }
        }

        public IQueryable<Quytrinh> ActiveQuytrinhs
        {
            get
            {
                return context.Quytrinhs
                    .Where(p => p.inttrangthai == (int)enumQuytrinh.inttrangthai.IsActive);
            }
        }
        public int Them(Quytrinh hs)
        {
            //hs.ngaytao =
            context.Quytrinhs.Add(hs);
            context.SaveChanges();
            return hs.intid;
        }

        public int Them(string strten, int idloai, int intsongay, DateTime? dteThoigianApdung, int inttrangthai)
        {
            if (!string.IsNullOrEmpty(strten))
            {
                Quytrinh hs = new Quytrinh
                {
                    //hs.ngaytao =
                    intidloai = idloai,
                    inttrangthai = inttrangthai, //(int)enumQuytrinh.inttrangthai.IsActive,
                    strten = strten,
                    intSoNgay = intsongay,
                    strNgayApdung = dteThoigianApdung
                };
                return Them(hs);
            }
            else
            {
                return 0;
            }
        }

        public int Sua(int intid, string strten, int intsongay, DateTime? dteThoigianApdung, int inttrangthai)
        {
            var hs = context.Quytrinhs.FirstOrDefault(p => p.intid == intid);
            hs.strten = strten;
            hs.intSoNgay = intsongay;
            hs.strNgayApdung = dteThoigianApdung;
            hs.inttrangthai = inttrangthai;
            context.SaveChanges();
            return hs.intid;
        }

        public string SaveNumberOfElements(int intid, int? numberOfElements)
        {
            try
            {
                var hs = context.Quytrinhs.FirstOrDefault(p => p.intid == intid);
                hs.numberOfElements = numberOfElements;
                context.SaveChanges();
                return hs.strten;
            }
            catch
            {
                return null;
            }
        }
        public int? GetNumberOfElements(int idquytrinh)
        {
            var n = context.Quytrinhs
                .FirstOrDefault(p => p.intid == idquytrinh).numberOfElements;
            return n;

        }
    }
}
