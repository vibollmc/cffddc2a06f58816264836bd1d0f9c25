using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.DTO.File;
using QLVB.DTO;
using System.Web;

namespace QLVB.Core.Contract
{
    public interface IFileManager
    {
        #region Common

        /// <summary>
        /// lay duong dan folder cua file dinh kem
        /// </summary>
        /// <param name="strloai"></param>
        /// <param name="dtengaycapnhat"></param>
        /// <returns></returns>
        string GetFolderDownload(string strloai, DateTime dtengaycapnhat);

        /// <summary>
        /// tra ve duong dan va ten file de download
        /// </summary>
        /// <param name="strloai"></param>
        /// <param name="dtengaycapnhat"></param>
        /// <param name="strfilename"></param>
        /// <returns></returns>
        string GetPhysicalPath(string strloai, DateTime dtengaycapnhat, string strfilename);

        /// <summary>
        /// tạo thư mục đường dẫn để lưu file tuy theo loai vb dinh kem
        /// </summary>
        /// <param name="strloai"></param>
        /// <returns></returns>
        string SetPathUpload(string strloai);


        /// <summary>
        /// tra ve image tuy theo loai file 
        /// </summary>
        /// <param name="strtenfile"></param>
        /// <returns></returns>
        string GetFileTypeImages(string strtenfile);

        string GetFileExtention(string strtenfile);

        bool CheckFileExits(string filepath);

        #endregion Common

        #region FileReader
        /// <summary>
        /// lay duong dan ~/ cua file pdf 
        /// </summary>
        /// <param name="idfile"></param>
        /// <param name="intloai"></param>
        /// <returns></returns>
        PdfViewerModel GetPdfViewer(int idfile, int intloai);

        PdfViewerModel PdfReader(int idfile, int intloai);

        DocxViewerModel GetDocxViewer(int idfile, int intloai);

        DocxViewerModel DocxReader(int idfile, int intloai);

        #endregion FileReader

        /// <summary>
        /// download van ban dinh kem tuy loai :
        /// van ban den, van ban di, van ban du thao 
        /// </summary>
        /// <param name="idfile"></param>
        /// <param name="intloai"></param>
        /// <returns></returns>
        FileDownloadResult DownloadVanban(int idfile, int intloai);

        /// <summary>
        /// download file : vanbandendientu, email inbox, outbox
        /// </summary>
        /// <param name="idfile"></param>
        /// <param name="intloai"></param>
        /// <returns></returns>
        FileDownloadResult DownloadVBDT(int idfile, int intloai);


        FileDownloadResult DownloadHoso(int idfile, int intloai);

        /// <summary>
        /// luu file van ban vao server
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="idvanban"></param>
        /// <param name="idloai">loai file: vb den/di/duthao</param>
        /// <returns></returns>
        ResultFunction UploadVanban(HttpPostedFileBase filename, int idvanban, int idloai);

        /// <summary>
        /// danh dau xoa file van ban trong database
        /// </summary>
        /// <param name="idfile"></param>        
        void DeActiveFileUploadVanban(int idfile);

        /// <summary>
        /// xoa file 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="idvanban"></param>
        /// <param name="idloai"></param>
        void DeleteFileUploadVanban(string[] fileName, int idvanban, int idloai);

        /// <summary>
        /// luu cac file dinh kem trong hoso
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="idhoso"></param>
        /// <param name="idtailieu"></param>
        /// <param name="idloai"></param>
        /// <returns></returns>
        ResultFunction UploadHoso(HttpPostedFileBase filename, int idhoso, int idtailieu, int idloai);

        /// <summary>
        /// xoa file dinh kem trong ho so
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="idhoso"></param>
        /// <param name="idtailieu"></param>
        /// <param name="idloai"></param>
        void DeleteFileUploadHoso(string[] fileName, int idhoso, int idtailieu, int idloai);

        /// <summary>
        /// update avatar cua user
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        ResultFunction UploadImageProfile(HttpPostedFileBase filename);

        /// <summary>
        /// kiem tra quyen va lay duong dan download cua vanbanden/di
        /// </summary>
        /// <returns></returns>
        string CheckFolderFileVanbanDownload(int intloai, int idfile, int idcanbo, int idvanban);
    }
}
