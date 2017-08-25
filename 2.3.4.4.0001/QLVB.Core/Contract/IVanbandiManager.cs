using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.DTO;
using QLVB.DTO.Vanbandi;
using QLVB.DTO.Vanbandientu;
using QLVB.Domain.Entities;
using QLVB.DTO.Edxml;

namespace QLVB.Core.Contract
{
    public interface IVanbandiManager
    {
        #region ListVanban
        ToolbarViewModel GetToolbar();

        CategoryViewModel GetCategory();

        IEnumerable<ListVanbandiViewModel> GetListVanbandi(
            string strngaykycat, int? idloaivb, int? idsovb, string strvbphathanh, string hoibao,
            int? intsobd, int? intsokt, string strngaykybd, string strngaykykt,
            string strkyhieu, string strnguoiky, string strnguoisoan, string strnguoiduyet,
            string strnoinhan, string strtrichyeu, string strhantraloi, string strdonvisoan,
             int? idkhan, int? idmat
            );

        SearchVBViewModel GetViewSearch();

        #endregion ListVanban

        #region Listvanbanlienquan

        IEnumerable<ListVanbandilienquanViewModel> GetListVanbandilienquan(
            int idhoso,
            string strngaykycat, int? idloaivb, int? idsovb,
            int? intsobd, int? intsokt, string strngaykybd, string strngaykykt,
            string strkyhieu, string strnguoiky, string strnguoisoan, string strnguoiduyet,
            string strnoinhan, string strtrichyeu, string strhantraloi, string strdonvisoan
            );

        int SaveVBDiLienquan(List<int> listidvanban, int idhosocongviec);

        /// <summary>
        /// lay cac van ban den lien quan den id hoso 
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        IEnumerable<ListVanbandiViewModel> GetHosoVBDiLQ(int idhoso);

        #endregion Listvanbanlienquan


        #region ViewDetail

        DetailVBDiViewModel GetViewDetail(int idvanban);

        #endregion ViewDetail

        #region DuyetVanban

        ResultFunction DuyetVanban(int idvanban, int intduyet);

        #endregion Duyetvanban

        #region Updatevanban

        /// <summary>
        /// load du lieu cua form them moi van ban
        /// </summary>
        /// <param name="idloaivb"></param>
        /// <param name="idsovb"></param>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        ThemvanbanViewModel GetLoaitruong(int? idloaivb, int? idsovb, int? idvanban);

        /// <summary>
        /// lay cac truong thay  doi: so di khi thay doi so van ban
        /// </summary>
        /// <param name="idsovb"></param>
        /// <returns></returns>
        AjaxSovanban GetSovanban(int idsovb);
        /// <summary>
        /// lay ten cac don vi de nhap noi nhan vb
        /// </summary>
        /// <returns></returns>
        IEnumerable<ListTochucdoitacViewModel> GetTenDonvi(string q);

        IEnumerable<ListTochucdoitacViewModel> GetTenDonvi();

        IEnumerable<ListTochucdoitacViewModel> GetNoinhan(int? idvanban, string strnoinhantiep);
        /// <summary>
        /// save van ban moi
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// 0 : loi
        /// !=0: id vanban sau khi save
        /// </returns>
        int Savevanban(ThemvanbanViewModel model);

        /// <summary>
        /// cap nhat van ban
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Editvanban(int idvanban, Vanbandi model);

        /// <summary>
        /// kiem tra xem so di nay da ton tai chua
        /// </summary>
        /// <param name="intso"></param>
        /// <param name="dtengayky"></param>
        /// <param name="idsovanban"></param>
        /// <returns>true/fasle</returns>
        bool _CheckSophu(int? intso, DateTime dtengayky, int? idsovanban);

        QLVB.DTO.Vanbandi.DeleteVBViewModel GetDeleteVanban(int id);

        /// <summary>
        /// xoa van ban
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResultFunction DeleteVanban(int id);

        /// <summary>
        /// kiem tra lien ket van ban
        /// </summary>
        /// <param name="idvanbandi">id van ban di</param>
        /// <returns>
        /// id van ban den
        /// 0: khong co
        /// >0: da lien ket
        /// </returns>
        int CheckLienketVanban(int idvanbandi);

        /// <summary>
        /// kiem tra tra loi van ban co ton tai chua
        /// </summary>
        /// <param name="strTraloivb"></param>
        /// <param name="idvanbandi"></param>
        /// <returns></returns>
        TraloiVanbanViewModel CheckTraloiVanban(string strTraloivb, int idvanbandi);

        #endregion Updatevanban

        #region Upload

        /// <summary>
        /// lay danh sach cac file dinh kem cua van ban
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        UploadVBDiViewModel GetListFile(int idvanban);
        #endregion

        #region Capquyenxem
        /// <summary>
        /// lay danh sach cac tat cac user trong don vi
        /// neu user da co quyen xem roi thi isCheck = true  
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <returns></returns>
        ListUserCapquyenxemViewModel GetListUserCapquyenxem(int idvanban);

        ResultFunction SaveCapquyenxem(int idvanban, List<int> listidcanbo);

        /// <summary>
        /// cap quyen xem van ban la public hoac private
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="intpublic">trang thai public/private hien co cua van ban dang xet</param>
        /// <returns></returns>
        ResultFunction CapquyenxemPublic(int idvanban, int intpublic);

        #endregion Capquyenxem

        #region Email

        /// <summary>
        /// ds cac don vi gui vbdt idvanban
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        ListEmailDonviViewModel GetListEmailDonvi(int idvanban);

        /// <summary>
        /// lay id, dia chi cua cac don vi
        /// </summary>
        /// <param name="listiddonvi"></param>
        /// <returns></returns>
        IEnumerable<ListSendDonviViewModel> GetListSendDonvi(List<int> listiddonvi);

        #endregion Email

        #region ListVBDientu

        IEnumerable<ListVanbandiViewModel> GetListVBDientuDonvi(
           int? iddonvi, string strngaykybd, string strngaykykt, int? intSoNgaygui
           );


        IEnumerable<ListVanbandiViewModel> GetListLoaiVBDientu(
           int? idloai, string strngaykybd, string strngaykykt);

        #endregion ListVBDientu



    }
}
