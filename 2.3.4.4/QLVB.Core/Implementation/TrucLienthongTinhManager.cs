using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.DTO.Truclienthongtinh;
using QLVB.Core.Contract;
using QLVB.Common.Logging;
using QLVB.Common.Sessions;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;

using System.Net;
using System.IO;
using System.Xml.Serialization;
using System.Net.Sockets;

using Newtonsoft.Json;
using System.Xml;
using System.Data;
using System.Net.Http;


using QLVB.Core.WebServiceTruclienthongTinh;




namespace QLVB.Core.Implementation
{
    public class TrucLienthongTinhManager:ITrucLienthongTinhManager
    {
        #region Constructor
        private ILogger _logger;
        private ISessionServices _session;
        private IConfigRepository _configRepo;

        public TrucLienthongTinhManager(
               ILogger logger, IConfigRepository configRepo, ISessionServices session)
        {
            _logger = logger;
            _configRepo = configRepo;
            _session = session;
        }

        #endregion Constructor

        private ConfigTruclienthong _GetConfigTruc()
        {
            try
            {
                ConfigTruclienthong config = new ConfigTruclienthong();

                config.TrucLienthongTinh = _configRepo.GetConfig(ThamsoHethong.TrucLienthongTinh);
                config.UsernameTrucTinh = _configRepo.GetConfig(ThamsoHethong.UsernameTrucTinh);
                config.PasswordTrucTinh = _configRepo.GetConfig(ThamsoHethong.PasswordTrucTinh);
                config.MaDonviTrucTinh = _configRepo.GetConfig(ThamsoHethong.MaDonviTrucTinh);

                return config;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
            
        }

        public NSSGatewayServiceSoapService ConnectGateway()
        {
            try
            {
                ConfigTruclienthong config = _GetConfigTruc();
                NSSGatewayServiceSoapService webService = new NSSGatewayServiceSoapService();
                webService.Url = config.TrucLienthongTinh;

                NetworkCredential networkCredential = new NetworkCredential(config.UsernameTrucTinh, config.PasswordTrucTinh);
                Uri uri = new Uri(config.TrucLienthongTinh);

                ICredentials credentials = networkCredential.GetCredential(uri, "Basic");
                webService.Credentials = credentials;
                return webService;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
        }
        public List<OrganizationVM> GetAllOrganization()
        {
            try
            {
                List<OrganizationVM> AllDonvi = new List<OrganizationVM>();
                OrganizationVM donvi;
                NSSGatewayServiceSoapService webService = ConnectGateway();
                string[] Organizations = webService.getOrganizations("1");
                foreach (var obj in Organizations)
                {
                    donvi = Deserialize<OrganizationVM>(obj);
                    donvi.ChuanLienThong = (int)(2803);
                    AllDonvi.Add(donvi);
                }
                return AllDonvi;
            }
            catch
            {
                throw;
            }

        }

        public bool GuiCongVanDenLienThongGSoft(int DonViCVID, CongvandiVM Congvandi, 
                            List<OrganizationVM> NoiNhanList, string userName)
        {
            try
            {
                bool isCompleted = false;
                NSSGatewayServiceSoapService webService = ConnectGateway();
                string urlUploadFile = webService.getUploadFileURL(GetIPAddress()); //IP may client

                StringBuilder data = new StringBuilder();
                string receivingsystemid = "";
                foreach (var obj in NoiNhanList)
                {
                    receivingsystemid += obj.code + ";";
                }
                receivingsystemid = receivingsystemid.TrimEnd(";".ToCharArray());

                DateTime senddate = DateTime.UtcNow;
                DateTime Jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                long senddateLongTime = (long)((senddate - Jan1St1970).TotalMilliseconds);
                
                string content = "";
                string MadinhdanhTruc = _configRepo.GetConfig(ThamsoHethong.MaDonviTrucTinh);
                
                    content = WriteCongVanDenToXML(Congvandi);
                    data = new StringBuilder();
                    data.Append("<message>");
                    data.Append("<required-answer><![CDATA[0]]></required-answer>");
                    data.Append("<send-date><![CDATA[" + senddateLongTime + "]]></send-date>");
                    data.Append("<sending-system-id><![CDATA[" + MadinhdanhTruc + "]]></sending-system-id>");
                    data.Append("<receiving-system-id><![CDATA[" + receivingsystemid + "]]></receiving-system-id>");
                    data.Append("<document-type><![CDATA[1.1.0.1]]></document-type>");
                    data.Append("<document-code><![CDATA[" + Congvandi.SoKyHieu + "]]></document-code>");
                    data.Append("<description><![CDATA[" + Congvandi.TrichYeu + "]]></description>");
                    data.Append("<content><![CDATA[" + content + "]]></content>");
                  
                    //Kiểm tra upload File
                    string AttachFilePath = System.Web.HttpContext.Current.Server.MapPath("~/FileAttach" + "/" + Congvandi.IDFileAttach);
                    if (!string.IsNullOrEmpty(Congvandi.AttachFile_Path))
                    {
                        data.Append("<attach-files>");
                        string[] AttachmentFiles = Congvandi.AttachFile_Path.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        int k = 0;
                        for (int i = 0; i < AttachmentFiles.Length; i++)
                        {
                            string fullFileName = AttachFilePath + "\\" + AttachmentFiles[i];
                            //if (File.Exists(fullFileName))
                            //{
                            //    var fileMetaTemp = GetMetadataFileUpload(fullFileName);
                            //    GSoftResultUploadFile objGSoftResultUploadFile = UploadFile(urlUploadFile, "file_meta_data", fileMetaTemp, "file", fullFileName);
                            //    if (objGSoftResultUploadFile.httpCode == "200")
                            //    {
                            //        data.Append("<attach-file>");
                            //        data.Append("<attach-file-id><![CDATA[" + objGSoftResultUploadFile.fileEntryId + "]]></attach-file-id>");
                            //        data.Append("<file-name><![CDATA[" + objGSoftResultUploadFile.fileName + "]]></file-name>");
                            //        data.Append("<extension><![CDATA[" + objGSoftResultUploadFile.extension + "]]></extension>");
                            //        data.Append("<mime-type><![CDATA[" + objGSoftResultUploadFile.mimeType + "]]></mime-type>");
                            //        data.Append("<title><![CDATA[" + objGSoftResultUploadFile.title + "]]></title>");
                            //        data.Append("<description><![CDATA[" + objGSoftResultUploadFile.description + "]]></description>");
                            //        data.Append("<extra-settings><![CDATA[" + objGSoftResultUploadFile.extraSettings + "]]></extra-settings>");
                            //        data.Append("<file-size><![CDATA[" + objGSoftResultUploadFile.fileSize + "]]></file-size>");
                            //        data.Append("</attach-file>");
                            //    }
                            //    k++;
                            //}
                        }
                        data.Append("</attach-files>");
                    }
                    data.Append("</message>");
                    webService.sendMessage(data.ToString());
                    isCompleted = true;
               
                return isCompleted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string WriteCongVanDenToXML(CongvandiVM congvandi)
        {
            try
            {
                string sXML = 
                    "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                        + "<van-ban-qua-mang>"
                            + "<ten-loai-van-ban> <![CDATA[" + congvandi.TenLoaiCV + "]]> </ten-loai-van-ban>"
                            + "<ma-loai-van-ban> <![CDATA[]]> </ma-loai-van-ban>"
                            + "<so-ky-hieu-van-ban> <![CDATA[" + congvandi.SoKyHieu + "]]> </so-ky-hieu-van-ban>"
                            + "<ten-noi-phat-hanh> <![CDATA["  + congvandi.NoiGuiName + "]]> </ten-noi-phat-hanh>"
                            + "<ma-noi-phat-hanh> <![CDATA[]]> </ma-noi-phat-hanh>"
                            + "<ngay-phat-hanh> <![CDATA[1439139600000]]> </ngay-phat-hanh>"
                            + "<nguoi-ky> <![CDATA[]]> </nguoi-ky>"
                            + "<noi-dung-trich-yeu> <![CDATA[" + congvandi.TrichYeu + "]]> </noi-dung-trich-yeu>"
                            + "<ten-cap-gui> <![CDATA[]]> </ten-cap-gui>"
                            + "<ma-cap-gui> <![CDATA[]]> </ma-cap-gui>"
                            + "<truong-mo-rong> <![CDATA[]]> </truong-mo-rong>"
                            + "<van-ban-goc-id> <![CDATA[]]> </van-ban-goc-id>"
                            + "<ma-phong-ban-so-hoa> <![CDATA[]]> </ma-phong-ban-so-hoa>"
                            + "<thu-tu-so-hoa> <![CDATA[0]]> </thu-tu-so-hoa>"
                            + "<ma-nguoi-so-hoa> <![CDATA[]]> </ma-nguoi-so-hoa>" 
                            + "<trang-thai-tep-tin-dinh-kem> <![CDATA[]]> </trang-thai-tep-tin-dinh-kem>"
                            + "<thu-tu-gui> <![CDATA[0]]> </thu-tu-gui>"
                            + "<chuc-vu-nguoi-ky> <![CDATA[]]> </chuc-vu-nguoi-ky>"
                            + "<so-to> <![CDATA[0]]> </so-to>"
                            + "<so-ban> <![CDATA[0]]> </so-ban>"
                            + "<do-khan> <![CDATA[0]]> </do-khan>"
                            + "<phan-loai-van-ban> <![CDATA[0]]> </phan-loai-van-ban>"
                            + "<thoi-han-chi-dao> <![CDATA[0]]> </thoi-han-chi-dao>"
                            + "<don-vi-chi-dao> <![CDATA[]]> </don-vi-chi-dao>" 
                            + "<so-phat-hanh-chi-dao> <![CDATA[]]> </so-phat-hanh-chi-dao>" 
                            + "<ngay-ban-hanh-chi-dao> <![CDATA[]]> </ngay-ban-hanh-chi-dao>"
                        + " </van-ban-qua-mang>";
                return Base64Encode(sXML);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region PrivateMethods

        private string GetIPAddress()
        {
            string ipAddress = "";
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            ipAddress = Convert.ToString(ipHostInfo.AddressList
                                        .FirstOrDefault(address => address.AddressFamily == AddressFamily.InterNetwork));
            return ipAddress;
        }

        private  string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        private  string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        private string RemoveXmlDeclaration(string strXML)
        {
            return strXML.Remove(0, strXML.IndexOf(@"?>", 0) + 2).Replace("\n", "");
        }
        private T Deserialize<T>(string input) where T : class
        {
            input = RemoveXmlDeclaration(input);
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }

        private string Serialize<T>(T ObjectToSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, ObjectToSerialize);
                return textWriter.ToString();
            }
        }

        #endregion PrivateMethods
    }
}
