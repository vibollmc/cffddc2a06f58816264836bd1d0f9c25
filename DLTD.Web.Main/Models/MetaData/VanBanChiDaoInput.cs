using System;
using System.Collections.Generic;
using DLTD.Web.Main.Models.Enum;
namespace DLTD.Web.Main.Models.MetaData
{
    public class VanBanChiDaoInput
    {
        public long? Id { get; set; }
        public int? UserId { get; set; }
        public string YKienChiDao { get; set; }
        public long? IdVanBan { get; set; }
        public int? IdDonVi { get; set; }
        public DoKhan DoKhan { get; set; }
        public int? NguonChiDao { get; set; }
        public DateTime? ThoiHanXuLy { get; set; }
        public string DonViPhoiHop { get; set; }
        public string SoKH { get; set; }
        public DateTime? Ngayky { get; set; }
        public string Trichyeu { get; set; }

        public int? IdNguoiChiDao { get; set; }
        public int? IdNguoiTheoDoi { get; set; }
        public IList<FileDinhKemInput> FileDinhKem { get; set; }
    }
    public class FileDinhKemInput
    {
        public string TenFile { get; set; }
        public string UrlFile { get; set; }
    }
}
