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
using QLVB.DTO.Vanbandientu;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.Donvi;
using ActiveUp.Net.Mail;
using ActiveUp.Net.Imap4;
using ActiveUp.Net.Security;
using ActiveUp.Net.Dns;
using QLVB.DTO.File;
using System.Web;
using CipherText;

namespace QLVB.Core.Implementation
{
    public class MailActiveManager// : IMailManager
    {
        #region Constructor

        private ILogger _logger;
        private ITochucdoitacRepository _tochucRepo;
        private IMailFormatManager _mailFormat;
        private IFileManager _fileManager;
        private IMailInboxRepository _inboxRepo;
        private IAttachMailRepository _attachMailRepo;
        private IVanbandenmailRepository _vbdenMailRepo;

        public MailActiveManager(ILogger logger, ITochucdoitacRepository tochucRepo,
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

        #region POP3

        /// <summary>
        /// tra ve so mail con tren hop thu sau khi lay ve
        /// </summary>
        /// <returns></returns>
        public ResultFunction RecievePOP3()
        {
            ResultFunction kq = new ResultFunction();
            var account = _mailFormat.GetAccountSetting();
            Pop3Client pop = new Pop3Client();
            try
            {
                // Connect to the pop3 client
                pop.Connect(account.incomingMailServer, account.portIncomingServer, account.accountName, account.password);
                int MessageCount = pop.MessageCount;
                if (MessageCount > 0)
                {
                    int MessageDownload = MessageCount > 3 ? 3 : MessageCount;

                    string folderPath = _fileManager.SetPathUpload(AppConts.FileEmailInbox);

                    int p = 1;
                    for (p = 1; p <= MessageDownload; p++)
                    {
                        ActiveUp.Net.Mail.Message message = pop.RetrieveMessageObject(p);

                        int msgAttachCount = message.Attachments.Count;

                        //Header msgHeader = pop.RetrieveHeaderObject(p);

                        message.Charset = account.charset; //"1252";

                        message.BodyText.Charset = account.charset;
                        string subject = message.Subject;

                        string bodytext = message.BodyText.Text;
                        string bodytextconvert = ConvertUnicode.Utf8ToUnicode(bodytext);

                        message.BodyHtml.Charset = account.charset;
                        string bodyhtml = message.BodyHtml.Text;

                        string from = message.From.Email;

                        string datesend = DateServices.FormatDateTimeVN(message.Date);

                        string strheader = "From: " + from
                                    + " Date Sever: " + datesend;
                        //+ " To: " + message

                        int idmail = 0;
                        int idinbox = 0;
                        int idloai = (int)enumAttachMail.intloai.MailInbox;

                        if (subject == "[ema]TTTH[/ema]")
                        {
                            idloai = (int)enumAttachMail.intloai.Vanbandendientu;
                            folderPath = _fileManager.SetPathUpload(AppConts.FileEmail);

                            // luu vao table vanbandenmail
                            bool IsAttach = msgAttachCount > 0 ? true : false;
                            idmail = _mailFormat.DecodeQLVB1(bodytext, IsAttach);

                            // luu vao mail inbox
                            idinbox = _mailFormat.SaveMailInbox(subject, strheader, bodytext, from, (int)enumMailInbox.intloai.TTTH);

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
                            for (int i = 0; i < msgAttachCount; i++)
                            {
                                if (idloai == (int)enumAttachMail.intloai.Vanbandendientu)
                                {
                                    try
                                    {
                                        string strmota = message.Attachments[i].Filename;
                                        string fileSavepath = _mailFormat.SaveAttachment(idmail, strmota, folderPath);
                                        message.Attachments[i].StoreToFile(fileSavepath);

                                        //  kiem tra file da save ve server chua                                    
                                        if (System.IO.File.Exists(fileSavepath))
                                        {
                                            // insert vao database attachmail                                    
                                            _mailFormat.InsertAttachment(idmail, fileSavepath, strmota, idloai);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.Info(ex.Message);
                                    }
                                }
                                if (idloai == (int)enumAttachMail.intloai.MailInbox)
                                {
                                    try
                                    {
                                        string strmota = message.Attachments[i].Filename;
                                        string fileSavepath = _mailFormat.SaveAttachment(idmail, strmota, folderPath);
                                        //message.Attachments[i].StoreToFile(fileSavepath);

                                        //  kiem tra file da save ve server chua                                    
                                        if (System.IO.File.Exists(fileSavepath))
                                        {
                                            // insert vao database attachmail                                    
                                            //_mailFormat.InsertAttachment(idmail, fileSavepath, idloai);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.Info(ex.Message);
                                    }
                                }

                            }
                        }// end attach file

                        // delete mail 


                    }

                    // tra ve so mail chua down ve
                    kq.id = MessageCount - p;
                    return kq;
                }

                else
                {
                    //this.AddLogEntry("There is no message in this pop3 account");
                    kq.id = 0;
                    return kq;
                }
            }
            catch (Pop3Exception pexp)
            {
                _logger.Info("POP3 Error: " + pexp.Message);
            }
            catch (Exception ex)
            {
                if (ex.Message == "POP3 Error: The requested name is valid, but no data of the requested type was found")
                {
                    _logger.Info("POP3 Error: Không kết nối được với Mail Server");
                }
                else
                {
                    _logger.Info("POP3 Error: " + ex.Message);
                }
            }
            finally
            {
                if (pop.IsConnected)
                {
                    pop.Disconnect();
                }
            }
            return kq;
        }

        #endregion POP3


        #region SMTP
        public ResultFunction SendSMTP(int idvanban, List<ListSendDonviViewModel> listiddonvi)
        {
            ResultFunction kq = new ResultFunction();

            var account = _mailFormat.GetAccountSetting();

            int intloai = (int)enumAttachVanban.intloai.Vanbanden;

            List<string> allfiles = _mailFormat.GetFileAttach(idvanban, intloai);

            // We create the message object
            ActiveUp.Net.Mail.Message message = new ActiveUp.Net.Mail.Message();

            message.Charset = account.charset;

            // We assign the sender email
            message.From.Email = account.emailAddress;

            foreach (var p in listiddonvi)
            {
                message.To.Add(p.stremailvbdt);
            }

            // We assign the recipient email
            //message.To.Add(toEmail);

            // We assign the subject
            //message.Subject = "Tiêu đề gửi mail";
            string subject = "Tiêu đề gửi mail";
            message.Subject = "=?UTF-8?B?" + Convert.ToBase64String(Encoding.UTF8.GetBytes(subject)) + "?=";

            // We assign the body text
            //message.BodyText.Text = "trích yếu nội dung văn bản";
            string bodytext = "trích yếu nội dung văn bản";

            //message.BodyHtml.Charset = charset;
            //message.BodyHtml.Text = bodytext;

            message.BodyText.Charset = account.charset;
            message.BodyText.Text = bodytext;

            // We now add each attachments
            foreach (string attachmentPath in allfiles)
            {
                message.Attachments.Add(attachmentPath, false);
            }

            try
            {

                SmtpClient.Send(message, account.outgoingServer, account.portOutgoingServer,
                    account.accountName, account.password, SaslMechanism.Login);

                //SmtpClient.Send(message, "mail.example.com", 25, "user1@example.com", "userpassword", SaslMechanism.CramMd5);

                //this.AddLogEntry("Message sent successfully.");
            }
            catch (SmtpException ex)
            {
                _logger.Info("SMTP Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Info("SMTP Error: " + ex.Message);
            }
            return kq;
        }


        public ResultFunction SendSMTP_TTTHBaocao(List<TTTHBaocaoViewModel> listbaocao)
        {
            ResultFunction kq = new ResultFunction();
            return kq;
        }
        #endregion SMTP


    }
}
