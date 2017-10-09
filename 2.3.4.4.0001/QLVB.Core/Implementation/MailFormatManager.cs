using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Core.Contract;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.Common.Logging;
using QLVB.DTO.Mail;
using QLVB.Common.Utilities;
using QLVB.Common.Date;
using System.IO;
using QLVB.DTO.Vanbandientu;
using QLVB.DTO.Vanbandientu.EdXML;
using YAXLib;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace QLVB.Core.Implementation
{
    public class MailFormatManager : IMailFormatManager
    {
        #region Constructor
        private ILogger _logger;
        private ISoVanbanRepository _sovbRepo;
        private ITochucdoitacRepository _tochucRepo;
        private IGuiVanbanRepository _guivbRepo;
        private IVanbandiRepository _vanbandiRepo;
        private IConfigRepository _configRepo;
        private IAttachVanbanRepository _fileRepo;
        private IFileManager _fileManager;
        private IVanbandenmailRepository _vbdenMailRepo;
        private IAttachMailRepository _attachMailRepo;
        private IMailInboxRepository _inboxRepo;
        private IMailOutboxRepository _outboxRepo;
        private IPhanloaiVanbanRepository _plvanbanRepo;
        private IVanbandenRepository _vbdenRepo;
        //private IVanbandiRepository _vbdiRepo;

        public MailFormatManager(ILogger logger, ISoVanbanRepository sovbRepo,
            ITochucdoitacRepository tochucRepo, IGuiVanbanRepository guivbRepo,
            IVanbandiRepository vanbandiRepo, IConfigRepository configRepo,
            IAttachVanbanRepository fileRepo, IFileManager fileManager,
            IVanbandenmailRepository vbdenMailRepo, IAttachMailRepository attachMailRepo,
            IMailInboxRepository inboxRepo, IMailOutboxRepository outboxRepo,
            IPhanloaiVanbanRepository plvanbanRepo,
            IVanbandenRepository vbdenRepo//, IVanbandiRepository vbdiRepo
            )
        {
            _logger = logger;
            _sovbRepo = sovbRepo;
            _tochucRepo = tochucRepo;
            _guivbRepo = guivbRepo;
            _vanbandiRepo = vanbandiRepo;
            _configRepo = configRepo;
            _fileRepo = fileRepo;
            _fileManager = fileManager;
            _vbdenMailRepo = vbdenMailRepo;
            _attachMailRepo = attachMailRepo;
            _inboxRepo = inboxRepo;
            _outboxRepo = outboxRepo;
            _plvanbanRepo = plvanbanRepo;
            _vbdenRepo = vbdenRepo;
            //_vbdiRepo = vbdiRepo;
        }

        #endregion Constructor

        #region Config
        /// <summary>
        /// cau hinh mail
        /// </summary>
        /// <returns></returns>
        public AccountSettingViewModel GetAccountSetting()
        {
            string email = _configRepo.GetConfig(ThamsoHethong.UsernameMail);

            int pos = email.IndexOf('@');
            string name = email.Substring(0, pos);

            AccountSettingViewModel account = new AccountSettingViewModel();
            account.accountName = name;
            account.emailAddress = email;
            account.password = _configRepo.GetConfig(ThamsoHethong.PasswordMail);

            //pop3
            account.incomingMailServer = _configRepo.GetConfig(ThamsoHethong.POP3Server);
            account.portIncomingServer = _configRepo.GetConfigToInt(ThamsoHethong.POP3Port);

            // smtp
            account.isOutgoingWithAuthentication = true;
            account.portOutgoingServer = _configRepo.GetConfigToInt(ThamsoHethong.SMTPPort);
            account.outgoingServer = _configRepo.GetConfig(ThamsoHethong.SMTPServer);

            account.charset = "utf-8";

            return account;
        }

        /// <summary>
        /// ten don vi gui mail (dll donvi)
        /// </summary>
        /// <returns></returns>
        public string GetTendonvi()
        {
            //string strten = GetTenDonvi(Donvi.Donvi.GetTenDonVi());
            return Donvi.Donvi.GetTenDonVi();
        }

        /// <summary>
        /// ma dinh danh cua don vi 
        /// </summary>
        /// <returns></returns>
        public string GetMaDinhdanh()
        {
            return Donvi.Donvi.GetMaDinhDanh();
        }

        /// <summary>
        /// thong tin van ban gui mail
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        public ThongtinVanbanViewModel GetThongtinVanban(int idvanban)
        {
            ThongtinVanbanViewModel vanban = new ThongtinVanbanViewModel();

            var _vb = _vanbandiRepo.Vanbandis
                .Where(p => p.intid == idvanban)
                .FirstOrDefault();
            if (_vb == null)
            {
                return null;
            }
            vanban.strsodi = _vb.intso.ToString();
            vanban.strkyhieu = _vb.strkyhieu;
            vanban.strloaivanban = "";
            vanban.strngayky = DateServices.FormatDateVN(_vb.strngayky);
            vanban.strnguoiky = _vb.strnguoiky;
            vanban.strnoiky = "Đồng Nai"; // khai bao trong config

            return vanban;
        }

        public List<string> GetFileAttach(int idvanban, int intloai)
        {
            string strloai = string.Empty;
            if (intloai == (int)enumAttachVanban.intloai.Vanbanden)
            {
                strloai = AppConts.FileCongvanden;
            }
            if (intloai == (int)enumAttachVanban.intloai.Vanbandi)
            {
                strloai = AppConts.FileCongvanphathanh;
            }

            List<string> allfiles = new List<string>();
            var files = _fileRepo.AttachVanbans
                .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                .Where(p => p.intidvanban == idvanban)
                .Where(p => p.intloai == intloai);
            if (files != null)
            {
                foreach (var f in files)
                {
                    string filePath = _fileManager.GetPhysicalPath(strloai, (DateTime)f.strngaycapnhat, f.strtenfile);
                    allfiles.Add(filePath);
                }
            }
            return allfiles;
        }

        #endregion Config

        #region WriteFile

        /// <summary>
        /// tao file ebxml theo quy dinh cua BTTTT
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns>file path</returns>
        public string CreateEdXML(int idvanban)
        {
            var vanban = _vanbandiRepo.Vanbandis
                .FirstOrDefault(p => p.intid == idvanban);

            string folderSavepath = _fileManager.SetPathUpload(AppConts.FileEmailOutbox);
            string fileName = "QLVB.xml";
            string fileSavepath = Path.Combine(folderSavepath, fileName);

            string node = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileSavepath))
            {
                file.WriteLine(node);

                string urlenvelope = "http://schemas.xmlsoap.org/soap/envelope/";
                string urlschema = "http://www.w3.org/2001/XMLSchema-instance";
                string urllink = "http://www.w3.org/1999/xlink";
                string urllocation = "http://schemas.xmlsoap.org/soap/envelope/";
                node = "<soap-env:Envelope xmlns:soap-env=\"" + urlenvelope + "\" xmlns:xsi=\"" + urlschema
                   + "\" xmlns:xlink=\"" + urllink + "\" xsi:schemaLocation=\"" + urllocation + "\">";

                file.WriteLine(node);
            }

            _CreateSoapHeader(fileSavepath);

            return "";
        }

        #region Private Methods

        /// <summary>
        /// tao node trong xml tuy theo cap tab
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private string CreateNodeEdXML(string value, string name, int inttab)
        {
            //http://blogs.msdn.com/b/csharpfaq/archive/2004/03/12/88415.aspx
            //http://msdn.microsoft.com/en-us/library/h21280bw.aspx

            string strtab = string.Empty;
            for (int i = 1; i <= inttab; i++)
            {
                strtab += "\t";
            }
            string truongbd = "<edXML:" + name + ">";
            string truongkt = "</edXML:" + name + ">";

            string strvalue = strtab + truongbd + value + truongkt;
            return strvalue;
        }

        private void _CreateSoapHeader(string filepath)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true))
            {
                string node = "\t" + "<soap-env:Header>";
                file.WriteLine(node);
            }
            _AddMessageHeader(filepath);

            //_AddTraceHeaderList

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true))
            {
                string node = "\t" + "</soap-env:Header>";
                file.WriteLine(node);
            }
        }

        private void _AddMessageHeader(string filepath)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true))
            {
                string node = "\t\t" + "<edXML:MessageHeader  OriginalBodyRequested=\"false\" ImmediateResponseRequired=\"true\">";
                file.WriteLine(node);
            }
            _AddHeaderFrom(filepath);

            string strMaDinhDanh = "00.01.H03";
            string strTenDonvi = "Sở Thông tin và Truyền thông Bắc Kạn";
            string strEmail = "to@dongnai.gov.vn";
            _AddHeaderTo(filepath, strMaDinhDanh, strTenDonvi, strEmail);

            string strCodeNumber = "02";
            string strkyhieu = "UBND-TTTH";
            _AddHeaderCode(filepath, strCodeNumber, strkyhieu);

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true))
            {
                string stridvanban = "1234";
                string node = CreateNodeEdXML(stridvanban, "DocumentId", 3);
                file.WriteLine(node);
            }




            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true))
            {
                string node = "\t\t" + "</edXML:MessageHeader>";
                file.WriteLine(node);
            }
        }

        private void _AddHeaderFrom(string filepath)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true))
            {
                string node = "\t\t\t" + "<edXML:From>";
                file.WriteLine(node);

                string value = GetMaDinhdanh();
                node = CreateNodeEdXML(value, "OrganId", 4);
                file.WriteLine(node);

                value = GetTendonvi();
                node = CreateNodeEdXML(value, "OrganName", 4);
                file.WriteLine(node);

                value = GetAccountSetting().emailAddress;
                node = CreateNodeEdXML(value, "Email", 4);
                file.WriteLine(node);

                node = "\t\t\t" + "</edXML:From>";
                file.WriteLine(node);
            }
        }

        private void _AddHeaderTo(string filepath, string strMaDinhDanh, string strTenDonvi, string strEmail)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true))
            {
                string node = "\t\t\t" + "<edXML:To>";
                file.WriteLine(node);

                node = CreateNodeEdXML(strMaDinhDanh, "OrganId", 4);
                file.WriteLine(node);

                node = CreateNodeEdXML(strTenDonvi, "OrganName", 4);
                file.WriteLine(node);

                node = CreateNodeEdXML(strEmail, "Email", 4);
                file.WriteLine(node);

                node = "\t\t\t" + "</edXML:To>";
                file.WriteLine(node);
            }
        }


        /// <summary>
        /// so va ky hieu van ban
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="strCodeNumber"></param>
        /// <param name="strCodeNotation"></param>
        private void _AddHeaderCode(string filepath, string strCodeNumber, string strCodeNotation)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true))
            {
                string node = "\t\t\t" + "<edXML:Code>";
                file.WriteLine(node);

                node = CreateNodeEdXML(strCodeNumber, "CodeNumber", 4);
                file.WriteLine(node);

                node = CreateNodeEdXML(strCodeNotation, "CodeNotation", 4);
                file.WriteLine(node);

                node = "\t\t\t" + "</edXML:From>";
                file.WriteLine(node);
            }
        }

        /// <summary>
        /// loai van ban
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="strType"></param>
        /// <param name="strTypeName"></param>
        private string _AddHeaderDocumentType(string filepath, string strType, string strTypeName)
        {
            string node = "\t\t\t" + "<edXML:DocumentType>" + "\n";

            node += CreateNodeEdXML(strType, "Type", 4) + "\n";

            node += CreateNodeEdXML(strTypeName, "TypeName", 4) + "\n";

            node = "\t\t\t" + "</edXML:DocumentType>" + "\n";

            return node;
        }

        private void writefile()
        {
            // Example #1: Write an array of strings to a file. 
            // Create a string array that consists of three lines. 
            string[] lines = { "First line", "Second line", "Third line" };
            // WriteAllLines creates a file, writes a collection of strings to the file, 
            // and then closes the file.
            System.IO.File.WriteAllLines(@"C:\Users\Public\TestFolder\WriteLines.txt", lines);


            // Example #2: Write one string to a text file. 
            string text = "A class is the most powerful data type in C#. Like a structure, " +
                           "a class defines the data and behavior of the data type. ";
            // WriteAllText creates a file, writes the specified string to the file, 
            // and then closes the file.
            System.IO.File.WriteAllText(@"C:\Users\Public\TestFolder\WriteText.txt", text);

            // Example #3: Write only some strings in an array to a file. 
            // The using statement automatically closes the stream and calls  
            // IDisposable.Dispose on the stream object. 
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"E:\WriteLines2.xml"))
            {
                string ebxmlbd = "<edXML:From>";
                string ebxmlkt = "</edXML:From>";
                foreach (string line in lines)
                {
                    // If the line doesn't contain the word 'Second', write the line to the file. 
                    if (!line.Contains("Second"))
                    {
                        file.WriteLine("\t" + ebxmlbd + line + ebxmlkt);
                    }
                    else
                    {
                        file.WriteLine("\t\t" + ebxmlbd + line + ebxmlkt);
                    }
                }
            }

            // Example #4: Append new text to an existing file. 
            // The using statement automatically closes the stream and calls  
            // IDisposable.Dispose on the stream object. 
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Public\TestFolder\WriteLines2.txt", true))
            {
                file.WriteLine("Fourth line");
            }
        }
        public void read()
        {
            // create reader & open file
            System.IO.TextReader tr = new StreamReader("date.txt");

            // read a line of text
            Console.WriteLine(tr.ReadLine());

            // close the stream
            tr.Close();
        }

        #endregion Private Methods

        #endregion WriteFile

        #region QLVB1

        #region Receive
        /// <summary>
        /// lay gia tri truong can tim
        /// </summary>
        /// <param name="strmessage"></param>
        /// <param name="strtruong"></param>
        /// <returns></returns>
        public string _GetTruong(string strmessage, string strtruong)
        {
            //[strNgayky]5/5/2014[/strNgayky][strKyhieu]TB-UBND[/strKyhieu]
            //[intidphanloaicongvanden]18[/intidphanloaicongvanden][intKhan]1[/intKhan]
            //[intMat]104[/intMat]
            //[strTrichyeu]trich yeu van ban[/strTrichyeu]
            //[strNguoiky]Võ Văn Tính[/strNguoiky]
            //[strNoigui]các sở ban ngành tỉnh, các ban ngành đoàn thể huyện[/strNoigui]
            //[strLoaivanban]Thông báo[/strLoaivanban][strNoiguiVB]UBND HUYỆN NHƠN TRẠCH - TỈNH ĐỒNG NAI[/strNoiguiVB]

            // Baocao
            //[strFromAddress]vbhnhontrach@dongnai.gov.vn[/strFromAddress]
            //[strNgayguiVB]05/06/2014 09:26:04[/strNgayguiVB]
            //[intidVB]57447[/intidVB][intidDonvi]318[/intidDonvi]

            try
            {
                string truongbd = "[" + strtruong + "]";
                string truongkt = "[/" + strtruong + "]";

                int lenbd = truongbd.Length;
                int intPosbd = strmessage.IndexOf(truongbd, StringComparison.OrdinalIgnoreCase);

                int lenkt = truongkt.Length;
                int intPoskt = strmessage.IndexOf(truongkt, StringComparison.OrdinalIgnoreCase);

                string strvalue = strmessage.Substring(intPosbd + lenbd, intPoskt - intPosbd - lenbd);
                if (!string.IsNullOrEmpty(strvalue))
                {
                    return strvalue;
                }
                else
                {
                    return null;
                }
            }
            catch //(Exception ex)
            {
                _logger.Error("Decode:_" + strtruong + "_" + strmessage);
                return null;
            }
        }

        /// <summary>
        /// giai ma va luu vao database
        /// </summary>
        /// <param name="strmessage"></param>
        /// <returns>idvanbandenmail</returns>
        public int DecodeQLVB1(string strmessage, bool isAttach)
        {
            CipherText.ConvertServices _convert = new CipherText.ConvertServices();

            int intPos = strmessage.IndexOf("[TTTHmail]");
            string strbody = string.Empty;
            try
            {
                strbody = strmessage.Substring(0, intPos);
                strbody = _convert.GiaiMa_Unicode(strbody);
            }
            catch
            {
                _logger.Error("POP3 Error Decode TTTHmail: " + strmessage);
                return 0;
            }


            int intPosBC = strmessage.IndexOf("[TTTHBCmail]");
            string strbaocao = "";
            if (intPosBC != -1)
            {
                try
                {
                    strbaocao = strmessage.Substring(intPos + 10, intPosBC - intPos - 10);
                    strbaocao = _convert.GiaiMa_Unicode(strbaocao);
                }
                catch
                {
                    _logger.Error("POP3 Error Decode TTTHBCmail: " + strmessage);
                }
            }

            try
            {
                Vanbandenmail vb = new Vanbandenmail();
                vb.intattach = isAttach ? (int)enumVanbandenmail.intattach.Co : (int)enumVanbandenmail.intattach.Khong;
                vb.intidphanloaivanbanden = Convert.ToInt32(_GetTruong(strbody, "intidphanloaicongvanden"));
                vb.intkhan = Convert.ToInt32(_GetTruong(strbody, "intKhan"));
                //vb.intloai = 
                vb.intso = Convert.ToInt32(_GetTruong(strbody, "intMat"));
                vb.strkyhieu = _GetTruong(strbody, "strKyhieu");
                string strngay = _GetTruong(strbody, "strNgayky");
                DateTime ngayky;

                if (DateTime.TryParseExact(strngay, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out ngayky))
                {
                    vb.strngayky = ngayky;
                }
                else
                {
                    if (Utils.IsDate(strngay))
                    {
                        vb.strngayky = Convert.ToDateTime(strngay);
                    }
                    else
                    {
                        vb.strngayky = DateTime.Today;
                    }
                }                 
                
                vb.strnguoiky = _GetTruong(strbody, "strNguoiky");
                vb.strnoigui = _GetTruong(strbody, "strNoigui");
                vb.strnoiguivb = _GetTruong(strbody, "strNoiguiVB");
                vb.strloaivanban = _GetTruong(strbody, "strLoaivanban");
                vb.strtrichyeu = _GetTruong(strbody, "strTrichyeu");

                strngay = _GetTruong(strbaocao, "strNgayguiVB");
                if (Utils.IsDate(strngay))
                {
                    vb.strngayguivb = Convert.ToDateTime(strngay);
                }
                else
                {
                    vb.strngayguivb = null;// DateServices.FormatDateEn(strngay);
                }

                vb.strAddressSend = _GetTruong(strbaocao, "strFromAddress");

                vb.strngaynhanvb = DateTime.Now;

                vb.intnhanvanbantu = enumVanbandenmail.intnhanvanbantu.Email;

                int id = _vbdenMailRepo.Them(vb);

                return id;
            }
            catch (Exception ex)
            {
                _logger.Error("POP3 Error add mail: " + ex.Message);
                return 0;
            }
        }


        /// <summary>
        /// thong tin bao cao theo doi cua van ban dien tu
        /// </summary>
        /// <param name="strmessage"></param>
        /// <returns>1: OK ; 0: Error</returns>
        public int DecodeBaocaoQLVB1(string strmessage)
        {
            CipherText.ConvertServices _convert = new CipherText.ConvertServices();

            int intPosBC = strmessage.IndexOf("[TTTHmail]");

            if (intPosBC == -1)
            {
                intPosBC = strmessage.IndexOf("[TTTHBCmail]");
            }

            string strbaocao = "";
            try
            {
                if (intPosBC != -1)
                {
                    strbaocao = strmessage.Substring(0, intPosBC);
                    strbaocao = _convert.GiaiMa_Unicode(strbaocao);

                    int idvanban = Convert.ToInt32(_GetTruong(strbaocao, "intidVB"));
                    int iddonvi = Convert.ToInt32(_GetTruong(strbaocao, "intidDonvi"));
                    string strngay = _GetTruong(strbaocao, "strNgayguiVB");

                    using (var context = new QLVB.DAL.QLVBDatabase())
                    {
                        string strngaynhan = _GetTruong(strbaocao, "strNgaynhanVB");//DateServices.FormatDateTimeEn(DateTime.Now);
                        string sSql =
                            "Update guivanban set inttrangthainhan=1,"
                            + "strngaynhan ='" + strngaynhan + "' "
                            + " where intidvanban='" + idvanban.ToString() + "' "
                            + " and intiddonvi='" + iddonvi.ToString() + "' "
                            + " and cast(strngaygui as smalldatetime) = cast('" + strngay + "' as smalldatetime) ";
                        context.Database.ExecuteSqlCommand(sSql);
                    }
                    return 1;
                }
                else
                {
                    _logger.Error(strmessage);
                    return 0;
                }
            }
            catch
            {
                _logger.Error(strmessage);
                return 0;
            }

        }

        /// <summary>
        /// lay thong tin yeu cau bao cao phan hoi da nhan vbdt
        /// </summary>
        /// <param name="strmessage"></param>
        /// <returns></returns>
        public TTTHBaocaoViewModel GetTTTHBaocao(string strmessage)
        {
            try
            {
                CipherText.ConvertServices _convert = new CipherText.ConvertServices();

                int intPos = strmessage.IndexOf("[TTTHmail]");
                string strbody = strmessage.Substring(0, intPos);
                strbody = _convert.GiaiMa_Unicode(strbody);

                int intPosBC = strmessage.IndexOf("[TTTHBCmail]");
                string strbaocao = "";
                TTTHBaocaoViewModel baocao = new TTTHBaocaoViewModel();
                if (intPosBC != -1)
                {
                    strbaocao = strmessage.Substring(intPos + 10, intPosBC - intPos - 10);
                    strbaocao = _convert.GiaiMa_Unicode(strbaocao);

                    string strngaynhan = String.Format("{0:MM/dd/yyyy HH:mm:ss }", DateTime.Now);
                    strngaynhan = _SetTruong(strngaynhan, "strNgaynhanVB");
                    strbaocao = strngaynhan + strbaocao;
                    baocao.strbody = strbaocao;
                    baocao.strFrom = _GetTruong(strbaocao, "strFromAddress");
                    return baocao;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// lay duong dan de luu file dinh kem
        /// </summary>
        /// <param name="idmail"></param>
        /// <param name="strmota"></param>
        /// <returns>physicalPath de luu file</returns>
        public string SaveAttachment(int idmail, string strmota, string folderPath)
        {
            try
            {
                int intsttfile;
                var vb = _attachMailRepo.AttachMails
                    .Where(f => f.intloai == (int)enumAttachMail.intloai.MailInbox)
                    .Where(f => f.intidmail == idmail);
                intsttfile = (vb.Count() == 0) ? 1 : vb.Count() + 1;

                strmota = !string.IsNullOrEmpty(strmota) ? strmota : "fileEmail_" + idmail.ToString() + ".pdf";

                var fileExt = _fileManager.GetFileExtention(strmota);

                //  dinh dang file : idmail_intsttfile.*
                string fileName = idmail.ToString() + "_" + intsttfile.ToString() + "." + fileExt;

                string fileSavepath = System.IO.Path.Combine(folderPath, fileName);

                //=========================================================
                // kiem tra xem file nay co ton tai chua??
                // neu ton tai roi thi dat lai ten moi (them bien dem)
                //=========================================================
                int count = 0;
                while (System.IO.File.Exists(fileSavepath))
                {
                    count++;
                    fileName = idmail.ToString() + "_" + intsttfile.ToString()
                                + "_" + count.ToString() + "." + fileExt;
                    fileSavepath = System.IO.Path.Combine(folderPath, fileName);
                }
                return fileSavepath;
            }
            catch (Exception ex)
            {
                _logger.Error("POP3 Error: Attachments not save to folder. " + ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// insert vao table attachmail
        /// </summary>
        /// <param name="idmail"></param>
        /// <param name="strtenfile"></param>
        /// <param name="intloai"></param>
        /// <returns>id attachmail</returns>
        public int InsertAttachment(int idmail, string strtenfile, string strmota, int intloai)
        {
            try
            {
                System.IO.FileInfo file = new System.IO.FileInfo(strtenfile);

                AttachMail fileAttach = new AttachMail();
                fileAttach.intidmail = idmail;
                fileAttach.intloai = intloai; //(int)enumAttachMail.intloai.MailInbox;
                fileAttach.strtenfile = file.Name;
                fileAttach.strmota = strmota;

                int intid = _attachMailRepo.Them(fileAttach);
                return intid;
            }
            catch (Exception ex)
            {
                _logger.Error("POP3 Error: Not Insert into Attachmail. idmail: " + idmail.ToString() + ", filename: " + strtenfile + ex.Message);
                return (int)ResultViewModels.Error;
            }
        }

        public int SaveMailInbox(string subject, string strheader, string strcontent, string strfrom, int intloai)
        {
            try
            {
                MailInbox inbox = new MailInbox();
                inbox.strsubject = subject;
                inbox.strcontent = strcontent;
                inbox.straddress = strfrom;
                inbox.strheader = strheader;
                inbox.strngaynhan = DateTime.Now;
                inbox.intloai = intloai;
                int idinbox = _inboxRepo.Them(inbox);
                return idinbox;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return 0;
            }

        }

        #endregion Receive

        #region SendQLVB1
        /// <summary>
        /// ma hoa theo qlvb1 va gui mail
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="listiddonvi"></param>
        /// <returns></returns>
        public string EncodeQLVB1(int idvanban, int intloaivanban)
        {
            string strbody = string.Empty;
            switch (intloaivanban)
            {
                case (int)enumGuiVanban.intloaivanban.Vanbandi:
                    strbody = _GetNoidungVBDi(idvanban);
                    break;
                case (int)enumGuiVanban.intloaivanban.Vanbanden:
                    strbody = _GetNoidungVBDen(idvanban);
                    break;
            }
            try
            {
                if (!string.IsNullOrEmpty(strbody))
                {
                    CipherText.ConvertServices _convert = new CipherText.ConvertServices();
                    strbody = _convert.Mahoa_Utf8(strbody) + "[TTTHmail]";
                }
                return strbody;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return "";
            }
        }

        public string EncodeQLVB1(string strmessage)
        {
            if (!string.IsNullOrEmpty(strmessage))
            {
                CipherText.ConvertServices _convert = new CipherText.ConvertServices();
                return _convert.Mahoa_Utf8(strmessage) + "[TTTHmail]";
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ma hoa bao cao them qlvb1 [TTTHBCmail]
        /// </summary>
        /// <param name="strFrom"></param>
        /// <param name="idvanban"></param>
        /// <param name="iddonvi"></param>
        /// <returns></returns>
        public string EncodeBaocaoQLVB1(string strFrom, int idvanban, int iddonvi)
        {
            string strkq = string.Empty;
            strkq += _SetTruong(strFrom, "strFromAddress");
            string strngaygui = String.Format("{0:MM/dd/yyyy HH:mm:ss }", DateTime.Now); //DateServices.FormatDateTimeVN(DateTime.Now);
            strkq += _SetTruong(strngaygui, "strNgayguiVB");
            strkq += _SetTruong(idvanban.ToString(), "intidVB");
            strkq += _SetTruong(iddonvi.ToString(), "intidDonvi");

            CipherText.ConvertServices _convert = new CipherText.ConvertServices();
            strkq = _convert.Mahoa_Utf8(strkq) + "[TTTHBCmail]";

            return strkq;
        }

        private string _GetNoidungVBDi(int idvanban)
        {

            var vb = _vanbandiRepo.Vanbandis.FirstOrDefault(p => p.intid == idvanban);
            string strkq = string.Empty;
            if (vb != null)
            {
                string strngayky = String.Format("{0:MM/dd/yyyy}", vb.strngayky); //DateServices.FormatDateEn(vb.strngayky);
                strkq += _SetTruong(strngayky, "strNgayky");
                strkq += _SetTruong(vb.strkyhieu, "strKyhieu");
                strkq += _SetTruong(vb.intidphanloaivanbandi.ToString(), "intidphanloaicongvanden");
                strkq += _SetTruong(vb.intidkhan.ToString(), "intKhan");
                strkq += _SetTruong(vb.intso.ToString(), "intMat");
                strkq += _SetTruong(vb.strtrichyeu, "strTrichyeu");
                strkq += _SetTruong(vb.strnguoiky, "strNguoiky");
                strkq += _SetTruong(vb.strnoinhan, "strNoigui");
                string loaivb = _plvanbanRepo.GetAllPhanloaiVanbans
                        .FirstOrDefault(p => p.intid == vb.intidphanloaivanbandi).strtenvanban;
                strkq += _SetTruong(loaivb, "strLoaivanban");

                string strnoiguivb = QLVB.Donvi.Donvi.GetTenDonVi();
                strkq += _SetTruong(strnoiguivb, "strNoiguiVB");


                return strkq;
            }
            else
            {
                _logger.Info("SMTP Error: Không tìm thấy id văn bản đi " + idvanban.ToString());
                return null;
            }
        }

        private string _GetNoidungVBDen(int idvanban)
        {

            var vb = _vbdenRepo.Vanbandens.FirstOrDefault(p => p.intid == idvanban);
            string strkq = string.Empty;
            if (vb != null)
            {
                string strngayky = String.Format("{0:MM/dd/yyyy}", vb.strngayky); //DateServices.FormatDateEn(vb.strngayky);
                strkq += _SetTruong(strngayky, "strNgayky");
                strkq += _SetTruong(vb.strkyhieu, "strKyhieu");
                strkq += _SetTruong(vb.intidphanloaivanbanden.ToString(), "intidphanloaicongvanden");
                strkq += _SetTruong(vb.intidkhan.ToString(), "intKhan");
                strkq += _SetTruong(vb.intsoden.ToString(), "intMat");
                strkq += _SetTruong(vb.strtrichyeu, "strTrichyeu");
                strkq += _SetTruong(vb.strnguoiky, "strNguoiky");
                strkq += _SetTruong(vb.strnoigui, "strNoigui");
                string loaivb = _plvanbanRepo.GetAllPhanloaiVanbans
                        .FirstOrDefault(p => p.intid == vb.intidphanloaivanbanden).strtenvanban;
                strkq += _SetTruong(loaivb, "strLoaivanban");

                string strnoiguivb = QLVB.Donvi.Donvi.GetTenDonVi();
                strkq += _SetTruong(strnoiguivb, "strNoiguiVB");


                return strkq;
            }
            else
            {
                _logger.Info("SMTP Error: Không tìm thấy id văn bản đến " + idvanban.ToString());
                return null;
            }
        }

        private string _SetTruong(string strnoidung, string strtruong)
        {
            string truongbd = "[" + strtruong + "]";
            string truongkt = "[/" + strtruong + "]";

            string strvalue = truongbd + strnoidung + truongkt;
            return strvalue;
        }

        /// <summary>
        /// lay duong dan file de attach vao mail
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        public List<ListFileToAttach> GetFileVanbanToAttach(int idvanban, int intloai)
        {
            string strloai = string.Empty;
            if (intloai == (int)enumAttachVanban.intloai.Vanbanden)
            {
                strloai = AppConts.FileCongvanden;
            }
            if (intloai == (int)enumAttachVanban.intloai.Vanbandi)
            {
                strloai = AppConts.FileCongvanphathanh;
            }

            var files = _fileRepo.AttachVanbans
                    .Where(p => p.intidvanban == idvanban)
                    .Where(p => p.intloai == intloai) //(int)enumAttachVanban.intloai.Vanbandi
                    .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive);

            var allfiles = files.ToList();

            List<ListFileToAttach> listfile = new List<ListFileToAttach>();
            foreach (var f in allfiles)
            {
                ListFileToAttach model = new ListFileToAttach
                {
                    intidfile = f.intid,
                    strmota = f.strmota,
                    filePath = _fileManager.GetPhysicalPath(strloai, (DateTime)f.strngaycapnhat, f.strtenfile)
                };
                listfile.Add(model);
            }

            return listfile;
        }

        /// <summary>
        /// luu nhung van ban da gui cho don vi
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="iddonvi"></param>
        /// <param name="intloai"></param>
        /// <returns>intid</returns>
        public int SaveGuiVanban(int idvanban, int iddonvi, int intloaivanban)
        {
            GuiVanban vb = new GuiVanban
            {
                intidvanban = idvanban,
                intloaivanban = intloaivanban,
                intiddonvi = iddonvi,
                inttrangthaigui = (int)enumGuiVanban.inttrangthaigui.Dagui,
                strngaygui = DateTime.Now,
                inttrangthainhan = (int)enumGuiVanban.inttrangthainhan.Chuanhan,
                intloaigui = (int)enumGuiVanban.intloaigui.Email
            };
            return _guivbRepo.Them(vb);
        }

        /// <summary>
        /// save vao table MailOubox
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="strcontent"></param>
        /// <param name="strTo"></param>
        /// <param name="intloai"></param>
        /// <returns></returns>
        public int SaveMailOutbox(string subject, string strcontent, string strTo, int intloai)
        {
            MailOutbox outbox = new MailOutbox();
            outbox.strsubject = subject;
            outbox.strcontent = strcontent;
            outbox.straddress = strTo;
            outbox.strngaygui = DateTime.Now;
            outbox.intloai = intloai;
            int id = _outboxRepo.Them(outbox);
            return id;
        }

        /// <summary>
        /// cap nhat van ban da gui dien tu
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        public int UpdateVBDT(int idvanban, int intloai)
        {
            //update trang thai van ban
            if (intloai == (int)enumGuiVanban.intloaivanban.Vanbandi)
            {
                _vanbandiRepo.CapnhatVBDT(idvanban, (int)enumVanbandi.intguivbdt.Dagui);
            }
            if (intloai == (int)enumGuiVanban.intloaivanban.Vanbanden)
            {
                _vbdenRepo.CapnhatVBDT(idvanban, (int)enumVanbanden.bitguivbdt.Dagui);
            }
            return 1;
        }

        #endregion SendQLVB1

        #endregion QLVB1


        #region 512

        public edXMLMessage CreateEdXMLMessage_512(int idvanban, List<int> listiddonvi)
        {
            edXMLMessage msg = new edXMLMessage();

            var vanban = _vanbandiRepo.Vanbandis.FirstOrDefault(p => p.intid == idvanban);

            msg.Header = new edXMLHeader();
            msg.Header.MessageHeader = new edXMLMessageHeader();
            //===========================================================
            msg.Header.MessageHeader.From = new edXMLDonvi();
            msg.Header.MessageHeader.From.OrganId = GetMaDinhdanh();
            msg.Header.MessageHeader.From.OrganName = GetTendonvi();
            msg.Header.MessageHeader.From.OrganAdd = _configRepo.GetConfig(ThamsoHethong.DiachiDonvi);//"Số 2 Nguyễn Văn Trị P Thanh Bình TP Biên Hòa";
            msg.Header.MessageHeader.From.Telephone = _configRepo.GetConfig(ThamsoHethong.DienthoaiDonvi);//"0613822501";
            msg.Header.MessageHeader.From.Email = GetAccountSetting().emailAddress;

            //===========================================================
            msg.Header.MessageHeader.To = new List<edXMLDonvi>();
            var listdonvi = _tochucRepo.GetActiveTochucdoitacs
                .Where(p => listiddonvi.Contains(p.intid))
                .ToList();
            foreach (var d in listdonvi)
            {
                edXMLDonvi donvi = new edXMLDonvi();
                donvi.OrganId = d.strmadinhdanh;
                donvi.OrganName = d.strtentochucdoitac;
                donvi.OrganAdd = d.strdiachi;
                donvi.Telephone = d.strphone;
                donvi.Email = d.stremailvbdt;

                msg.Header.MessageHeader.To.Add(donvi);
            }
            //===========================================================
            msg.Header.MessageHeader.Code = new edXMLCode();
            msg.Header.MessageHeader.Code.CodeNumber = vanban.intso.ToString();
            msg.Header.MessageHeader.Code.CodeNotation = vanban.strkyhieu;

            //===========================================================
            msg.Header.MessageHeader.PromulgationInfo = new edXMLPromulgationInfo();
            msg.Header.MessageHeader.PromulgationInfo.PromulgationDate = DateServices.FormatDateVN(vanban.strngayky);

            msg.Header.MessageHeader.Subject = vanban.strtrichyeu;

            msg.Header.MessageHeader.SignerInfo = new edXMLSignerInfo();
            msg.Header.MessageHeader.SignerInfo.FullName = vanban.strnguoiky;

            msg.Header.MessageHeader.OtherInfo = new edXMLOtherInfo();
            msg.Header.MessageHeader.OtherInfo.Priority = (int)enumEdXMLOtherInfo.Priority.Thuong;

            //==============================================================
            msg.Header.TraceHeaderList = new List<edXMLTraceHeader>();

            edXMLTraceHeader tracerFrom = new edXMLTraceHeader();
            tracerFrom.OrganId = GetMaDinhdanh();
            tracerFrom.Timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            msg.Header.TraceHeaderList.Add(tracerFrom);

            string strNgaygui = DateServices.FormatDateTimeVN(DateTime.Now);
            foreach (var d in listdonvi)
            {
                edXMLTraceHeader tracerTo = new edXMLTraceHeader();
                tracerTo.OrganId = d.strmadinhdanh;
                tracerTo.Timestamp = strNgaygui;

                msg.Header.TraceHeaderList.Add(tracerTo);
            }

            //=====================================================
            msg.Body = new edXMLBody();
            msg.Body.Manifest = new edXMLManifest();
            msg.Body.Manifest.Reference = new List<Reference>();

            var listfile = GetFileVanbanToAttach(idvanban, (int)enumAttachVanban.intloai.Vanbandi);
            foreach (var f in listfile)
            {
                Reference Refer = new Reference();
                Refer.AttachmentName = f.strmota;

                msg.Body.Manifest.Reference.Add(Refer);
            }

            return msg;
        }

        public string edXMLMessageToString_512(edXMLMessage msg)
        {
            try
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/");
                ns.Add("edXML", "http://www.e-doc.vn/Schema/");
                ns.Add("xlink", "http://www.w3.org/1999/xlink");

                using (MemoryStream mStream = new MemoryStream())
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(edXMLMessage));
                    xmlSerializer.Serialize(mStream, msg, ns);

                    StreamReader rd = new StreamReader(mStream);
                    mStream.Position = 0;
                    string ret = rd.ReadToEnd();
                    return ret;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return string.Empty;
            }
        }

        public edXMLMessage StringToEdXMLMessage_512(string xml)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(edXMLMessage));
                MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
                edXMLMessage resultingMessage = (edXMLMessage)serializer.Deserialize(memStream);
                return resultingMessage;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }

        private string _FilterEdXML_Document_512(string strmessage)
        {
            try
            {
                string truongbd = "<edXML:AllDocument";
                string truongkt = "</edXML:AllDocument>";

                int lenbd = truongbd.Length;
                int intPosbd = strmessage.IndexOf(truongbd);

                int lenkt = truongkt.Length;
                int intPoskt = strmessage.IndexOf(truongkt);

                string strvalue = strmessage.Substring(intPosbd + lenbd, intPoskt - intPosbd - lenbd);
                if (!string.IsNullOrEmpty(strvalue))
                {
                    return strvalue;
                }
                else
                {
                    return null;
                }
            }
            catch //(Exception ex)
            {
                return null;
            }
        }

        public string edXMLDocumentToString_512(int idvanban, int intloai)
        {
            try
            {
                XNamespace edXML = "http://www.e-doc.vn/Schema/";

                XElement root = new XElement(edXML + "AllDocument",
                    new XAttribute(XNamespace.Xmlns + "edXML", edXML)
                    );

                //List<edXMLAttachment> listFiles = new List<edXMLAttachment>();

                string strloai = string.Empty;
                if (intloai == (int)enumAttachVanban.intloai.Vanbanden)
                {
                    strloai = AppConts.FileCongvanden;
                }
                if (intloai == (int)enumAttachVanban.intloai.Vanbandi)
                {
                    strloai = AppConts.FileCongvanphathanh;
                }

                var files = _fileRepo.AttachVanbans
                        .Where(p => p.intidvanban == idvanban)
                        .Where(p => p.intloai == intloai) //(int)enumAttachVanban.intloai.Vanbandi
                        .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive);

                foreach (var f in files)
                {
                    var filePath = _fileManager.GetPhysicalPath(strloai, (DateTime)f.strngaycapnhat, f.strtenfile);
                    var fileExt = _fileManager.GetFileExtention(f.strtenfile);

                    edXMLAttachment model = new edXMLAttachment
                    {
                        ContentID = f.intid.ToString(),
                        ContentTransferEncoding = "base64",
                        //ContentType = 
                        Value = QLVB.Common.Crypt.CryptServices.EncodeFileToBase64(filePath)
                    };
                    //listFiles.Add(model);
                    //=======================================
                    XElement Document = new XElement(edXML + "Document");


                    XElement Attach = new XElement(edXML + "Attach");
                    Document.Add(Attach);

                    //var fileExt = _fileManager.GetFileExtention(msg.Attach.)

                    XElement Attachment = new XElement(edXML + "Attachment",
                        new XAttribute("Content-Type", "application/" + fileExt),
                        new XAttribute("Content-Transfer-Encoding", "base64")
                        );
                    Attach.Add(Attachment);

                    XElement name = new XElement("name", f.strmota);
                    XElement value = new XElement("value", model.Value);

                    Attachment.Add(name);
                    Attachment.Add(value);


                    root.Add(Document);
                }
                //=================================================

                string xml = string.Empty;

                xml = root.ToString();

                return xml;
            }
            catch
            {
                return null;
            }
        }

        #endregion 512

        #region 2803

        /// <summary>
        /// create edxml 
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="listiddonvi"></param>
        /// <returns></returns>
        public edXMLMessage CreateEdXMLMessage_2803(int idvanban, List<int> listiddonvi, int intloaivanban)
        {
            edXMLMessage msg = new edXMLMessage();

            //var vanban = _vanbandiRepo.Vanbandis.FirstOrDefault(p => p.intid == idvanban);

            msg.Header = new edXMLHeader();
            msg.Header.MessageHeader = new edXMLMessageHeader();
            //===========================================================
            msg.Header.MessageHeader.From = new edXMLDonvi();
            msg.Header.MessageHeader.From.OrganId = GetMaDinhdanh();
            msg.Header.MessageHeader.From.OrganName = GetTendonvi();
            msg.Header.MessageHeader.From.OrganAdd = _configRepo.GetConfig(ThamsoHethong.DiachiDonvi);//"Số 2 Nguyễn Văn Trị P Thanh Bình TP Biên Hòa";
            msg.Header.MessageHeader.From.Telephone = _configRepo.GetConfig(ThamsoHethong.DienthoaiDonvi);//"0613822501";
            msg.Header.MessageHeader.From.Email = GetAccountSetting().emailAddress;

            //===========================================================
            msg.Header.MessageHeader.To = new List<edXMLDonvi>();
            var listdonvi = _tochucRepo.GetActiveTochucdoitacs
                .Where(p => listiddonvi.Contains(p.intid))
                .ToList();
            foreach (var d in listdonvi)
            {
                edXMLDonvi donvi = new edXMLDonvi();
                donvi.OrganId = d.strmadinhdanh;
                donvi.OrganName = d.strtentochucdoitac;
                donvi.OrganAdd = d.strdiachi;
                donvi.Telephone = d.strphone;
                donvi.Email = d.stremailvbdt;

                msg.Header.MessageHeader.To.Add(donvi);
            }
            //===========================================================    

            int intloaiAttachfile = 0;
            switch (intloaivanban)
            {
                case (int)enumGuiVanban.intloaivanban.Vanbandi:
                    var vanban = _vanbandiRepo.Vanbandis.FirstOrDefault(p => p.intid == idvanban);

                    msg.Header.MessageHeader.Code = new edXMLCode();
                    msg.Header.MessageHeader.Code.CodeNumber = vanban.intso.ToString();
                    msg.Header.MessageHeader.Code.CodeNotation = vanban.strkyhieu;

                    //===========================================================
                    msg.Header.MessageHeader.PromulgationInfo = new edXMLPromulgationInfo();
                    msg.Header.MessageHeader.PromulgationInfo.PromulgationDate = DateServices.FormatDateVN(vanban.strngayky);

                    msg.Header.MessageHeader.Subject = vanban.strtrichyeu;

                    msg.Header.MessageHeader.SignerInfo = new edXMLSignerInfo();
                    msg.Header.MessageHeader.SignerInfo.FullName = vanban.strnguoiky;

                    intloaiAttachfile = (int)enumAttachVanban.intloai.Vanbandi;

                    break;

                case (int)enumGuiVanban.intloaivanban.Vanbanden:
                    var vbden = _vbdenRepo.Vanbandens.FirstOrDefault(p => p.intid == idvanban);

                    msg.Header.MessageHeader.Code = new edXMLCode();
                    msg.Header.MessageHeader.Code.CodeNumber = vbden.intsoden.ToString();
                    msg.Header.MessageHeader.Code.CodeNotation = vbden.strkyhieu;

                    //===========================================================
                    msg.Header.MessageHeader.PromulgationInfo = new edXMLPromulgationInfo();
                    msg.Header.MessageHeader.PromulgationInfo.PromulgationDate = DateServices.FormatDateVN(vbden.strngayden);

                    msg.Header.MessageHeader.Subject = vbden.strtrichyeu;

                    msg.Header.MessageHeader.SignerInfo = new edXMLSignerInfo();
                    msg.Header.MessageHeader.SignerInfo.FullName = vbden.strnguoiky;

                    intloaiAttachfile = (int)enumAttachVanban.intloai.Vanbanden;

                    break;
            }

            //=========================================================

            msg.Header.MessageHeader.OtherInfo = new edXMLOtherInfo();
            msg.Header.MessageHeader.OtherInfo.Priority = (int)enumEdXMLOtherInfo.Priority.Thuong;

            //==============================================================
            msg.Header.TraceHeaderList = new List<edXMLTraceHeader>();

            edXMLTraceHeader tracerFrom = new edXMLTraceHeader();
            tracerFrom.OrganId = GetMaDinhdanh();
            tracerFrom.Timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            msg.Header.TraceHeaderList.Add(tracerFrom);

            string strNgaygui = DateServices.FormatDateTimeVN(DateTime.Now);
            foreach (var d in listdonvi)
            {
                edXMLTraceHeader tracerTo = new edXMLTraceHeader();
                tracerTo.OrganId = d.strmadinhdanh;
                tracerTo.Timestamp = strNgaygui;

                msg.Header.TraceHeaderList.Add(tracerTo);
            }

            //=====================================================
            msg.Body = new edXMLBody();
            msg.Body.Manifest = new edXMLManifest();
            msg.Body.Manifest.Reference = new List<Reference>();

            var listfile = GetFileVanbanToAttach(idvanban, intloaiAttachfile);
            foreach (var f in listfile)
            {
                Reference Refer = new Reference();
                Refer.href = "cid:" + f.intidfile.ToString();
                Refer.AttachmentName = f.strmota;

                msg.Body.Manifest.Reference.Add(Refer);
            }

            return msg;
        }

        private List<edXMLAttachment> CreatEdXMLAttachment_base64(int idvanban, int intloai)
        {
            List<edXMLAttachment> listFiles = new List<edXMLAttachment>();

            string strloai = string.Empty;
            if (intloai == (int)enumAttachVanban.intloai.Vanbanden)
            {
                strloai = AppConts.FileCongvanden;
            }
            if (intloai == (int)enumAttachVanban.intloai.Vanbandi)
            {
                strloai = AppConts.FileCongvanphathanh;
            }

            var files = _fileRepo.AttachVanbans
                    .Where(p => p.intidvanban == idvanban)
                    .Where(p => p.intloai == intloai) //(int)enumAttachVanban.intloai.Vanbandi
                    .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive);

            foreach (var f in files)
            {
                var filePath = _fileManager.GetPhysicalPath(strloai, (DateTime)f.strngaycapnhat, f.strtenfile);

                edXMLAttachment model = new edXMLAttachment
                {
                    ContentID = f.intid.ToString(),
                    ContentTransferEncoding = "base64",
                    ContentType = _fileManager.GetFileExtention(f.strtenfile),
                    Value = QLVB.Common.Crypt.CryptServices.EncodeFileToBase64(filePath)
                };
                listFiles.Add(model);
            }

            return listFiles;
        }

        public string edXMLMessageToString_2803(edXMLMessage msg)
        {
            try
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/");
                ns.Add("edXML", "http://www.e-doc.vn/Schema/");
                ns.Add("xlink", "http://www.w3.org/1999/xlink");

                using (MemoryStream mStream = new MemoryStream())
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(edXMLMessage));
                    xmlSerializer.Serialize(mStream, msg, ns);

                    StreamReader rd = new StreamReader(mStream);
                    mStream.Position = 0;
                    string ret = rd.ReadToEnd();
                    return ret;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return string.Empty;
            }
        }

        public edXMLDocument CreateEdXMLDocument_2803(int idvanban, int intloaivanban)
        {
            try
            {
                edXMLDocument doc = new edXMLDocument();
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
                doc.Attach = CreatEdXMLAttachment_base64(idvanban, intloaifile);
                return doc;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }

        }
        public string edXMLDocumentToString_2803(edXMLDocument doc)
        {
            try
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("edXML", "http://www.e-doc.vn/Schema/");

                using (MemoryStream mStream = new MemoryStream())
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(edXMLDocument));
                    xmlSerializer.Serialize(mStream, doc, ns);

                    StreamReader rd = new StreamReader(mStream);
                    mStream.Position = 0;
                    string ret = rd.ReadToEnd();
                    return ret;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return string.Empty;
            }
        }


        public edXMLMessage StringToEdXMLMessage_2803(string xml)
        {
            try
            {
                xml = _FilterEdXML_Envelope(xml);

                XmlSerializer serializer = new XmlSerializer(typeof(edXMLMessage));
                MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
                edXMLMessage result = (edXMLMessage)serializer.Deserialize(memStream);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }
        public edXMLDocument StringToEdXMLDocument_2803(string xml)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(edXMLDocument));
                MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
                edXMLDocument result = (edXMLDocument)serializer.Deserialize(memStream);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// luu edxml thanh vbdt
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="IsAttach"></param>
        /// <returns>idvanban</returns>
        public int SaveEdXML(string xml, bool IsAttach)
        {
            int idvanban = 0;

            // xml = _FilterEdXML_Envelope(xml);

            edXMLMessage msg = StringToEdXMLMessage_2803(xml);
            if (msg != null)
            {
                try
                {
                    int intkhan = (msg.Header.MessageHeader.OtherInfo != null) ?
                                            (int)msg.Header.MessageHeader.OtherInfo.Priority : 0;

                    int intso = (msg.Header.MessageHeader.Code != null) ?
                                Convert.ToInt32(msg.Header.MessageHeader.Code.CodeNumber) : 0;

                    string strkyhieu = (msg.Header.MessageHeader.Code != null) ?
                                msg.Header.MessageHeader.Code.CodeNotation : string.Empty;

                    string strngayky = (msg.Header.MessageHeader.PromulgationInfo != null) ?
                                msg.Header.MessageHeader.PromulgationInfo.PromulgationDate : string.Empty;
                    DateTime? dteNgayky = null;
                    if (!string.IsNullOrEmpty(strngayky))
                    {
                        try
                        {
                            dteNgayky = DateServices.FormatDateEn(strngayky);
                        }
                        catch { _logger.Info("Error edXML: ngày ký"); }
                    }

                    string strnguoiky = (msg.Header.MessageHeader.SignerInfo != null) ?
                                msg.Header.MessageHeader.SignerInfo.FullName : string.Empty;

                    string strnoigui = string.Empty;
                    if (msg.Header.MessageHeader.To != null)
                    {
                        var listnoigui = msg.Header.MessageHeader.To;
                        foreach (var d in listnoigui)
                        {
                            strnoigui += d.OrganName + ",";
                        }
                    }

                    string strnoiguivb = (msg.Header.MessageHeader.From.OrganName != null) ?
                            msg.Header.MessageHeader.From.OrganName : string.Empty;

                    string strloaivb = (msg.Header.MessageHeader.DocumentType != null) ?
                            msg.Header.MessageHeader.DocumentType.TypeName : string.Empty;

                    string strtrichyeu = (msg.Header.MessageHeader.Subject != null) ?
                            msg.Header.MessageHeader.Subject : string.Empty;

                    string stremailguivb = (msg.Header.MessageHeader.From.Email != null) ?
                            msg.Header.MessageHeader.From.Email : string.Empty;

                    string OrganId = (msg.Header.MessageHeader.From.OrganId != null) ?
                            msg.Header.MessageHeader.From.OrganId : string.Empty;

                    string strngayguivb = string.Empty;
                    if (msg.Header.TraceHeaderList != null)
                    {
                        var tracert = msg.Header.TraceHeaderList;
                        foreach (var v in tracert)
                        {
                            if (v.OrganId == OrganId)
                            {
                                strngayguivb = v.Timestamp;
                                break;
                            }
                        }
                    }
                    DateTime? dteNgayguivb = null;
                    if (!string.IsNullOrEmpty(strngayguivb))
                    {
                        try
                        {
                            dteNgayguivb = DateServices.FormatDateTimeEn(strngayguivb);
                        }
                        catch { _logger.Info("Error edXML: ngày gửi "); }
                    }

                    idvanban = _SaveVanbanmail(IsAttach, null,
                        intkhan, intso, strkyhieu, dteNgayky,
                        strnguoiky, strnoigui, strnoiguivb,
                        strloaivb, strtrichyeu,
                        dteNgayguivb, stremailguivb
                        );

                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
            }

            return idvanban;
        }
        private int _SaveVanbanmail(bool isAttach, int? intidphanloaivanbanden,
            int? intkhan, int intso, string strkyhieu, DateTime? dtengayky,
            string strnguoiky, string strnoigui, string strnoiguivb,
            string strloaivb, string strtrichyeu,
            DateTime? dtengayguivb, string stremailguivb)
        {
            try
            {
                Vanbandenmail vb = new Vanbandenmail();
                vb.intattach = isAttach ? (int)enumVanbandenmail.intattach.Co : (int)enumVanbandenmail.intattach.Khong;
                vb.intidphanloaivanbanden = intidphanloaivanbanden;
                vb.intkhan = intkhan;
                //vb.intloai = 
                vb.intso = intso;
                vb.strkyhieu = strkyhieu;
                //string strngay = dtengayky;
                //if (Utils.IsDate(strngay))
                //{
                //    vb.strngayky = Convert.ToDateTime(strngay);
                //}
                //else
                //{
                //    vb.strngayky = null; //DateServices.FormatDateEn(strngay);
                //}
                vb.strngayky = dtengayky;

                vb.strnguoiky = strnguoiky;
                vb.strnoigui = strnoigui;
                vb.strnoiguivb = strnoiguivb;
                vb.strloaivanban = strloaivb;
                vb.strtrichyeu = strtrichyeu;

                //strngay = dtengayguivb;
                //if (Utils.IsDate(strngay))
                //{
                //    vb.strngayguivb = Convert.ToDateTime(strngay);
                //}
                //else
                //{
                //    vb.strngayguivb = null;// DateServices.FormatDateEn(strngay);
                //}
                vb.strngayguivb = dtengayguivb;

                vb.strAddressSend = stremailguivb;

                vb.strngaynhanvb = DateTime.Now;

                vb.intnhanvanbantu = enumVanbandenmail.intnhanvanbantu.Email;

                int id = _vbdenMailRepo.Them(vb);

                return id;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// loc bo cac truong phat sinh tu mail dongnai. chi lay truong edXML:envelope
        /// </summary>
        /// <param name="strmessage"></param>
        /// <returns></returns>
        private string _FilterEdXML_Envelope(string strmessage)
        {
            try
            {
                string truongbd = "<SOAP-ENV:Envelope";
                string truongkt = "</SOAP-ENV:Envelope>";

                int lenbd = truongbd.Length;
                int intPosbd = strmessage.IndexOf(truongbd);

                int lenkt = truongkt.Length;
                int intPoskt = strmessage.IndexOf(truongkt);

                string strvalue = strmessage.Substring(intPosbd, intPoskt - intPosbd + lenkt);
                if (!string.IsNullOrEmpty(strvalue))
                {
                    return strvalue;
                }
                else
                {
                    return null;
                }
            }
            catch //(Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// lay don vi hoi bao nhan vbdt
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public QLVB.DTO.Vanbandientu.EdXMLBC.edXMLBaocao GetBTTTT_2803_Baocao(string xml)
        {
            QLVB.DTO.Vanbandientu.EdXMLBC.edXMLBaocao baocao = new QLVB.DTO.Vanbandientu.EdXMLBC.edXMLBaocao();

            //xml = _FilterEdXML_Envelope(xml);
            edXMLMessage msg = StringToEdXMLMessage_2803(xml);
            if (msg != null)
            {
                //==============don vi gui =================================================
                baocao.FromEmail = (msg.Header.MessageHeader.From.Email != null) ?
                                msg.Header.MessageHeader.From.Email : string.Empty;

                baocao.FromOrganId = (msg.Header.MessageHeader.From.OrganId != null) ?
                        msg.Header.MessageHeader.From.OrganId : string.Empty;

                string strngayguivb = string.Empty;
                if (msg.Header.TraceHeaderList != null)
                {
                    var tracert = msg.Header.TraceHeaderList;
                    foreach (var v in tracert)
                    {
                        if (v.OrganId == baocao.FromOrganId)
                        {
                            strngayguivb = v.Timestamp;
                            break;
                        }
                    }
                }
                baocao.FromTimestamp = strngayguivb;
                //=============van ban gui ===========================================
                baocao.CodeNumber = (msg.Header.MessageHeader.Code != null) ?
                                  msg.Header.MessageHeader.Code.CodeNumber : "";

                baocao.CodeNotation = (msg.Header.MessageHeader.Code != null) ?
                            msg.Header.MessageHeader.Code.CodeNotation : string.Empty;

                baocao.PromulgationDate = (msg.Header.MessageHeader.PromulgationInfo != null) ?
                            msg.Header.MessageHeader.PromulgationInfo.PromulgationDate : string.Empty;

                //==========don vi nhan=============================================
                baocao.ToOrganId = GetMaDinhdanh();
                baocao.ToEmail = GetAccountSetting().emailAddress;
                baocao.ToTimestamp = DateServices.FormatDateTimeVN(DateTime.Now);

                return baocao;
            }
            else
            {
                return null;
            }
        }

        public string edXMLBaocaoToString(QLVB.DTO.Vanbandientu.EdXMLBC.edXMLBaocao edBaocao)
        {
            XNamespace edXML = "http://www.e-doc.vn/Schema/";
            XNamespace xlink = "http://www.w3.org/1999/xlink";
            XNamespace SOAPENV = "http://schemas.xmlsoap.org/soap/envelope/";

            XElement root = new XElement(SOAPENV + "Envelope",
                new XAttribute(XNamespace.Xmlns + "edXML", edXML),
                new XAttribute(XNamespace.Xmlns + "xlink", xlink),
                new XAttribute(XNamespace.Xmlns + "SOAP-ENV", SOAPENV)
                );

            XElement Header = new XElement(SOAPENV + "Header");
            root.Add(Header);

            XElement MessageHeader = new XElement(edXML + "MessageHeader");
            Header.Add(MessageHeader);

            //XElement From = new XElement(edXML + "From");
            //MessageHeader.Add(From);

            //XElement OrganId = new XElement(edXML + "OrganId", edBaocao.FromOrganId);
            //XElement Email = new XElement(edXML + "Email", edBaocao.FromEmail);
            //XElement Timestamp = new XElement(edXML + "Timestamp", edBaocao.FromTimestamp);
            //From.Add(OrganId);
            //From.Add(Email);
            //From.Add(Timestamp);

            XElement Code = new XElement(edXML + "Code");
            XElement CodeNumber = new XElement(edXML + "CodeNumber", edBaocao.CodeNumber);
            XElement CodeNotation = new XElement(edXML + "CodeNotation", edBaocao.CodeNotation);
            MessageHeader.Add(Code);
            Code.Add(CodeNumber);
            Code.Add(CodeNotation);

            XElement PromulgationInfo = new XElement(edXML + "PromulgationInfo");
            XElement PromulgationDate = new XElement(edXML + "PromulgationDate", edBaocao.PromulgationDate);
            MessageHeader.Add(PromulgationInfo);
            PromulgationInfo.Add(PromulgationDate);

            XElement TraceHeaderList = new XElement(edXML + "TraceHeaderList");
            MessageHeader.Add(TraceHeaderList);

            XElement TraceHeaderFrom = new XElement(edXML + "TraceHeader");
            TraceHeaderList.Add(TraceHeaderFrom);
            XElement FromOrganId = new XElement(edXML + "OrganId", edBaocao.FromOrganId);
            //XElement FromEmail = new XElement(edXML + "Email", edBaocao.FromEmail);
            XElement FromTimestamp = new XElement(edXML + "Timestamp", edBaocao.FromTimestamp);
            TraceHeaderFrom.Add(FromOrganId);
            //TraceHeaderFrom.Add(FromEmail);
            TraceHeaderFrom.Add(FromTimestamp);

            XElement TraceHeaderTo = new XElement(edXML + "TraceHeader");
            TraceHeaderList.Add(TraceHeaderTo);
            XElement ToOrganId = new XElement(edXML + "OrganId", edBaocao.ToOrganId);
            //XElement ToEmail = new XElement(edXML + "Email", edBaocao.ToEmail);
            XElement ToTimestamp = new XElement(edXML + "Timestamp", edBaocao.ToTimestamp);
            TraceHeaderTo.Add(ToOrganId);
            //TraceHeaderTo.Add(ToEmail);
            TraceHeaderTo.Add(ToTimestamp);


            string xml = string.Empty;
            var sb = new StringBuilder();
            using (StringWriter sw = new StringWriterUtf8(sb))
            {
                root.Save(sw);
                xml = sw.ToString();
                //xml = root.ToString();
            }
            return xml;
        }

        public class StringWriterUtf8 : StringWriter
        {
            public StringWriterUtf8(StringBuilder sb)
                : base(sb)
            {
            }

            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
            }
        }

        public int UpdateEdXMLBaocao(string xml)
        {
            try
            {
                //xml = _FilterEdXML_Envelope(xml);
                QLVB.DTO.Vanbandientu.EdXMLBC.edXMLBaocao edBaocao = StringToEdXMLBCFrom(xml);

                if (edBaocao != null)
                {
                    int intso = Convert.ToInt32(edBaocao.CodeNumber);
                    string strkyhieu = edBaocao.CodeNotation;
                    DateTime? dtengayky = DateServices.FormatDateEn(edBaocao.PromulgationDate);

                    int idvanban = _vanbandiRepo.Vanbandis
                        .Where(p => p.intso == intso)
                        .Where(p => p.strkyhieu == strkyhieu)
                        .Where(p => p.strngayky == dtengayky)
                        .FirstOrDefault().intid;

                    int iddonvi = _tochucRepo.GetAllTochucdoitacs
                        //.Where(p => p.stremailvbdt == edBaocao.ToEmail)
                        .Where(p => p.strmadinhdanh == edBaocao.ToOrganId)
                        .FirstOrDefault().intid;

                    DateTime? dtengaygui = DateServices.FormatDateTimeEn(edBaocao.FromTimestamp);
                    string strngaygui = DateServices.FormatDateTimeEn(dtengaygui);

                    using (var context = new QLVB.DAL.QLVBDatabase())
                    {
                        string strngaynhan = DateServices.FormatDateTimeEn(DateTime.Now);
                        string sSql =
                            "Update guivanban set inttrangthainhan=1,"
                            + "strngaynhan ='" + strngaynhan + "' "
                            + " where intidvanban='" + idvanban.ToString() + "' "
                            + " and intiddonvi='" + iddonvi.ToString() + "' "
                            + " and cast(strngaygui as smalldatetime) = cast('" + strngaygui + "' as smalldatetime) ";
                        context.Database.ExecuteSqlCommand(sSql);
                    }

                    //_guivbRepo.UpdateHoibao(idvanban, iddonvi, dtengaygui);


                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return 1;
        }

        private QLVB.DTO.Vanbandientu.EdXMLBC.edXMLBaocao StringToEdXMLBCFrom(string xml)
        {
            try
            {
                xml = _FilterEdXML_Envelope(xml);

                QLVB.DTO.Vanbandientu.EdXMLBC.edXMLBaocao edBaocao = new QLVB.DTO.Vanbandientu.EdXMLBC.edXMLBaocao();

                XElement allData = XElement.Parse(xml, LoadOptions.None);
                if (allData != null)
                {
                    //string Update = allData.AncestorsAndSelf().FirstOrDefault().Attribute("Update").Value;

                    XNamespace edXML = "http://www.e-doc.vn/Schema/";
                    //XNamespace SOAPENV = "http://schemas.xmlsoap.org/soap/envelope/";

                    var vanban = allData.Descendants(edXML + "Code")
                             .Select(p => new
                             {
                                 so = p.Element(edXML + "CodeNumber").Value,
                                 kh = p.Element(edXML + "CodeNotation").Value
                             });
                    edBaocao.CodeNotation = vanban.Select(p => p.kh).FirstOrDefault();
                    edBaocao.CodeNumber = vanban.Select(p => p.so).FirstOrDefault();

                    var ngayky = allData.Descendants(edXML + "PromulgationDate").FirstOrDefault().Value;
                    edBaocao.PromulgationDate = ngayky;


                    edBaocao.FromEmail = GetAccountSetting().emailAddress;
                    edBaocao.FromOrganId = GetMaDinhdanh();

                    var tracert = allData.Descendants(edXML + "TraceHeader");
                    foreach (var t in tracert)
                    {
                        var fromId = t.Descendants(edXML + "OrganId").FirstOrDefault().Value;
                        if ((fromId == edBaocao.FromOrganId) && (edBaocao.FromTimestamp == null))
                        {
                            edBaocao.FromTimestamp = t.Descendants(edXML + "Timestamp").FirstOrDefault().Value;
                        }
                        else
                        {
                            edBaocao.ToOrganId = fromId;
                            //edBaocao.ToEmail = t.Descendants(edXML + "Email").FirstOrDefault().Value;
                            edBaocao.ToTimestamp = t.Descendants(edXML + "Timestamp").FirstOrDefault().Value;
                        }
                    }
                }

                return edBaocao;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }

        #endregion 2803

        #region AutoSend

        /// <summary>
        /// luu cac don vi gui vbdt de tu dong gui 
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="listSendDonvi"></param>
        /// <returns></returns>
        public ResultFunction SaveAutoSendMail(int idvanban, List<ListSendDonviViewModel> listSendDonvi, int intloaivanban)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                foreach (var p in listSendDonvi)
                {
                    SaveAutoSendVanban(idvanban, p.intid, intloaivanban, p.isvbdt);
                }
                kq.id = (int)ResultViewModels.Success;
                kq.message = "Cập nhật tự động gửi thành công";
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
                kq.message = ex.Message;
            }
            return kq;
        }

        private int SaveAutoSendVanban(int idvanban, int iddonvi, int intloai, bool IsVbdt)
        {
            GuiVanban vb = new GuiVanban
            {
                intidvanban = idvanban,
                intloaivanban = intloai,
                intiddonvi = iddonvi,
                inttrangthaigui = IsVbdt == true ?
                        (int)enumGuiVanban.inttrangthaigui.Chuagui :
                        (int)enumGuiVanban.inttrangthaigui.Chuagui_normal,  // gui binh thuong khong ma hoa
                strngaygui = DateTime.Now,
                inttrangthainhan = (int)enumGuiVanban.inttrangthainhan.Chuanhan
            };
            return _guivbRepo.Them(vb);
        }

        /// <summary>
        /// lay id van ban tu dong gui
        /// </summary>
        /// <returns></returns>
        public GuiVanban GetIdVanbanAutoSend()
        {
            try
            {
                var vb = _guivbRepo.GuiVanbans.Where(p => p.inttrangthaigui != (int)enumGuiVanban.inttrangthaigui.Dagui)
                    .OrderBy(p => p.strngaygui).FirstOrDefault();
                return vb;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }

        public List<ListSendDonviViewModel> GetListSendDonviAutoSend(GuiVanban vanban)
        {
            try
            {
                var listdonvi = _guivbRepo.GuiVanbans
                    .Where(p => p.intidvanban == vanban.intidvanban)
                    .Where(p => p.intloaivanban == vanban.intloaivanban)
                    .Where(p => p.inttrangthaigui != (int)enumGuiVanban.inttrangthaigui.Dagui)
                    .Join(
                        _tochucRepo.GetAllTochucdoitacs,
                        v => v.intiddonvi,
                        t => t.intid,
                        (v, t) => t
                    )
                    .Select(p => new ListSendDonviViewModel
                    {
                        intid = p.intid,
                        stremailvbdt = p.Isvbdt == (int)enumTochucdoitac.isvbdt.IsActive ? p.stremailvbdt : p.stremail,
                        isvbdt = p.Isvbdt == (int)enumTochucdoitac.isvbdt.IsActive ? true : false
                    }).ToList();

                // chon so email 1 lan gui
                int EmailPerRequest = AppSettings.AutoEmailPerRequest;
                if (listdonvi.Count > EmailPerRequest)
                {   // uu tien gui vbdt cho cac don vi su dung pm
                    var dv = listdonvi.Where(p => p.isvbdt == true);
                    if (dv.Count() > 0)
                    {
                        listdonvi = dv.ToList();
                    }
                }

                return listdonvi;

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// cap nhat trang thai da gui vb khi thuc hien tu dong gui vb
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="iddonvi"></param>
        /// <param name="intloai"></param>
        /// <returns></returns>
        public int UpdateGuiVanban(int idvanban, int iddonvi, int intloaivanban)
        {
            try
            {
                return _guivbRepo.UpdateTrangthaiGui(idvanban, iddonvi, intloaivanban);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// lay noi dung van ban gui binh thuong
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="intloai"></param>
        /// <returns></returns>
        public string GetNoidungVanbanSendNormal(int idvanban, int intloaivanban)
        {
            try
            {
                string strnoidung = "";
                switch (intloaivanban)
                {
                    case (int)enumGuiVanban.intloaivanban.Vanbandi:
                        var vb = _vanbandiRepo.Vanbandis.FirstOrDefault(p => p.intid == idvanban);
                        strnoidung = "Số văn bản: " + vb.intso + ", ký hiệu: " + vb.strkyhieu;
                        strnoidung += ", người ký: " + vb.strnguoiky + ", trích yếu: " + vb.strtrichyeu;
                        break;
                    case (int)enumGuiVanban.intloaivanban.Vanbanden:
                        var vbden = _vbdenRepo.GetVanbandenById(idvanban);
                        strnoidung = "Số văn bản: " + vbden.intsoden + ", ký hiệu: " + vbden.strkyhieu;
                        strnoidung += ", người ký: " + vbden.strnguoiky + ", trích yếu: " + vbden.strtrichyeu;
                        break;
                }
                return strnoidung;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return string.Empty;
            }
        }
        public string GetTrichyeuVanbanSendNormal(int idvanban, int intloai)
        {
            try
            {
                string strnoidung = "";
                switch (intloai)
                {
                    case (int)enumGuiVanban.intloaivanban.Vanbandi:
                        var vb = _vanbandiRepo.Vanbandis.FirstOrDefault(p => p.intid == idvanban);
                        strnoidung = vb.strtrichyeu;
                        break;
                    case (int)enumGuiVanban.intloaivanban.Vanbanden:
                        var vbden = _vbdenRepo.GetVanbandenById(idvanban);
                        strnoidung = vbden.strtrichyeu;
                        break;
                }
                return strnoidung;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return string.Empty;
            }
        }
        #endregion AutoSend
    }
}
