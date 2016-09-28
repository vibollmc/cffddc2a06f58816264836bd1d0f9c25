using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFDoituongxulyRepository : IDoituongxulyRepository
    {
        private QLVBDatabase context;

        public EFDoituongxulyRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Doituongxuly> Doituongxulys
        {
            get { return context.Doituongxulys; }
        }

        /// <summary>
        /// chỉ lấy các user đã từng tham gia xử lý
        /// kể cả đã chuyển xử lý
        /// </summary>
        public IQueryable<Doituongxuly> GetAllCanboxulys
        {
            get
            {
                return context.Doituongxulys
                    .Where(p => p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaogiaoviec
                            || p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaophutrach
                            || p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Xulychinh
                            || p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Phoihopxuly
                            || p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Chuyenxuly);
            }
        }

        /// <summary>
        /// chỉ lấy các user đang tham gia xử lý
        /// không lấy user đã chuyển xử lý
        /// </summary>
        public IQueryable<Doituongxuly> GetCanboDangXulys
        {
            get
            {
                return context.Doituongxulys
                    .Where(p => p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaogiaoviec
                            || p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaophutrach
                            || p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Xulychinh
                            || p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Phoihopxuly);
            }
        }

        public int Them(Doituongxuly dt)
        {
            // cac truong mac dinh khi them moi
            dt.strngaytao = DateTime.Now;

            context.Doituongxulys.Add(dt);
            context.SaveChanges();
            return dt.intid;
        }

        public void Sua(int intid, Doituongxuly dt)
        {
            var _dt = context.Doituongxulys.SingleOrDefault(p => p.intid == intid);
            _dt.intidcanbo = dt.intidcanbo;
            //_dt.intidhosocongviec = dt.intidhosocongviec;
            //_dt.strthoigian = dt.strthoigian;
            //_dt.intnguoitao = dt.intnguoitao;
            _dt.intvaitro = dt.intvaitro;
            _dt.intvaitrocu = dt.intvaitrocu;
            _dt.strthaotac = dt.strthaotac;
            //_dt.strngaychuyen = dt.strngaychuyen;
            //_dt.intnguoichuyen = dt.intnguoichuyen;

            context.SaveChanges();
        }

        public void Xoa(int intid)
        {
            var dt = context.Doituongxulys.SingleOrDefault(p => p.intid == intid);
            context.Doituongxulys.Remove(dt);
            context.SaveChanges();
        }

        public void XoaCacCanbo(int idhosocongviec)
        {
            var dt = context.Doituongxulys.Where(p => p.intidhosocongviec == idhosocongviec).ToList();
            foreach (var d in dt)
            {
                context.Doituongxulys.Remove(d);
            }
            context.SaveChanges();
        }

        /// <summary>
        /// cap nhat vai tro xu ly
        /// chuyen vaitro qua vaitrocu
        /// ghi nhan thoi gian va nguoi chuyen
        /// </summary>
        /// <param name="intid">intid doituongxuly</param>
        /// <param name="idcanbo">idcanbo thuc hien</param>
        public void CapnhatVaitrocu(int intid, int idcanbo)
        {
            var dt = context.Doituongxulys.FirstOrDefault(p => p.intid == intid);
            dt.intvaitrocu = dt.intvaitro;
            dt.intvaitro = (int)enumDoituongxuly.intvaitro_doituongxuly.Chuyenxuly;

            dt.strngaychuyen = DateTime.Now;
            dt.intnguoichuyen = idcanbo;

            context.SaveChanges();
        }

        /// <summary>
        /// kiem tra xem co idcanbo da co vai tro nay trong ho so chua?
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <param name="idcanbo"></param>
        /// <param name="intvaitro"></param>
        /// <returns>
        /// true:  co
        /// false: khong co
        /// </returns>
        public bool KiemtraVaitroxuly(int idhosocongviec, int idcanbo, int intvaitro)
        {
            var dt = context.Doituongxulys
                .Where(p => p.intidhosocongviec == idhosocongviec)
                .Where(p => p.intidcanbo == idcanbo)
                .Where(p => p.intvaitro == intvaitro)
                .FirstOrDefault();
            if (dt == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

    }
}
