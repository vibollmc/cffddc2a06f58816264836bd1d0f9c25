using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DLTD.Web.Main.Models.Enum;

namespace DLTD.Web.Main.Models
{
     [Table("T_DonViPhoiHop")]
    public class DonViPhoiHop
    {
         [Key]
         public int? Id { get; set; }

         [ForeignKey("VanBanChiDao")]
         public long? IdVanBan { get; set; }
         [ForeignKey("DonVi")]
         public int? IdDonvi { get; set; }

         public TrangThai TrangThai { get; set; }

         public virtual VanBanChiDao VanBanChiDao { get; set; }
         public virtual DonVi DonVi { get; set; }
         public virtual ICollection<TinhHinhPhoiHop> TinhHinhPhoiHop { get; set; } 
    }
}