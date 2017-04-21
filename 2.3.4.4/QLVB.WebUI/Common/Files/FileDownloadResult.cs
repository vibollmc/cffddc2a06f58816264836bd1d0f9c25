using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLVB.WebUI.Common.Files;

namespace QLVB.WebUI.Common.Files
{
    public class FileDownloadResult : ContentResult
    {
        private string fileName;
        private string strmota;
        private byte[] fileData;

        public FileDownloadResult(string fileName, byte[] fileData, string strmota)
        {
            this.fileName = fileName;
            this.fileData = fileData;
            this.strmota = strmota;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (string.IsNullOrEmpty(this.fileName))
                throw new Exception("A file name is required.");
            if (this.fileData == null)
                throw new Exception("File data is required.");
            var contentDisposition = string.Format("attachment; filename={0}", this.strmota);
            context.HttpContext.Response.AddHeader("Content-Disposition", contentDisposition);

            var cd = new System.Net.Mime.ContentDisposition
            {
                // for example foo.bak
                FileName = this.fileName,

                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false,
            };
            //Response.AppendHeader("Content-Disposition", cd.ToString());
            //ContentType = cd.ToString();
            //=========================================================
            //
            //=========================================================
            string fileExt = FileDisplay.GetFileExtention(this.fileName).ToLower();
            if (fileExt == "pdf")
            {

            }

            ContentType = "application/force-download";
            context.HttpContext.Response.BinaryWrite(this.fileData);
        }
    }
}