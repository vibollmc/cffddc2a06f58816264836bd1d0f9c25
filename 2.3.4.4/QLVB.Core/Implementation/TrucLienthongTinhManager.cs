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
using System.Globalization;
using System.Net.Http;
using System.Web;
using LinqToLdap;
using QLVB.Common.Utilities;
using QLVB.Core.WebServiceTruclienthongTinh;
using QLVB.DTO;


namespace QLVB.Core.Implementation
{
    public class TrucLienthongTinhManager:ITrucLienthongTinhManager
    {
        #region Constructor & private variables
        private readonly ILogger _logger;
        private readonly ISessionServices _session;
        private readonly IConfigRepository _configRepo;
        private readonly IVanbandiRepository _vanbandiRepository;
        private readonly IPhanloaiVanbanRepository _phanloaiVanbanRepository;
        private readonly ITinhchatvanbanRepository _tinhchatvanbanRepository;
        private readonly IAttachVanbanRepository _attachVanbanRepository;
        private readonly IFileManager _fileManager;
        private readonly IVanbandenmailRepository _vanbandenmailRepository;
        private readonly IMailFormatManager _mailFormatManager;
        public TrucLienthongTinhManager(
               ILogger logger, IConfigRepository configRepo, ISessionServices session, IVanbandiRepository vanbandiRepository, IPhanloaiVanbanRepository phanloaiVanbanRepository, ITinhchatvanbanRepository tinhchatvanbanRepository, IAttachVanbanRepository attachVanbanRepository, IFileManager fileManager, IVanbandenmailRepository vanbandenmailRepository, IMailFormatManager mailFormatManager)
        {
            _logger = logger;
            _configRepo = configRepo;
            _session = session;
            _vanbandiRepository = vanbandiRepository;
            _phanloaiVanbanRepository = phanloaiVanbanRepository;
            _tinhchatvanbanRepository = tinhchatvanbanRepository;
            _attachVanbanRepository = attachVanbanRepository;
            _fileManager = fileManager;
            _vanbandenmailRepository = vanbandenmailRepository;
            _mailFormatManager = mailFormatManager;
        }

        #endregion Constructor

        #region implement functions
        public NSSGatewayServiceSoapService ConnectGateway()
        {
            try
            {
                var config = _GetConfigTruc();
                var webService = new NSSGatewayServiceSoapService {Url = config.TrucLienthongTinh};

                var networkCredential = new NetworkCredential(config.UsernameTrucTinh, config.PasswordTrucTinh);
                var uri = new Uri(config.TrucLienthongTinh);

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
                var AllDonvi = new List<OrganizationVM>();
                OrganizationVM donvi;
                var webService = ConnectGateway();
                var Organizations = webService.getOrganizations("1");
                var madonviTrucTinh = _configRepo.GetConfig(ThamsoHethong.MaDonviTrucTinh);
                foreach (var obj in Organizations)
                {
                    donvi = Deserialize<OrganizationVM>(obj);
                    donvi.ChuanLienThong = (int)(2803);

                    if (madonviTrucTinh == donvi.code)
                    {
                        AllDonvi.Insert(0, donvi);
                    }
                    else
                    {
                        AllDonvi.Add(donvi);
                    }
                }
                return AllDonvi;
            }
            catch
            {
                throw;
            }

        }
        
