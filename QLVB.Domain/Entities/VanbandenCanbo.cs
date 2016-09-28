using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace QLVB.Domain.Entities
{

    public class VanbandenCanbo
    {
        //[Key, ForeignKey("Canbo"), Column("intid")]
        //[Key, Column(Order = 0)]
        public int intidcanbo { get; set; }

        //[Key, ForeignKey("Vanbanden"), Column("intid")]
        //[Key, Column(Order = 1)]
        public int intidvanban { get; set; }

        //public virtual Canbo Canbos { get; set; }

        //public virtual Vanbanden Vanbandens { get; set; }

        public int? inttrangthai { get; set; }
    }

    public class enumvanbandencanbo
    {
        public enum inttrangthai
        {
            Chuaxem = 0,
            Daxem = 1
        }
    }
}
