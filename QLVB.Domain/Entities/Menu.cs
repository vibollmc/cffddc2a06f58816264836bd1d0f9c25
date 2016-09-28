using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{

    public class Menu
    {

        public Int32 Id { get; set; }

        public Int32? ParentId { get; set; }

        public Int32 intlevel { get; set; }

        public string strmota { get; set; }

        public string straction { get; set; }

        public string strcontroller { get; set; }
        public string stricon { get; set; }

        public Int32? blopenwindow { get; set; }

        public Int32? intorder { get; set; }

        public string strquyen { get; set; }

        public int? inttrangthai { get; set; }

        //[ForeignKey("ParentId")]
        //public ICollection<Menu> menuChild { get; set; }

        public virtual Menu ParentMenu { get; set; }

        public virtual ICollection<Menu> SubMenu { get; set; }

    }

    public class enummenu
    {
        public enum inttrangthai
        {
            NotActive = 0,
            IsActive = 1
        }
    }
}