        public bool GuiVanBan(int vanbandiId, IList<OrganizationVM> noiNhan)
        {
            try
            {
                var vanbandi = _vanbandiRepository.Vanbandis.FirstOrDefault(x => x.intid == vanbandiId);

                var webService = ConnectGateway();
                var urlUploadFile = webService.getUploadFileURL(GetIPAddress()); //IP may client

                var data = new StringBuilder();
                var receivingsystemid = "";
                foreach (var obj in noiNhan)
                {
                    receivingsystemid += obj.code + ";";
                }
                receivingsystemid = receivingsystemid.TrimEnd(";".ToCharArray());

                var senddate = DateTime.UtcNow;
                var Jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var senddateLongTime = (long)((senddate - Jan1St1970).TotalMilliseconds);

                var content = "";
                var MadinhdanhTruc = _configRepo.GetConfig(ThamsoHethong.MaDonviTrucTinh);

                content = WriteVanBanDiToXml(vanbandi);
                data = new StringBuilder();
                data.Append("<message>");
                data.Append("<required-answer><![CDATA[0]]></required-answer>");
                data.Append("<send-date><![CDATA[" + senddateLongTime + "]]></send-date>");
                data.Append("<sending-system-id><![CDATA[" + MadinhdanhTruc + "]]></sending-system-id>");
                data.Append("<receiving-system-id><![CDATA[" + receivingsystemid + "]]></receiving-system-id>");
                data.Append("<document-type><![CDATA[1.1.0.1]]></document-type>");
                data.Append("<document-code><![CDATA[" + vanbandi.intso + "/" + vanbandi.strkyhieu + "]]></document-code>");
                data.Append("<description><![CDATA[" + vanbandi.strtrichyeu + "]]></description>");
                data.Append("<content><![CDATA[" + content + "]]></content>");

                var attachments = GetFileAttachments(vanbandiId);
                if (attachments != null && attachments.Count > 0)
                {
                    data.Append("<attach-files>");
                    var k = 0;
                    foreach (var attachment in attachments)
                    {
                        if (File.Exists(attachment))
                        {
                            var fileMetaTemp = GetMetadataFileUpload(attachment);
                            var resultUploadFile = UploadFile(urlUploadFile, "file_meta_data", fileMetaTemp, "file", attachment);
                            if (resultUploadFile.httpCode == "200")
                            {
                                data.Append("<attach-file>");
                                data.Append("<attach-file-id><![CDATA[" + resultUploadFile.fileEntryId + "]]></attach-file-id>");
                                data.Append("<file-name><![CDATA[" + resultUploadFile.fileName + "]]></file-name>");
                                data.Append("<extension><![CDATA[" + resultUploadFile.extension + "]]></extension>");
                                data.Append("<mime-type><![CDATA[" + resultUploadFile.mimeType + "]]></mime-type>");
                                data.Append("<title><![CDATA[" + resultUploadFile.title + "]]></title>");
                                data.Append("<description><![CDATA[" + resultUploadFile.description + "]]></description>");
                                data.Append("<extra-settings><![CDATA[" + resultUploadFile.extraSettings + "]]></extra-settings>");
                                data.Append("<file-size><![CDATA[" + resultUploadFile.fileSize + "]]></file-size>");
                                data.Append("</attach-file>");
                            }
                            k++;
                        }
                    }
                    data.Append("</attach-files>");
                }
                data.Append("</message>");
                webService.sendMessage(data.ToString());
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public ResultFunction NhanVanBan()
        {
            var kq = new ResultFunction {id = -1};
            try
            {
                var clientDownLoadFile = new WebClient();
                var webService = ConnectGateway();
                var loaiVb = _configRepo.GetConfig(ThamsoHethong.LoaiVanbanTrucTinh);
                var receivedMessageIdsByDocument = webService.getReceivedMessageIdsByDocumentType(loaiVb);
                
                foreach (var messageIdsByDocument in receivedMessageIdsByDocument)
                {
                    var sRespone = webService.getMessageByMessageId(messageIdsByDocument);
                    var objReceivedMessage = Deserialize<QlvbReceivedMessage>(sRespone);
                    objReceivedMessage.attachfiles = getAttachFile(sRespone);
                    sRespone = Base64Decode(objReceivedMessage.content);
                    var objDocument = Deserialize<QlvbDocument>(sRespone);
                    objDocument.attachfiles = objReceivedMessage.attachfiles;
                    //xử lý văn bản

                    var vbdenMail = new Vanbandenmail();
                    vbdenMail.intattach = objDocument.attachfiles != null && objDocument.attachfiles.Count > 0 ? (int)enumVanbandenmail.intattach.Co : (int)enumVanbandenmail.intattach.Khong;


                    if (!string.IsNullOrWhiteSpace(objDocument.tenloaivanban))
                    {
                        var loaivb =
                            _phanloaiVanbanRepository.GetActivePhanloaiVanbans.FirstOrDefault(
                                x => string.Equals(x.strtenvanban, objDocument.tenloaivanban, StringComparison.CurrentCultureIgnoreCase));
                        if (loaivb != null)
                        {
                            vbdenMail.intidphanloaivanbanden = loaivb.intid;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(objDocument.dokhan))
                    {
                        var dokhan =
                            _tinhchatvanbanRepository.GetActiveTinhchatvanbans.FirstOrDefault(
                                x => string.Equals(x.strtentinhchatvb, objDocument.dokhan, StringComparison.CurrentCultureIgnoreCase));
                        if (dokhan != null)
                        {
                            vbdenMail.intkhan = dokhan.intid;
                        }
                    }

                    vbdenMail.strtrichyeu = objDocument.noidungtrichyeu;
                    vbdenMail.strngaynhanvb = DateTime.Now;

                    DateTime outDateTime;
                    var jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    double outd;
                    int outi;

                    if (double.TryParse(objDocument.ngayphathanh, out outd))
                    {
                        vbdenMail.strngayky = jan1St1970.AddMilliseconds(outd);
                    }
                    else if (DateTime.TryParseExact(objDocument.ngayphathanh, "dd/MM/yyyy", null, DateTimeStyles.None, out outDateTime))
                    {
                        vbdenMail.strngayky = outDateTime;
                    }

                    vbdenMail.strkyhieu = objDocument.sokyhieuvanban;
                    vbdenMail.strnoiguivb = objDocument.tennoiphathanh;

                    vbdenMail.strnguoiky = objDocument.nguoiky;

                    if (int.TryParse(objDocument.soto, out outi))
                    {
                        vbdenMail.intsoto = outi;
                    }
                    if (int.TryParse(objDocument.soban, out outi))
                    {
                        vbdenMail.intsoban = outi;
                    }


                    //Lưu tập tin đính kèm 

                    var idmail = _vanbandenmailRepository.Them(vbdenMail);

                    if (idmail > 0 && objDocument.attachfiles != null &&
                        objDocument.attachfiles.Count > 0)
                    {
                        foreach (var item in objDocument.attachfiles)
                        {
                            try
                            {
                                var strmota = item.filename;
                                var fileSavepath = _mailFormatManager.SaveAttachment(idmail, strmota, _fileManager.SetPathUpload(AppConts.FileEmail));

                                var urlDownloadFile = webService.getDownloadFileURL(GetIPAddress(), item.attachfileid);
                                var content = clientDownLoadFile.DownloadData(urlDownloadFile);
                                File.WriteAllBytes(fileSavepath, content);

                                _mailFormatManager.InsertAttachment(idmail, fileSavepath, strmota, (int)enumAttachMail.intloai.Vanbandendientu);
                            }
                            catch (Exception ex) // catch 404
                            {
                            }
                        }
                    }
                    //sau khi lấy văn bản, xác nhận  văn bản đã lấy thành công
                    webService.updateReceiveFinish(messageIdsByDocument);
                }

                kq.id = (int) ResultViewModels.Success;
                kq.message = receivedMessageIdsByDocument.Count().ToString();
                
            }
            catch (Exception ex)
            {
                kq.message = ex.Message;
            }

            return kq;
        }
        #endregion implement functions

        #region PrivateMethods

        private IList<string> GetFileAttachments(int vanbandiId)
        {
            var fileAttachs = Enumerable.ToList(_attachVanbanRepository.AttachVanbans
                    .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbandi && p.intidvanban == vanbandiId)
                    .OrderBy(p => p.intid));

            return fileAttachs.Select(fileAttach => GetFileAttachments(fileAttach)).ToList();
        }

        private string GetFileAttachments(AttachVanban attach)
        {
            var idcanbo = _session.GetUserId();
            var idvanban = (int)attach.intidvanban;

            var strLoaiFile = _fileManager.CheckFolderFileVanbanDownload((int)enumAttachVanban.intloai.Vanbandi, attach.intid, idcanbo, idvanban);
            if (string.IsNullOrEmpty(strLoaiFile))
            {
                return null;
            }

            var filename = attach.strtenfile;
            var folderPath = _fileManager.GetFolderDownload(strLoaiFile, (DateTime)attach.strngaycapnhat);
            var filepath = folderPath + "/" + filename; //Server.MapPath(folderPath);

            //var fileData = GetFileData(filename, filepath);
            return HttpContext.Current.Server.MapPath(filepath);
        }

        private string WriteVanBanDiToXml(Vanbandi vanbandi)
        {
            try
            {
                var tenLoaiVb = string.Empty;
                var maLoaiVb = string.Empty;
                var tendonviTrucTinh = _configRepo.GetConfig(ThamsoHethong.TenDonviTrucTinh);
                var madonviTrucTinh = _configRepo.GetConfig(ThamsoHethong.MaDonviTrucTinh);
                var ngayKy = vanbandi.strngayky;
                var jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var ngayKyEncode = (long)((ngayKy - jan1St1970).TotalMilliseconds);
                var dokhanVb = string.Empty;

                if (vanbandi.intidphanloaivanbandi.HasValue)
                {
                    var loaivb = _phanloaiVanbanRepository.GetLoaiVB(vanbandi.intidphanloaivanbandi.Value);
                    if (loaivb != null)
                    {
                        tenLoaiVb = loaivb.strtenvanban;
                        maLoaiVb = loaivb.strmavanban;
                    }
                }
                if (vanbandi.intidkhan.HasValue)
                {
                    var dokhan =
                        _tinhchatvanbanRepository.GetAllTinhchatvanbans.FirstOrDefault(
                            x => x.intid == vanbandi.intidkhan);
                    if (dokhan != null)
                    {
                        dokhanVb = dokhan.strtentinhchatvb;
                    }
                }

                var sXML =
                    "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                        + "<van-ban-qua-mang>"
                            + "<ten-loai-van-ban> <![CDATA[" + tenLoaiVb + "]]> </ten-loai-van-ban>"
                            + "<ma-loai-van-ban> <![CDATA[" + maLoaiVb + "]]> </ma-loai-van-ban>"
                            + "<so-ky-hieu-van-ban> <![CDATA[" + vanbandi.intso + "/" + vanbandi.strkyhieu + "]]> </so-ky-hieu-van-ban>"
                            + "<ten-noi-phat-hanh> <![CDATA[" + tendonviTrucTinh + "]]> </ten-noi-phat-hanh>"
                            + "<ma-noi-phat-hanh> <![CDATA[" + madonviTrucTinh + "]]> </ma-noi-phat-hanh>"
                            + "<ngay-phat-hanh> <![CDATA[" + ngayKyEncode + "]]> </ngay-phat-hanh>"
                            + "<nguoi-ky> <![CDATA[" + vanbandi.strnguoiky + "]]> </nguoi-ky>"
                            + "<noi-dung-trich-yeu> <![CDATA[" + vanbandi.strtrichyeu + "]]> </noi-dung-trich-yeu>"
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
                            + "<so-to> <![CDATA[" + vanbandi.intsoto + "]]> </so-to>"
                            + "<so-ban> <![CDATA[" + vanbandi.intsoban + "]]> </so-ban>"
                            + "<do-khan> <![CDATA[" + dokhanVb + "]]> </do-khan>"
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
        private ConfigTruclienthong _GetConfigTruc()
        {
            try
            {
                var config = new ConfigTruclienthong();

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

        private string GetIPAddress()
        {
            var ipAddress = "";
            var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
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
        private string RemoveXmlDeclaration(string strXml)
        {
            return strXml.Remove(0, strXml.IndexOf(@"?>", 0) + 2).Replace("\n", "");
        }
        private T Deserialize<T>(string input) where T : class
        {
            input = RemoveXmlDeclaration(input);
            var ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (var sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }

        private string Serialize<T>(T objectToSerialize)
        {
            var xmlSerializer = new XmlSerializer(objectToSerialize.GetType());

            using (var textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, objectToSerialize);
                return textWriter.ToString();
            }
        }

        private string GetMetadataFileUpload(string filePath)
        {
            var fileMetaData = new FileMetadataUpload();
            if (!System.IO.File.Exists(filePath))
            {
                return string.Empty;
            }
            else
            {
                var fileInfo = new FileInfo(filePath);

                fileMetaData.FileName = fileInfo.Name;
                fileMetaData.Extension = fileInfo.Extension;
                fileMetaData.Title = fileInfo.Name;
                fileMetaData.Description = string.Empty;
                fileMetaData.ExtraSettings = string.Empty;
                return JsonConvert.SerializeObject(fileMetaData);
            }
        }

        private ResultUploadFile UploadFile(string url, string fileMetaDataParamName, string fileMetaDataValue, string fileParamName, string filePath)
        {
            var objResultUploadFile = new ResultUploadFile();
            Stream fs = null;
            try
            {
                var config = _GetConfigTruc();

                using (var handler = new HttpClientHandler { Credentials = new NetworkCredential(config.UsernameTrucTinh, config.PasswordTrucTinh) })
                {
                    var client = new HttpClient(handler) { Timeout = TimeSpan.FromMinutes(6) };
                    client.DefaultRequestHeaders.Add("Connection", "Keep-alive");
                    fs = File.OpenRead(filePath);
                    var streamContent = new StreamContent(fs, 10000);
                    streamContent.Headers.Add("Content-Type", "application/octet-stream");
                    streamContent.Headers.Add("Content-Disposition", "form-data; name=\"file\"; filename=\"" + Path.GetFileName(filePath) + "\"");
                    var mulContent = new MultipartFormDataContent();
                    mulContent.Add(new StringContent(fileMetaDataValue), fileMetaDataParamName);
                    mulContent.Add(streamContent, "file", Path.GetFileName(filePath));
                    var message = client.PostAsync(url, mulContent);

                    var input = message.Result.Content.ReadAsStringAsync();
                    objResultUploadFile = JsonConvert.DeserializeObject<ResultUploadFile>(input.Result);
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                if (fs != null)
                {
                    try
                    {
                        fs.Close();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return objResultUploadFile;
        }

        private List<AttachFile> getAttachFile(string xmlInput)
        {
            var source = new List<AttachFile>();
            var xml = new XmlDocument();
            xml.LoadXml(xmlInput);
            var xnList = xml.SelectNodes("message/attach-files/attach-file");
            if (xnList == null) return source;
            foreach (XmlNode xn in xnList)
            {
                if (string.IsNullOrEmpty(xn.InnerText)) continue;

                var objAttachFile = new AttachFile
                {
                    attachfileid = xn["attach-file-id"].InnerText,
                    filename = xn["file-name"].InnerText,
                    extension = xn["extension"].InnerText,
                    mimetype = xn["mime-type"].InnerText,
                    title = xn["title"].InnerText,
                    description = xn["description"].InnerText,
                    extrasettings = xn["extra-settings"].InnerText,
                    filesize = xn["file-size"].InnerText
                };
                source.Add(objAttachFile);
            }
            return source;
        }

        #endregion PrivateMethods

        #region public related class
        public class ResultUploadFile
        {
            public string fileSize { get; set; }
            public string extension { get; set; }
            public string title { get; set; }
            public string httpCode { get; set; }
            public string description { get; set; }
            public string fileName { get; set; }
            public string extraSettings { get; set; }
            public string fileEntryId { get; set; }
            public string mimeType { get; set; }
        }

        public class FileMetadataUpload
        {
            public string FileName { get; set; }
            public string Extension { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string ExtraSettings { get; set; }
        }

        [XmlRoot("message")]
        public class QlvbReceivedMessage
        {
            [XmlElement("message-id")]
            public string messageid { get; set; }
            [XmlElement("for-message-id")]
            public string formessageid { get; set; }
            [XmlElement("required-answer")]
            public string requiredanswer { get; set; }
            [XmlElement("send-date")]
            public string senddate { get; set; }
            [XmlElement("sending-system-id")]
            public string sendingsystemid { get; set; }
            [XmlElement("receiving-system-id")]
            public string receivingsystemid { get; set; }
            [XmlElement("document-type")]
            public string documenttype { get; set; }
            [XmlElement("document-code")]
            public string documentcode { get; set; }
            [XmlElement("description")]
            public string description { get; set; }
            [XmlElement("content")]
            public string content { get; set; }
            [XmlElement("title")]
            public string title { get; set; }
            [XmlElement("department-id")]
            public string departmentid { get; set; }
            [XmlElement("department-send-id")]
            public string departmentsendid { get; set; }
            [XmlElement("option")]
            public string option { get; set; }
            [XmlElement("stateProcess")]
            public string stateProcess { get; set; }
            [XmlElement("signature")]
            public string signature { get; set; }
            [XmlElement("edxml")]
            public string edxml { get; set; }
            [XmlElement("attach-files")]
            public List<AttachFile> attachfiles { get; set; }
        }
        [XmlRoot("attach-file")]
        public class AttachFile
        {
            [XmlElement("attach-file-id")]
            public string attachfileid { get; set; }
            [XmlElement("file-name")]
            public string filename { get; set; }
            [XmlElement("extension")]
            public string extension { get; set; }
            [XmlElement("mime-type")]
            public string mimetype { get; set; }
            [XmlElement("title")]
            public string title { get; set; }
            [XmlElement("description")]
            public string description { get; set; }
            [XmlElement("extra-settings")]
            public string extrasettings { get; set; }
            [XmlElement("file-size")]
            public string filesize { get; set; }

        }

        [XmlRoot("van-ban-qua-mang")]
        public class QlvbDocument
        {
            [XmlElement("ten-loai-van-ban")]
            public string tenloaivanban { get; set; }
            [XmlElement("ma-loai-van-ban")]
            public string maloaivanban { get; set; }
            [XmlElement("so-ky-hieu-van-ban")]
            public string sokyhieuvanban { get; set; }
            [XmlElement("ten-noi-phat-hanh")]
            public string tennoiphathanh { get; set; }
            [XmlElement("<ma-noi-phat-hanh")]
            public string manoiphathanh { get; set; }
            [XmlElement("ngay-phat-hanh")]
            public string ngayphathanh { get; set; }
            [XmlElement("nguoi-ky")]
            public string nguoiky { get; set; }
            [XmlElement("noi-dung-trich-yeu")]
            public string noidungtrichyeu { get; set; }
            [XmlElement("ten-cap-gui")]
            public string tencapgui { get; set; }
            [XmlElement("ma-cap-gui")]
            public string macapgui { get; set; }
            [XmlElement("truong-mo-rong")]
            public string truongmorong { get; set; }
            [XmlElement("van-ban-goc-id")]
            public string vanbangocid { get; set; }
            [XmlElement("ma-phong-ban-so-hoa")]
            public string maphongbansohoa { get; set; }
            [XmlElement("thu-tu-so-hoa")]
            public string thutusohoa { get; set; }
            [XmlElement("ma-nguoi-so-hoa")]
            public string manguoisohoa { get; set; }
            [XmlElement("trang-thai-tep-tin-dinh-kem")]
            public string trangthaiteptindinhkem { get; set; }
            [XmlElement("thu-tu-gui")]
            public string thutugui { get; set; }
            [XmlElement("chuc-vu-nguoi-ky")]
            public string chucvunguoiky { get; set; }
            [XmlElement("so-to")]
            public string soto { get; set; }
            [XmlElement("so-ban")]
            public string soban { get; set; }
            [XmlElement("do-khan")]
            public string dokhan { get; set; }
            [XmlElement("phan-loai-van-ban")]
            public string phanloaivanban { get; set; }
            [XmlElement("thoi-han-chi-dao")]
            public string thoihanchidao { get; set; }
            [XmlElement("don-vi-chi-dao")]
            public string donvichidao { get; set; }
            [XmlElement("so-phat-hanh-chi-dao")]
            public string sophathanhchidao { get; set; }
            [XmlElement("ngay-ban-hanh-chi-dao")]
            public string ngaybanhanhchidao { get; set; }
            [XmlElement("attach-files")]
            public List<AttachFile> attachfiles { get; set; }
        }
        #endregion
    }
}
