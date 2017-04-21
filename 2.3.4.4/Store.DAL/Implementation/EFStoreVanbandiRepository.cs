using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.DAL.Abstract;
using QLVB.Domain.Entities;

namespace Store.DAL.Implementation
{
    public class EFStoreVanbandiRepository : IStoreVanbandiRepository
    {
        private QLVBStoreDatabase context;

        public EFStoreVanbandiRepository(QLVBStoreDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Vanbandi> Vanbandis
        {
            get { return context.Vanbandis.AsNoTracking(); }
        }

        //public void Them(Vanbandi vb)
        //{
        //    // them mac dinh ngay tao va ngay sua la ngay hien tai
        //    vb.strngaytao = DateTime.Now;
        //    vb.strngaysua = null;

        //    context.Vanbandis.Add(vb);
        //    context.SaveChanges();
        //}

        public int Them(Vanbandi vb)
        {
            try
            {
                // them mac dinh ngay tao va ngay sua la ngay hien tai
                vb.strngaytao = DateTime.Now;
                vb.strngaysua = null;
                vb.intpublic = (int)enumVanbandi.intpublic.Private;

                context.Vanbandis.Add(vb);
                context.SaveChanges();

                return vb.intid;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void Sua(int intid, Vanbandi vb)
        {
            try
            {
                var _vb = context.Vanbandis.FirstOrDefault(p => p.intid == intid);
                _vb.intiddiachiluutru = vb.intiddiachiluutru;
                _vb.intiddonvinhap = vb.intiddonvinhap;
                _vb.intidkhan = vb.intidkhan;
                _vb.intidlinhvuc = vb.intidlinhvuc;
                _vb.intidmat = vb.intidmat;
                _vb.intidnguoiduyet = vb.intidnguoiduyet;
                _vb.intidnguoisua = vb.intidnguoisua;

                _vb.intidphanloaivanbandi = vb.intidphanloaivanbandi;
                _vb.intidsovanban = vb.intidsovanban;
                _vb.intmucquantrong = vb.intmucquantrong;
                _vb.intquyphamphapluat = vb.intquyphamphapluat;
                _vb.intso = vb.intso;
                _vb.intsoban = vb.intsoban;
                _vb.intsobanFile = vb.intsobanFile;
                _vb.intsosao = vb.intsosao;
                _vb.intsoto = vb.intsoto;
                _vb.intsotoFile = vb.intsotoFile;
                //_vb.inttrangthai =
                _vb.strdonvisoan = vb.strdonvisoan;
                _vb.strhanxuly = vb.strhanxuly;
                _vb.strkyhieu = vb.strkyhieu;
                //_vb.strmorong = vb.strmorong;
                _vb.strngayky = vb.strngayky;
                _vb.strngaysao = vb.strngaysao;
                _vb.strngaysua = DateTime.Now;  //  ngay sua la ngay hien tai
                //_vb.strngaytao
                _vb.strnguoiduyet = vb.strnguoiduyet;
                _vb.strnguoiky = vb.strnguoiky;
                _vb.strnguoisoan = vb.strnguoisoan;
                _vb.strnoidung = vb.strnoidung;
                _vb.strnoinhan = vb.strnoinhan;
                _vb.strtomtat = vb.strtomtat;
                _vb.strtraloivanbanso = vb.strtraloivanbanso;  // tra loi van ban so
                _vb.strtrichyeu = vb.strtrichyeu;
                _vb.strtukhoa = vb.strtukhoa;

                //_vb.intdangvb
                //_vb.intpublic

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void Xoa(int intid)
        {
            try
            {
                var vb = context.Vanbandis.FirstOrDefault(p => p.intid == intid);
                context.Vanbandis.Remove(vb);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // ==================================
        // Duyet hoac huy duyet van ban
        // ==================================
        public void Duyet(int intid, int inttrangthai)
        {
            var vb = context.Vanbandis.FirstOrDefault(p => p.intid == intid);
            vb.inttrangthai = inttrangthai;
            context.SaveChanges();
        }

        // cap quyen xem public/private
        public void CapquyenxemPublic(int intid, int intpublic)
        {
            var vb = context.Vanbandis.FirstOrDefault(p => p.intid == intid);
            vb.intpublic = intpublic;
            context.SaveChanges();
        }

        public void CapnhatVBDT(int intid, int intguivbdt)
        {
            var vb = context.Vanbandis.FirstOrDefault(p => p.intid == intid);
            vb.intguivbdt = intguivbdt;
            context.SaveChanges();
        }

    }
}
