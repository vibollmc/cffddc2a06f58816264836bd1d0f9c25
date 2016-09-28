using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.Domain.Abstract;

namespace QLVB.DAL
{
    public class EFHosoQuytrinhRepository : IHosoQuytrinhRepository
    {
        private QLVBDatabase context;
        public EFHosoQuytrinhRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<HosoQuytrinh> HosoQuytrinhs
        {
            get { return context.HosoQuytrinhs; }
        }
        public HosoQuytrinh GetHosoQuytrinhByID(int id)
        {
            return context.HosoQuytrinhs.FirstOrDefault(p => p.intid == id);
        }
        public int Them(HosoQuytrinh hoso)
        {
            try
            {
                context.HosoQuytrinhs.Add(hoso);
                context.SaveChanges();
                return hoso.intid;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int Sua()
        {
            return 0;
        }

        public int CapnhatTrangthai(int idhoso, int inttrangthai)
        {
            try
            {
                var hoso = context.HosoQuytrinhs
                    .Where(p => p.intidhoso == idhoso)
                    .Where(p => p.inttrangthai == (int)enumHosoQuytrinh.inttrangthai.Dangtamngung);
                //.FirstOrDefault();

                // cap nhat toan bo thanh tiep tiep tuc xu ly
                foreach (var h in hoso)
                {
                    h.strNgayTieptucXuly = DateTime.Now;
                    h.inttrangthai = inttrangthai;
                }
                context.SaveChanges();

                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
