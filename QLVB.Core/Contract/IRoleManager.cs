using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Core.Contract
{
    /// <summary>
    /// kiem tra quyen cua cac user tren tung van ban den, di, ho so cong viec
    /// tra ve gia tri True neu user co quyen
    /// </summary>
    public interface IRoleManager
    {
        #region GetRole
        /// <summary>
        /// lay quyen cua user trong session
        /// va luu quyen vao session
        /// </summary>
        /// <returns></returns>
        object[] GetUserRole();

        /// <summary>
        /// kiem tra xem user co quyen nay khong?
        /// </summary>
        /// <param name="strRole"></param>
        /// <returns></returns>
        bool IsRole(string strRole);

        #endregion GetRole

        #region Vanban

        /// <summary>
        /// kiem tra xem user co quyen xem van ban den khong
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        bool IsViewVanbanden(int idvanban, int idcanbo);

        /// <summary>
        /// kiem tra xem user co quyen download file dinh kem cua van ban den khong
        /// </summary>
        /// <param name="idfile"></param>
        /// <param name="idvanban"></param>
        /// <param name="idcanbo"></param>
        /// <returns></returns>
        bool IsDownloadFileVanbanden(int idfile, int idcanbo, int idvanban);

        /// <summary>
        /// kiem tra xem user co quyen xem van ban di khong        
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        bool IsViewVanbandi(int idvanban, int idcanbo);

        /// <summary>
        /// kiem tra xem user co quyen download file dinh kem cua van ban di khong        
        /// </summary>
        /// <param name="idfile"></param>
        /// <param name="idvanban"></param>
        /// <param name="idcanbo"></param>
        /// <returns></returns>
        bool IsDownloadFileVanbandi(int idfile, int idcanbo, int idvanban);

        #endregion Vanban

        #region Hosocongviec

        /// <summary>
        /// kiem tra xem user co quyen xem thong tin ho so khong
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <param name="idcanbo"></param>
        /// <returns></returns>
        bool IsViewHosocongviec(int idhosocongviec, int idcanbo);

        /// <summary>
        /// kiem tra xem user co quyen xu ly ho so khong
        /// user: ldgv, ldpt, xlc, phxl va DA CHUYEN XU LY      
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <param name="idcanbo"></param>
        /// <returns>true: neu co quyen xu ly</returns>
        bool IsXulyHosocongviec(int idhosocongviec, int idcanbo);

        /// <summary>
        /// kiem tra user co dang xu ly khong, 
        /// user: ldgv, ldpt, xlc va phxl
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <param name="idcanbo"></param>
        /// <returns>true: neu co quyen xu ly</returns>
        bool IsDangXulyHosocongviec(int idhosocongviec, int idcanbo);

        /// <summary>
        /// kiem tra ho so da duoc xu ly chua(hoan thanh chua )
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <returns>true: da hoan thanh</returns>
        bool CheckHoanthanhHoso(int idhosocongviec);

        /// <summary>
        /// kiem tra xem ho so nay co cho phep user dong khong
        /// </summary>
        /// <param name="idcongviec"></param>
        /// <returns></returns>
        bool CheckQuyenDongHoso(int idcongviec);

        /// <summary>
        /// kiem tra xem van ban den nay da duoc phan xu ly chua
        /// tranh phan 2 lan 1 van ban (2 ho so xu ly cho 1 van ban)
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns>
        /// true: van ban nay chua phan xu ly
        /// false: van ban nay da phan xu ly roi, khong phan moi nua
        /// </returns>
        bool CheckPhanHosocongviec(int idvanban, int intloai);
        /// <summary>
        /// kiem tra xem user co quyen download file dinh kem trong ho so cong viec
        /// </summary>
        /// <param name="idfile"></param>
        /// <param name="idcanbo"></param>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        bool IsDownloadFileHosocongviec(int idfile, int idcanbo, int idhoso);

        #endregion Hosocongviec

        #region Tinhhinhxuly



        #endregion Tinhhinhxuly
    }
}
