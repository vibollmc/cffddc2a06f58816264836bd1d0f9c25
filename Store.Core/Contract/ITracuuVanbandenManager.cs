using QLVB.DTO.Vanbanden;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Contract
{
    public interface ITracuuVanbandenManager
    {
        #region Listvanban


        /// <summary>
        /// lay danh sach cac van ban ma user duoc phep xem
        /// tuy theo quyen cua user
        /// </summary>
        /// <returns></returns>
        IEnumerable<ListVanbandenViewModel> GetListVanbanden
            (string ngayden, int? idloaivb,
            int? idkhoiph, int? idsovb, string xuly,
            int? intsodenbd, int? intsodenkt, string strngaydenbd, string strngaydenkt,
            string strngaykybd, string strngaykykt, string strsokyhieu, string strnguoiky,
            string strnoigui, string strtrichyeu, string strnguoixuly
            );


        SearchVBViewModel GetViewSearch();


        #endregion Listvanban

        #region ViewDetail

        DetailVBDenViewModel GetViewDetail(int id);

        #endregion ViewDetail

    }
}
