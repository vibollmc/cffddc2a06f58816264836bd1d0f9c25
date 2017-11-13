using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Core.Contract;
using QLVB.Common.Logging;
using QLVB.Common.Date;
using QLVB.Common.Utilities;
using QLVB.DTO.Vanbandi;
using QLVB.DTO;
using QLVB.DTO.Mail;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.Donvi;
using QLVB.DTO.File;
using System.Web;
using CipherText;
using MailBee;
using MailBee.DnsMX;
using MailBee.Mime;
using MailBee.SmtpMail;
using MailBee.Pop3Mail;
using MailBee.ImapMail;
using MailBee.Security;
using MailBee.AntiSpam;
using MailBee.Outlook;
using QLVB.DTO.Vanbandientu;
using QLVB.DTO.Vanbandientu.EdXML;
using QLVB.DTO.Vanbandientu.EdXMLBC;
using System.IO;

namespace QLVB.Core.Implementation
{
    public class MailBeeManager : IMailManager
    {
        #region Constructor

        private ILogger _logger;
        private ITochucdoitacRepository _tochucRepo;
        private IMailFormatManager _mailFormat;
        private IFileManager _fileManager;
        private IMailInboxRepository _inboxRepo;
        private IAttachMailRepository _attachMailRepo;
        private IVanbandenmailRepository _vbdenMailRepo;

        public MailBeeManager(ILogger logger, ITochucdoitacRepository tochucRepo,
            IMailFormatManager mailFormat, IFileManager fileManager,
            IMailInboxRepository inboxRepo, IAttachMailRepository attachMailRepo,
            IVanbandenmailRepository vbdenMailRepo
            )
        {
            _logger = logger;
            _tochucRepo = tochucRepo;
            _mailFormat = mailFormat;
            _fileManager = fileManager;
            _inboxRepo = inboxRepo;
            _attachMailRepo = attachMailRepo;
            _vbdenMailRepo = vbdenMailRepo;
        }

        #endregion Constructor

        #region VBDT

        /// <summary>
        /// nhan vbdt
        /// </summary>
        /// <param name="EmailPerRequest"></param>
        /// <param name="intloai">
        /// 0: nhận thủ công
        /// 1: nhận tự động
        /// </param>
        /// <returns></returns>
        public ResultFunction NhanVBDT(int EmailPerRequest, int intloai)
        {
            ResultFunction kq = new ResultFunction();

            if (intloai == 0)
            {
                kq = NhanPOP3(EmailPerRequest, intloai);
            }
            else
            {   // ==1: tu dong nhan
                // su dung Singletons
                //kq = NhanPOP3(EmailPerRequest, intloai);

                string isSending = QLVB.Common.Sessions.SingletonApp.IsSend;
                string isReceiving = QLVB.Common.Sessions.SingletonApp.IsReceive;

                if (!string.IsNullOrEmpty(isReceiving))
                {
                    if (isReceiving.ToLower() == "true")
                    {
                        //co auto dang chay
                        kq.id = 0;
                        kq.message = "other runed";
                        //_logger.Info("POP3: Tự động nhận : đang có services");
                    }
                    else
                    {   // khong co auto dang chay
                        if (isSending.ToLower() != "true")
                        {   // uu tien cho tu dong gui truoc
                            QLVB.Common.Sessions.SingletonApp.IsReceive = "true"; // ghi nho dang chay

                            _logger.Info("POP3: Tự động nhận văn bản điện tử");

                            kq = NhanPOP3(EmailPerRequest, intloai);

                            QLVB.Common.Sessions.SingletonApp.IsReceive = "false"; // da xong
                        }
                    }
                }
                else
                {
                    QLVB.Common.Sessions.SingletonApp.IsReceive = "false";
                    kq.id = 0;
                    kq.message = "other runed";
                }
            }

            return kq;
        }

        /// <summary>
        /// gui van ban dien tu
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="listSendDonvi"></param>
        /// <param name="intloaivanban"></param>
        /// <param name="intAutoSend">
        /// 0: gui binh thuong
        /// 1: luu vao DB de gui tu dong
        /// 2: gui tu dong
        /// </param>
        /// <returns></returns>
        public ResultFunction GuiVBDT(int idvanban, List<ListSendDonviViewModel> listSendDonvi,
            int intloaivanban, int intAutoSend)
        {
            ResultFunction kq = new ResultFunction();

            bool IsEmailBTTTT = AppSettings.IsEmailBTTTT;
            bool IsAutoSendConfig = AppSettings.IsAutoSendMail;

            if ((IsAutoSendConfig) && (intAutoSend == 1))
            {   // tu dong gui thi chi luu vao DB
                kq = _mailFormat.SaveAutoSendMail(idvanban, listSendDonvi, intloaivanban);
            }
            else
            {   // thuc hien gui mail

                bool isUpdateTrangthai = (intAutoSend == 2) ? true : false;

                // phan biet cac don vi gui binh thuong va gui theo tieu chuan ma hoa
                var listSendDonvi_normal = listSendDonvi.Where(p => p.isvbdt == false).ToList();
                var listSendDonvi_encode = listSendDonvi.Where(p => p.isvbdt == true).ToList();

                if (listSendDonvi_encode.Count > 0)
                {
                    if (IsEmailBTTTT)
                    {
                        int loai = AppSettings.LoaiVBDT;
                        switch (loai)
                        {
                            case 512:
                                //kq = SendSMTP_BTTTT_512(idvanban, listSendDonvi);
                                break;
                            case 2803:
                                kq = SendSMTP_BTTTT_2803(idvanban, listSendDonvi_encode, intloaivanban, isUpdateTrangthai);
                                break;
                            default:
                                kq.id = -1;
                                kq.message = "Lỗi! không tìm thấy tiêu chuẩn gửi văn bản điện tử";
                                break;
                        }
                    }
                    else
                    {
                        kq = SendSMTP_TTTH(idvanban, listSendDonvi_encode, intloaivanban, isUpdateTrangthai);
                    }
                }
                if (listSendDonvi_normal.Count > 0)
                {
                    // gui mail binh thuong khong ma hoa               
                    kq = SendSMTP_Donvikhac(idvanban, listSendDonvi_normal, intloaivanban, isUpdateTrangthai);
                }
            }
            return kq;
        }

