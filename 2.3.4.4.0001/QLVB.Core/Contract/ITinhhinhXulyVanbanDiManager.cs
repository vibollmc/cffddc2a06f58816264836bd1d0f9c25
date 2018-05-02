using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.DTO.TinhhinhXulyVanbanDi;
using QLVB.DTO;
using QLVB.DTO.Tinhhinhxuly;

namespace QLVB.Core.Contract
{
    public interface ITinhhinhXulyVanbanDiManager
    {
        IEnumerable<TinhhinhXulyVanBanDi> GetTinhhinhXulyVanbanDi(int idguivanban);
        ResultFunction InsertTinhhinhXulyVanbanDi(TinhhinhXulyVanbanDiViewModel data);
        ListcoquanbenngoaiViewModel GetListDonvi(int? iddonvi, string strngaybd, string strngaykt, int? idloaingay, int? idsovb);
        IEnumerable<XLVanbandi> TonghopVBDi(int iddonvi, string strngaybd, string strngaykt, int idloaingay, int idsovb);

        IEnumerable<QLVB.DTO.Vanbandi.ListVanbandiViewModel> GetListVanbandi
            (int intloai,  int iddonvi, string strngaybd, string strngaykt, int idloaingay, int idsovb);
    }
}
