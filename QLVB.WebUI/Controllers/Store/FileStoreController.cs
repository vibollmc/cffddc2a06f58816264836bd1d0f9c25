using QLVB.Domain.Entities;
using QLVB.DTO.File;
using QLVB.WebUI.Common.OpenXML;
using Store.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLVB.WebUI.Controllers.Store
{
    public class FileStoreController : Controller
    {
        #region Constructor

        private IStoreFileManager _file;

        public FileStoreController(IStoreFileManager file)
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


        public ActionResult DownloadFile(int idfile, int intloai)
        {
            FileDownloadResult file = _file.DownloadVanban(0, 0);
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
                    //file = _file.DownloadVBDT(idfile, (int)enumAttachMail.intloai.Vanbandendientu);
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
    }
}