        public ResultFunction AutoSendVBDT()
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                string isSending = QLVB.Common.Sessions.SingletonApp.IsSend;
                if (!string.IsNullOrEmpty(isSending))
                {
                    if (isSending.ToLower() == "true")
                    {
                        //co auto dang chay
                        kq.id = 0;
                        kq.message = "other runed";
                        //_logger.Info("SMTP: Tự động gửi : đang có services");
                    }
                    else
                    {   // khong co auto dang chay
                        QLVB.Common.Sessions.SingletonApp.IsSend = "true"; // ghi nho dang chay

                        var vanban = _mailFormat.GetIdVanbanAutoSend();
                        if (vanban != null)
                        {
                            var listSendDonvi = _mailFormat.GetListSendDonviAutoSend(vanban);

                            _logger.Info("SMTP: Tự động gửi văn bản điện tử");

                            kq = GuiVBDT((int)vanban.intidvanban, listSendDonvi, (int)vanban.intloaivanban, 2);

                            // kiem tra xem con vb chua tu dong gui khong
                            var vbconlai = _mailFormat.GetIdVanbanAutoSend();
                            if ((vbconlai != null) && (kq.id == (int)ResultViewModels.Success))
                            {   // con van ban va da gui van ban thanh cong
                                kq.id = -2;  // danh dau tu dong gui tiep
                            }
                        }
                        QLVB.Common.Sessions.SingletonApp.IsSend = "false"; //  da xong
                    }
                }
                else
                {
                    QLVB.Common.Sessions.SingletonApp.IsSend = "false";
                    kq.id = 0;
                    kq.message = "other runed";
                }
            }
            catch (Exception ex)
            {
                QLVB.Common.Sessions.SingletonApp.IsSend = "false";
                _logger.Error(ex.Message);
            }
            return kq;
        }

        #endregion VBDT


        #region POP3

        /// <summary>
        /// tra ve so mail con tren hop thu sau khi lay ve
        /// HIEN KHONG SU DUNG
        /// </summary>
        /// <returns></returns>
        private ResultFunction RecievePOP3()
        {
            ResultFunction kq = new ResultFunction();
            kq.id = -1;
            var account = _mailFormat.GetAccountSetting();

            try
            {
                Pop3.LicenseKey = AppConts.MailBeeLicenseKey;

                Pop3 pop = new Pop3();
                pop.Connect(account.incomingMailServer, account.portIncomingServer);
                pop.Login(account.accountName, account.password, AuthenticationMethods.Auto);
                // so van ban dien tu da nhan
                int VBDT = 0;
                int VBhoibao = 0;
                // so mail down ve
                int MessageCount = pop.InboxMessageCount;
                if (pop.InboxMessageCount > 0)
                {
                    // If the inbox contains less than 10 e-mails, adjust to that.
                    int MessageDownload = pop.InboxMessageCount > 3 ? 3 : pop.InboxMessageCount;
                    string folderPath = "";

                    // As e-mail indices on the server start with 1 (not 0), must add 1.
                    int startIndex = 1; // pop.InboxMessageCount - MessageDownload + 1;

                    MailMessageCollection msgs =
                        pop.DownloadEntireMessages(startIndex, MessageDownload);

                    List<TTTHBaocaoViewModel> listbaocao = new List<TTTHBaocaoViewModel>();

                    foreach (MailMessage msg in msgs)
                    {
                        int msgAttachCount = msg.Attachments.Count;
                        msg.Charset = account.charset;
                        string subject = msg.Subject;
                        string bodytext = msg.BodyPlainText;
                        string from = msg.From.Email;

                        string datesent = DateServices.FormatDateTimeVN(msg.Date);
                        string strheader = "From: " + from
                                    + " Date Sever: " + datesent;
                        //string strheader = msg.RawHeader;

                        int idmail = 0;
                        int idinbox = 0;
                        int idloai = (int)enumAttachMail.intloai.MailInbox;

                        if (subject == "[ema]TTTH[/ema]")
                        {
                            VBDT++;
                            idloai = (int)enumAttachMail.intloai.Vanbandendientu;
                            folderPath = _fileManager.SetPathUpload(AppConts.FileEmail);

                            // luu vao table vanbandenmail
                            bool IsAttach = msgAttachCount > 0 ? true : false;
                            idmail = _mailFormat.DecodeQLVB1(bodytext, IsAttach);

                            // luu vao mail inbox
                            idinbox = _mailFormat.SaveMailInbox(subject, strheader, bodytext, from, (int)enumMailInbox.intloai.TTTH);

                            TTTHBaocaoViewModel baocao = _mailFormat.GetTTTHBaocao(bodytext);
                            if (baocao != null)
                            {
                                listbaocao.Add(baocao);
                            }
                        }
                        else if (subject == "[ema]TTTHBC[/ema]")
                        {
                            VBhoibao++;
                            _mailFormat.DecodeBaocaoQLVB1(bodytext);
                        }
                        else
                        {
                            idloai = (int)enumAttachMail.intloai.MailInbox;
                            folderPath = _fileManager.SetPathUpload(AppConts.FileEmailInbox);

                            // luu vao mail inbox
                            idinbox = _mailFormat.SaveMailInbox(subject, strheader, bodytext, from, (int)enumMailInbox.intloai.Khac);

                        }

                        if (msgAttachCount > 0)
                        {
                            foreach (Attachment attach in msg.Attachments)
                            {
                                string strtenfile = attach.Filename;
                                if (idloai == (int)enumAttachMail.intloai.Vanbandendientu)
                                {
                                    try
                                    {
                                        string strmota = attach.Filename;
                                        string fileSavepath = _mailFormat.SaveAttachment(idmail, strmota, folderPath);
                                        attach.Save(fileSavepath, false);
                                        //  kiem tra file da save ve server chua                                    
                                        if (System.IO.File.Exists(fileSavepath))
                                        {
                                            // insert vao database attachmail                                    
                                            _mailFormat.InsertAttachment(idmail, fileSavepath, strmota, idloai);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.Info("POP3 Error: " + ex.Message);
                                    }
                                }
                                if (idloai == (int)enumAttachMail.intloai.MailInbox)
                                {
                                    try
                                    {
                                        string strmota = attach.Filename;
                                        string fileSavepath = _mailFormat.SaveAttachment(idmail, strmota, folderPath);
                                        attach.Save(fileSavepath, false);

                                        //  kiem tra file da save ve server chua                                    
                                        if (System.IO.File.Exists(fileSavepath))
                                        {
                                            // insert vao database attachmail                                    
                                            _mailFormat.InsertAttachment(idmail, fileSavepath, strmota, idloai);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.Info("POP3 Error: " + ex.Message);
                                    }
                                }

                            }
                        }
                    } // het duyet tung mail

                    pop.DeleteMessages(startIndex, MessageDownload);

                    pop.Disconnect();

                    kq.id = MessageCount - MessageDownload;
                    kq.message = "Nhận thành công " + MessageDownload.ToString() + " văn bản điện tử. Số văn bản chưa nhận là "
                        + kq.id;

                    // end receive mail
                    // star send mail bao cao
                    if (listbaocao.Count > 0)
                    {
                        SendSMTP_TTTHBaocao(listbaocao);
                    }

                    return kq;
                }
                else
                {
                    //Console.WriteLine("Inbox is empty");
                    kq.message = "Không có văn bản điện tử mới";
                    return kq;
                }
            }
            catch (MailBeeException ex)
            {
                string error = DisplayError(ex);
                _logger.Info("POP3 Error: " + error);
                kq.message = error;
            }
            return kq;
        }

        /// <summary>
        /// tra ve id:so mail con tren hop thu sau khi lay ve. message: so vbdt da nhan
        /// 
        /// </summary>
        /// <param name="EmailPerRequest">so mail lay ve moi lan</param>
        /// <param name="intloai">
        /// 0: nhận thủ công
        /// 1: nhận tự động
        /// </param>
        /// <returns></returns>
        private ResultFunction NhanPOP3(int EmailPerRequest, int intloai)
        {
            ResultFunction kq = new ResultFunction();
            kq.id = -1;
            var account = _mailFormat.GetAccountSetting();

            try
            {
                Pop3.LicenseKey = AppConts.MailBeeLicenseKey;

                Pop3 pop = new Pop3();
                pop.Connect(account.incomingMailServer, account.portIncomingServer);
                pop.Login(account.accountName, account.password, AuthenticationMethods.Auto);
                // so van ban dien tu da nhan
                int VBDT = 0;
                int VBhoibao = 0;
                // so mail down ve
                int MessageCount = pop.InboxMessageCount;
                if (pop.InboxMessageCount > 0)
                {
                    // If the inbox contains less than 10 e-mails, adjust to that.
                    int MessageDownload = (pop.InboxMessageCount > EmailPerRequest) ? EmailPerRequest : pop.InboxMessageCount;
                    string folderPath = "";

                    // As e-mail indices on the server start with 1 (not 0), must add 1.
                    int startIndex = 1; // pop.InboxMessageCount - MessageDownload + 1;

                    MailMessageCollection msgs =
                        pop.DownloadEntireMessages(startIndex, MessageDownload);

                    List<TTTHBaocaoViewModel> listTTTHbaocao = new List<TTTHBaocaoViewModel>();
                    List<edXMLBaocao> listEdXMLBaocao = new List<edXMLBaocao>();

                    foreach (MailMessage msg in msgs)
                    {
                        int msgAttachCount = msg.Attachments.Count;
                        msg.Charset = account.charset;
                        string subject = msg.Subject;
                        string bodytext = msg.BodyPlainText;
                        string from = msg.From.Email;

                        string datesent = DateServices.FormatDateTimeVN(msg.Date);
                        string strheader = "From: " + from
                                    + " Date Sever: " + datesent;
                        //string strheader = msg.RawHeader;

                        int idmail = 0;
                        int idinbox = 0;
                        int idloai = (int)enumAttachMail.intloai.MailInbox;

                        // ========luu nhat ky mail ============
                        bool isLogVBDTDen = AppSettings.IsLogVBDTDen;
                        if (isLogVBDTDen)
                        {
                            //idloai = (int)enumAttachMail.intloai.MailInbox;
                            //folderPath = _fileManager.SetPathUpload(AppConts.FileEmailInbox);

                            // luu vao mail inbox
                            idinbox = _mailFormat.SaveMailInbox(subject, strheader, bodytext, from, (int)enumMailInbox.intloai.Log);

                        }

                        //==========luu noi dung mail====================================
                        switch (subject)
                        {
                            case "[ema]TTTH[/ema]":
                                VBDT++;
                                idloai = (int)enumAttachMail.intloai.Vanbandendientu;
                                folderPath = _fileManager.SetPathUpload(AppConts.FileEmail);

                                // luu vao table vanbandenmail
                                bool IsAttach = msgAttachCount > 0 ? true : false;
                                idmail = _mailFormat.DecodeQLVB1(bodytext, IsAttach);


                                if (idmail == 0)
                                {   // bi loi khong luu vao hop thu van ban den dien tu
                                    // luu vet vao mail inbox
                                    idinbox = _mailFormat.SaveMailInbox(subject, strheader, bodytext, from, (int)enumMailInbox.intloai.TTTH);
                                }
                                else
                                {
                                    TTTHBaocaoViewModel baocao = _mailFormat.GetTTTHBaocao(bodytext);
                                    if (baocao != null)
                                    {
                                        listTTTHbaocao.Add(baocao);
                                    }
                                }

                                break;
                            case "[ema]TTTHBC[/ema]":
                                VBhoibao++;
                                int hoibao = _mailFormat.DecodeBaocaoQLVB1(bodytext);
                                if (hoibao == 0)
                                {   // bi loi khong luu vao hop thu van ban den dien tu
                                    // luu vet vao mail inbox
                                    idinbox = _mailFormat.SaveMailInbox(subject, strheader, bodytext, from, (int)enumMailInbox.intloai.TTTH);
                                }
                                break;
                            case "[EDXML]":
                                VBDT++;
                                idloai = (int)enumAttachMail.intloai.Vanbandendientu;
                                folderPath = _fileManager.SetPathUpload(AppConts.FileEmail);

                                // luu vao table vanbandenmail
                                IsAttach = msgAttachCount > 0 ? true : false;
                                idmail = _mailFormat.SaveEdXML(bodytext, IsAttach);

                                if (idmail == 0)
                                {
                                    // bi loi khong luu vao hop thu van ban den dien tu
                                    // luu vet vao mail inbox
                                    if (bodytext.Length > 3000) { bodytext = bodytext.Substring(0, 2900); }

                                    idinbox = _mailFormat.SaveMailInbox(subject, strheader, bodytext, from, (int)enumMailInbox.intloai.BTTTT_2803);

                                }
                                else
                                {
                                    edXMLBaocao edBaocao = _mailFormat.GetBTTTT_2803_Baocao(bodytext);
                                    if (edBaocao != null)
                                    {
                                        listEdXMLBaocao.Add(edBaocao);
                                    }
                                }

                                break;
                            case "[EDXMLBC]":
                                VBhoibao++;
                                _mailFormat.UpdateEdXMLBaocao(bodytext);
                                break;
                            default:
                                idloai = (int)enumAttachMail.intloai.MailInbox;
                                folderPath = _fileManager.SetPathUpload(AppConts.FileEmailInbox);

                                // luu vao mail inbox
                                idinbox = _mailFormat.SaveMailInbox(subject, strheader, bodytext, from, (int)enumMailInbox.intloai.Khac);

                                break;

                        }


                        //===========luu file dinh kem==================================================
                        if (msgAttachCount > 0)
                        {
                            foreach (Attachment attach in msg.Attachments)
                            {
                                //string strtenfile = attach.Filename;

                                if (idloai == (int)enumAttachMail.intloai.Vanbandendientu)
                                {   // luu file van ban dien tu
                                    try
                                    {
                                        string strmota = attach.Filename;
                                        string fileSavepath = _mailFormat.SaveAttachment(idmail, strmota, folderPath);
                                        attach.Save(fileSavepath, false);
                                        //  kiem tra file da save ve server chua                                    
                                        if (System.IO.File.Exists(fileSavepath))
                                        {
                                            if (subject == "[EDXML]")
                                            {
                                                bool isMahoaFileBase64 = AppSettings.MahoaFileBase64;
                                                try
                                                {
                                                    if (isMahoaFileBase64)
                                                    {
                                                        edXMLMessage edxml = _mailFormat.StringToEdXMLMessage_2803(bodytext);
                                                        string filenameattach = string.Empty;

                                                        foreach (var attxml in edxml.Body.Manifest.Reference)
                                                        {
                                                            string contentid = "cid:" + strmota;// +attach.Name;
                                                            if (contentid.Contains(attxml.href))
                                                            {
                                                                filenameattach = attxml.AttachmentName;
                                                                break;
                                                            }
                                                        }
                                                        string fileDecodeSavePath = _mailFormat.SaveAttachment(idmail, filenameattach, folderPath);

                                                        bool isZipFile = AppSettings.IsZipFile;
                                                        if (isZipFile)
                                                        {
                                                            using (var ms = new MemoryStream())
                                                            {
                                                                using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(fileSavepath))
                                                                {
                                                                    foreach (Ionic.Zip.ZipEntry e in zip)
                                                                    {
                                                                        e.Extract(ms);

                                                                        ms.Position = 0;
                                                                        var sr = new StreamReader(ms);
                                                                        var base64 = sr.ReadToEnd();

                                                                        QLVB.Common.Crypt.CryptServices.DecodeFileFromBase64(base64, fileDecodeSavePath);

                                                                        // insert vao database attachmail                                    
                                                                        _mailFormat.InsertAttachment(idmail, fileDecodeSavePath, filenameattach, idloai);

                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {   // khong nen file
                                                            var base64 = System.IO.File.ReadAllText(fileSavepath);

                                                            QLVB.Common.Crypt.CryptServices.DecodeFileFromBase64(base64, fileDecodeSavePath);

                                                            // insert vao database attachmail                                    
                                                            _mailFormat.InsertAttachment(idmail, fileDecodeSavePath, filenameattach, idloai);
                                                        }

                                                    }
                                                    else
                                                    {   // khong ma hoa 
                                                        // insert vao database attachmail                                    
                                                        _mailFormat.InsertAttachment(idmail, fileSavepath, strmota, idloai);
                                                    }
                                                }
                                                catch { }
                                            }
                                            else
                                            {   // khong phai edxml thi luu binh thuong
                                                // insert vao database attachmail                                    
                                                _mailFormat.InsertAttachment(idmail, fileSavepath, strmota, idloai);
                                            }

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.Error("POP3 Error: " + ex.Message);
                                    }
                                }

                                if (idloai == (int)enumAttachMail.intloai.MailInbox)
                                {   // luu file mail khac 
                                    try
                                    {
                                        string strmota = attach.Filename;
                                        string fileSavepath = _mailFormat.SaveAttachment(idmail, strmota, folderPath);
                                        attach.Save(fileSavepath, false);

                                        //  kiem tra file da save ve server chua                                    
                                        if (System.IO.File.Exists(fileSavepath))
                                        {
                                            // insert vao database attachmail                                    
                                            _mailFormat.InsertAttachment(idmail, fileSavepath, strmota, idloai);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.Error("POP3 Error: " + ex.Message);
                                    }
                                }

                            }
                        }
                    } // het duyet tung mail

                    pop.DeleteMessages(startIndex, MessageDownload);

                    pop.Disconnect();

                    // so van ban con lai
                    kq.id = MessageCount - MessageDownload;
                    // so van ban da nhan
                    kq.message = MessageDownload.ToString();

                    // end receive mail
                    // star send mail bao cao
                    if (listTTTHbaocao.Count > 0)
                    {
                        SendSMTP_TTTHBaocao(listTTTHbaocao);
                    }
                    if (listEdXMLBaocao.Count > 0)
                    {
                        SendSMTP_BTTTT_2803_Baocao(listEdXMLBaocao);
                    }

                    return kq;
                }
                else
                {
                    kq.id = 0;
                    kq.message = "0";
                    //kq.message = "Không có văn bản điện tử mới";
                    return kq;
                }
            }
            catch (MailBeeException ex)
            {
                string error = DisplayError(ex);
                if (intloai == 0)
                {
                    _logger.Info("POP3 Error: " + error);
                }
                else
                {
                    _logger.Error("POP3 Error: " + error);
                }

                kq.id = -1;
                kq.message = error;
            }
            return kq;
        }

        #endregion POP3

        #region ErrorMail
        private string DisplayError(MailBeeException ex)
        {
            string strerror = string.Empty;
            switch (ex.ErrorCode)
            {
                case ErrorCodes.BadCredentials:
                    strerror = "Sai tên đăng nhập hoặc mật khẩu Mail Server";
                    break;
                case ErrorCodes.NotConnected:
                    strerror = "Không truy cập Mail Server";
                    break;
                case ErrorCodes.HostNotFound:
                    strerror = "Không tìm thấy Mail Server";
                    break;
                case ErrorCodes.HostUnreachable:
                    strerror = "Không tìm thấy Mail Server";
                    break;
                default:
                    strerror = ex.Message;
                    break;
            }
            return "Lỗi " + ex.ErrorCode.ToString() + " : " + strerror;
        }

        private string DisplaySMTPError(MailBeeSmtpNegativeResponseException ex)
        {
            string strerror = string.Empty;
            switch (ex.ResponseCode)
            {
                case 535:
                    strerror = "Sai tên đăng nhập hoặc mật khẩu Mail Server";
                    break;
                case ErrorCodes.NotConnected:
                    strerror = "Không truy cập Mail Server";
                    break;
                case ErrorCodes.HostNotFound:
                    strerror = "Không tìm thấy Mail Server";
                    break;
                case ErrorCodes.HostUnreachable:
                    strerror = "Không tìm thấy Mail Server";
                    break;
                default:
                    strerror = ex.Message;
                    break;
            }
            return "Lỗi " + ex.ErrorCode.ToString() + " : " + strerror;
        }

        #endregion ErrorMail

        #region SMTP


        private ResultFunction SendSMTP_TTTH(int idvanban, List<ListSendDonviViewModel> listSendDonvi,
            int intloaivanban, bool isUpdateTrangthai)
        {
            ResultFunction kq = new ResultFunction();
            var account = _mailFormat.GetAccountSetting();
            //int intloai = (int)enumAttachVanban.intloai.Vanbanden;

            Smtp.LicenseKey = AppConts.MailBeeLicenseKey;
            Smtp mailer = new Smtp();
            //mailer.SmtpServers.Add
            //    (account.outgoingServer, account.accountName, account.password)
            //    .Port = account.portOutgoingServer;

            SmtpServer server = new SmtpServer();
            server.SmtpOptions = ExtendedSmtpOptions.NoChunking;
            server.AccountName = account.accountName;
            server.Password = account.password;
            server.Name = account.outgoingServer;
            server.Port = account.portOutgoingServer;
            server.Timeout = 90000;  // 90 sec
            mailer.SmtpServers.Add(server);


            mailer.From.Email = account.emailAddress;
            mailer.Message.Charset = account.charset;
            string subject = "[ema]TTTH[/ema]";
            mailer.Message.Subject = subject;

            string strnoidung = string.Empty;

            switch (intloaivanban)
            {
                case (int)enumGuiVanban.intloaivanban.Vanbandi:

                    strnoidung = _mailFormat.EncodeQLVB1(idvanban, intloaivanban);
                    List<ListFileToAttach> listfile = _mailFormat.GetFileVanbanToAttach(idvanban, (int)enumAttachVanban.intloai.Vanbandi);
                    foreach (var file in listfile)
                    {
                        if (System.IO.File.Exists(file.filePath))
                        {
                            mailer.Message.Attachments.Add(file.filePath, file.strmota);
                        }
                        else
                        {
                            _logger.Warn("Gửi mail văn bản đi: " + idvanban + ", không tìm thấy file đính kèm: " + file.filePath);
                        }
                    }
                    break;

                case (int)enumGuiVanban.intloaivanban.Vanbanden:

                    strnoidung = _mailFormat.EncodeQLVB1(idvanban, intloaivanban);
                    listfile = _mailFormat.GetFileVanbanToAttach(idvanban, (int)enumAttachVanban.intloai.Vanbanden);
                    foreach (var file in listfile)
                    {
                        if (System.IO.File.Exists(file.filePath))
                        {
                            mailer.Message.Attachments.Add(file.filePath, file.strmota);
                        }
                        else
                        {
                            _logger.Warn("Gửi mail văn bản đến: " + idvanban + ", không tìm thấy file đính kèm: " + file.filePath);
                        }
                    }
                    break;
            }

            bool isSent = false;
            bool isLogVBDi = AppSettings.IsLogVBDTDi;
            foreach (var p in listSendDonvi)
            {
                mailer.Message.To = new EmailAddressCollection(p.stremailvbdt); // .Add(p.stremailvbdt);
                string strbaocao = _mailFormat.EncodeBaocaoQLVB1(account.emailAddress, idvanban, p.intid);
                string strcontent = strnoidung + strbaocao;
                mailer.Message.BodyPlainText = strcontent;
                try
                {
                    mailer.Send();
                    if (isUpdateTrangthai)
                    {   // tu dong gui thi cap nhat trang thai
                        _mailFormat.UpdateGuiVanban(idvanban, p.intid, intloaivanban);
                    }
                    else
                    {   // them moi trong table guivanban
                        _mailFormat.SaveGuiVanban(idvanban, p.intid, intloaivanban);
                    }
                    // luu nhat ky gui
                    if (isLogVBDi)
                    {
                        _mailFormat.SaveMailOutbox(subject, strcontent, p.stremailvbdt, (int)enumMailOutbox.intloai.TTTH);
                    }

                    isSent = true;
                    kq.id = (int)ResultViewModels.Success;
                    kq.message = "Thư gửi thành công";
                }
                catch (MailBeeSmtpNegativeResponseException e)
                {
                    string error = DisplaySMTPError(e);
                    _logger.Info("SMTP Error: " + error);
                    kq.id = (int)e.ResponseCode;
                    kq.message = error;
                }
                catch (MailBeeException ex)
                {
                    string error = DisplayError(ex);
                    _logger.Info("SMTP Error: " + error);
                    kq.message = error;
                    kq.id = (int)ex.ErrorCode;
                }
            }
            if (isSent)
            {
                // cap nhat trang thai van ban da gui dien tu VBDt
                _mailFormat.UpdateVBDT(idvanban, intloaivanban);
            }

            return kq;
        }

        /// <summary>
        /// gui mail hoi bao 
        /// </summary>
        /// <param name="listbaocao"></param>
        /// <returns></returns>
        private ResultFunction SendSMTP_TTTHBaocao(List<TTTHBaocaoViewModel> listbaocao)
        {
            ResultFunction kq = new ResultFunction();
            var account = _mailFormat.GetAccountSetting();
            //int intloai = (int)enumAttachVanban.intloai.Vanbanden;

            Smtp.LicenseKey = AppConts.MailBeeLicenseKey;
            Smtp mailer = new Smtp();
            //mailer.SmtpServers.Add
            //    (account.outgoingServer, account.accountName, account.password)
            //    .Port = account.portOutgoingServer;

            SmtpServer server = new SmtpServer();
            server.SmtpOptions = ExtendedSmtpOptions.NoChunking;
            server.AccountName = account.accountName;
            server.Password = account.password;
            server.Name = account.outgoingServer;
            server.Port = account.portOutgoingServer;
            server.Timeout = 90000;  // 90 sec
            mailer.SmtpServers.Add(server);


            mailer.From.Email = account.emailAddress;
            mailer.Message.Charset = account.charset;
            string subject = "[ema]TTTHBC[/ema]";
            mailer.Message.Subject = subject;

            foreach (var b in listbaocao)
            {
                mailer.Message.To = new EmailAddressCollection(b.strFrom);
                mailer.Message.BodyPlainText = _mailFormat.EncodeQLVB1(b.strbody);
                try
                {
                    mailer.Send();
                    kq.id = (int)ResultViewModels.Success;
                }
                catch (MailBeeSmtpNegativeResponseException e)
                {
                    string error = DisplaySMTPError(e);
                    _logger.Info("SMTP Error: " + error);
                    kq.id = (int)e.ResponseCode;
                    kq.message = error;
                }
                catch (MailBeeException ex)
                {
                    string error = DisplayError(ex);
                    _logger.Info("SMTP Error: " + error);
                    kq.message = error;
                    kq.id = (int)ex.ErrorCode;
                }
            }
            return kq;
        }


        private ResultFunction SendSMTP_BTTTT_512(int idvanban, List<ListSendDonviViewModel> listSendDonvi)
        {
            ResultFunction kq = new ResultFunction();
            var account = _mailFormat.GetAccountSetting();
            //int intloai = (int)enumAttachVanban.intloai.Vanbanden;

            Smtp.LicenseKey = AppConts.MailBeeLicenseKey;
            Smtp mailer = new Smtp();
            mailer.SmtpServers.Add
                (account.outgoingServer, account.accountName, account.password)
                .Port = account.portOutgoingServer;

            mailer.From.Email = account.emailAddress;
            mailer.Message.Charset = account.charset;
            string subject = "[ema]TTTH[/ema]";
            mailer.Message.Subject = subject;

            string strnoidung = "";//_mailFormat.EncodeQLVB1(idvanban);

            edXMLDocument doc = _mailFormat.CreateEdXMLDocument_2803(idvanban, (int)enumGuiVanban.intloaivanban.Vanbandi);
            string strdoc = _mailFormat.edXMLDocumentToString_2803(doc);
            strnoidung += strdoc;

            //var listdonvi = listSendDonvi.ToList();
            bool isSent = false;
            foreach (var p in listSendDonvi)
            {
                mailer.Message.To = new EmailAddressCollection(p.stremailvbdt); // .Add(p.stremailvbdt);
                string strbaocao = _mailFormat.EncodeBaocaoQLVB1(account.emailAddress, idvanban, p.intid);
                string strcontent = strnoidung + strbaocao;
                mailer.Message.BodyPlainText = strcontent;
                try
                {
                    //mailer.Send();
                    _mailFormat.SaveGuiVanban(idvanban, p.intid, (int)enumGuiVanban.intloaivanban.Vanbandi);
                    _mailFormat.SaveMailOutbox(subject, strcontent, p.stremailvbdt, (int)enumMailOutbox.intloai.TTTH);
                    isSent = true;
                    kq.id = (int)ResultViewModels.Success;
                    kq.message = "Thư gửi thành công";
                }
                catch (MailBeeSmtpNegativeResponseException e)
                {
                    string error = DisplaySMTPError(e);
                    _logger.Info("SMTP Error: " + error);
                    kq.id = (int)e.ResponseCode;
                    kq.message = error;
                }
                catch (MailBeeException ex)
                {
                    string error = DisplayError(ex);
                    _logger.Info("SMTP Error: " + error);
                    kq.message = error;
                    kq.id = (int)ex.ErrorCode;
                }
            }
            if (isSent)
            {
                // cap nhat VBDt
                _mailFormat.UpdateVBDT(idvanban, (int)enumGuiVanban.intloaivanban.Vanbandi);
            }

            return kq;
        }

        private ResultFunction SendSMTP_BTTTT_2803(int idvanban, List<ListSendDonviViewModel> listSendDonvi,
            int intloaivanban, bool isUpdateTrangthai)
        {
            ResultFunction kq = new ResultFunction();
            var account = _mailFormat.GetAccountSetting();
            //int intloai = (int)enumAttachVanban.intloai.Vanbanden;

            Smtp.LicenseKey = AppConts.MailBeeLicenseKey;
            Smtp mailer = new Smtp();
            mailer.SmtpServers.Add
                (account.outgoingServer, account.accountName, account.password)
                .Port = account.portOutgoingServer;

            mailer.From.Email = account.emailAddress;
            mailer.Message.Charset = account.charset;
            string subject = "[EDXML]";
            mailer.Message.Subject = subject;

            List<int> listidDonvi = new List<int>();
            foreach (var d in listSendDonvi)
            {
                listidDonvi.Add(d.intid);
            }

            edXMLMessage edXML = _mailFormat.CreateEdXMLMessage_2803(idvanban, listidDonvi, intloaivanban);
            string strnoidung = _mailFormat.edXMLMessageToString_2803(edXML);

            // add file dinh kem
            bool isMahoaFileBase64 = AppSettings.MahoaFileBase64;
            try
            {
                if (isMahoaFileBase64)
                {
                    bool isZipFile = AppSettings.IsZipFile;

                    edXMLDocument doc = _mailFormat.CreateEdXMLDocument_2803(idvanban, intloaivanban);
                    //string strdoc = _mailFormat.edXMLDocumentToString_2803(doc);
                    foreach (var d in doc.Attach)
                    {
                        try
                        {
                            if (isZipFile)
                            {
                                using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                                {
                                    string foldePath = _fileManager.SetPathUpload(AppConts.FileEmailOutbox);
                                    string fileName = d.ContentID + ".zip";
                                    string fullName = System.IO.Path.Combine(foldePath, fileName);

                                    zip.AddEntry(d.ContentID + ".txt", d.Value);
                                    zip.Save(fullName);

                                    mailer.Message.Attachments.Add(fullName, fileName);
                                }
                            }
                            else
                            {
                                string foldePath = _fileManager.SetPathUpload(AppConts.FileEmailOutbox);
                                string fileName = d.ContentID + ".txt";
                                string fullName = System.IO.Path.Combine(foldePath, fileName);

                                System.IO.File.WriteAllText(fullName, d.Value);

                                mailer.Message.Attachments.Add(fullName, fileName);
                            }

                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex.Message);
                        }
                    }
                    //strnoidung += strdoc;
                }
                else
                {
                    int intloaifile = 0;
                    switch (intloaivanban)
                    {
                        case (int)enumGuiVanban.intloaivanban.Vanbandi:
                            intloaifile = (int)enumAttachVanban.intloai.Vanbandi;
                            break;
                        case (int)enumGuiVanban.intloaivanban.Vanbanden:
                            intloaifile = (int)enumAttachVanban.intloai.Vanbanden;
                            break;
                    }
                    List<ListFileToAttach> listfile = _mailFormat.GetFileVanbanToAttach(idvanban, intloaifile);
                    foreach (var file in listfile)
                    {
                        if (System.IO.File.Exists(file.filePath))
                        {
                            mailer.Message.Attachments.Add(file.filePath, file.strmota);
                        }
                        else
                        {
                            _logger.Warn("Gửi mail văn bản: " + idvanban + ", không tìm thấy file đính kèm: " + file.filePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            bool isSent = false;
            bool isLogVBDi = AppSettings.IsLogVBDTDi;
            foreach (var p in listSendDonvi)
            {
                mailer.Message.To = new EmailAddressCollection(p.stremailvbdt); // .Add(p.stremailvbdt);
                string strcontent = strnoidung;
                mailer.Message.BodyPlainText = strcontent;
                try
                {
                    mailer.Send();
                    if (isUpdateTrangthai)
                    {   // tu dong gui thi cap nhat trang thai
                        _mailFormat.UpdateGuiVanban(idvanban, p.intid, intloaivanban);
                    }
                    else
                    {   // them moi trong table guivanban
                        _mailFormat.SaveGuiVanban(idvanban, p.intid, intloaivanban);
                    }
                    if (isLogVBDi)
                    {
                        // luu nhat ky gui                   
                        _mailFormat.SaveMailOutbox(subject, strcontent, p.stremailvbdt, (int)enumMailOutbox.intloai.BTTTT_2803);
                    }
                    isSent = true;
                    kq.id = (int)ResultViewModels.Success;
                    kq.message = "Thư gửi thành công";
                }
                catch (MailBeeSmtpNegativeResponseException e)
                {
                    string error = DisplaySMTPError(e);
                    _logger.Info("SMTP Error: " + error);
                    kq.id = (int)e.ResponseCode;
                    kq.message = error;
                }
                catch (MailBeeException ex)
                {
                    string error = DisplayError(ex);
                    _logger.Info("SMTP Error: " + error);
                    kq.message = error;
                    kq.id = (int)ex.ErrorCode;
                }
            }
            if (isSent)
            {
                // cap nhat trang thai van ban da gui dien tu VBDt
                _mailFormat.UpdateVBDT(idvanban, intloaivanban);
            }

            return kq;
        }

        /// <summary>
        /// gui mail hoi bao theo 2803
        /// </summary>
        /// <param name="listbaocao"></param>
        /// <returns></returns>
        private ResultFunction SendSMTP_BTTTT_2803_Baocao(List<QLVB.DTO.Vanbandientu.EdXMLBC.edXMLBaocao> listbaocao)
        {
            ResultFunction kq = new ResultFunction();
            var account = _mailFormat.GetAccountSetting();
            //int intloai = (int)enumAttachVanban.intloai.Vanbanden;

            Smtp.LicenseKey = AppConts.MailBeeLicenseKey;
            Smtp mailer = new Smtp();
            mailer.SmtpServers.Add
                (account.outgoingServer, account.accountName, account.password)
                .Port = account.portOutgoingServer;

            mailer.From.Email = account.emailAddress;
            mailer.Message.Charset = account.charset;
            string subject = "[EDXMLBC]";
            mailer.Message.Subject = subject;

            foreach (var b in listbaocao)
            {
                mailer.Message.To = new EmailAddressCollection(b.FromEmail);
                mailer.Message.BodyPlainText = _mailFormat.edXMLBaocaoToString(b);
                try
                {
                    mailer.Send();
                    kq.id = (int)ResultViewModels.Success;
                }
                catch (MailBeeSmtpNegativeResponseException e)
                {
                    string error = DisplaySMTPError(e);
                    _logger.Info("SMTP Error: " + error);
                    kq.id = (int)e.ResponseCode;
                    kq.message = error;
                }
                catch (MailBeeException ex)
                {
                    string error = DisplayError(ex);
                    _logger.Info("SMTP Error: " + error);
                    kq.message = error;
                    kq.id = (int)ex.ErrorCode;
                }
            }
            return kq;
        }

        /// <summary>
        /// gui binh thuong khong ma hoa theo cac tieu chuan khac
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="listSendDonvi"></param>
        /// <param name="strnoidung"></param>
        /// <returns></returns>
        private ResultFunction SendSMTP_Donvikhac(int idvanban, List<ListSendDonviViewModel> listSendDonvi,
            int intloaivanban, bool isUpdateTrangthai)
        {
            ResultFunction kq = new ResultFunction();
            var account = _mailFormat.GetAccountSetting();
            //int intloai = (int)enumAttachVanban.intloai.Vanbanden;

            Smtp.LicenseKey = AppConts.MailBeeLicenseKey;
            Smtp mailer = new Smtp();
            //mailer.SmtpServers.Add
            //    (account.outgoingServer, account.accountName, account.password)
            //    .Port = account.portOutgoingServer;

            SmtpServer server = new SmtpServer();
            server.SmtpOptions = ExtendedSmtpOptions.NoChunking;
            server.AccountName = account.accountName;
            server.Password = account.password;
            server.Name = account.outgoingServer;
            server.Port = account.portOutgoingServer;
            server.Timeout = 90000;  // 90 sec
            mailer.SmtpServers.Add(server);

            mailer.From.Email = account.emailAddress;
            mailer.Message.Charset = account.charset;

            string subject = _mailFormat.GetTrichyeuVanbanSendNormal(idvanban, intloaivanban);
            //"Văn bản " + QLVB.Donvi.Donvi.GetTenDonVi();
            mailer.Message.Subject = subject;

            string strnoidung = "";

            switch (intloaivanban)
            {
                case (int)enumGuiVanban.intloaivanban.Vanbandi:

                    strnoidung = _mailFormat.GetNoidungVanbanSendNormal(idvanban, intloaivanban);
                    List<ListFileToAttach> listfile = _mailFormat.GetFileVanbanToAttach(idvanban, (int)enumAttachVanban.intloai.Vanbandi);
                    foreach (var file in listfile)
                    {
                        if (System.IO.File.Exists(file.filePath))
                        {
                            mailer.Message.Attachments.Add(file.filePath, file.strmota);
                        }
                        else
                        {
                            _logger.Warn("Gửi mail văn bản đi: " + idvanban + ", không tìm thấy file đính kèm: " + file.filePath);
                        }
                    }
                    break;

                case (int)enumGuiVanban.intloaivanban.Vanbanden:

                    strnoidung = _mailFormat.GetNoidungVanbanSendNormal(idvanban, intloaivanban);
                    listfile = _mailFormat.GetFileVanbanToAttach(idvanban, (int)enumAttachVanban.intloai.Vanbanden);
                    foreach (var file in listfile)
                    {
                        if (System.IO.File.Exists(file.filePath))
                        {
                            mailer.Message.Attachments.Add(file.filePath, file.strmota);
                        }
                        else
                        {
                            _logger.Warn("Gửi mail văn bản đến: " + idvanban + ", không tìm thấy file đính kèm: " + file.filePath);
                        }
                    }
                    break;
            }

            bool isLogVBDi = AppSettings.IsLogVBDTDi;
            foreach (var b in listSendDonvi)
            {
                mailer.Message.To = new EmailAddressCollection(b.stremailvbdt);
                mailer.Message.BodyPlainText = strnoidung;
                try
                {
                    mailer.Send();
                    if (isUpdateTrangthai)
                    {   // tu dong gui thi cap nhat trang thai
                        _mailFormat.UpdateGuiVanban(idvanban, b.intid, intloaivanban);
                    }
                    else
                    {   // them moi trong table guivanban
                        _mailFormat.SaveGuiVanban(idvanban, b.intid, intloaivanban);
                    }
                    if (isLogVBDi)
                    {
                        // luu nhat ky gui
                        _mailFormat.SaveMailOutbox(subject, strnoidung, b.stremailvbdt, (int)enumMailOutbox.intloai.Khac);
                    }
                    kq.id = (int)ResultViewModels.Success;
                    kq.message = "Thư gửi thành công";
                }
                catch (MailBeeSmtpNegativeResponseException e)
                {
                    string error = DisplaySMTPError(e);
                    _logger.Info("SMTP Error: " + error);
                    kq.id = (int)e.ResponseCode;
                    kq.message = error;
                }
                catch (MailBeeException ex)
                {
                    string error = DisplayError(ex);
                    _logger.Info("SMTP Error: " + error);
                    kq.message = error;
                    kq.id = (int)ex.ErrorCode;
                }
            }
            return kq;
        }


        #endregion SMTP

        #region SendEmailKhac
        public ResultFunction SendEmailKhac(int intloaivanban, int idvanban, string donviEmailKhac,
            string strtieudeEmailKhac, string strnoidungEmailKhac)
        {
            ResultFunction kq = new ResultFunction();
            var account = _mailFormat.GetAccountSetting();

            Smtp.LicenseKey = AppConts.MailBeeLicenseKey;
            Smtp mailer = new Smtp();
            mailer.SmtpServers.Add
                (account.outgoingServer, account.accountName, account.password)
                .Port = account.portOutgoingServer;

            mailer.From.Email = account.emailAddress;
            mailer.Message.Charset = account.charset;

            mailer.Message.Subject = strtieudeEmailKhac;

            string strnoidung = strnoidungEmailKhac;

            switch (intloaivanban)
            {
                case (int)enumGuiVanban.intloaivanban.Vanbandi:
                    List<ListFileToAttach> listfile = _mailFormat.GetFileVanbanToAttach(idvanban, (int)enumAttachVanban.intloai.Vanbandi);
                    foreach (var file in listfile)
                    {
                        if (System.IO.File.Exists(file.filePath))
                        {
                            mailer.Message.Attachments.Add(file.filePath, file.strmota);
                        }
                        else
                        {
                            _logger.Warn("Gửi mail văn bản đi: " + idvanban + ", không tìm thấy file đính kèm: " + file.filePath);
                        }
                    }
                    break;

                case (int)enumGuiVanban.intloaivanban.Vanbanden:
                    listfile = _mailFormat.GetFileVanbanToAttach(idvanban, (int)enumAttachVanban.intloai.Vanbanden);
                    foreach (var file in listfile)
                    {
                        if (System.IO.File.Exists(file.filePath))
                        {
                            mailer.Message.Attachments.Add(file.filePath, file.strmota);
                        }
                        else
                        {
                            _logger.Warn("Gửi mail văn bản đến: " + idvanban + ", không tìm thấy file đính kèm: " + file.filePath);
                        }
                    }
                    break;
            }

            string[] listemail = donviEmailKhac.Split(';');

            foreach (var email in listemail)
            {
                mailer.Message.To = new EmailAddressCollection(email);
                mailer.Message.BodyPlainText = strnoidung;
                try
                {
                    mailer.Send();

                    kq.id = (int)ResultViewModels.Success;
                    kq.message = "Thư gửi thành công";
                }
                catch (MailBeeSmtpNegativeResponseException e)
                {
                    string error = DisplaySMTPError(e);
                    _logger.Info("SMTP Error: " + error);
                    kq.id = (int)e.ResponseCode;
                    kq.message = error;
                }
                catch (MailBeeException ex)
                {
                    string error = DisplayError(ex);
                    _logger.Info("SMTP Error: " + error);
                    kq.message = error;
                    kq.id = (int)ex.ErrorCode;
                }
            }
            return kq;
        }

        #endregion SendEmailKhac

    }
}
