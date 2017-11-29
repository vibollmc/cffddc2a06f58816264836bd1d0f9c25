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
        private QLVBDatabase _context;

        public EFTinhhinhXulyVanBanDiRepository(QLVBDatabase context)
        {
            _context = context;
        }
        public IQueryable<TinhhinhXulyVanBanDi> TinhhinhXulyVanBanDis
        {
            get { return _context.TinhhinhXulyVanBanDis; }
        }
        public int Them(TinhhinhXulyVanBanDi xuly)
        {

            var xl = _context.TinhhinhXulyVanBanDis.FirstOrDefault(
                x => x.intidguivanban == xuly.intidguivanban
                && x.strmaxuly == xuly.strmaxuly
                && x.strngayxuly == xuly.strngayxuly
                && x.strnguoixuly == xuly.strnguoixuly
                && x.strphongban == xuly.strphongban
                );
            if (xl != null) return xl.intid;
            
            _context.TinhhinhXulyVanBanDis.Add(xuly);
            _context.SaveChanges();
            return xuly.intid;
        }
        public int getIdGuiVanban(int idvanban, string strmadinhdanh, string strtendonvi, int intloaivanban, enumGuiVanban.intloaigui intloaigui)

        {

            var tochuc = _context.Tochucdoitacs.FirstOrDefault(
                       x => (intloaigui == enumGuiVanban.intloaigui.Tructinh && x.strmatructinh.Trim() == strmadinhdanh.Trim())
                           || (intloaigui == enumGuiVanban.intloaigui.Chinhphu && x.strmadinhdanh.Trim() == strmadinhdanh.Trim()));

            GuiVanban vb = null;

            if (tochuc != null)
            {
                vb = _context.GuiVanbans
                    .FirstOrDefault(
                        p =>
                            p.intidvanban == idvanban &&
                            (p.strtendonvi == strtendonvi || p.intiddonvi == tochuc.intid) &&
                            p.intloaivanban == intloaivanban && p.intloaigui == (int)intloaigui);
            }
            else
            {
                vb = _context.GuiVanbans
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
