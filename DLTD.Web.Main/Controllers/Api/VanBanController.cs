using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using DLTD.Web.Main.Common;
using DLTD.Web.Main.DAL;
using DLTD.Web.Main.Models.MetaData;
using DLTD.Web.Main.Models.Enum;

namespace DLTD.Web.Main.Controllers.Api
{
    public class VanBanController : ApiController
    {
        // GET api/<controller>
        [EnableCors(origins:"*", headers:"*", methods:"*", SupportsCredentials = true)]
        public string Get()
        {
            var identity = (ClaimsIdentity)User.Identity;
            if (!identity.IsAuthenticated) return "";
            var claim = identity.Claims.SingleOrDefault(x=>x.Type == ClaimTypes.NameIdentifier);

            return claim == null ? null : claim.Value;
        }
        // POST api/<controller>
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public async Task<HttpResponseMessage> Post()
        {
            var httpRequest = HttpContext.Current.Request;
            string fileName = null;
            string filePath = null;
            string fileUrl = null;
            try
            {
                var fileDinhKem = new List<FileDinhKemInput>();

                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        if (postedFile == null || postedFile.ContentLength == 0) continue;
                        var folderUpload = HttpContext.Current.Server.MapPath("~/Uploads/");
                        if (!Directory.Exists(folderUpload)) Directory.CreateDirectory(folderUpload);
                        fileName = postedFile.FileName;
                        var fileNameBeSave = string.Format("{0:yyyyMMddHHmmss}-{1}", DateTime.Now, fileName);
                        fileUrl = string.Format("~/Uploads/{0}", fileNameBeSave);
                        filePath = string.Format("{0}{1}", folderUpload, fileNameBeSave);

                        postedFile.SaveAs(filePath);

                        if (!string.IsNullOrWhiteSpace(fileName) && !string.IsNullOrWhiteSpace(fileUrl))
                        {
                            fileDinhKem.Add(new FileDinhKemInput
                            {
                                TenFile = fileName,
                                UrlFile = fileUrl,
                            });
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(httpRequest.Form["FileVBDinhKem"]))
                {
                    var arrFiles = httpRequest.Form["FileVBDinhKem"].Split('$')
                        .Where(f => !string.IsNullOrWhiteSpace(f))
                        .Select(f => f.Split('*'))
                        .Where(arr => arr.Length == 2);
                    foreach (var arr in arrFiles)
                    {
                        string url;
                        if (DownloadFile(arr[0], arr[1], out url))
                        {
                            fileDinhKem.Add(new FileDinhKemInput
                            {
                                TenFile = arr[1],
                                UrlFile = url
                            });
                        }
                    }
                }

                var data = new VanBanChiDaoInput
                {
                    //KyHieu = httpRequest.Form["KyHieu"],
                    IdDonVi = httpRequest.Form["IdDonVi"].ToIntExt(),
                    IdVanBan = httpRequest.Form["IdVanBan"].ToIntExt(),
                    //NgayDen = httpRequest.Form["NgayDen"].ToDateTimeExt(),
                    //NoiGui = httpRequest.Form["NoiGui"],
                    //SoDen = httpRequest.Form["SoDen"],
                    ThoiHanXuLy = httpRequest.Form["ThoiHanXuLy"].ToDateTimeExt(),
                    //TrichYeuNoiDung = httpRequest.Form["TrichYeuNoiDung"],
                    YKienChiDao = httpRequest.Form["YKienChiDao"],
                    UserId = httpRequest.Form["UserId"].ToIntExt(),
                    DonViPhoiHop = httpRequest.Form["IdDonViPhoiHop"],
                    DoKhan = (DoKhan)(httpRequest.Form["DoKhan"].ToIntExt()),
                    NguonChiDao = httpRequest.Form["IdKhoi"].ToIntExt(),
                    IdNguoiTheoDoi = httpRequest.Form["NguoiTheoDoi"].ToIntExt(),
                    IdNguoiChiDao = httpRequest.Form["NguoiChiDao"].ToIntExt(),
                    SoKH = httpRequest.Form["SoKH"],
                    Ngayky = httpRequest.Form["Ngayky"].ToDateTimeExt(),
                    Trichyeu = httpRequest.Form["Trichyeu"],
                    FileDinhKem = fileDinhKem
                };

                
                var results = await VanBanChiDaoManagement.Go.SaveVanBanChiDaoFromApi(data);

                if (results) return Request.CreateResponse(HttpStatusCode.Created, "OK");

                if (!string.IsNullOrWhiteSpace(filePath)) File.Delete(filePath);

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Cannot create item");
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrWhiteSpace(filePath)) File.Delete(filePath);

                var logFloder = HttpContext.Current.Server.MapPath("~/Log/");
                if (!Directory.Exists(logFloder)) Directory.CreateDirectory(logFloder);

                var logFile = File.CreateText(logFloder + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log");

                logFile.WriteLine(ex.StackTrace);

                logFile.Close();
                logFile.Dispose();

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        private bool DownloadFile(string url, string fileName, out string fileUrl)
        {
            using (var webClient = new WebClient())
            {
                
                try
                {
                    var folderUpload = HttpContext.Current.Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(folderUpload)) Directory.CreateDirectory(folderUpload);

                    fileUrl = string.Format("~/Uploads/{0:yyyyMMddHHmmss}-{1}", DateTime.Now, fileName);
                    var filePath = HttpContext.Current.Server.MapPath(fileUrl);


                    webClient.DownloadFile(new Uri(url), filePath);
                    return true;
                }
                catch(Exception ex)
                {
                    fileUrl = null;
                    var logFloder = HttpContext.Current.Server.MapPath("~/Log/");
                    if (!Directory.Exists(logFloder)) Directory.CreateDirectory(logFloder);

                    var logFile = File.CreateText(logFloder + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log");

                    logFile.WriteLine(ex.StackTrace + "\r\nFileUrl:" + url + " name: " + fileName);

                    logFile.Close();
                    logFile.Dispose();

                    return false;
                }
            }
        }
    }
}