using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DLTD.Web.Main.Models.Enum;

namespace DLTD.Web.Main.Models
{
    [Table("T_VanBanChiDao")]
    public class VanBanChiDao
    {
        public VanBanChiDao()
        {
            NgayTao = DateTime.Now;
        }
        [Key]
        public long? Id { get; set; }

        [ForeignKey("NguoiGui")]
        public int? UserId { get; set; }
        public string Trichyeu { get; set; }
        [MaxLength(512)]
        public string SoKH { get; set; }

        [MaxLength(2048)]
        public string YKienChiDao { get; set; }
        public DateTime? ThoiHanXuLy { get; set; }
        public DateTime? Ngayky { get; set; }
        public TrangThaiVanBan TrangThai { get; set; }
        public long? IdVanBan { get; set; }

        public DateTime? NgayTao { get; set; }
        public DateTime? NgayHoanThanh { get; set; }

        [ForeignKey("DonVi")]
        public int? IdDonVi { get; set; }

        public DoKhan DoKhan { get; set; }
        //0: thường, 1: hỏa tốc
        [ForeignKey("NguonChiDao")]
        public int? IdNguonChiDao { get; set; }
        //Nguồn chỉ đạo

        [ForeignKey("NguoiChiDao")]
        public int? IdNguoiChiDao { get; set; }
        [ForeignKey("NguoiTheoDoi")]
        public int? IdNguoiTheoDoi { get; set; }
        public int? IsTralai { get; set; }
        public int? LydoTraLai { get; set; }
        public DateTime? NgayTra { get; set; }
        public TrangThaiVanBan? TinhHinhThucHienNoiBo { get; set; }
        public virtual DangNhap NguoiGui { get; set; }
        public virtual DonVi DonVi { get; set; }
        public virtual Khoi NguonChiDao { get; set; }
        public virtual DangNhap NguoiChiDao { get; set; }
        public virtual DangNhap NguoiTheoDoi { get; set; }
        public virtual ICollection<FileVanBanChiDao> FileDinhKem { get; set; }
        public virtual ICollection<DonViPhoiHop> DonViPhoihop { get; set; }
        public virtual ICollection<TinhHinhThucHien> TinhHinhThucHien { get; set; }
    }
}
