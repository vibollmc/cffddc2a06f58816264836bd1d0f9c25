using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Caching;


namespace QLVB.Common.Utilities
{
    public class AppConts
    {
        // ================================================
        //  GENERAL
        // ================================================

        public const string GlobalName = "HỆ THỐNG QUẢN LÝ VĂN BẢN";
        public const string Version = "2.3.2.6";
        public const int NullId = -1;
        public const int DefaultId = 0;
        public const string NullIpAddress = "0.0.0.0";
        public const string Nam = "2015";
        public const string Dienthoai = "061.3820704";
        // ================================================
        //  MAILBEE
        // ================================================

        public const string MailBeeLicenseKey = "MN600-D31B240F1BD21BEC1BEBD833140B-9575";

        // ================================================
        //  CACHE
        // ================================================

        public const byte DefaultCacheTimeOut = 60;
        //public const CacheItemPriority DefaultCacheItemPriority = CacheItemPriority.Normal;

        //=================================================
        // SESSION
        //=================================================
        // id can bo dang su dung de lay quyen truy cap
        public const string SessionUserId = "UserId";
        public const string SessionUserName = "UserName";
        //public const string SessionDonviId = "DonviId";

        // id can bo thuc khi login
        public const string SessionRealUserId = "RealUserId";

        //public const string SessionMyRoute = "MyRoute";
        public const string SessionUserRoles = "UserRoles";

        // gia tri VBDen/di
        public const string SessionSearchPageType = "PageType";
        public const string SessionSearchPageValues = "PageValues";

        // Search VBden/di
        public const string SessionSearchType = "SearchType";
        public const string SessionSearchTypeValues = "SearchValues";

        // dung cho cac muc: vb dien tu, VBLQ
        public const string SessionSearchList = "SearchList";
        public const string SessionSearchListValues = "SearchListValues";

        public const string SessionSearchPageList = "PageList";
        public const string SessionSearchPageListValues = "PageListValues";

        // tong hop tinh hinh xu ly VB den/1 cua/ykcd

        public const string SessionTonghopXuly = "TonghopXuly";
        public const string SessionTonghopValues = "TonghopValues";

        //=====================================================
        //  NOIDUNG: DINH KEM FILE VAN BAN
        //=====================================================
        public const string FileCongvanden = "Congvanden";
        public const string FileCongvanphathanh = "Congvanphathanh";
        public const string FileEmail = "Email";

        //public const string FileEmailContent = "EmailContent";
        public const string FileEmailInbox = "EmailContent/Inbox";
        public const string FileEmailOutbox = "EmailContent/Outbox";

        public const string FileDongbo = "Dongbo";
        public const string FileHoso = "Hoso";
        public const string FileVanbanduthao = "Vanbanduthao";

        public const string ImageProfile = "~/Noidung/AvatarUsers/"; // chua thay doi folder 

        public const string FileEdxmlInbox = "Edxml/Inbox";
        public const string FileEdxmlOutbox = "Edxml/Outbox";

        //=====================================================
        //  ERROR: cac loi khi truy cap
        //=====================================================
        public const string ErrAccessDenied = "AccessDenied";
        public const string ErrLog = "Không có quyền truy cập ";
    }
}
