using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Core.Contract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Tinhhinhxuly;


namespace QLVB.Core.Contract
{
    public interface ITinhhinhxulyManager
    {
        #region Vanbanden

        /// <summary>
        /// lay ds cac phong ban 
        /// </summary>
        /// <returns></returns>
        ListdonviViewModel GetListDonvi(int? iddonvi, string strngaybd, string strngaykt, int? idloaingay, int? idsovb);

        /// <summary>
        /// thong tin tong hop tinh hinh xu ly cua iddonvi
        /// </summary>
        /// <param name="iddonvi"></param>
        /// <param name="strngaybd"></param>
        /// <param name="strngaykt"></param>
        /// <returns></returns>
        IEnumerable<XLVanbanden> TonghopVBDen(int iddonvi, string strngaybd, string strngaykt, int idloaingay, int idsovb);

        IEnumerable<QLVB.DTO.Vanbanden.ListVanbandenViewModel> GetListVanbanden
            (int intloai, int idcanbo, int iddonvi, string strngaybd, string strngaykt, int idloaingay, int idsovb);

        #endregion Vanbanden

        #region Quytrinh

        ListQuytrinhViewModel GetListQuytrinh(int? intidloaiquytrinh, int? idquytrinh, string strngaybd, string strngaykt);

        IEnumerable<QLVB.DTO.Quytrinh.EditQuytrinhViewModel> GetQuytrinh(int idloaiquytrinh);

        /// <summary>
        /// thong tin tong hop tinh hinh xu ly cua iddonvi, idquytrinh
        /// </summary>
        /// <param name="idquytrinh"></param>
        /// <param name="iddonvi"></param>
        /// <param name="strngaybd"></param>
        /// <param name="strngaykt"></param>
        /// <returns></returns>
        IEnumerable<QuytrinhVBDenViewModel> TonghopQuytrinh(int? idloaiquytrinh, int? idquytrinh, int? iddonvi, string strngaybd, string strngaykt);

        IEnumerable<QLVB.DTO.Vanbanden.ListVanbandenViewModel> GetListQuytrinhVBDen
            (int? idquytrinh, int intloai, int? idloaiquytrinh, string strngaybd, string strngaykt, string NodeId);


        string XemTonghopQuytrinhFlowchart(int idquytrinh, int intloai, string strngaybd, string strngaykt);

        #endregion Quytrinh
    }
}
