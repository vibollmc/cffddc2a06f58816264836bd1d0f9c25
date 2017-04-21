using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{

    public class Quyen
    {
        public Int32 intid { get; set; }

        public string strtenquyen { get; set; }

        public string strquyen { get; set; }

        public Nullable<Int32> intorder { get; set; }

        public int? inttrangthai { get; set; }

        public Int32 intidmenu { get; set; }

        //[ForeignKey("intidmenu")]
        //public virtual Menu menu { get; set; }

        //[ForeignKey("intidnhomquyen")]
        //public virtual QuyenNhomQuyen quyennhomquyen { get; set; }
    }
    public class enumQuyen
    {
        public enum inttrangthai
        {
            NotActive = 0,
            IsActive = 1
        }
    }
}