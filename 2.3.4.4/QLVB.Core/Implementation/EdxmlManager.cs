using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Core.Contract;
using INet;
using INet.SecurityToken;
using INet.SecurityToken.Model;

using INet.Mercury;
using INet.Mercury.Model;
using QLVB.Common.Logging;
using QLVB.Common.Sessions;
using QLVB.Domain.Abstract;

using ThirdParty.Sharp.Zlib;
using INet.EdXml2;
using INet.EdXml2.Util;
using INet.EdXml2.Attachment;
using INet.EdXml2.Header;
using INet.EdXml2.Body;

using QLVB.Domain.Entities;
using QLVB.Common.Utilities;
using QLVB.Common.Date;
using QLVB.DTO.Mail;
using QLVB.DTO.Edxml;
using QLVB.DTO;
using System.IO;
using From = INet.EdXml2.Header.From;
using ResponseFor = INet.EdXml2.Header.ResponseFor;

using System.Text.RegularExpressions;
using ThirdParty.Sharp.Zlib;
using INet.StatusXml;
using INet.StatusXml.Util;
using FromRe = INet.StatusXml.SOAP.SOAPHeader.From;
using ReponseforRe = INet.StatusXml.SOAP.SOAPHeader.ResponseFor;
using INet.StatusXml.SOAP.SOAPHeader;



namespace QLVB.Core.Implementation
{
    public class EdxmlManager : IEdxmlManager
    {
        #region Constructor

        private ILogger _logger;
        private ISessionServices _session;
        private ICanboRepository _canboRepo;
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
        private IMailFormatManager _mailFormat;       


