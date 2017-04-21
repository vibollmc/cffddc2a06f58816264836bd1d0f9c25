using QLVB.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using QLVB.WebUI.Common.NLog;
using QLVB.Domain.Entities;
using QLVB.Common.Utilities;

namespace QLVB.WebUI.Common.Files
{
    public class FileServices
    {
        //=======================================================
        //  thu vien dung chung
        //=======================================================
        private static NLogLogger _logger = new NLogLogger();

        /// <summary>
        /// tạo thư mục đường dẫn để lưu file tuy theo loai vb dinh kem
        /// </summary>
        /// <param name="strloai"></param>
        /// <returns></returns>
        public static string SetPathUpload(string strloai)
        {
            string folderpath = "";
            try
            {
                string strnoidung = AppSettings.Noidung;
                switch (strloai)
                {
                    case AppConts.FileCongvanden:
                        strnoidung += "/" + AppConts.FileCongvanden;
                        break;
                    case AppConts.FileCongvanphathanh:
                        strnoidung += "/" + AppConts.FileCongvanphathanh;
                        break;
                    case AppConts.FileDongbo:
                        strnoidung += "/" + AppConts.FileDongbo;
                        break;

                }
                DateTime ngayhientai = DateTime.Now;
                string strfoldernam = ngayhientai.ToString("yyyy"); //ngayhientai.Year.ToString();
                //folderpath += "\\" + strfoldernam;
                strnoidung += "/" + strfoldernam;
                folderpath = HttpContext.Current.Server.MapPath(strnoidung);
                if (!Directory.Exists(folderpath))
                {
                    Directory.CreateDirectory(folderpath);
                }

                string strfolderthang = ngayhientai.ToString("MM"); //ngayhientai.Month.ToString();
                //folderpath += "\\" + strfolderthang;
                strnoidung += "/" + strfolderthang;
                //HostingEnvironment.MapPath
                folderpath = HttpContext.Current.Server.MapPath(strnoidung);
                if (!Directory.Exists(folderpath))
                {
                    Directory.CreateDirectory(folderpath);
                }

                string strfolderngay = ngayhientai.ToString("dd"); //ngayhientai.Day.ToString();
                //folderpath += "\\" + strfolderngay;
                strnoidung += "/" + strfolderngay;
                folderpath = HttpContext.Current.Server.MapPath(strnoidung);
                if (!Directory.Exists(folderpath))
                {
                    Directory.CreateDirectory(folderpath);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return folderpath;
        }

        /// <summary>
        /// lay duong dan folder dongbo 
        /// </summary>
        /// <returns></returns>
        public static string SetPathSynch()
        {
            string folderpath = "";
            string strnoidung = AppSettings.Noidung;
            strnoidung += "/" + AppConts.FileDongbo;
            folderpath = HttpContext.Current.Server.MapPath(strnoidung);
            return folderpath;
        }

        public static bool CheckFileExists(string fileName, string folderPath)
        {
            string fileSavepath = Path.Combine(folderPath, fileName);
            bool flag = false;
            if (System.IO.File.Exists(fileSavepath))
            {
                flag = true;
            }
            return flag;
        }

        public static bool CheckFileExists(string fileSavepath)
        {
            if (System.IO.File.Exists(fileSavepath))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// lay duong dan folder de luu file
        /// </summary>
        /// <param name="intloai"></param>
        /// <returns></returns>
        public static string GetFolderPath(int intloai)
        {
            string folderSavepath = string.Empty;
            switch (intloai)
            {
                case (int)enumAttachVanban.intloai.Vanbanden:
                    folderSavepath = FileServices.SetPathUpload(AppConts.FileCongvanden);
                    break;
                case (int)enumAttachVanban.intloai.Vanbandi:
                    folderSavepath = FileServices.SetPathUpload(AppConts.FileCongvanphathanh);
                    break;

            }
            return folderSavepath;
        }

        /// <summary>
        /// lay duong dan folder cua file dinh kem
        /// </summary>
        /// <param name="strloai"></param>
        /// <param name="dtengaycapnhat"></param>
        /// <returns></returns>
        public static string GetFolderDownload(string strloai, DateTime dtengaycapnhat)
        {
            string folderpath = "";
            try
            {
                string strnoidung = AppSettings.Noidung;
                switch (strloai)
                {
                    case AppConts.FileCongvanden:
                        strnoidung += "/" + AppConts.FileCongvanden;
                        break;
                    case AppConts.FileCongvanphathanh:
                        strnoidung += "/" + AppConts.FileCongvanphathanh;
                        break;
                    case AppConts.FileDongbo:
                        strnoidung += "/" + AppConts.FileDongbo;
                        break;

                }
                string strfoldernam = dtengaycapnhat.ToString("yyyy"); //dtengaycapnhat.Year.ToString();
                strnoidung += "/" + strfoldernam;

                string strfolderthang = dtengaycapnhat.ToString("MM"); //dtengaycapnhat.Month.ToString();
                strnoidung += "/" + strfolderthang;

                string strfolderngay = dtengaycapnhat.ToString("dd"); //dtengaycapnhat.Day.ToString();
                strnoidung += "/" + strfolderngay;
                folderpath = strnoidung;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return folderpath;
        }
        //=============================================================
        //ResolveServerUrl(VirtualPathUtility.ToAbsolute("~/images/image1.gif"),false))
        /// <summary>
        /// tra ve duong dan tren website
        /// </summary>
        /// <param name="serverUrl"></param>
        /// <param name="forceHttps"></param>
        /// <returns></returns>
        public static string ResolveServerUrl(string serverUrl, bool forceHttps)
        {
            if (serverUrl.IndexOf("://") > -1)
                return serverUrl;

            string newUrl = serverUrl;
            Uri originalUri = System.Web.HttpContext.Current.Request.Url;
            newUrl = (forceHttps ? "https" : originalUri.Scheme) +
                "://" + originalUri.Authority + newUrl;
            return newUrl;
        }

        //public static class UriHelperExtensions
        //{
        //    // Prepend the provided path with the scheme, host, and port of the request.
        //    public static string FormatAbsoluteUrl(this Uri url, string path)
        //    {
        //        return string.Format(
        //           "{0}/{1}", url.FormatUrlStart(), path.TrimStart('/'));
        //    }

        //    // Generate a string with the scheme, host, and port if not 80.
        //    public static string FormatUrlStart(this Uri url)
        //    {
        //        return string.Format("{0}://{1}{2}", url.Scheme,
        //           url.Host, url.Port == 80 ? string.Empty : ":" + url.Port);
        //    }
        //    <img src="@Request.Url.FormatAbsoluteUrl("/images/img.jpg")" alt="Alt text" />
        //}


    }
}