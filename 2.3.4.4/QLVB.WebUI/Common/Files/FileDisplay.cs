using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QLVB.WebUI.Common.Files
{
    public class FileDisplay
    {
        public static string GetFileExtention(string strtenfile)
        {
            string fileExt = "";
            if (!string.IsNullOrEmpty(strtenfile))
            {
                //int len = strtenfile.Length;
                //string[] split = strtenfile.Split(new Char[] { '.' });
                //foreach (string s in split)
                int i = strtenfile.LastIndexOf('.');
                fileExt = strtenfile.Substring(++i);
            }
            return fileExt;
        }

        public static string GetFileType(string strtenfile)
        {
            //string strUrl = FileServices.ResolveServerUrl(VirtualPathUtility.ToAbsolute("~/Content/images/attach/unknown.gif"), false);
            string imageUrl = string.Empty;
            //string fileType = "";
            string fileExt = GetFileExtention(strtenfile).ToLower();
            switch (fileExt)
            {
                case "doc":
                    //strUrl = FileServices.ResolveServerUrl(VirtualPathUtility.ToAbsolute("~/Content/images/attach2/docx_win-32_32.png"), false);
                    imageUrl = "~/Content/Images/FileType2/docx_win-32_32.png";
                    break;
                case "pdf":
                    //strUrl = FileServices.ResolveServerUrl(VirtualPathUtility.ToAbsolute("~/Content/images/attach2/pdf-32_32.png"), false);
                    imageUrl = "~/Content/Images/FileType2/pdf-32_32.png";
                    break;

            }
            //fileType = "<img src='" + strUrl + " ' align='absmiddle'>";
            return imageUrl;
        }


    }
}