using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DLTD.Web.Main.Models.Enum;

namespace DLTD.Web.Main.Models
{
    [Table("T_TinhHinhPhoiHop")]
    public class TinhHinhPhoiHop
    {
        [Key]
        public long? Id { get; set; }
        [ForeignKey("DonViPhoiHop")]
        public int? IdDonViPhoiHop { get; set; }

        [MaxLength(2048)]
        public string NoiDungThucHien { get; set; }
        public DateTime? NgayXuLy { get; set; }
        [ForeignKey("NguoiDung")]
        public int? UserId { get; set; }
        public virtual ICollection<FileTinhHinhPhoiHop> FileDinhKem { get; set; }
        public virtual DonViPhoiHop DonViPhoiHop { get; set; }
        public virtual DangNhap NguoiDung { get; set; }
    }
}