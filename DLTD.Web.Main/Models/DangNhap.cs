using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DLTD.Web.Main.Models.Enum;

namespace DLTD.Web.Main.Models
{
    [Table("T_DangNhap")]
    public class DangNhap
    {
        [Key]
        public int? Id { get; set; }
        [MaxLength(50)]
        public string TenDangNhap { get; set; }
        [MaxLength(255)]
        public string MatKhau { get; set; }
        [MaxLength(10)]
        public string Ma { get; set; }
        [MaxLength(50)]
        public string Ten { get; set; }
        public GioiTinh GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(20)]
        public string SoDienThoai { get; set; }
        [MaxLength(512)]
        public string UrlImage { get; set; }
        [ForeignKey("DonViTrucThuoc")]
        public int? IdDonVi { get; set; }
        public TrangThai TrangThai { get; set; }
        public LoaiQlvb DangNhapTu { get; set; }

        public NhomNguoiDung? NhomNguoiDung { get; set; }

        public virtual DonViTrucThuoc DonViTrucThuoc { get; set; }
        public virtual ICollection<TinhHinhThucHien> TinhHinhThucHien { get; set; }
        public virtual ICollection<TinhHinhPhoiHop> TinhHinhPhoiHop { get; set; }
        public virtual ICollection<VanBanChiDao> VanBanChiDao { get; set; }
        public virtual ICollection<VanBanChiDao> VanBanTheoDoi { get; set; }
        public virtual ICollection<VanBanChiDao> VanBanLanhDao { get; set; }
    }
}
