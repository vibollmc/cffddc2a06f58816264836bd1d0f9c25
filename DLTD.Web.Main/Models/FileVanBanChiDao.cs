using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DLTD.Web.Main.Models
{
    public class FileVanBanChiDao:FileDinhKem
    {
        public FileVanBanChiDao()
        {
            CreatedAt = DateTime.Now;
        }
        [ForeignKey("VanBanChiDao")]
        public long? IdVanBanChiDao { get; set; }
        public virtual VanBanChiDao VanBanChiDao { get; set; }
    }
}
