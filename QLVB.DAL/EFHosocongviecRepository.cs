using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFHosocongviecRepository : IHosocongviecRepository
    {
        private QLVBDatabase context;

        public EFHosocongviecRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Hosocongviec> Hosocongviecs
        {
            get { return context.Hosocongviecs; }
        }
        public Hosocongviec GetHSCVById(int id)
        {
            return context.Hosocongviecs.FirstOrDefault(p => p.intid == id);
        }
        public int Them(Hosocongviec hs)
        {
            // cac truong mac dinh khi them moi
            //hs.strngaymohoso = DateTime.Now;
            hs.inttrangthai = (int)enumHosocongviec.inttrangthai.Dangxuly;

            context.Hosocongviecs.Add(hs);
            context.SaveChanges();
            return hs.intid;
        }

        /// <summary>
        /// cap nhat ho so cong viec va mo lai ho so
        /// rieng strnoidung thi cap nhat tuy theo quyen cua user
        /// </summary>
        /// <param name="intid"></param>
        /// <param name="hs"></param>
        /// <param name="IsUpdateStrNoidung"></param>
        public void Sua(int intid, Hosocongviec hs, bool IsUpdateStrNoidung)
        {
            try
            {
                // kiem tra cac truong khong cho phep cap nhat
                var _hs = context.Hosocongviecs.SingleOrDefault(p => p.intid == intid);
                //_hs.intiddonvi = hs.intiddonvi;
                //_hs.intidnguoinhap = hs.intidnguoinhap;
                _hs.intkhan = hs.intkhan;
                _hs.intlinhvuc = hs.intlinhvuc;
                //_hs.intloai = hs.intloai;
                //_hs.intluuhoso = hs.intluuhoso;
                _hs.intmat = hs.intmat;
                _hs.intmucdo = hs.intmucdo;
                _hs.intsoden = hs.intsoden;
                //_hs.intsotudong = hs.intsotudong;

                // khi phan xu ly 1 van ban thi  cho phep mo lai ho so 
                _hs.inttrangthai = (int)enumHosocongviec.inttrangthai.Dangxuly;

                //_hs.strketqua = hs.strketqua;
                //_hs.strngayketthuc = hs.strngayketthuc;
                //_hs.strngaymohoso = hs.strngaymohoso;  // khong cap nhat ngay mo ho so
                if (IsUpdateStrNoidung)
                {
                    // cap nhat noi dung tuy theo quyen cua user
                    _hs.strnoidung = hs.strnoidung;
                }
                _hs.strsohieuht = hs.strsohieuht;
                _hs.strthoihanxuly = hs.strthoihanxuly;
                _hs.strtieude = hs.strtieude;
                _hs.strngaymohoso = hs.strngaymohoso;

                _hs.intdonghoso = hs.intdonghoso;

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Xoa(int intid)
        {
            try
            {
                var hs = context.Hosocongviecs.SingleOrDefault(p => p.intid == intid);
                string strtieude = hs.strtieude;
                context.Hosocongviecs.Remove(hs);
                context.SaveChanges();
                return strtieude;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void LuuHoso(int intid, int idcanbo)
        {
            try
            {
                var hs = context.Hosocongviecs.FirstOrDefault(p => p.intid == intid);
                hs.intluuhoso = (int)enumHosocongviec.intluuhoso.Co;
                hs.inttrangthai = (int)enumHosocongviec.inttrangthai.Dahoanthanh;
                hs.intidnguoihoanthanh = idcanbo;
                hs.strngayketthuc = DateTime.Now;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void HoanthanhHoso(int intid, int idcanbo)
        {
            try
            {
                var hs = context.Hosocongviecs.FirstOrDefault(p => p.intid == intid);
                hs.intluuhoso = (int)enumHosocongviec.intluuhoso.Khong;
                hs.inttrangthai = (int)enumHosocongviec.inttrangthai.Dahoanthanh;
                hs.intidnguoihoanthanh = idcanbo;
                hs.strngayketthuc = DateTime.Now;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void Trinhky(int intid, int idcanbo)
        {
            try
            {
                var hs = context.Hosocongviecs.FirstOrDefault(p => p.intid == intid);
                hs.intluuhoso = (int)enumHosocongviec.intluuhoso.DangTrinhky;
                hs.inttrangthai = (int)enumHosocongviec.inttrangthai.Dangxuly;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void TamngungQuytrinh(int intid, int intluuhoso, DateTime dteHanxuly)
        {
            try
            {
                var hs = context.Hosocongviecs.FirstOrDefault(p => p.intid == intid);
                hs.strngayketthuc = dteHanxuly;
                hs.intluuhoso = intluuhoso;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void CapnhatThoihanxuly(int idhoso, DateTime dteHanxuly)
        {
            try
            {
                var hs = context.Hosocongviecs.FirstOrDefault(p => p.intid == idhoso);
                hs.strthoihanxuly = dteHanxuly;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
