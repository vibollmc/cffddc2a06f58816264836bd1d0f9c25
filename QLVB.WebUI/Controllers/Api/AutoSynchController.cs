using QLVB.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using QLVB.WebUI.Common.Files;
using System.IO;
using QLVB.WebUI.Models;

namespace QLVB.WebUI.Controllers.Api
{
    public class AutoSynchController : ApiController
    {
        public Task<IEnumerable<FileDesc>> Post()
        {
            //upload va luu tai folder Dongbo

            //HttpRuntimeSection section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
            //section.MaxRequestLength = 10000;
            //section.ExecutionTimeout = 360;

            // kiem tra xem co cho phep module dong bo chay khong
            bool autoSynch = AppSettings.AutoSynch;
            if (autoSynch == false) { return null; }

            string folderSynch = FileServices.SetPathSynch();

            string rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, String.Empty);

            string userSynch = AppSettings.UserSynch;
            string passSynch = WebApiFileServices.GetHeaderValue(userSynch, Request);
            try
            {
                if (WebApiFileServices.ValidUser(userSynch, passSynch))
                {
                    if (Request.Content.IsMimeMultipartContent())
                    {
                        var streamProvider = new CustomMultipartFormDataStreamProvider(folderSynch);
                        var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith<IEnumerable<FileDesc>>(t =>
                        {

                            if (t.IsFaulted || t.IsCanceled)
                            {
                                throw new HttpResponseException(HttpStatusCode.InternalServerError);
                            }

                            IEnumerable<FileDesc> fileInfo = streamProvider.FileData.Select(i =>
                            {
                                //string tenfile = Path.GetFileName(i.Headers.ContentDisposition.FileName.Trim('"'));

                                var info = new FileInfo(i.LocalFileName);
                                string filename = info.Name;
                                long filelength = info.Length;
                                WebApiFileServices.SetFileSynch(filename);

                                var _file = new FileDesc(0, filename, rootUrl + "/" + AppConts.FileDongbo + "/" + filename, filelength / 1024);

                                return _file;

                            });
                            return fileInfo;
                        });

                        return task;
                    }
                    else
                    {
                        //_logger.Error(HttpStatusCode.NotAcceptable.ToString());
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
                    }
                }
                else
                {
                    //_logger.Warn("Không có quyền đồng bộ file");                    
                    //throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is forbidden"));
                    return null;
                }
            }
            catch
            {
                //_logger.Error(ex.Message);
                return null;
            }
        }
    }
}
