using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace QLVB.Domain.Entities
{
//    [Table("VanbandiCanbo")]
    public class VanbandiCanbo
    {
        //[Key, ForeignKey("Canbo"), Column("intid")]
        //[Key, Column(Order = 0)]
        public int intidcanbo { get; set; }

        //[Key, ForeignKey("Vanbandi"), Column("intid")]
        //[Key, Column(Order = 1)]
        public int intidvanban { get; set; }

        //public virtual Canbo Canbos { get; set; }

        //public virtual Vanbandi Vanbandis { get; set; }
    }
}
