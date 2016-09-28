using System.Collections.Generic;

namespace DLTD.Web.Main.ViewModels
{
    public class TonghopTinhhinhXulyViewModel    {
      
        public IEnumerable<KhoiViewModel> Khoi { get; set; }

        public IEnumerable<KhoiViewModel> KhoiDonVi { get; set; }

        public IEnumerable<DonViViewModel> DonVi { get; set; }
        public IEnumerable<DangNhapViewModel> Nguoitheodoi { get; set; }
        public IEnumerable<DangNhapViewModel> Nguoichidao { get; set; }
        public int TongNhiemVu { get; set; }
        public int Moi { get; set; }
        public int DangThucHien { get; set; }
        public int HoanThanh { get; set; }
        public int HoanThanhQuaHan { get; set; }
        public int QuaHan { get; set; }
      
        
    }

    public class TinhHinhXuLyViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int TongNhiemVu { get; set; }
        public int Moi { get; set; }
        public int DangThucHien { get; set; }
        public int HoanThanh { get; set; }
        public int HoanThanhQuaHan { get; set; }
        public int QuaHan { get; set; }
    }
}
       