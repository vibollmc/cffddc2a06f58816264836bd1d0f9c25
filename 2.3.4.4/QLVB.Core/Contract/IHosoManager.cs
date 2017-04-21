using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Core.Contract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Hoso;

namespace QLVB.Core.Contract
{
    public interface IHosoManager
    {
        #region Index

        CategoryCongviecViewModel GetCategoryCongviec();

        IEnumerable<ListCongviecViewModel> GetListCongviec(
            string strngayhscat, string strxuly, int? intsobd, int? intsokt, string strngayhsbd, string strngayhskt,
            string strtieude, string strhantraloi, int? idlinhvuc
            );

        SearchCongviecViewModel GetViewSearch();

        ResultFunction DeleteHoso(int idhoso);

        #endregion Index

        #region ViewDetailHosocongviec

        /// <summary>
        /// hien thi thong thi chi tiet va qua trinh xu ly cua ho so
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <returns></returns>
        DetailHosoViewModel GetDetailHoso(int idhosocongviec);

        #endregion ViewDetailHosocongviec

        #region PhanXLVB

        /// <summary>
        /// load du lieu form phan xu ly van ban
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        PhanXLVBViewModel GetFormPhanXLVB(int idvanban);

        /// <summary>
        /// luu phan xu ly van ban
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="model"></param>
        /// <returns>        
        /// </returns>
        ResultFunction SavePhanXLVB(int idvanban, PhanXLVBViewModel model);

        int SavePhanXLNhieuVB(List<int> listidvanban, string strykienxuly, PhanXLVBViewModel model);

        #endregion PhanXLVB

        #region ThemHoso
        /// <summary>
        /// load du lieu form them hoso
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        PhanXLVBViewModel GetFormThemHoso(int idhoso);

        /// <summary>
        /// luu them hoso
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="model"></param>
        /// <returns>        
        /// </returns>
        ResultFunction SaveHoso(PhanXLVBViewModel model);

        /// <summary>
        /// lay thong tin cac user thuoc don vi : iddonvi
        /// </summary>
        /// <param name="iddonvi"></param>
        /// <returns></returns>
        IEnumerable<QLVB.DTO.Donvi.EditUserViewModel> GetListCanbo(int iddonvi);

        #endregion ThemHoso

        #region XulyHosocongviec

        #region Thongtinchung

        /// <summary>
        /// kiem tra xem user co quyen xu ly ho so khong
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        bool IsXulyHoso(int idhoso);

        ToolbarXulyViewModel GetToolbarXuly(int idhoso, int? intBack);

        /// <summary>
        /// danh sach tat ca nguoi dang xu ly cua ho so : 
        /// Lanh dao giao viec, lanh dao phu trach, xu ly chinh, phoi hop xu ly
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        IEnumerable<DanhsachNguoixulyViewModel> GetDanhsachNguoixuly(int idhoso);

        /// <summary>
        /// cac thong tin chung cua hoso
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        ThongtinHosoViewModel GetThongtinHoso(int idhoso);

        /// <summary>
        ///  tra ve intiddoituongxuly cua idcanbo dang xem/xu ly ho so
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="idcanbo"></param>
        /// <returns>0:khong phai user dang trong luong xu ly</returns>
        int GetIdDoituongxuly(int idhoso, int idcanbo);



        #endregion Thongtinchung

        #region Phoihopxuly

        /// <summary>
        /// lay danh sach cac tat cac user trong don vi
        /// neu user da tham gia xu ly roi thi isCheck = true  
        /// neu user la nguoi phoi hop xu ly thi isphoihopxuly =true
        /// de chi cho phep them/bot nguoi phoi hop xu ly thoi
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <returns></returns>
        PhoihopXulyViewModel GetUserPhoihopxuly(int idhosocongviec);

        /// <summary>
        /// them can bo phoi hop xu ly vao ho so
        /// </summary>
        /// 
        /// <returns></returns>
        ResultFunction SavePhoihopxuly(int idhosocongviec, List<int> listidphoihopxuly);


        #endregion Phoihopxuly

        #region Ykien

        /// <summary>
        /// ghi nhan y kien xu ly cua can bo
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="strykien"></param>
        /// <returns></returns>
        ResultFunction SaveYkienxuly(int idhoso, string strykien);

