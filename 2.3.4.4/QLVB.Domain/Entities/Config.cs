using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Entities
{
    public class Config
    {
        public int intid { get; set; }

        public string strthamso { get; set; }

        public string strgiatri { get; set; }

        public string strmota { get; set; }

        public int? intorder { get; set; }

        public int? intgroup { get; set; }

        public int? inttrangthai { get; set; }
    }

    /// <summary>
    /// cac truong tham so trong bang config
    /// </summary>
    public class ThamsoHethong
    {
        public const string SoNgayHienThi = "SoNgayHienThi";
        public const string SMTPServer = "SMTPServer";
        public const string POP3Server = "POP3Server";
        public const string SMTPPort = "SMTPPort";
        public const string POP3Port = "POP3Port";
        public const string UsernameMail = "UsernameMail";
        public const string PasswordMail = "PasswordMail";

        /// <summary>
        /// can bo da chuyen xu ly co quyen xem ho so khong
        /// </summary>
        public const string IsXulyHoso = "IsXulyHoso";  // true/false

        public const string IsXulychinhKetthucHoso = "IsXulychinhKetthucHoso"; //true/false

        public const string ThoihanXLVB = "ThoihanXLVB";

        // mã định danh của đơn vị theo quy định của Bộ TTTT
        // trường này không hiển thị để cho thay đổi
        // tuy theo dll donvi
        public const string MaDinhDanh = "MaDinhDanh";

        public const string SMTPAuthentication = "SMTPAuthentication";

        /// <summary>
        /// hien thi van ban den da xu ly
        /// </summary>
        public const string IsViewVBDenDaXL = "IsViewVBDenDaXL";

        // dia chi va dien thoai dung de gui nhan vbdt theo 2803
        public const string DiachiDonvi = "DiachiDonvi";
        public const string DienthoaiDonvi = "DienthoaiDonvi";

        // ======= bao cao so lieu ve UBT
        public const string IsSendTonghopVB = "IsSendTonghopVB";
        public const string IPAddressUBT = "IPAddressUBT";
        public const string TimeSend = "TimeSend";
        public const string TimeAutoSendVB = "TimeAutoSendVB";

        //=========truc lien thong cua tinh
        public const string TrucLienthongTinh = "TrucLienthongTinh";
        public const string UsernameTrucTinh = "UsernameTrucTinh";
        public const string PasswordTrucTinh = "PasswordTrucTinh";
        public const string MaDonviTrucTinh = "MaDonviTrucTinh";

    }

    public class enumConfig
    {
        public enum inttrangthai
        {
            NotActive = 0,
            IsActive = 1
        }
    }

    public class enumMailConfig
    {
        public enum SMTPAuthentication
        {
            None = 1,
            SaslCramMD5 = 2,
            SaslDigestMD5 = 3,
            SaslGssApi = 4,
            SaslNtlm = 5
        }
    }

}
