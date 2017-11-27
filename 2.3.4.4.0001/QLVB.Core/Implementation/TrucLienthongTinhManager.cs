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
using INet.StatusXml;
using System.Web.Hosting;


namespace QLVB.Core.Implementation
{
    public class TrucLienthongTinhManager:ITrucLienthongTinhManager
    {
        #region Constructor & private variables
        private readonly ILogger _logger;
        private readonly ISessionServices _session;
        private readonly ICanboRepository _canboRepository;
        private readonly IDonviManager _donviManager;
        private readonly IConfigRepository _configRepo;
        private readonly IVanbandiRepository _vanbandiRepository;
        private readonly IPhanloaiVanbanRepository _phanloaiVanbanRepository;
        private readonly ITinhchatvanbanRepository _tinhchatvanbanRepository;
        private readonly IAttachVanbanRepository _attachVanbanRepository;
        private readonly IFileManager _fileManager;
        private readonly IVanbandenmailRepository _vanbandenmailRepository;
        private readonly IMailFormatManager _mailFormatManager;
        private readonly IMailInboxRepository _mailInboxRepo;
        private readonly IMailOutboxRepository _mailOutboxRepo;
        private readonly IGuiVanbanRepository _guivbRepo;
        private readonly IVanbandientuManager _vbdtManager;      
        private readonly IVanbandenRepository _vbdenRepo;
        private readonly ITochucdoitacRepository _tochucRepo;
        private readonly IVanbandiRepository _vanbandiRepo;
        private readonly ITinhhinhXulyVanBanDiReponsitory _tinhhinhxulyvanbandiRepo;

        public TrucLienthongTinhManager(
               ILogger logger, IConfigRepository configRepo, ISessionServices session, 
               IVanbandiRepository vanbandiRepository, IPhanloaiVanbanRepository phanloaiVanbanRepository, 
               ITinhchatvanbanRepository tinhchatvanbanRepository, IAttachVanbanRepository attachVanbanRepository, 
               IFileManager fileManager, IVanbandenmailRepository vanbandenmailRepository, IMailFormatManager mailFormatManager,
               IMailInboxRepository mailInboxRepo, IMailOutboxRepository mailOutboxRepo, IGuiVanbanRepository guivbRepo,
               IVanbandientuManager vbdtManager, IVanbandenRepository vbdenRepo, ITochucdoitacRepository tochucRepo, IVanbandiRepository vanbandiRepo,
               ICanboRepository canboRepository, IDonviManager donviManager, ITinhhinhXulyVanBanDiReponsitory tinhhinhxulyvanbandiRepo)
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
            _mailInboxRepo = mailInboxRepo;
            _mailOutboxRepo = mailOutboxRepo;          
            _vbdtManager = vbdtManager;
            _vbdenRepo = vbdenRepo;
            _tochucRepo = tochucRepo;
            _vanbandiRepo = vanbandiRepo;
            _guivbRepo = guivbRepo;
            _canboRepository = canboRepository;
            _donviManager = donviManager;
            _tinhhinhxulyvanbandiRepo = tinhhinhxulyvanbandiRepo;
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

                var orgs = this.GetAllOrganization();

                var data = new StringBuilder();
                var receivingsystemid = "";


