using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.Domain.Abstract;

namespace QLVB.DAL
{
    public class EFHosoQuytrinhxulyRepository : IHosoQuytrinhXulyRepository
    {
        private QLVBDatabase context;

        public EFHosoQuytrinhxulyRepository(QLVBDatabase _context)
        {
            context = _context;
        }
        public IQueryable<HosoQuytrinhXuly> HosoQuytrinhxulys
        {
            get { return context.HosoQuytrinhxulys; }
        }
        public int Them(HosoQuytrinhXuly hoso)
        {
            try
            {
                context.HosoQuytrinhxulys.Add(hoso);
                context.SaveChanges();
                return hoso.intid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int CapnhatTrangthai_DaXuly(int id, int inttrangthai)
        {
            try
            {
                var hs = context.HosoQuytrinhxulys
                    .FirstOrDefault(p => p.intid == id);

                hs.inttrangthai = inttrangthai;
                hs.strNgaykt = DateTime.Now;

                context.SaveChanges();
                return 1;// hs.intid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int CapnhatTrangthai_DangXuly(int id, int inttrangthai)
        {
            try
            {
                var hs = context.HosoQuytrinhxulys
                    .FirstOrDefault(p => p.intid == id);

                hs.inttrangthai = inttrangthai;
                hs.strNgaybd = DateTime.Now;

                context.SaveChanges();
                return 1;// hs.intid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int CapnhatTrangthai_DaXuly(int idhoso, string nodeidFrom, int inttrangthai)
        {
            try
            {
                var hs = context.HosoQuytrinhxulys
                    .Where(p => p.intidhoso == idhoso)
                    .Where(p => p.nodeidFrom == nodeidFrom);
                foreach (var h in hs)
                {
                    h.inttrangthai = inttrangthai;
                    h.strNgaykt = DateTime.Now;
                }
                context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int CapnhatTrangthai_DangXuly(int idhoso, string nodeidFrom, int inttrangthai)
        {
            try
            {
                var hs = context.HosoQuytrinhxulys
                    .Where(p => p.intidhoso == idhoso)
                    .Where(p => p.nodeidFrom == nodeidFrom);
                foreach (var h in hs)
                {
                    h.inttrangthai = inttrangthai;
                    h.strNgaybd = DateTime.Now;
                }
                context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// cap nhat trang thai cua tat cac cac node tru node da duyet
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="listidNodeDaduyet"></param>
        /// <param name="inttrangthai"></param>
        /// <returns></returns>
        public int CapnhatTrangthai(int idhoso, List<int> listidNodeDaduyet, int inttrangthai)
        {
            try
            {
                var hs = context.HosoQuytrinhxulys
                    .Where(p => p.intidhoso == idhoso)
                    .Where(p => !listidNodeDaduyet.Contains(p.intidFrom));
                foreach (var h in hs)
                {
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

        public bool Xoa(int idhoso)
        {
            try
            {
                var hs = context.HosoQuytrinhxulys
                    .Where(p => p.intidhoso == idhoso);
                foreach (var p in hs)
                {
                    context.HosoQuytrinhxulys.Remove(p);
                }
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool CapnhatCanbo(int idhoso, string NodeId, int idcanbo)
        {
            try
            {
                var hs = context.HosoQuytrinhxulys
                    .Where(p => p.intidhoso == idhoso)
                    .Where(p => p.nodeidFrom == NodeId);
                foreach (var p in hs)
                {
                    p.intidCanbo = idcanbo;
                }
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// cap nhat thoi gian xu ly tai nodefrom
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="intidFrom"></param>
        /// <param name="intsongay"></param>
        /// <returns></returns>
        public int CapnhatThoigianXuly(int idhoso, int intidFrom, int intsongay)
        {
            try
            {
                var hs = context.HosoQuytrinhxulys
                    .Where(p => p.intidhoso == idhoso)
                    .Where(p => p.intidFrom == intidFrom);
                foreach (var h in hs)
                {
                    h.intSongay = intsongay;
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
