using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Entities
{
    public class TinhTrangQuytrinh
    {
        public int intidvanban { get; set; }

        public int intidhoso { get; set; }

        public int intidquytrinh { get; set; }

        //public int intidcanbo { get; set; }

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
    public class enumtinhtrangquytrinh
    {
        public enum intloai
        {
            DaXL_Dunghan = 1,
            DaXL_Trehan = 2,
            DangXL = 3,
            QuahanXL = 4

        }
    }
}
