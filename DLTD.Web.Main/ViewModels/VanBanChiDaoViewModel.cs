using System;
using System.ComponentModel.DataAnnotations;
using DLTD.Web.Main.Models.Enum;
using System.Collections.Generic;

namespace DLTD.Web.Main.ViewModels
{
    public class VanBanChiDaoViewModel
    {
        [Display(Name = "Id")]
        public long? Id { get; set; }
        [Display(Name = "User Id")]
        public int? UserId { get; set; }
        [Display(Name = "Người theo dõi")]
        public string NguoiTheoDoi { get; set; }

        [Display(Name = "Ngày ký")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Ngayky { get; set; }

        [Display(Name = "Trích yếu")]
        public string Trichyeu { get; set; }

        [Display(Name = "Số KH")]
        public string SoKH { get; set; }

        [Display(Name = "Ý kiến chỉ đạo")]
        public string YKienChiDao { get; set; }
        [Display(Name = "Thời hạn xử lý")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ThoiHanXuLy { get; set; }

        [Display(Name = "Thời hạn xử lý")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NgayHoanThanh { get; set; }

        [Display(Name = "Trạng thái")]
        public TrangThaiVanBan TrangThai { get; set; }
        [Display(Name = "Id Văn bản")]
        public long? IdVanBan { get; set; }
        [Display(Name = "Ngày khởi tạo")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NgayTao { get; set; }
        [Display(Name = "Id đơn vị")]

        public int? IdDonVi { get; set; }
        [Display(Name = "Đơn vị Xử lý chính")]
        public string TenDonVi { get; set; }

        [Display(Name = "File đính kèm")]
        public int FileDinhKem { get; set; }
        public string LinkFileDinhKem { get; set; }

        [Display(Name = "Độ khẩn")]
        public DoKhan DoKhan { get; set; }

        [Display(Name = "Nguồn chỉ Đạo")]
        public int?NguonChiDao { get; set; }

        [Display(Name = "Trạng thái")]
        public string TrangThaiVB
        {
            get
            {
                if (this.TrangThai == TrangThaiVanBan.TraLai) return "Trả lại";

                if (this.NgayHoanThanh.HasValue && (this.ThoiHanXuLy != null && this.ThoiHanXuLy >= this.NgayHoanThanh))
                    return "Hoàn thành";

                if (this.NgayHoanThanh.HasValue && this.ThoiHanXuLy != null && this.ThoiHanXuLy < this.NgayHoanThanh)
                    return "Hoàn thành trễ hạn";

                if (this.TrangThai == TrangThaiVanBan.DangXuLy &&!this.NgayHoanThanh.HasValue 
                    &&(this.ThoiHanXuLy != null || this.ThoiHanXuLy >= DateTime.Today))

                    return "Đang thực hiện";

                if (!this.NgayHoanThanh.HasValue && this.ThoiHanXuLy != null && this.ThoiHanXuLy < DateTime.Today)
                    return "Trễ hạn";

                return "Mới";
            }
        }
    }
}