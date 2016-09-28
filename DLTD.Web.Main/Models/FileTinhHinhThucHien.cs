using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DLTD.Web.Main.Models
{
    public class FileTinhHinhThucHien:FileDinhKem
    {
        public FileTinhHinhThucHien()
        {
            CreatedAt = DateTime.Now;
        }
        [ForeignKey("TinhHinhThucHien")]
        public long? IdTinhHinhThucHien { get; set; }
        public virtual TinhHinhThucHien TinhHinhThucHien { get; set; }
    }
}