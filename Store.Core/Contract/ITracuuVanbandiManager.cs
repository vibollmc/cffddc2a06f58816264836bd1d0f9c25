using QLVB.DTO.Vanbandi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Contract
{
    public interface ITracuuVanbandiManager
    {
        IEnumerable<ListVanbandiViewModel> GetListVanbandi(
            string strngaykycat, int? idloaivb, int? idsovb, string strvbphathanh,
            int? intsobd, int? intsokt, string strngaykybd, string strngaykykt,
            string strkyhieu, string strnguoiky, string strnguoisoan, string strnguoiduyet,
            string strnoinhan, string strtrichyeu, string strhantraloi, string strdonvisoan,
            int? idkhan, int? idmat
            );

        SearchVBViewModel GetViewSearch();

        DetailVBDiViewModel GetViewDetail(int idvanban);
    }
}