                foreach (var obj in noiNhan)
                {
                    if (!receivingsystemid.Contains(obj.code + ";"))
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

                // cap nhat trang thai van ban da gui dien tu VBDt
                _mailFormatManager.UpdateVBDT(vanbandiId, (int)enumGuiVanban.intloaivanban.Vanbandi);

                foreach (var donvi in noiNhan)
                {
                    var tochuc = _tochucRepo.GetAllTochucdoitacs.FirstOrDefault(x => x.strmatructinh == donvi.code);

                    var org = orgs.FirstOrDefault(x => x.code.Trim() == donvi.code.Trim());
                    // luu vanban da gui tren truc
                    if (tochuc == null)
                    {
                        _vbdtManager._SaveGuiVanban(vanbandiId, (int) enumGuiVanban.intloaivanban.Vanbandi, null,
                            org != null ? org.name : donvi.name, (int) enumGuiVanban.intloaigui.Tructinh);
                    }
                    else
                    {
                        _vbdtManager._SaveGuiVanban(vanbandiId, (int)enumGuiVanban.intloaivanban.Vanbandi, tochuc.intid,
                            org != null ? org.name : donvi.name, (int)enumGuiVanban.intloaigui.Tructinh);
                    }
                }
                // luu nhat ky gui vbdt
                _LogVBDiTructinh(vanbandiId, data.ToString());

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
                
                // bi loi ngay cho nay!!!!
                var receivedMessageIdsByDocument = webService.getReceivedMessageIdsByDocumentType(loaiVb);

                
                int VBDTPerRequest = AppSettings.AutoEmailPerRequest;
                int dem = 0;

                foreach (var messageIdsByDocument in receivedMessageIdsByDocument)                
                {                  
                    // kiem tra nhan khoang 20 vb/1 lan de tranh treo may
                    if (dem > VBDTPerRequest) { break; }
                    dem++;

                    QlvbReceivedMessage objReceivedMessage = null;
                    var sRespone = string.Empty;
                    try
                    {
                        sRespone = webService.getMessageByMessageId(messageIdsByDocument);

                        objReceivedMessage = Deserialize<QlvbReceivedMessage>(sRespone);
                        objReceivedMessage.attachfiles = getAttachFile(sRespone);
                        sRespone = Base64Decode(objReceivedMessage.content);

                    }
                    catch (Exception ex)
                    {
                        webService.updateErrorMessage(messageIdsByDocument, ex.Message);
                        continue;
                    }

                    // ghi nhat ky  
                    string strheader =  "senddate:" + objReceivedMessage.senddate + " sendid:" + objReceivedMessage.sendingsystemid + " receiveid:" + objReceivedMessage.receivingsystemid;                   
                    int idLog = _LogVBDenTructinh(messageIdsByDocument, sRespone,strheader);

                    // fix loi error xml
                    //var objDocument = Deserialize<QlvbDocument>(sRespone);
                    QlvbDocument objDocument = null;
                    try
                    {                        
                        objDocument = Deserialize<QlvbDocument>(sRespone);                     
                    }
                    catch (Exception ex)  // Skip when got document xml error  
                    {
                        // ghi nhat ky khong giai ma xml duoc                    
                        _UpdateLogVBDenTructinh(idLog, (int)enumMailInbox.inttrangthai.Error);
                        _logger.Error("xml truc tinh: " + ex.Message);
                        //danh dau de khong nhan vb nay nua
                        //webService.updateReceiveFinish(messageIdsByDocument);
                        webService.updateErrorMessage(messageIdsByDocument, ex.Message);

                        continue;
                    }

                    if (objDocument == null) continue;

                    objDocument.attachfiles = objReceivedMessage.attachfiles;

                    //xử lý văn bản

                    var jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

                    var vbdenMail = new Vanbandenmail();

                    if (!string.IsNullOrWhiteSpace(objDocument.tenloaivanban))
                    {
                        var loaivb =
                            _phanloaiVanbanRepository.GetActivePhanloaiVanbans.FirstOrDefault(
                                x => x.strtenvanban.ToLower() == objDocument.tenloaivanban.ToLower());
                        if (loaivb != null)
                        {
                            vbdenMail.intidphanloaivanbanden = loaivb.intid;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(objDocument.dokhan))
                    {
                        var dokhan =
                            _tinhchatvanbanRepository.GetActiveTinhchatvanbans.FirstOrDefault(
                                x => x.strtentinhchatvb.ToLower() == objDocument.dokhan.ToLower());
                        if (dokhan != null)
                        {
                            vbdenMail.intkhan = dokhan.intid;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(objDocument.vanbangocid))
                    {

                        vbdenMail.strvanbangocid = objDocument.vanbangocid;
                       
                    }
                    if (!string.IsNullOrEmpty(objReceivedMessage.senddate))
                    {
                        double dbdate;
                        if (double.TryParse(objReceivedMessage.senddate, out dbdate))
                        {
                            vbdenMail.strngayguivb = jan1St1970.AddMilliseconds(dbdate).ToLocalTime();
                        }
                        else
                        {
                            DateTime ngaygui;
                            if (DateTime.TryParseExact(objReceivedMessage.senddate, "dd/MM/yyyy",
                                                      CultureInfo.InvariantCulture,
                                                      DateTimeStyles.None,
                                                      out ngaygui))
                            {
                                vbdenMail.strngayguivb = ngaygui;
                            }
                        }                        
                    }
                    else
                    {
                        DateTime date;
                        if (DateTime.TryParseExact(objDocument.ngaybanhanhchidao, "dd/MM/yyyy",
                                                   CultureInfo.InvariantCulture,
                                                   DateTimeStyles.None,
                                                   out date))
                        {
                            vbdenMail.strngayguivb = date;
                        }
                        else
                        {
                            // Parse failed
                        }
                    }
                    
                    vbdenMail.strtrichyeu = objDocument.noidungtrichyeu;
                    vbdenMail.strngaynhanvb = DateTime.Now;

                    DateTime outDateTime;
                   
                    double outd;
                    int outi;

                    if (double.TryParse(objDocument.ngayphathanh, out outd))
                    {
                 
                        vbdenMail.strngayky = jan1St1970.AddMilliseconds(outd);
                        if (vbdenMail.strngayky.Value.Hour != 0)
                        {
                            vbdenMail.strngayky = jan1St1970.AddMilliseconds(outd).AddDays(1);
                        }
                    }
                    else if (DateTime.TryParseExact(objDocument.ngayphathanh, "dd/MM/yyyy", null, DateTimeStyles.None, out outDateTime))
                    {
                        vbdenMail.strngayky = outDateTime;
                    }

                    vbdenMail.strkyhieu = objDocument.sokyhieuvanban;
                    vbdenMail.strnoiguivb = objDocument.tennoiphathanh;

                    vbdenMail.strmadinhdanh = (!string.IsNullOrEmpty(objDocument.manoiphathanh)) ? objDocument.manoiphathanh : objReceivedMessage.sendingsystemid ;
                                        

                    vbdenMail.strnguoiky = objDocument.nguoiky;

                    if (int.TryParse(objDocument.soto, out outi))
                    {
                        vbdenMail.intsoto = outi;
                    }
                    if (int.TryParse(objDocument.soban, out outi))
                    {
                        vbdenMail.intsoban = outi;
                    }

                    vbdenMail.intnhanvanbantu = enumVanbandenmail.intnhanvanbantu.TrucLienThongTinh;

                    //Lưu tập tin đính kèm 

                    vbdenMail.intattach = objDocument.attachfiles != null && objDocument.attachfiles.Count > 0 ? (int)enumVanbandenmail.intattach.Co : (int)enumVanbandenmail.intattach.Khong;

                    var idmail = _vanbandenmailRepository.Them(vbdenMail);

                    var getFileSuccess = true;

                    if (idmail > 0 && objDocument.attachfiles != null &&
                        objDocument.attachfiles.Count > 0)
                    {
                        var numberOfAttach = 0;
                        foreach (var item in objDocument.attachfiles)
                        {
                            try
                            {
                                var strmota = item.filename;
                                var fileSavepath = _mailFormatManager.SaveAttachment(idmail, strmota, _fileManager.SetPathUpload(AppConts.FileEmail));
                                var urlDownloadFile = webService.getDownloadFileURL(GetIPAddress(), item.attachfileid);                                                         
                                var content = clientDownLoadFile.DownloadData(urlDownloadFile);
                                File.WriteAllBytes(fileSavepath, content);
                                var idAttach = _mailFormatManager.InsertAttachment(idmail, fileSavepath, strmota, (int)enumAttachMail.intloai.Vanbandendientu);
                                if (idAttach > 0) numberOfAttach++;
                            }
                            catch (Exception ex) // catch 404
                            {
                                getFileSuccess = false;
                                _logger.Error(ex.Message);
                                webService.updateErrorMessage(messageIdsByDocument, "Error download file: " + ex.Message + ", File ID: " + item.attachfileid);
                                continue;
                            }
                        }

                        if (numberOfAttach == 0)
                            _vanbandenmailRepository.UpdateIntAttach(idmail, enumVanbandenmail.intattach.Khong);
                    }
                    if (getFileSuccess)
                    {
                        //sau khi lấy văn bản, xác nhận  văn bản đã lấy thành công
                        webService.updateReceiveFinish(messageIdsByDocument);
                    }
                    SendStatusByIdVanbanDenMail(idmail, "01", "Đã đến","", "");
                }

                //kq.id = (int) ResultViewModels.Success;                
                //kq.message = receivedMessageIdsByDocument.Count().ToString();

                kq.id = receivedMessageIdsByDocument.Count() - dem;
                kq.message = dem.ToString();

            }
            catch (Exception ex)
            {
                kq.message = ex.Message;
            }

            return kq;
        }

        public bool ReceiveStatus()
        {
            try
            {
                var webService = ConnectGateway();
                var loaiVb = "6.1.0.1";
                var receivedMessageIdsByDocument = webService.getReceivedMessageIdsByDocumentType(loaiVb);

                if (receivedMessageIdsByDocument == null || receivedMessageIdsByDocument.Count() == 0) return false;

                var orgs = this.GetAllOrganization();

                foreach (var messageIdsByDocument in receivedMessageIdsByDocument)
                {
                    try
                    {
                        Envelope status = null;
                        QlvbReceivedMessage objMessageStatus = null;
                        try
                        {
                            var sRespone = webService.getMessageByMessageId(messageIdsByDocument);
                            objMessageStatus = Deserialize<QlvbReceivedMessage>(sRespone);
                            sRespone = Base64Decode(objMessageStatus.content);

                            if (!sRespone.StartsWith("<?xml version="))
                            {
                                sRespone = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + sRespone;
                            }
                            status = Deserialize<Envelope>(sRespone);
                        }
                        catch(Exception ex) //catch error deserialize
                        {
                            webService.updateErrorMessage(messageIdsByDocument, ex.Message);
                        }

                        if (status == null) continue;

                        var headerStatus = status.Header.Status;

                        var from = headerStatus.From;

                        var responseFor = headerStatus.ResponseFor;
                        var sokyhieu = responseFor.Code.Trim('/');
                        var madinhdanhdonvi = objMessageStatus.sendingsystemid;
                        var trangthai = headerStatus.StatusCode;
                        var vanbangocid = headerStatus.ResponseFor.DocumentId;
                        var nguoixuly = headerStatus.StaffInfo.Staff;
                        var phongxuly = headerStatus.StaffInfo.Department;
                        var diengiai = headerStatus.Description;
                        if (vanbangocid == null) vanbangocid = string.Empty;

                        var ngaythuchien = DateTime.Now;

                        if (!string.IsNullOrEmpty(objMessageStatus.senddate))
                        {
                            double dbdate;
                            if (double.TryParse(objMessageStatus.senddate, out dbdate))
                            {
                                var jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                ngaythuchien = jan1St1970.AddMilliseconds(dbdate);
                            }
                            else
                            {
                                DateTime ngaygui;
                                if (DateTime.TryParseExact(objMessageStatus.senddate, "dd/MM/yyyy",
                                                          CultureInfo.InvariantCulture,
                                                          DateTimeStyles.None,
                                                          out ngaygui))
                                {
                                    ngaythuchien = ngaygui;
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(headerStatus.Timestamp))
                        {
                            DateTime oDate;
                            if (DateTime.TryParseExact(headerStatus.Timestamp, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out oDate))
                            {
                                ngaythuchien = oDate;
                            }
                        }
                        

                        //var ngaythuchien = string.IsNullOrWhiteSpace(headerStatus.Timestamp)
                        //    ? DateTime.Now
                        //    : DateTime.ParseExact(headerStatus.Timestamp, "dd/MM/yyyy HH:mm:ss", null);

                        Vanbandi vanbandi = null;

                        int idvanbandi;
                        if (int.TryParse(vanbangocid, out idvanbandi))
                        {
                            vanbandi = _vanbandiRepo.Vanbandis.FirstOrDefault(p => p.intid == idvanbandi);
                        }
                        else
                        {
                            vanbandi = _vanbandiRepo.Vanbandis.OrderByDescending(x => x.strngayky)
                                .FirstOrDefault(
                                    p => (p.intso != null && p.intso + "/" + p.strkyhieu == sokyhieu) || 
                                        (p.intso == null && p.strkyhieu == sokyhieu));
                        }

                        if (vanbandi!=null)
                        {
                            var org = orgs.FirstOrDefault(x => x.code == madinhdanhdonvi);                                
                            if (org != null)
                            {
                                var ketqua = _guivbRepo.UpdateTrangthaiNhan(vanbandi.intid,
                                    madinhdanhdonvi,
                                    org.name,
                                    (int)enumGuiVanban.intloaivanban.Vanbandi,
                                    (enumGuiVanban.inttrangthaiphanhoi)int.Parse(trangthai), ngaythuchien,
                                    enumGuiVanban.intloaigui.Tructinh);
                                var xuly = new TinhhinhXulyVanBanDi();                                
                                xuly.strmaxuly = trangthai;
                                xuly.strnguoixuly = nguoixuly;
                                xuly.strphongban = phongxuly;
                                xuly.strdiengiai= diengiai;
                                xuly.intidguivanban = _tinhhinhxulyvanbandiRepo.getIdGuiVanban(vanbandi.intid,
                                    madinhdanhdonvi, org.name,(int)enumGuiVanban.intloaivanban.Vanbandi, enumGuiVanban.intloaigui.Tructinh);

                                var ketqua1 = _tinhhinhxulyvanbandiRepo.Them(xuly);

                                if( (ketqua > 0) && (ketqua1 > 0)) //Update finish
                                {
                                    webService.updateReceiveFinish(messageIdsByDocument);
                                }
                                else
                                {
                                   webService.updateErrorMessage(messageIdsByDocument, "Can't find Organization");
                                }

                            }
                        }
                        else
                        {
                           webService.updateErrorMessage(messageIdsByDocument, "Can't find document");
                        }
                    }
                    
                    catch (Exception ex)
                    {
                        webService.updateErrorMessage(messageIdsByDocument, ex.Message);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private string BuildXmlQlvbObject(QlvbReceivedMessage message)
        {
            var stringBuilder = new StringBuilder();
            
            //stringBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            stringBuilder.Append("<message>");
            stringBuilder.Append("<required-answer><![CDATA[0]]></required-answer>");
            stringBuilder.AppendFormat("<send-date><![CDATA[{0}]]></send-date>", message.senddate);
            stringBuilder.AppendFormat("<sending-system-id><![CDATA[{0}]]></sending-system-id>", message.sendingsystemid);
            stringBuilder.AppendFormat("<receiving-system-id><![CDATA[{0}]]></receiving-system-id>", message.receivingsystemid);
            stringBuilder.AppendFormat("<document-type><![CDATA[{0}]]></document-type>", message.documenttype);
            stringBuilder.AppendFormat("<document-code><![CDATA[{0}]]></document-code>", message.documentcode);
            stringBuilder.AppendFormat("<description>{0}</description>", message.description);
            stringBuilder.AppendFormat("<content><![CDATA[{0}]]></content>", message.content);
            stringBuilder.Append("<title />");
            stringBuilder.Append("<department-id />");
            stringBuilder.Append("<department-send-id />");
            stringBuilder.Append("<attach-files />");
            stringBuilder.Append("<option><![CDATA[0]]></option>");
            stringBuilder.Append("<stateProcess><![CDATA[]]></stateProcess>");
            stringBuilder.AppendFormat("<signature><![CDATA[{0}]]></signature>", message.signature);
            stringBuilder.Append("<edxml><![CDATA[0]]></edxml>");
            stringBuilder.Append("</message>");

            return stringBuilder.ToString();
        }

        private string BuildXmlContentStatus(Envelope status)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            stringBuilder.Append("<SOAP-ENV:Header>");
            stringBuilder.Append("<edXML:Status xmlns:edXML=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            stringBuilder.Append("<edXML:ResponseFor>");
            stringBuilder.AppendFormat("<edXML:OrganId>{0}</edXML:OrganId>", status.Header.Status.ResponseFor.OrganId);
            stringBuilder.AppendFormat("<edXML:Code>{0}</edXML:Code>", status.Header.Status.ResponseFor.Code);
            stringBuilder.AppendFormat("<edXML:PromulgationDate>{0}</edXML:PromulgationDate>", status.Header.Status.ResponseFor.PromulgationDate);
            stringBuilder.AppendFormat("<edXML:DocumentId>{0}</edXML:DocumentId>", status.Header.Status.ResponseFor.DocumentId);
            stringBuilder.Append("</edXML:ResponseFor>");
            stringBuilder.Append("<edXML:From>");
            stringBuilder.AppendFormat("<edXML:OrganId>{0}</edXML:OrganId>", status.Header.Status.From.OrganId);
            stringBuilder.AppendFormat("<edXML:OrganName>{0}</edXML:OrganName>", status.Header.Status.From.OrganName);
            stringBuilder.Append("<edXML:OrganizationInCharge />");
            stringBuilder.Append("<edXML:OrganAdd />");
            stringBuilder.Append("<edXML:Email />");
            stringBuilder.Append("<edXML:Telephone />");
            stringBuilder.Append("<edXML:Fax />");
            stringBuilder.Append("<edXML:Website />");
            stringBuilder.Append("</edXML:From>");
            stringBuilder.AppendFormat("<edXML:StatusCode>{0}</edXML:StatusCode>", status.Header.Status.StatusCode);
            stringBuilder.AppendFormat("<edXML:Description>{0}</edXML:Description>", status.Header.Status.Description);
            stringBuilder.AppendFormat("<edXML:Timestamp>{0}</edXML:Timestamp>", status.Header.Status.Timestamp);
            stringBuilder.Append("<edXML:StaffInfo>");
            stringBuilder.AppendFormat("<edXML:Department>{0}</edXML:Department>", status.Header.Status.StaffInfo == null ? string.Empty : status.Header.Status.StaffInfo.Department);
            stringBuilder.AppendFormat("<edXML:Staff>{0}</edXML:Staff>", status.Header.Status.StaffInfo == null ? string.Empty : status.Header.Status.StaffInfo.Staff);
            stringBuilder.Append("</edXML:StaffInfo>");
            stringBuilder.Append("</edXML:Status>");
            stringBuilder.Append("</SOAP-ENV:Header>");
            stringBuilder.Append("<SOAP-ENV:Body />");
            stringBuilder.Append("</SOAP-ENV:Envelope>");

            var xml = Base64Encode(stringBuilder.ToString());

            return xml;
        }

        private string SendStatusByIdVanbanDenMail(int? idvanbandenmail, string status, string statusDescription, string nguoi, string phong)
        {
            try
            {
                var vanbandenMail =
                    _vanbandenmailRepository.Vanbandenmails.FirstOrDefault(
                        x =>
                            x.intid == idvanbandenmail &&
                            x.intnhanvanbantu == enumVanbandenmail.intnhanvanbantu.TrucLienThongTinh);

                if (vanbandenMail == null) return null;

                var madonviNhan = vanbandenMail.strmadinhdanh;
                var orgs = this.GetAllOrganization();
                var orgnhan = orgs.FirstOrDefault(x => x.code == madonviNhan);
                var madonvinhanchinhphu = orgnhan.edxmlCode;
                var tendonviNhan = vanbandenMail.strnoiguivb;
                var sokyhieuvanban = vanbandenMail.intso + "/" + vanbandenMail.strkyhieu;
                if (vanbandenMail.intso == null) sokyhieuvanban = vanbandenMail.strkyhieu;
                var trichyeu = vanbandenMail.strtrichyeu;
                var documentid = vanbandenMail.strvanbangocid;
                var ngayky =string.Format("{0:dd/MM/yyyy}", vanbandenMail.strngayky) ;

                if (string.IsNullOrEmpty(madonviNhan)) return null;

                var madonviTrucTinh = _configRepo.GetConfig(ThamsoHethong.MaDonviTrucTinh);
                var donviGui = orgs.FirstOrDefault(x => x.code == madonviTrucTinh);
                var madonviguichinhphu = donviGui.edxmlCode;
                var tendonvigui = _configRepo.GetConfig(ThamsoHethong.TenDonviTrucTinh);

                //var nguoigui = string.Empty;
                //var phongban = string.Empty;
                var nguoigui = nguoi;
                var phongban = phong;
                var userid = _session.GetUserId();

                var canbo = _canboRepository.GetAllCanboByID(userid);

                if(nguoigui==string.Empty || phong == string.Empty)

                {
                    if (canbo != null)
                    {
                        nguoigui = canbo.strhoten;

                        if (canbo.intdonvi != null)
                        {
                            var donvi = _donviManager.GetDonvi(canbo.intdonvi.Value);

                            if (donvi != null) phongban = donvi.strtendonvi;
                        }
                    }
                }
               
                var messageStatus = new Envelope
                {
                    Header = new Header
                    {
                        Status = new Status
                        {
                            ResponseFor = new ResponseFor
                            {
                                Code = sokyhieuvanban,
                                OrganId = madonvinhanchinhphu,
                                PromulgationDate = ngayky,
                                DocumentId = documentid
                            },
                            From = new From
                            {
                                OrganId = madonviguichinhphu,
                                OrganName = tendonvigui
                            },
                            StatusCode = status,
                            Description = statusDescription,
                            Timestamp = string.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now),
                            StaffInfo = new StaffInfo
                            {
                                Staff = nguoigui,
                                Department = phongban
                            }

                        }
                    }
                };



                var content = BuildXmlContentStatus(messageStatus);

                var senddate = DateTime.UtcNow;
                var jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var senddateLongTime = (long)((senddate - jan1St1970).TotalMilliseconds);

                var message = new QlvbReceivedMessage
                {
                    content = content,
                    senddate = senddateLongTime.ToString(),
                    sendingsystemid = madonviTrucTinh,
                    receivingsystemid = madonviNhan,
                    documenttype = "6.1.0.1",
                    documentcode = sokyhieuvanban,
                    description = trichyeu,
                };

                var webService = ConnectGateway();

                var contentSending = BuildXmlQlvbObject(message);

                var results = webService.sendMessage(contentSending);

                return results;
            }
            catch (Exception e)
            {
                _logger.Warn(e.Message);
                return null;
            }
        }

        public string SendStatus(int idvanban, string status, string statusDescription, string nguoi, string phong)
        {
            var vanbanden = _vbdenRepo.GetVanbandenById(idvanban);
            if (vanbanden == null) return null;

            return this.SendStatusByIdVanbanDenMail(vanbanden.intidvanbandenmail, status, statusDescription, nguoi, phong);
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
                            + "<van-ban-goc-id> <![CDATA["+vanbandi.intid+"]]> </van-ban-goc-id>"
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
            catch (Exception ex)
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
        /// <summary>
        /// ghi nhat ky nhan vbdt tu truc tinh
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        private int _LogVBDenTructinh(string id, string content, string strheader)
        {
            bool isLogVBDTDen = AppSettings.IsLogVBDTDen;
            int idLog = 0;
            if (isLogVBDTDen)
            {
               try
                {
                    MailInbox vbdt = new MailInbox();
                    vbdt.strsubject = id;
                    vbdt.strcontent = content;
                    vbdt.strheader = strheader;
                    vbdt.intloai = (int)enumMailInbox.intloai.Tructinh;
                    
                    idLog = _mailInboxRepo.Them(vbdt);                    
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);                   
                }
            }
            return idLog;
        }

        private void _UpdateLogVBDenTructinh(int intid, int inttrangthai)
        {
            try
            {
                if (intid > 0)
                {
                    _mailInboxRepo.UpdateTrangthai(intid, inttrangthai);
                }                
            }
            catch { }
            
        }
        /// <summary>
        /// luu nhat ky gui vbdt tren truc tinh
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="content"></param>
        private void _LogVBDiTructinh(int idvanban, string content)
        {
            bool isLogVBDTDi = AppSettings.IsLogVBDTDi;
            if (isLogVBDTDi)
            {
                try
                {
                    MailOutbox vbdt = new MailOutbox();
                    vbdt.strsubject = idvanban.ToString();
                    vbdt.strcontent = content;
                    vbdt.intloai = (int)enumMailOutbox.intloai.Tructinh;
                    _mailOutboxRepo.Them(vbdt);

                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
            }
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

        [XmlRoot(ElementName = "ResponseFor", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class ResponseFor
        {
            [XmlElement(ElementName = "OrganId", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public string OrganId { get; set; }
            [XmlElement(ElementName = "Code", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public string Code { get; set; }
            [XmlElement(ElementName = "PromulgationDate", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public string PromulgationDate { get; set; }

            [XmlElement(ElementName = "DocumentId", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = true)]
            public string DocumentId { get; set; }
        }

        [XmlRoot(ElementName = "From", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class From
        {
            [XmlElement(ElementName = "OrganId", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public string OrganId { get; set; }
            [XmlElement(ElementName = "OrganName", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public string OrganName { get; set; }
            [XmlElement(ElementName = "OrganizationInCharge", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public string OrganizationInCharge { get; set; }
            [XmlElement(ElementName = "OrganAdd", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public string OrganAdd { get; set; }
            [XmlElement(ElementName = "Email", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public string Email { get; set; }
            [XmlElement(ElementName = "Telephone", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public string Telephone { get; set; }
            [XmlElement(ElementName = "Fax", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public string Fax { get; set; }
            [XmlElement(ElementName = "Website", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public string Website { get; set; }
        }

        [XmlRoot(ElementName = "StaffInfo", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class StaffInfo
        {
            [XmlElement(ElementName = "Department", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public string Department { get; set; }
            [XmlElement(ElementName = "Staff", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public string Staff { get; set; }
        }

        [XmlRoot(ElementName = "Status", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Status
        {
            [XmlElement(ElementName = "ResponseFor", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public ResponseFor ResponseFor { get; set; }
            [XmlElement(ElementName = "From", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public From From { get; set; }
            [XmlElement(ElementName = "StatusCode", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public string StatusCode { get; set; }
            [XmlElement(ElementName = "Description", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public string Description { get; set; }
            [XmlElement(ElementName = "Timestamp", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public string Timestamp { get; set; }
            [XmlElement(ElementName = "StaffInfo", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public StaffInfo StaffInfo { get; set; }
            [XmlAttribute(AttributeName = "edXML", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string EdXML { get; set; }
        }

        [XmlRoot(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Header
        {
            [XmlElement(ElementName = "Status", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public Status Status { get; set; }
        }

        [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Envelope
        {
            [XmlElement(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public Header Header { get; set; }
            [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public string Body { get; set; }
            [XmlAttribute(AttributeName = "SOAP-ENV", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string SOAPENV { get; set; }
        }
          
        #endregion
    }
}
