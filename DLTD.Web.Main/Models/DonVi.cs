using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DLTD.Web.Main.Models.Enum;

namespace DLTD.Web.Main.Models
{
    [Table("T_DonVi")]
    public class DonVi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? Id { get; set; }
        [ForeignKey("Khoi")]
        public int? IdKhoi { get; set; }
        [MaxLength(20)]
        public string Ma { get; set; }
        [MaxLength(512)]
        public string Ten { get; set; }
        [MaxLength(512)]
        public string DiaChi { get; set; }
        [MaxLength(20)]
        public string DienThoai { get; set; }
        [MaxLength(20)]
        public string Fax { get; set; }
        [MaxLength(255)]
        public string Email { get; set; }
        [MaxLength(255)]
        public string EmailVbdt { get; set; }
        public TrangThai TrangThai { get; set; }

        public virtual Khoi Khoi { get; set; }
        public virtual ICollection<DonViPhoiHop> DonViPhoihop { get; set; }
        public virtual ICollection<VanBanChiDao> VanBanChiDao { get; set; }
    }
}
