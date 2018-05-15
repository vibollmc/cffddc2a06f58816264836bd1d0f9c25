using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Core.Contract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Vanbanden;

namespace QLVB.Core.Contract
{
    public interface IVanbandenManager
    {
        #region Listvanban
        /// <summary>
        /// hien thi cac nut chuc nang cua toolbar
        /// </summary>
        /// <returns></returns>
        ToolbarViewModel GetToolbar();

        /// <summary>
        /// lay danh muc menu cua van ban den
        /// </summary>
        /// <returns></returns>
        CategoryViewModel GetCategory();

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
            string strnoigui, string strtrichyeu, string strnguoixuly, string strdangvanban,
            bool isSearch
            );


        SearchVBViewModel GetViewSearch();

        /// <summary>
        /// lay idhosocongviec cua van ban dang xem
        /// de hien thi nut Xu ly
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns>idhosocongviec
        /// == 0 : khong co
        /// !=0 : hien thi nut xu ly
        /// </returns>
        int GetIdHosocongviec(int idvanban);

        /// <summary>
        /// lay idhosocongviec cua van ban dang chon
        /// de hien thi menu phai
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        int GetIdHosoCV(int idvanban);


        /// <summary>
        /// lay cac y kien cua chuyen vien trong van ban
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <returns>
        /// true: co y kien
        /// false: khong y kien
        /// </returns>
        bool GetYkienvanbanden(int? idhosocongviec);

        /// <summary>
        /// lay cac y kien cua chuyen vien trong van ban
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns>
        /// true: co y kien
        /// false: khong y kien
        /// </returns>
        ListVanbandenViewModel GetYkienvanbanden_2(int? idvanban);

        #endregion Listvanban

        #region Listvanbanlienquan

        /// <summary>
        /// lay danh sach cac van ban ma user duoc phep xem
        /// tuy theo quyen cua user
        /// </summary>
        /// <returns></returns>
        IEnumerable<ListVanbandenlienquanViewModel> GetListVanbandenlienquan
            (int idhoso,
            string ngayden, int? idloaivb,
            int? idkhoiph, int? idsovb, string xuly,
            int? intsodenbd, int? intsodenkt, string strngaydenbd, string strngaydenkt,
            string strngaykybd, string strngaykykt, string strsokyhieu, string strnguoiky,
            string strnoigui, string strtrichyeu, string strnguoixuly
            );

        int SaveVBDenLienquan(List<int> listidvanban, int idhosocongviec);

        /// <summary>
        /// lay cac van ban den lien quan den id hoso 
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        IEnumerable<ListVanbandenViewModel> GetHosoVBDenLQ(int idhoso);



        #endregion Listvanbanlienquan

        #region ViewDetail

        DetailVBDenViewModel GetViewDetail(int id);

        #endregion ViewDetail

        #region DuyetVanban

        ResultFunction DuyetVanban(int idvanban, int intduyet);

        #endregion DuyetVanban

        #region UpdateVanban

        /// <summary>
        /// load du lieu cua form them moi van ban
        /// </summary>
        /// <param name="idloaivb"></param>
        /// <param name="idsovb"></param>
        /// <param name="idvanban"></param>
        /// <param name="idmail"></param>
        /// <returns></returns>
        ThemVanbanViewModel GetLoaitruong(int? idloaivb, int? idsovb, int? idvanban, int? idmail);

        /// <summary>
        /// lay ten cac don vi theo khoi phat hanh
        /// </summary>
        /// <param name="idkhoiph"></param>
        /// <returns></returns>
        IEnumerable<ListTochucdoitacViewModel> GetTenDonvi(int idkhoiph);

        /// <summary>
        /// lấy các trường: max số đến và khối phát hành khi 
        /// thay đổi sổ văn bản
        /// </summary>
        /// <param name="idsovb"></param>
        /// <returns></returns>
        AjaxSovanban GetSovanban(int idsovb);

        /// <summary>
        /// kiem tra van ban den co bi trung khong
        /// </summary>
        /// <param name="strsokyhieu"></param>
        /// <param name="strngayky"></param>
        /// <param name="strcoquan"></param>
        /// <returns>
        /// </returns>
        CheckVBTrungViewModel KiemtraVBtrung(string strsokyhieu, string strngayky, string strcoquan, int? idmail);

        /// <summary>
        /// cap nhat thoi han xu ly vb, tiep nhan vb giay
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        ResultFunction CapnhatThoihanXulyVBDT(int idvanban);
        /// <summary>
        /// cap nhat van ban dien tu dinh kem vao vb giay da co
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="idmail"></param>
        /// <returns></returns>
        ResultFunction CapnhatVBDTDinhkem(int idvanban, int idmail);

        /// <summary>
        /// save van ban moi
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// 0: Khong save duoc
        /// !=0: id van ban sau khi save
        /// </returns>
        int Savevanban(ThemVanbanViewModel model);

        /// <summary>
        /// cap nhat van ban
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="model"></param>
        /// <returns>true/false</returns>
        bool Editvanban(int idvanban, Vanbanden model);

        DeleteVBViewModel GetDeleteVanban(int id);

        /// <summary>
        /// xoa van ban
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResultFunction DeleteVanban(int id);

        #endregion UpdateVanban

        #region Capquyenxem

        /// <summary>
        /// lay danh sach cac tat cac user trong don vi
        /// neu user da co quyen xem roi thi isCheck = true  
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <returns></returns>
        ListUserCapquyenxemViewModel GetListUserCapquyenxem(int idvanban);

        /// <summary>
        /// cap quyen xem cua idvanban cho cac user
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="listidcanbo"></param>
        /// <returns></returns>
        ResultFunction SaveCapquyenxem(int idvanban, List<int> listidcanbo);

        /// <summary>
        /// cap quyen xem van ban la public hoac private
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="intpublic">trang thai public/private hien co cua van ban dang xet</param>
        /// <returns></returns>
        ResultFunction CapquyenxemPublic(int idvanban, int intpublic);

        #endregion Capquyenxem

        #region Upload

        /// <summary>
        /// lay danh sach cac file dinh kem cua van ban
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        UploadVBDenViewModel GetListFile(int idvanban);

        #endregion Upload

        #region PhanXLNhieuVB

        FormPhanXLNhieuVBViewModel GetFormPhanXLNhieuVB();

        IEnumerable<ListVanbandenViewModel> GetListPhanXLNhieuVB(string listid);

        #endregion PhanXLNhieuVB

        #region Email

        QLVB.DTO.Vanbandi.ListEmailDonviViewModel GetListEmailDonvi(int idvanban);

        IEnumerable<QLVB.DTO.Vanbandientu.ListSendDonviViewModel> GetListSendDonvi(List<int> listiddonvi);

        #endregion Email


    }
}
