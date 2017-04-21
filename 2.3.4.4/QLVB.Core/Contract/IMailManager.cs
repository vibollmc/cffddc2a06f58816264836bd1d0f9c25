using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.DTO;
using QLVB.DTO.Vanbandientu;


namespace QLVB.Core.Contract
{
    public interface IMailManager
    {

        /// <summary>
        /// nhan vbdt
        /// </summary>
        /// <param name="EmailPerRequest"></param>
        /// <param name="intloai">
        /// 0: nhận thủ công
        /// 1: nhận tự động
        /// </param>
        /// <returns></returns>
        ResultFunction NhanVBDT(int EmailPerRequest, int intloai);

        /// <summary>
        /// gui van ban dien tu
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="listSendDonvi"></param>
        /// <param name="intloaivanban"></param>
        /// <param name="intAutoSend">
        /// 0: gui binh thuong
        /// 1: luu vao DB de gui tu dong
        /// 2: gui tu dong
        /// </param>
        /// <returns></returns>
        ResultFunction GuiVBDT(int idvanban, List<ListSendDonviViewModel> listSendDonvi, int intloaivanban, int intAutoSend);

        ResultFunction AutoSendVBDT();

        ResultFunction SendEmailKhac(int intloaivanban, int idvanban, string donviEmailKhac,
            string strtieudeEmailKhac, string strnoidungEmailKhac);

    }
}
