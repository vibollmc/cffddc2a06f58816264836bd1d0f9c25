using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLVB.Domain.Entities;
using Store.DAL.Abstract;

namespace Store.DAL.Implementation
{
    public class EFStoreVanbandenRepository : IStoreVanbandenRepository
    {
        private QLVBStoreDatabase context;

        public EFStoreVanbandenRepository(QLVBStoreDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Vanbanden> Vanbandens
        {
            get { return context.Vanbandens.AsNoTracking(); }
        }
        public Vanbanden GetVanbandenById(int id)
        {
            return context.Vanbandens.FirstOrDefault(p => p.intid == id);
        }
        public int Them(Vanbanden vb)
        {
            // cac truong mac dinh khi them moi
            // them mac dinh ngay tao va ngay sua la ngay hien tai
            vb.strngaytao = DateTime.Now;
            vb.strngaysua = null;
            vb.intpublic = (int)enumVanbanden.intpublic.Private;


            context.Vanbandens.Add(vb);
            context.SaveChanges();
            return vb.intid;
        }

        public void Sua(int intid, Vanbanden vb)
        {
            var _vb = context.Vanbandens.FirstOrDefault(p => p.intid == intid);
            _vb.intiddiachiluutru = vb.intiddiachiluutru;
            _vb.intiddonvinhap = vb.intiddonvinhap;
            _vb.intidkhan = vb.intidkhan;
            _vb.intidkhoiphathanh = vb.intidkhoiphathanh;
            //_vb.intidldvp = vb.intidldvp;
            _vb.intidlinhvuc = vb.intidlinhvuc;
            _vb.intidmat = vb.intidmat;
            _vb.intidnguoiduyet = vb.intidnguoiduyet;
            _vb.intidnguoisua = vb.intidnguoisua;
            //_vb.intidnguoitao = vb.intidnguoitao;
            _vb.intidphanloaivanbanden = vb.intidphanloaivanbanden;
            _vb.intidsovanban = vb.intidsovanban;
            _vb.intmucquantrong = vb.intmucquantrong;
            _vb.intquyphamphapluat = vb.intquyphamphapluat;
            _vb.intsoden = vb.intsoden;
            //_vb.inttrangthai = vb.inttrangthaiduyet;
            _vb.strhanxuly = vb.strhanxuly;
            _vb.strkyhieu = vb.strkyhieu;
            _vb.strngayden = vb.strngayden;
            _vb.strngayky = vb.strngayky;
            _vb.strngaysua = DateTime.Now;  //vb.strngaysua;
            //_vb.strngaytao = vb.strngaytao;
            _vb.strnguoiky = vb.strnguoiky;
            _vb.strnoidung = vb.strnoidung;
            _vb.strnoigui = vb.strnoigui;
            _vb.strnoinhan = vb.strnoinhan;
            _vb.strnoiphathanh = vb.strnoiphathanh;
            _vb.strtomtatnoidung = vb.strtomtatnoidung;
            _vb.strtraloivanbanso = vb.strtraloivanbanso;
            _vb.strtrichyeu = vb.strtrichyeu;
            _vb.strtukhoa = vb.strtukhoa;

            context.SaveChanges();
        }

        public void Xoa(int intid)
        {
            Vanbanden vb = context.Vanbandens.FirstOrDefault(p => p.intid == intid);
            context.Vanbandens.Remove(vb);
            context.SaveChanges();
        }

        // ==================================
        // Duyet hoac huy duyet van ban
        // ==================================
        public void Duyet(int intid, int inttrangthai)
        {
            var vb = context.Vanbandens.FirstOrDefault(p => p.intid == intid);
            vb.inttrangthai = inttrangthai;
            context.SaveChanges();
        }

        // =================================
        // Cap nhat strnoinhan(nguoi xu ly chinh) 
        // sau khi phan xu ly van ban
        // =================================
        public void CapnhatNguoixulychinh(int intid, string strxulychinh)
        {
            var vb = context.Vanbandens.FirstOrDefault(p => p.intid == intid);
            vb.strnoinhan = strxulychinh;
            context.SaveChanges();
        }

        // cap quyen xem public/private 
        public void CapquyenxemPublic(int intid, int intpublic)
        {
            var vb = context.Vanbandens.FirstOrDefault(p => p.intid == intid);
            vb.intpublic = intpublic;
            context.SaveChanges();
        }
        /// <summary>
        /// cap nhat dang vb: vb dien tu/vb giay/...
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="intdangvb"></param>
        public void CapnhatDangVanban(int idvanban, int intdangvb)
        {
            try
            {
                var vb = context.Vanbandens.FirstOrDefault(p => p.intid == idvanban);
                vb.intdangvb = intdangvb;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void CapnhatVBDT(int idvanban, int inttrangthai)
        {
            try
            {
                var vb = context.Vanbandens.FirstOrDefault(p => p.intid == idvanban);
                vb.bitguivbdt = inttrangthai;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object RunSqlListVBDen(string query)
        {
            try
            {
                var vb =
                context.Database.SqlQuery<QLVB.DTO.Vanbanden.ListVanbandenViewModel>(query);
                return vb;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object RunSqlListVBDenLienquan(string query)
        {
            try
            {
                var vb =
                context.Database.SqlQuery<QLVB.DTO.Vanbanden.ListVanbandenlienquanViewModel>(query);
                return vb;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
