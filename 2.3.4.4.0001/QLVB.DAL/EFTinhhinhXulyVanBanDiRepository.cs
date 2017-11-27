using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace QLVB.DAL
{
   public class EFTinhhinhXulyVanBanDiRepository:ITinhhinhXulyVanBanDiReponsitory
    {
        private QLVBDatabase context;

        public EFTinhhinhXulyVanBanDiRepository(QLVBDatabase _context)
        {
            context = _context;
        }
        public IQueryable<TinhhinhXulyVanBanDi> TinhhinhXulyVanBanDis
        {
            get { return context.TinhhinhXulyVanBanDis; }
        }
        public int Them(TinhhinhXulyVanBanDi xuly)
        {

            context.TinhhinhXulyVanBanDis.Add(xuly);
            context.SaveChanges();
            return xuly.intid;
        }
        public int getIdGuiVanban(int idvanban, string strmadinhdanh, string strtendonvi, int intloaivanban, enumGuiVanban.intloaigui intloaigui)

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
            if (vb != null) return vb.intid;
            else return 0;
          
        }
    }
}
