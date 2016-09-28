using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DLTD.Web.Main.Models
{
    [Table("T_FileDinhKem")]
    public abstract class FileDinhKem
    {
        [Key]
        public long? Id { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(512)]
        public string Url { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
