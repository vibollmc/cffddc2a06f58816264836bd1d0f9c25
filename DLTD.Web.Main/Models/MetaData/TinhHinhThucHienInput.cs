using System;

namespace DLTD.Web.Main.Models.MetaData
{
    public class TinhHinhThucHienInput
    {
        public int? UserId { get; set; }

        public int? IdDonVi { get; set; }
        public DateTime? NgayBaoCao { get; set; }
        public string NoiDungBaoCao { get; set; }
        public long? IdVanBanChiDao { get; set; }

        public string FileDinhKem { get; set; }
        public string FileUrl { get; set; }
    }
}