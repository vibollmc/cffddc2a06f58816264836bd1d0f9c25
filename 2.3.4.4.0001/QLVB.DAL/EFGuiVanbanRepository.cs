using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFGuiVanbanRepository : IGuiVanbanRepository
    {
        private QLVBDatabase context;

        public EFGuiVanbanRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<GuiVanban> GuiVanbans
        {
            get { return context.GuiVanbans; }
        }

        public int Them(GuiVanban vb)
        {
            // cac truong mac dinh
            //vb.inttrangthaigui = xac dinh tu ngoai
            vb.strngaygui = DateTime.Now;
            vb.inttrangthainhan = (int)enumGuiVanban.inttrangthainhan.Chuanhan;

            context.GuiVanbans.Add(vb);
            context.SaveChanges();

            return vb.intid;
        }
        public int UpdateHoibao(int idvanban, int iddonvi, DateTime? dteNgaygui)
        {
            var vb = context.GuiVanbans
                .Where(p => p.intidvanban == idvanban)
                .Where(p => p.intiddonvi == iddonvi)
                .Where(p => p.strngaygui == dteNgaygui)
                .FirstOrDefault();
            if (vb != null)
            {
                vb.strngaynhan = DateTime.Now;
                vb.inttrangthainhan = (int)enumGuiVanban.inttrangthainhan.Danhan;
                context.SaveChanges();
                return vb.intid;
            }
            else
            {
                return 0;
            }

        }
        public int UpdateTrangthaiGui(int idvanban, int iddonvi, int intloaivanban)
        {
            try
            {
                var vb = context.GuiVanbans
                .Where(p => p.intidvanban == idvanban)
                .Where(p => p.intiddonvi == iddonvi)
                .Where(p => p.intloaivanban == intloaivanban)
                .Where(p => p.inttrangthaigui != (int)enumGuiVanban.inttrangthaigui.Dagui)
                .FirstOrDefault();
                if (vb != null)
                {
                    // cap nhat ngay gui thuc te va trang thai
                    vb.strngaygui = DateTime.Now;
                    vb.inttrangthaigui = (int)enumGuiVanban.inttrangthaigui.Dagui;
                    context.SaveChanges();
                    return vb.intid;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateTrangthaiNhan(int idvanban, string strmadinhdanh, string strtendonvi, int intloaivanban,
            enumGuiVanban.inttrangthaiphanhoi trangthai, DateTime ngaythuchien, enumGuiVanban.intloaigui intloaigui)
        {
            try
            {

                var tochuc = context.Tochucdoitacs.FirstOrDefault(
                        x => (intloaigui == enumGuiVanban.intloaigui.Tructinh && x.strmatructinh.Trim() == strmadinhdanh.Trim())
                            || (intloaigui == enumGuiVanban.intloaigui.Chinhphu && x.strmadinhdanh.Trim() == strmadinhdanh.Trim()));

                GuiVanban vb = null;

                if (tochuc != null)
                {
                    vb = context.GuiVanbans
                        .FirstOrDefault(
                            p =>
                                p.intidvanban == idvanban && 
                                (p.strtendonvi == strtendonvi || p.intiddonvi == tochuc.intid) &&
                                p.intloaivanban == intloaivanban && p.intloaigui == (int)intloaigui);
                }
                else
                {
                    vb = context.GuiVanbans
                        .FirstOrDefault(
                            p =>
                                p.intidvanban == idvanban && p.strtendonvi == strtendonvi &&
                                p.intloaivanban == intloaivanban && p.intloaigui == (int)intloaigui);
                }

                if (vb != null)
                {
                    // cap nhat ngay nhận trạng thái phản hồi
                    if (trangthai == enumGuiVanban.inttrangthaiphanhoi.Datiepnhan)
                        vb.strngaytiepnhan = ngaythuchien;
                    else if (trangthai == enumGuiVanban.inttrangthaiphanhoi.Dangxuly)
                        vb.strngaydangxuly = ngaythuchien;
                    else if (trangthai == enumGuiVanban.inttrangthaiphanhoi.Hoanthanh)
                        vb.strngayhoanthanh = ngaythuchien;
                    else if (trangthai == enumGuiVanban.inttrangthaiphanhoi.DaDen)
                        vb.strngaynhan = ngaythuchien;
                    else if (trangthai == enumGuiVanban.inttrangthaiphanhoi.Phancong)
                        vb.strngayphancong = ngaythuchien;
                    else
                        return 0;

                    context.SaveChanges();
                    return vb.intid;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateTrangthaiNhan(int idvanban, int iddonvi, int intloaivanban,
            enumGuiVanban.inttrangthaiphanhoi trangthai, DateTime ngaythuchien)
        {
            try
            {

                var vb = context.GuiVanbans
                    .Where(p => p.intidvanban == idvanban)
                    .Where(p => p.intiddonvi == iddonvi)
                    .Where(p => p.intloaivanban == intloaivanban)
                    .FirstOrDefault();
                if (vb != null)
                {
                    // cap nhat ngay nhận trạng thái phản hồi
                    if (trangthai == enumGuiVanban.inttrangthaiphanhoi.Datiepnhan)
                        vb.strngaytiepnhan = ngaythuchien;
                    else if (trangthai == enumGuiVanban.inttrangthaiphanhoi.Dangxuly)
                        vb.strngaydangxuly = ngaythuchien;
                    else if (trangthai == enumGuiVanban.inttrangthaiphanhoi.Hoanthanh)
                        vb.strngayhoanthanh = ngaythuchien;
                    else
                        return 0;

                    context.SaveChanges();
                    return vb.intid;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
