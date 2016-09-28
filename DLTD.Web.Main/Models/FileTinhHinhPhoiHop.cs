using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DLTD.Web.Main.Models
{
    public class FileTinhHinhPhoiHop:FileDinhKem
    {
        public FileTinhHinhPhoiHop()
        {
            CreatedAt = DateTime.Now;
        }
        [ForeignKey("TinhHinhPhoiHop")]
        public long? IdTinhHinhPhoiHop { get; set; }
        public virtual TinhHinhPhoiHop TinhHinhPhoiHop { get; set; }
    }
}