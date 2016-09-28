using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{
    //[Table("QuyenNhomQuyen")]
    public class QuyenNhomQuyen
    {
        //[Key, ForeignKey("NhomQuyen"), Column(Order = 0)]
        //[Key, Column(Order = 0)]
        public Int32 intidnhomquyen { get; set; }

        //[Key, ForeignKey("Quyen"), Column(Order = 1)]
        //[Key, Column(Order = 1)]
        public Int32 intidquyen { get; set; }

        //public virtual NhomQuyen NhomQuyen { get; set; }

        //public virtual Quyen Quyen { get; set; }
    }
}