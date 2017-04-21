using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLVB.Core.Contract;
using QLVB.Domain.Entities;
using QLVB.DTO.File;
using QLVB.DTO;
using System.IO;
using QLVB.Common.Utilities;
using QLVB.WebUI.Common.OpenXML;
//using System.Xml.Linq;
//using OpenXmlPowerTools;
//using System.Drawing.Imaging;
//using DocumentFormat.OpenXml.Packaging;

namespace QLVB.WebUI.Controllers
{
    public class FileController : Controller
    {
        #region Constructor

        private IFileManager _file;

        public FileController(IFileManager file)
        {
            _file = file;
        }

        #endregion Constructor

        #region ViewPdf


        public ActionResult PdfLoading(int idfile, int intloai)
        {
            PdfViewerModel model = _file.PdfReader(idfile, intloai);

            if (model == null)
            {
                return PartialView("FileNotFound");
            }
            else
            {
                return PartialView(model);
            }
        }

        #endregion ViewPdf


        #region ViewDocx

        public ActionResult DocxLoading(int idfile, int intloai)
        {
            DocxViewerModel file = _file.DocxReader(idfile, intloai);
            DocxToHtml model = new DocxToHtml();
            model.html = WordReader.DocxConvertToHtml(file);

            return PartialView(model);
        }

        #endregion ViewDocx


        #region Download

        //http://www.prideparrot.com/blog/archive/2012/8/uploading_and_returning_files
        public ActionResult DownloadVanban(int idfile, int intloai)
        {
            FileDownloadResult filevanban = _file.DownloadVanban(idfile, intloai);

            ////=========================================================
            ////
            ////=========================================================
            ////string fileExt = FileDisplay.GetFileExtention(this.fileName).ToLower();
            ////if (fileExt == "pdf")
            ////{

            ////}

            string contentType = "application/force-download";

            //return File("~/App_Data/Images/" + FileName, System.Net.Mime.MediaTypeNames.Application.Octet, FileName);


            if (filevanban != null)
            {
                if (System.IO.File.Exists(filevanban.physicalFilePath))
                {
                    return File(filevanban.filePath, contentType, filevanban.strmota);
                }
                else
                {
                    return new EmptyResult();
                }
            }
            else
            {
                return new EmptyResult();
            }
        }

        public ActionResult DownloadHoso(int idfile, int intloai)
        {
            FileDownloadResult filehoso = _file.DownloadHoso(idfile, intloai);

            string contentType = "application/force-download";

            if (filehoso != null)
            {
                if (System.IO.File.Exists(filehoso.physicalFilePath))
                {
                    return File(filehoso.filePath, contentType, filehoso.strmota);
                }
                else
                {
                    return new EmptyResult();
                }
            }
            else
            {
                return new EmptyResult();
            }
        }

        public ActionResult DownloadVBDT(int idfile, int intloai)
        {
            FileDownloadResult filevanban = _file.DownloadVBDT(idfile, intloai);

            ////=========================================================
            ////
            ////=========================================================
            ////string fileExt = FileDisplay.GetFileExtention(this.fileName).ToLower();
            ////if (fileExt == "pdf")
            ////{

            ////}

            string contentType = "application/force-download";

            //return File("~/App_Data/Images/" + FileName, System.Net.Mime.MediaTypeNames.Application.Octet, FileName);


            if (filevanban != null)
            {
                if (System.IO.File.Exists(filevanban.physicalFilePath))
                {
                    return File(filevanban.filePath, contentType, filevanban.strmota);
                }
                else
                {
                    return new EmptyResult();
                }
            }
            else
            {
                return new EmptyResult();
            }
        }

