using System;

namespace DLTD.Web.Main.ViewModels
{
    public enum LoaiBaoCao
    {
        XuLyChinh = 1,
        PhoiHop = 2
    }
    public class TinhHinhThucHienViewModel
    {
        public long? Id { get; set; }
        public long? IdVanBanChiDao { get; set; }
        public string NoiDungThucHien { get; set; }
        public DateTime? NgayBaoCao { get; set; }
        public string DonVi { get; set; }
        public string NguoiDung { get; set; }
        public string FileDinhKem { get; set; }
        public string FileUrl { get; set; }
        public LoaiBaoCao LoaiBaoCao { get; set; }
    }
}