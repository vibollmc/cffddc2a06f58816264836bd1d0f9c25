using DLTD.Web.Main.Models.Enum;

namespace DLTD.Web.Main.ViewModels
{
    public class SearchVanBanModel
    {
        public string NgayKyTu { get; set; }
        public string NgayKyDen { get; set; }
        public string KyHieu { get; set; }
        public string DonViXuLy { get; set; }
        public string NguoiChiDao { get; set; }
        public string NguoiTheoDoi { get; set; }
        public string ThoiHanXuLyTu { get; set; }
        public string ThoiHanXuLyDen { get; set; }
        public string DoUuTien { get; set; }
        public string DonViPhoiHop { get; set; }
        public string NguonChiDao { get; set; }
        public string NoiDung { get; set; }
        public string YKienChiDao { get; set; }
        public string SearchText { get; set; }
        public TrangThaiVanBan? TrangThai { get; set; }

        public bool IsNormalSearch 
        {
            get { return !string.IsNullOrWhiteSpace(SearchText); }
        }

        public bool IsAdvanceSearch
        {
            get
            {
                return !string.IsNullOrWhiteSpace(NgayKyTu) ||
                       !string.IsNullOrWhiteSpace(NgayKyDen) ||
                       !string.IsNullOrWhiteSpace(KyHieu) ||
                       !string.IsNullOrWhiteSpace(DonViXuLy) ||
                       !string.IsNullOrWhiteSpace(NguoiChiDao) ||
                       !string.IsNullOrWhiteSpace(NguoiTheoDoi) ||
                       !string.IsNullOrWhiteSpace(ThoiHanXuLyTu) ||
                       !string.IsNullOrWhiteSpace(ThoiHanXuLyDen) ||
                       !string.IsNullOrWhiteSpace(DoUuTien) ||
                       !string.IsNullOrWhiteSpace(DonViPhoiHop) ||
                       !string.IsNullOrWhiteSpace(NguonChiDao) ||
                       !string.IsNullOrWhiteSpace(NoiDung) ||
                       !string.IsNullOrWhiteSpace(YKienChiDao);
            }
        }
    }
}