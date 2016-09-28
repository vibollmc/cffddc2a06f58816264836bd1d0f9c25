using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DLTD.Web.Main.Models.Enum;

namespace DLTD.Web.Main.Models
{
    [Table("T_TinhHinhThucHien")]
    public class TinhHinhThucHien
    {
        [Key]
        public long? Id { get; set; }
        [ForeignKey("VanBanChiDao")]
        public long? IdVanBanChiDao { get; set; }
        [MaxLength(2048)]
        public string NoiDungThucHien { get; set; }
        public DateTime? NgayBaoCao { get; set; }
        [ForeignKey("NguoiDung")]
        public int? UserId { get; set; }
        public virtual ICollection<FileTinhHinhThucHien> FileDinhKem { get; set; }
        public virtual VanBanChiDao VanBanChiDao { get; set; }

        public virtual DangNhap NguoiDung { get; set; }
    }
}