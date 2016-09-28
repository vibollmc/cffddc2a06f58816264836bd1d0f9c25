using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DLTD.Web.Main.Models.Enum;

namespace DLTD.Web.Main.Models
{
    [Table("T_DonViTrucThuoc")]
    public class DonViTrucThuoc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? Id { get; set; }
        public int? ParentId { get; set; }
        [MaxLength(20)]
        public string Ma { get; set; }
        [MaxLength(512)]
        public string Ten { get; set; }
        public int? Level { get; set; }
        public TrangThai TrangThai { get; set; }

        public virtual ICollection<DangNhap> NguoiDung { get; set; } 
    }
}