        public ActionResult DownloadFile(int idfile, int intloai)
        {
            FileDownloadResult file;
            switch (intloai)
            {
                case (int)enumDownloadFileViewModel.intloai.Vanbanden:
                    file = _file.DownloadVanban(idfile, (int)enumAttachVanban.intloai.Vanbanden);
                    break;
                case (int)enumDownloadFileViewModel.intloai.Vanbandi:
                    file = _file.DownloadVanban(idfile, (int)enumAttachVanban.intloai.Vanbandi);
                    break;
                case (int)enumDownloadFileViewModel.intloai.HSCV_Ykien:
                    file = _file.DownloadHoso(idfile, (int)enumAttachHoso.intloai.Ykien);
                    break;
                case (int)enumDownloadFileViewModel.intloai.VBDT:
                    file = _file.DownloadVBDT(idfile, (int)enumAttachMail.intloai.Vanbandendientu);
                    break;
                default:
                    file = _file.DownloadVanban(0, 0);
                    break;
            }

            string contentType = "application/force-download";

            if (file != null)
            {
                if (System.IO.File.Exists(file.physicalFilePath))
                {
                    return File(file.filePath, contentType, file.strmota);
                }
                else
                {
                    return new EmptyResult();
                }
            }
            else
            {
                return new EmptyResult();
            }
        }



        #endregion Download

        #region UploadVBDen

        public ActionResult UploadVBDen(IEnumerable<HttpPostedFileBase> files, int idvanban)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    //var fileName = Path.GetFileName(file.FileName);
                    //var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                    //file.SaveAs(physicalPath);

                    var kq = _file.UploadVanban(file, idvanban, (int)enumAttachVanban.intloai.Vanbanden);

                }
            }
            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult RemoveVBDen(string[] fileNames, int idvanban)
        {
            // The parameter of the Remove action must be called "fileNames"
            if (fileNames != null)
            {
                _file.DeleteFileUploadVanban(fileNames, idvanban, (int)enumAttachVanban.intloai.Vanbanden);
            }
            // Return an empty string to signify success
            return Content("");
        }
        #endregion UploadVBDen

        #region UploadVBDi

        public ActionResult UploadVBDi(IEnumerable<HttpPostedFileBase> files, int idvanban)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    //var fileName = Path.GetFileName(file.FileName);
                    //var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                    //file.SaveAs(physicalPath);

                    var kq = _file.UploadVanban(file, idvanban, (int)enumAttachVanban.intloai.Vanbandi);

                }
            }
            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult RemoveVBDi(string[] fileNames, int idvanban)
        {
            // The parameter of the Remove action must be called "fileNames"
            if (fileNames != null)
            {
                _file.DeleteFileUploadVanban(fileNames, idvanban, (int)enumAttachVanban.intloai.Vanbandi);
            }
            // Return an empty string to signify success
            return Content("");
        }
        #endregion UploadVBDi

        #region UploadYkien

        public ActionResult UploadYkien(IEnumerable<HttpPostedFileBase> files, int idhoso, int idykien)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    //var fileName = Path.GetFileName(file.FileName);
                    //var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                    //file.SaveAs(physicalPath);

                    int idloai = (int)enumAttachHoso.intloai.Ykien;
                    var kq = _file.UploadHoso(file, idhoso, idykien, idloai);

                }
            }
            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult RemoveYkien(string[] fileNames, int idhoso, int idykien)
        {
            // The parameter of the Remove action must be called "fileNames"
            if (fileNames != null)
            {
                int idloai = (int)enumAttachHoso.intloai.Ykien;
                _file.DeleteFileUploadHoso(fileNames, idhoso, idykien, idloai);
            }
            // Return an empty string to signify success
            return Content("");
        }

        #endregion UploadYkien

        #region ImageProfile

        public ActionResult UploadImageProfile(IEnumerable<HttpPostedFileBase> files)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    //var fileName = Path.GetFileName(file.FileName);
                    //var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                    //file.SaveAs(physicalPath);

                    var kq = _file.UploadImageProfile(file);

                    TempData["Message"] = kq.message;
                }
            }
            // Return an empty string to signify success
            //return Content("");
            return RedirectToAction("Option", "Account");
        }

        public ActionResult RemoveImageProfile(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"
            if (fileNames != null)
            {
                //_file.DeleteFileUploadVanban(fileNames, iduser, (int)enumAttachVanban.intloai.Vanbanden);
            }
            // Return an empty string to signify success
            return Content("");
        }

        #endregion ImageProfile
    }
}