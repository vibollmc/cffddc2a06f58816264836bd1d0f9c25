using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.Domain.Abstract;

namespace QLVB.DAL
{
    public class EFTonghopCanboRepository : ITonghopCanboRepository
    {
        private QLVBDatabase context;

        public EFTonghopCanboRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<TonghopCanbo> TonghopCanbos
        {
            get { return context.TonghopCanbos; }
        }

        public int Them(TonghopCanbo th)
        {
            try
            {
                // kiem tra xem da co vanban cua user nay chua
                var vb = context.TonghopCanbos
                    .Where(p => p.intidcanbo == th.intidcanbo)
                    .Where(p => p.intidtailieu == th.intidtailieu)
                    .Where(p => p.intloaitailieu == th.intloaitailieu)
                    .FirstOrDefault();

                if (vb == null)
                {   // chua co
                    th.strngaytao = DateTime.Now;
                    th.inttrangthai = (int)enumTonghopCanbo.inttrangthai.Chuaxem;
                    context.TonghopCanbos.Add(th);
                    context.SaveChanges();
                    return th.intid;
                }
                else
                {   // da co thi xem intloai
                    // cap nhat trang thai chua xem
                    vb.inttrangthai = (int)enumTonghopCanbo.inttrangthai.Chuaxem;

                    // chi cap nhat intloai khi khong phai la hoso xlvb
                    if (vb.intloai != (int)enumTonghopCanbo.intloai.Trinhky
                        || vb.intloai != (int)enumTonghopCanbo.intloai.Phieutrinh
                        || vb.intloai != (int)enumTonghopCanbo.intloai.Ykienxuly)
                    {
                        vb.intloai = th.intloai;
                        vb.strngaytao = DateTime.Now;
                    }
                    context.SaveChanges();
                    return vb.intid;
                }
            }
            catch
            {
                return 0;
            }
        }

        public int CapnhatTrangthaiVBDen(int idcanbo, int idtailieu)
        {
            try
            {
                var th = context.TonghopCanbos
                        .Where(p => p.intidcanbo == idcanbo)
                        .Where(p => p.intidtailieu == idtailieu)
                        .Where(p => p.intloaitailieu == (int)enumTonghopCanbo.intloaitailieu.Vanbanden)
                        .Where(p => p.intloai == (int)enumTonghopCanbo.intloai.Vanbanmoi
                            || p.intloai == (int)enumTonghopCanbo.intloai.Debiet
                            || p.intloai == (int)enumTonghopCanbo.intloai.HosoXLVBDen
                        )
                        .OrderByDescending(p => p.strngaytao)
                        .FirstOrDefault();

                th.inttrangthai = (int)enumTonghopCanbo.inttrangthai.Daxem;
                th.strngayxem = DateTime.Now;
                context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int CapnhatTrangthaiHosoVBDen(int idcanbo, int idtailieu)
        {
            try
            {
                var th = context.TonghopCanbos
                        .Where(p => p.intidcanbo == idcanbo)
                        .Where(p => p.intidtailieu == idtailieu)
                        .Where(p => p.intloaitailieu == (int)enumTonghopCanbo.intloaitailieu.Vanbanden)
                        .Where(p => p.intloai == (int)enumTonghopCanbo.intloai.Phieutrinh
                            || p.intloai == (int)enumTonghopCanbo.intloai.Ykienxuly
                            || p.intloai == (int)enumTonghopCanbo.intloai.Trinhky
                        )
                        .OrderByDescending(p => p.strngaytao)
                        .FirstOrDefault();

                th.inttrangthai = (int)enumTonghopCanbo.inttrangthai.Daxem;
                th.strngayxem = DateTime.Now;
                context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
    }

}
