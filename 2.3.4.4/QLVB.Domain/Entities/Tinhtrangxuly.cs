using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QLVB.Domain.Entities
{
    //[Table("vTinhtrangxuly")]
    public class Tinhtrangxuly
    {
        //  [Key]
        public int intidvanban { get; set; }

        public int intidcanbo { get; set; }

        public int? intidsovanban { get; set; }

        public DateTime? strngayden { get; set; }

        public int? intidphanloaivanbanden { get; set; }

        public int? intidlinhvuc { get; set; }

        public int? inttrangthai { get; set; }

        public DateTime? strngaymohoso { get; set; }

        public DateTime? strthoihanxuly { get; set; }

        public int? intluuhoso { get; set; }

        public DateTime? strngayketthuc { get; set; }

        // loai hosocongviec: vbden/quytrinh
        public int intloaihosocongviec { get; set; }

    }

    public class enumtinhtrangxuly
    {
        public enum intloai
        {
            LuuHS = 1,
            DaXL = 2,
            DangXL = 3,
            QuahanXL = 4,
            Trinhky = 5,

            TongCanbo = 6,

            TongLuuHS = 11,
            TongDaXL = 12,
            TongDangXL = 13,
            TongQuahanXL = 14,
            TongTrinhKy = 15
        }
    }

}
