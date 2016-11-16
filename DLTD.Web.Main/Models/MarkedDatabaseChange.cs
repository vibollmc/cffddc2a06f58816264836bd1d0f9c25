using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DLTD.Web.Main.Models
{
    [Table("T_MarkedDatabaseChange")]
    public class MarkedDatabaseChange
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(512)]
        public string ConnectionString { get; set; }
        public bool IsSyncData { get; set; }
    }
}