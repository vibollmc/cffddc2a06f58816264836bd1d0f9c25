using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DLTD.Web.Main.Common
{
    public class Util
    {
        public static void CreatePathUpload(out string folderUpload, out string fileUrl)
        {
            var dateTime = DateTime.Now;

            folderUpload = HttpContext.Current.Server.MapPath("~/Uploads");
            fileUrl = "Uploads";
            if (!Directory.Exists(folderUpload)) Directory.CreateDirectory(folderUpload);

            folderUpload = string.Format("{0}\\{1}", folderUpload, dateTime.Year);
            fileUrl = string.Format("{0}/{1}", fileUrl, dateTime.Year);

            if (!Directory.Exists(folderUpload)) Directory.CreateDirectory(folderUpload);

            folderUpload = string.Format("{0}\\{1:00}", folderUpload, dateTime.Month);
            fileUrl = string.Format("{0}/{1:00}", fileUrl, dateTime.Month);

            if (!Directory.Exists(folderUpload)) Directory.CreateDirectory(folderUpload);

            folderUpload = string.Format("{0}\\{1:00}", folderUpload, dateTime.Day);
            fileUrl = string.Format("{0}/{1:00}", fileUrl, dateTime.Day);

            if (!Directory.Exists(folderUpload)) Directory.CreateDirectory(folderUpload);
        }
    }
}