        /// <summary>
        /// lay id y kien de attach file, inttrangthai=0
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        ResultFunction GetIdYkien(int idhoso);

        /// <summary>
        /// update y kien xu ly cua can bo sau khi da attach file
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="strykien"></param>
        /// <returns></returns>
        ResultFunction UpdateYkienxuly(int idhoso, int idykien, string strykien);

        #endregion Ykien

        #region KetthucHoso

        ResultFunction LuuHoso(int idhoso);

        ResultFunction Trinhky(int idhoso);

        ResultFunction HoanthanhHoso(int idhoso);

        ResultFunction LuuNhieuHoso(List<int> listidvanban);

        ResultFunction HoanthanhNhieuHoso(List<int> listidvanban);

        #endregion KetthucHoso

        #region Phieutrinh

        /// <summary>
        /// thong tin phieu trinh
        /// </summary>
        /// <param name="idphieutrinh"></param>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        PhieutrinhViewModel GetPhieutrinh(int idphieutrinh, int idhoso);

        /// <summary>
        /// them moi noi dung trinh lanh dao
        /// </summary>
        /// <param name="idhoso"></param>        
        /// <param name="idlanhdaotrinh"></param>
        /// <param name="strnoidungtrinh"></param>
        /// <returns>idphieutrinh</returns>
        int SaveNoidungtrinh(int idhoso, int idlanhdaotrinh, string strnoidungtrinh);

        /// <summary>
        /// lanh dao cho y kien
        /// </summary>
        /// <param name="idphieutrinh"></param>
        /// <param name="idlanhdao"></param>
        /// <param name="strykienchidao"></param>
        /// <returns>idphieutrinh</returns>
        int SaveYkienchidao(int idphieutrinh, int idlanhdao, string strykienchidao);

        #endregion Phieutrinh

        #region Quytrinh

        /// <summary>
        /// ket thuc 1 buoc xu ly trong quy trinh va chuyen sang buoc tiep theo
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        ResultFunction KetthucBuocXuly(int idhoso);

        string ReadFlowChart(int idhoso, int? intUpdate);

        /// <summary>
        /// Tại bước rẽ nhánh, quay trở lại bước xử lý trước đó (được ĐN trong quy trình)
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        ResultFunction ReturnBuocXuly(int idhoso);

        /// <summary>
        /// cap nhat tinh trang tam ngung ho so quy trinh
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="valid"></param>
        /// <returns></returns>
        ResultFunction TamngungQuytrinh(int idhoso, bool valid, string strngay);

        /// <summary>
        /// lay cac thong tin xu ly cua idhoso tai nodeid
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="NodeId"></param>
        /// <returns></returns>
        EditHosoQuytrinhXulyViewModel GetEditHosoQuytrinhXuly(int idhoso, string NodeId);

        /// <summary>
        /// cap nhat thay doi hoso quy trinh xu ly: thay doi can bo 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultFunction UpdateHosoQuytrinhXuly(EditHosoQuytrinhXulyViewModel model);

        /// <summary>
        /// tuy chon buoc xu ly tiep theo khi co >2 buoc xu ly tiep
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="jsNodes"></param>
        /// <returns></returns>
        ResultFunction ChonBuocXuly(int idhoso, string jsNodes);


        #endregion Quytrinh

        #region PhathanhVB

        PhathanhVBViewModel GetPhathanhVanban(int idhoso, string listfile);

        ResultFunction SaveVanbanPhathanh(PhathanhVBViewModel model);

        #endregion PhathanhVB

        #endregion XulyHosocongviec

        #region PhanQuytrinh

        /// <summary>
        /// load du lieu form phan quy trinh xu ly
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        PhanQuytrinhViewModel GetFormPhanQuytrinh(int idvanban);

        /// <summary>
        /// lay ten cac quy trinh thuoc loai idloaiquytrinh
        /// </summary>
        /// <param name="idloaiquytrinh"></param>
        /// <returns></returns>
        IEnumerable<QLVB.DTO.Quytrinh.EditQuytrinhViewModel> GetQuytrinh(int idloaiquytrinh);

        /// <summary>
        /// phan quy trinh xu ly cho van ban
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultFunction SavePhanQuytrinh(int idvanban, PhanQuytrinhViewModel model);

        #endregion PhanQuytrinh
    }
}