        public EdxmlManager(ICanboRepository canborepo,
                ILogger logger, ISessionServices session,
                ISoVanbanRepository sovbRepo,
            ITochucdoitacRepository tochucRepo, IGuiVanbanRepository guivbRepo,
            IVanbandiRepository vanbandiRepo, IConfigRepository configRepo,
            IAttachVanbanRepository fileRepo, IFileManager fileManager,
            IVanbandenmailRepository vbdenMailRepo, IAttachMailRepository attachMailRepo,
            IMailInboxRepository inboxRepo, IMailOutboxRepository outboxRepo,
            IPhanloaiVanbanRepository plvanbanRepo,
            IVanbandenRepository vbdenRepo, IMailFormatManager mailFormat
                )
        {
            _canboRepo = canborepo;
            _logger = logger;
            _session = session;
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
            _mailFormat = mailFormat;
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
        /// ma dinh danh cua don vi tu phan mem
        /// </summary>
        /// <returns></returns>
        public string GetMaDinhdanh()
        {
            return Donvi.Donvi.GetMaDinhDanh();
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


        public DonviEdxmlViewModel getalldonvi(int idvanban)
        {
            try
            {
                string serviceEdxml = QLVB.Common.Utilities.AppSettings.ServiceEdxml;
                MercuryService service = IWSClientFactory.CreateMercuryService(serviceEdxml);
                GetAgencyResponse response = service.GetAgency();

                GetListAgenciesResponse listResponse = service.GetListAgencies();

                //return listResponse.Agencies;
                DonviEdxmlViewModel donvi = new DonviEdxmlViewModel();
                List<AgencyViewModel> agency = new List<AgencyViewModel>();
                foreach (var p in listResponse.Agencies)
                {
                    AgencyViewModel model = new AgencyViewModel();
                    model.Id = p.Id;
                    model.IsSend = false;
                    model.Madinhdanh = p.Code;
                    model.Pid = p.Pid;
                    model.strtendonvi = p.Name;
                    agency.Add(model);
                }
                donvi.listdonvi = agency;
                donvi.idvanban = idvanban;
                return donvi;
            }
            catch (Exception ex)
            {
                _logger.Warn("edxml error : " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// lay ma dinh danh tren truc lien thong
        /// </summary>
        /// <returns></returns>
        private string GetMadinhdanhTruclienthong()
        {
            string serviceEdxml = QLVB.Common.Utilities.AppSettings.ServiceEdxml;
            MercuryService service = IWSClientFactory.CreateMercuryService(serviceEdxml);
            GetAgencyResponse response = service.GetAgency();

            return response.AgencyResult.Code;
        }

        #region Send

        public string WriteEdxml(int idvanban, int intloaivanban, DonviEdxmlViewModel donvi)
        {
            try
            {
                EdXml2 edXML1 = new EdXml2();
                edXML1.Protocol = EdXmlEnum.Protocol.HTTP;

                From fr = new From();
                //fr.OrganId = GetMaDinhdanh();//"00.02.H19";
                fr.OrganId = GetMadinhdanhTruclienthong();
                //fr.OrganizationInCharge = "UBND Tỉnh Đồng Nai";
                fr.OrganName = GetTendonvi();//"Văn phòng UBND tỉnh Đồng Nai";
                fr.OrganAdd = _configRepo.GetConfig(ThamsoHethong.DiachiDonvi);
                fr.Telephone = _configRepo.GetConfig(ThamsoHethong.DienthoaiDonvi);
                fr.Email = _configRepo.GetConfig(ThamsoHethong.UsernameMail);

                edXML1.From = fr;

                List<To> listTo = new List<To>();
                foreach (var p in donvi.listdonvi)
                {
                    To iTo = new To();
                    iTo.OrganId = p.Madinhdanh;
                    iTo.OrganName = p.strtendonvi;
                    listTo.Add(iTo);
                }

                edXML1.ToList = listTo.ToArray();
                //========================================
                int intloaiAttachfile = 0;
                switch (intloaivanban)
                {
                    case (int)enumGuiVanban.intloaivanban.Vanbandi:
                        var vanban = _vanbandiRepo.Vanbandis.FirstOrDefault(p => p.intid == idvanban);

                        Code code = new Code();
                        code.Number = vanban.intso.ToString();
                        code.Notation = vanban.strkyhieu;
                        edXML1.Code = code;
                        edXML1.Promulgation = new Promulgation().WithDate(vanban.strngayky)
                                            .WithPlace(AppSettings.DiadanhDiaphuong);
                        edXML1.Document = new Document().WithType("18").WithName("Công văn");
                        edXML1.Subject.Value = vanban.strtrichyeu;

                        edXML1.SteeringType = 10;

                        edXML1.SignerInfo = new SignerInfo().WithFullName(vanban.strnguoiky);

                        //===========================================================

                        intloaiAttachfile = (int)enumAttachVanban.intloai.Vanbandi;

                        break;

                    case (int)enumGuiVanban.intloaivanban.Vanbanden:
                        var vbden = _vbdenRepo.Vanbandens.FirstOrDefault(p => p.intid == idvanban);

                        code = new Code();
                        code.Number = vbden.intsoden.ToString();
                        code.Notation = vbden.strkyhieu;
                        edXML1.Code = code;
                        edXML1.Promulgation = new Promulgation().WithDate((DateTime)vbden.strngayden)
                                                 .WithPlace(AppSettings.DiadanhDiaphuong);
                        edXML1.Document = new Document().WithType("18").WithName("Công văn");
                        edXML1.Subject.Value = vbden.strtrichyeu;

                        edXML1.SteeringType = 10;
                        edXML1.SignerInfo = new SignerInfo().WithFullName(vbden.strnguoiky);

                        intloaiAttachfile = (int)enumAttachVanban.intloai.Vanbanden;

                        break;
                }
                //===========================================

                edXML1.DocumentId.Value = idvanban.ToString();

                try
                {
                    var listfile = _mailFormat.GetFileVanbanToAttach(idvanban, intloaiAttachfile);

                    int numFileAttachment = listfile.Count();

                    string folderzip = AppSettings.Noidung + "/" + AppConts.FileEdxmlOutbox;
                    string physisfolderzip = System.Web.HttpContext.Current.Server.MapPath(folderzip);

                    FileAttach[] fas = new FileAttach[numFileAttachment];
                    for (int i = 0; i <= numFileAttachment - 1; i++)
                    {
                        if (System.IO.File.Exists(listfile[i].filePath))
                        {
                            //_logger.Error("log file dinh kem: " + listfile[i].filePath);

                            try
                            {
                                FileAttach fn = new FileAttach();
                                using (System.IO.FileStream fileStream = new System.IO.FileStream(listfile[i].filePath,
                                System.IO.FileMode.Open, System.IO.FileAccess.Read))
                                {

                                    byte[] buffer = new byte[fileStream.Length];
                                    fileStream.Read(buffer, 0, Convert.ToInt32(fileStream.Length));
                                    fn.FileName = fileStream.Name;
                                    fn.FileSize = fileStream.Length;
                                    fn.FileStream = new System.IO.MemoryStream(buffer);
                                    fn.OriginalName = listfile[i].strmota;
                                    fileStream.Close();
                                }
                                fn.FileStream = QLVB.Common.Edxml.EdXml2Override.Compressed(fn, physisfolderzip);
                                //EdXmlUtil.Compressed(this);
                                fn.FileSize = fn.FileStream.Length;

                                fas[i] = fn;
                            }
                            catch (Exception ex)
                            {
                                _logger.Error(ex.Message);
                            }
                            //try
                            //{
                            //    FileAttach fa = new FileAttach();                         
                            //    fa.ReadFile(listfile[i].filePath, listfile[i].strmota);
                            //    fas[i] = fa;
                            //}
                            //catch (Exception ex)
                            //{
                            //    _logger.Error(ex.Message);
                            //}
                        }
                    }
                    edXML1.FileAttachList = fas;
                }
                catch (Exception ex)
                {
                    _logger.Error("error edxml file dinh kem: " + ex.Message);
                }

                //=========================================
                edXML1.ToPlaces = new ToPlaces().WithPlace(new List<string>().ToArray());
                edXML1.OtherInfo = new OtherInfo().WithPriority(0);
                edXML1.Timestamp.Value = DateTime.Now;// thoi gian gui

                ResponseFor responseFor = new ResponseFor();
                responseFor.Code = "ResponseFor Code";
                responseFor.OrganId = "ResponseFor OrganId";
                responseFor.PromulgationDate = DateTime.Now;//new DateTime(2015, 8, 5, 5, 12, 30);
                //edXML1.ResponseFor = responseFor;

                ErrorList erl = new ErrorList();
                Error er;
                er = new Error();
                //er.ErrCode = "ErrCode 01";
                //er.ErrDescription = "ErrDescription 01";
                erl.Error.Add(er);

                edXML1.ErrorList = erl;

                string filepathEdxml = _fileManager.SetPathUpload(AppConts.FileEdxmlOutbox);
                string fileEdxml = filepathEdxml + "\\" + idvanban.ToString() + ".edxml";

                int count = 0;
                while (System.IO.File.Exists(fileEdxml))
                {
                    count++;
                    fileEdxml = filepathEdxml + "\\" + idvanban.ToString() + "_" + count.ToString() + ".edxml";
                }
                EdXmlInfo edXmlInfo = edXML1.ToFile(fileEdxml);

                edXML1.Dispose();

                return fileEdxml;
            }
            catch (Exception ex)
            {
                _logger.Error("Error Write edxml : " + ex.Message);
                return string.Empty;
            }

        }

        public string Sender(int idvanban, int intloaivanban, DonviEdxmlViewModel donvi)
        {
            string fileEdxml = WriteEdxml(idvanban, intloaivanban, donvi);
            if (string.IsNullOrEmpty(fileEdxml))
            {
                return "Error file edxml";
            }

            try
            {
                string serviceEdxml = QLVB.Common.Utilities.AppSettings.ServiceEdxml;
                MercuryService service = IWSClientFactory.CreateMercuryService(serviceEdxml);

                //send knobstick.
                GetSlotResponse slotResponse = service.RequestSlot(new GetSlotRequest().WithType(KnobStickType.ElectronicDocument));

                // build the slot.
                SendKnobStickResponse knobStickResponse = null;

                using (var stream = System.IO.File.OpenRead(fileEdxml))
                {
                    SendKnobStickRequest knobstickRequest = new SendKnobStickRequest()
                            .WithSlot(slotResponse.Slot.Id)
                            .WithContent(stream)
                            .WithKey("knobstick.edxml");

                    knobStickResponse = service.SendKnobStick(knobstickRequest);
                }
                // send knobtick to T371002.
                var deliverKnobstickRequest = new DeliverKnobStickRequest().WithId(knobStickResponse.KnobStickMetadata.Id);
                var deliverKnobstickResponse = service.DeliverKnobStick(deliverKnobstickRequest);

                return deliverKnobstickResponse.Status;
            }
            catch (Exception ex)
            {
                _logger.Warn("Error send edxml :  " + ex.Message);
                return "Error Edxml";
            }
        }

        #endregion Send

        #region Receive
        public ResultFunction Receiver()
        {
            ResultFunction kq = new ResultFunction();
            kq.id = -1;
            try
            {
                string serviceEdxml = QLVB.Common.Utilities.AppSettings.ServiceEdxml;
                MercuryService service = new MercuryServiceClient(serviceEdxml);

                List<string> listfileEdxml = new List<string>();

                // check knobstick.
                var checkKnobStickResponse = service.CheckKnobStick(new CheckKnobStickRequest().WithType(KnobStickType.ElectronicDocument));

                foreach (var knobstick in checkKnobStickResponse.KnobStickMetadatas)
                {
                    var processKnobStick = new ProcessKnobStickRequest()
                            .WithId(knobstick.Id)
                            .WithStatus(ProcessStatus.processing);

                    var processKnobstickResponse = service.ProcessKnobStick(processKnobStick);

                    if (string.Equals("OK", processKnobstickResponse.Status, StringComparison.Ordinal))
                    {
                        try
                        {
                            string filepathEdxml = _fileManager.SetPathUpload(AppConts.FileEdxmlInbox);
                            string fileEdxml = filepathEdxml + "\\" + knobstick.Id + ".edxml";

                            int count = 0;
                            while (System.IO.File.Exists(fileEdxml))
                            {
                                count++;
                                fileEdxml = filepathEdxml + "\\" + knobstick.Id + "_" + count.ToString() + ".edxml";
                            }

                            var getKnobstickResponse = service.GetKnobStick(new GetKnobStickRequest().WithId(knobstick.Id));
                            getKnobstickResponse.WriteResponseStreamToFile(fileEdxml);
                            getKnobstickResponse.Dispose();
                            service.ProcessKnobStick(processKnobStick.WithStatus(ProcessStatus.done));

                            listfileEdxml.Add(fileEdxml);

                            ReadEdxml(fileEdxml);
                        }
                        catch (Exception ex)
                        {
                            _logger.Warn("Error Receive edxml : " + ex.Message);
                            service.ProcessKnobStick(processKnobStick.WithStatus(ProcessStatus.fail));
                            //kq.message = "Lỗi nhận văn bản điện tử";
                            //return kq;
                        }
                    }
                }
                kq.message = listfileEdxml.Count().ToString();
                kq.id = (int)ResultViewModels.Success;
                return kq;
            }
            catch (Exception ex)
            {
                _logger.Warn("Error Receive edxml : " + ex.Message);
                kq.message = "Lỗi nhận văn bản điện tử";
                return kq;
            }

        }

        public int ReadEdxml(string fileEdxml)
        {
            //Read from file
            EdXml2 EdXml = new EdXml2();
            EdXml.FromFile(fileEdxml);

            bool IsAttach = EdXml.FileAttachList.Count() > 0 ? true : false;

            string strnoinhan = string.Empty;
            if (EdXml.ToList != null)
            {
                foreach (To t in EdXml.ToList)
                {
                    if (t != null)
                    {
                        strnoinhan += t.OrganName + "; ";
                    }
                }
            }
            int intso = Convert.ToInt32(EdXml.Code.Number);
            DateTime dteNgayky = EdXml.Promulgation.Date;
            DateTime dteNgayguivb = EdXml.Timestamp.Value;
            DateTime dteHantraloi = EdXml.DueDate.Value;

            int idmail = _SaveVanbanmail(IsAttach, null,
                        EdXml.OtherInfo.Priority, intso, EdXml.Code.Notation, dteNgayky,
                        EdXml.SignerInfo.FullName, strnoinhan, EdXml.From.OrganName, EdXml.From.OrganId,
                        EdXml.Document.Name, EdXml.Subject.Value, dteHantraloi,
                        dteNgayguivb, EdXml.From.Email
                        );

            string folderPath = _fileManager.SetPathUpload(AppConts.FileEmail);

            foreach (FileAttach fi in EdXml.FileAttachList)
            {
                try
                {
                    if (fi != null)
                    {
                        string strmota = fi.OriginalName;
                        string fileSavepath = _mailFormat.SaveAttachment(idmail, strmota, folderPath);

                        string folderpath = AppSettings.Noidung + "/" + AppConts.FileEmail;
                        string physisfolderzip = System.Web.HttpContext.Current.Server.MapPath(folderpath);

                        FileAttach fn = fi;
                        fn.FileStream = QLVB.Common.Edxml.EdXml2Override.Decompressed(fn, physisfolderzip);
                        fn.FileSize = fn.FileStream.Length;

                        using (FileStream fileStream = new FileStream(fileSavepath, FileMode.Create, FileAccess.Write))
                        {
                            byte[] array = new byte[fn.FileSize];
                            int count;
                            while ((count = fn.FileStream.Read(array, 0, array.Length)) > 0)
                            {
                                fileStream.Write(array, 0, count);
                            }
                            fileStream.Close();
                        }

                        // insert vao database attachmail                                    
                        _mailFormat.InsertAttachment(idmail, fileSavepath, strmota, (int)enumAttachMail.intloai.Vanbandendientu);

                    }
                }
                catch (Exception ex)
                {
                    _logger.Error("read edxml : " + ex.Message);
                }

            }
            EdXml.Dispose();

            return idmail;
        }


        private int _SaveVanbanmail(bool isAttach, int? intidphanloaivanbanden,
            int? intkhan, int intso, string strkyhieu, DateTime? dtengayky,
            string strnguoiky, string strnoinhan, string strnoiguivb, string strmadinhdanh,
            string strloaivb, string strtrichyeu, DateTime? dtehantraloi,
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

                vb.strngayky = dtengayky;

                vb.strnguoiky = strnguoiky;
                vb.strnoigui = strnoinhan;
                vb.strnoiguivb = strnoiguivb;
                vb.strloaivanban = strloaivb;
                vb.strtrichyeu = strtrichyeu;

                vb.strngayguivb = dtengayguivb;

                vb.strAddressSend = stremailguivb;

                vb.strngaynhanvb = DateTime.Now;

                if (dtehantraloi.Value.Year > 2010)
                {
                    vb.strhantraloi = dtehantraloi;
                }
                vb.strmadinhdanh = strmadinhdanh;

                int id = _vbdenMailRepo.Them(vb);

                return id;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return 0;
            }
        }


        #endregion Receive

        #region SendStatus

        public string SendStatus(int idvanban, string status, string statusDescription, string nguoigui, string phongban)
        {
            try
            {
                var vanbanden = _vbdenRepo.GetVanbandenById(idvanban);
                if (vanbanden == null) return null;

                var vanbandenMail =
                    _vbdenMailRepo.Vanbandenmails.FirstOrDefault(x => x.intid == vanbanden.intidvanbandenmail);

                if (vanbandenMail == null) return null;

                var madonviNhan = vanbandenMail.strmadinhdanh;
                var tendonviNhan = vanbandenMail.strnoiguivb;
                var sokyhieuvanban = vanbandenMail.intsoban + vanbandenMail.strkyhieu;

                if (string.IsNullOrEmpty(madonviNhan)) return null;

                if (madonviNhan.StartsWith(AppSettings.MaEdxmlDiaphuong)) return null;

                //Read from file
                var statusXml = new StatusXml();

                var headerStatus = statusXml.StatusXmlSoap.Envelope.Header.Status;

                var from = headerStatus.From;
                from.OrganId = GetMadinhdanhTruclienthong();
                //from.OrganizationInCharge = "From OrganizationInCharge";
                from.OrganName = GetTendonvi();
                //from.OrganizationInCharge = "From OrganizationInCharge";
                from.OrganAdd = _configRepo.GetConfig(ThamsoHethong.DiachiDonvi);
                from.Email = _configRepo.GetConfig(ThamsoHethong.UsernameMail);
                from.Telephone = _configRepo.GetConfig(ThamsoHethong.DienthoaiDonvi);
                //from.Fax = "From Fax";
                //from.Website = "From Website";

                var responseFor = headerStatus.ResponseFor;
                //TODO: truyen ma don vi nhan               
                responseFor.OrganId= madonviNhan;
                responseFor.Code = sokyhieuvanban;
                responseFor.PromulgationDateValue = DateTime.Now;

                headerStatus.StatusCode = status;
                headerStatus.Description = statusDescription;

                headerStatus.TimestampValue = DateTime.Now;

                var staffInfo = headerStatus.StaffInfo;
                staffInfo.Department = phongban;
                staffInfo.Staff = nguoigui;

                var filepathEdxml = _fileManager.SetPathUpload(AppConts.FileStatusEdxmlOutbox);
                var fileEdxml = filepathEdxml + "\\" + idvanban.ToString() + ".edxml";

                var count = 0;
                while (System.IO.File.Exists(fileEdxml))
                {
                    count++;
                    fileEdxml = filepathEdxml + "\\" + idvanban.ToString() + "_" + count.ToString() + ".edxml";
                }

                var statusXmlInfo = statusXml.ToFile(fileEdxml);

                statusXml.Dispose();

                //Send o day

                if (string.IsNullOrEmpty(fileEdxml))
                {
                    return "Error file edxml";
                }

                try
                {
                    var serviceEdxml = QLVB.Common.Utilities.AppSettings.ServiceEdxml;
                    var service = IWSClientFactory.CreateMercuryService(serviceEdxml);

                    //send knobstick.
                    var slotResponse = service.RequestSlot(new GetSlotRequest().WithType(KnobStickType.StatusDocument));

                    // build the slot.
                    SendKnobStickResponse knobStickResponse = null;

                    using (var stream = System.IO.File.OpenRead(fileEdxml))
                    {
                        var knobstickRequest = new SendKnobStickRequest()
                            .WithSlot(slotResponse.Slot.Id)
                            .WithContent(stream)
                            .WithKey("knobstick.edxml");

                        knobStickResponse = service.SendKnobStick(knobstickRequest);
                    }
                    // send knobtick to T371002.
                    var deliverKnobstickRequest =
                        new DeliverKnobStickRequest().WithId(knobStickResponse.KnobStickMetadata.Id);
                    var deliverKnobstickResponse = service.DeliverKnobStick(deliverKnobstickRequest);

                    return deliverKnobstickResponse.Status;
                }
                catch (Exception ex)
                {
                    _logger.Warn("Error send edxml :  " + ex.Message);
                    return "Error Edxml";
                }
            }
            catch(Exception e)
            {
                _logger.Warn(e.Message);
                return null;
            }
        }

        #endregion  SendStatus


        #region ReceiveStatus

        public string ReceiveStatus(String statusXmlName)
        {
            try
            {
                StatusXml statusXml = new StatusXml();
                statusXml.FromFile(AppSettings.Noidung + "/" + AppConts.FileStatusEdxmlOutbox + statusXmlName);

                Status headerStatus = statusXml.StatusXmlSoap.Envelope.Header.Status;

                FromRe from = headerStatus.From;
              
                ReponseforRe responseFor = headerStatus.ResponseFor; 
                string sokyhieu = responseFor.Code;
                string madinhdanhdonvi = from.OrganId;
                var trangthai = headerStatus.StatusCode;
                DateTime ngayphathanhvanban = headerStatus.TimestampValue.Value;
                StaffInfo staffInfo = headerStatus.StaffInfo;
                var tochucdoitac = _tochucRepo.GetActiveTochucdoitacs
                    .Where(p => p.strmadinhdanh == madinhdanhdonvi)
                    .FirstOrDefault();
                if (tochucdoitac!=null)
                {
               var vanbandi = _vanbandiRepo.Vanbandis            
               .Where(p => p.strngayky == ngayphathanhvanban)  
               .Where(p=> p.intid + p.strkyhieu == sokyhieu)              
               .FirstOrDefault();
                if (vanbandi != null)
                {
                  
                    var guivanban = _guivbRepo.GuiVanbans
                        .Where (p=>p.intidvanban ==vanbandi.intid)
                        .Where(p=>p.intiddonvi==tochucdoitac.intid)
                        .FirstOrDefault();
                   
                    if (guivanban!=null)
                    {
                       var ketqua= _guivbRepo.UpdateTrangthaiNhan(guivanban.intidvanban, guivanban.intid,enumGuiVanban.intloaivanban.Vanbandi,trangthai, ngayphathanhvanban);
                    } 
                }
               
                }

                statusXml.Dispose();

                return null;
            }
            catch (Exception e)
            {
                _logger.Warn(e.Message);
                return null;
            }

        }

        #endregion ReceiveStatus


    }
}