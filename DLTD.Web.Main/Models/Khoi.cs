using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DLTD.Web.Main.Models.Enum;

namespace DLTD.Web.Main.Models
{
    [Table("T_Khoi")]
    public class Khoi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? Id { get; set; }
        [MaxLength(20)]
        public string KyHieu { get; set; }
        [MaxLength(100)]
        public string Ten { get; set; }
        public bool? MacDinh { get; set; }
        public TrangThai TrangThai { get; set; }
        public bool? NguonChiDao { get; set; }

        public virtual ICollection<DonVi> DonVi { get; set; }
        public virtual ICollection<VanBanChiDao> VanBanChiDao { get; set; }
    }
}
