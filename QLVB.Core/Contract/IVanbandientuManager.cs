using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO.Vanbandientu;
using QLVB.DTO;

namespace QLVB.Core.Contract
{
    public interface IVanbandientuManager
    {
        #region vanbanden
        IEnumerable<ListVanbandendientuViewModel> GetListVanbandendientu(
            string strngaykycat, string strngaynhancat, int? inttinhtrangcat,
            int? intsodenbd, int? intsodenkt, string strngaynhanbd, string strngaynhankt,
            string strngaykybd, string strngaykykt, string strngayguibd, string strngayguikt,
            string strkyhieu, string strnoigui, string strtrichyeu
            );

        DetailVBDenViewModel GetViewDetail(int idvanban);

        int GetSongayhienthi();

        ToolbarVBDenViewModel GetToolbarVBDen();

        ResultFunction NhanEmail();

        DeleteVBViewModel GetDeleteVanban(int id);

        /// <summary>
        /// xoa van ban
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResultFunction DeleteVanban(int id);

        #endregion vanbanden

        #region vanbandi

        IEnumerable<ListVanbandidientuViewModel> GetListVanbandidientu(
            string strngaykycat, string strngaynhancat, string strDonviguicat,
            int? intsodibd, int? intsodikt, string strngaynhanbd, string strngaynhankt,
            string strngaykybd, string strngaykykt, string strngayguibd, string strngayguikt,
            string strkyhieu, string strnoigui, string strtrichyeu
            );

        /// <summary>
        /// danh sach cac don vi da gui idvanban
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        IEnumerable<ListDonviguiViewModels> GetListDonvigui(int idvanban);

        #endregion vanbandi

        #region AutoMail

        ResultFunction AutoReceiveMail();

        ResultFunction AutoSendMail();

        #endregion AutoMail

        #region TonghopVB

        IEnumerable<TonghopVBDenViewModel> TonghopVBDen(string strngaykybd, string strngaykykt,
            string danhmuc, int? intSongaygui, int LoaiTonghop);

        IEnumerable<ListVanbandendientuViewModel> GetListVBDenDientuDonvi(
            string stremail, string strngaykybd, string strngaykykt, int? intSoNgaygui, int LoaiTonghop);

        IEnumerable<TonghopVBDiViewModel> TonghopVBDi(string strngaykybd, string strngaykykt,
           int? intSongaygui);

        string GetTenDonviDi(int iddonvi);

        string GetTenDonviDi(string stremail, string danhmuc);

        #endregion TonghopVB
    }
}
