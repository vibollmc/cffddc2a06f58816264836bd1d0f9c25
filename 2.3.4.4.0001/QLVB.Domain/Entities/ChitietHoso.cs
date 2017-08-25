using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Entities
{
    public class ChitietHoso
    {
        public int intid { get; set; }

        // ho so cong viec
        public int? intidhosocongviec { get; set; }

        // can bo xu ly
        public int? intidcanbo { get; set; }

        // vai tro xu ly
        public int? intvaitro { get; set; }

        // thao tac cua nguoi su dung        
        public string strthaotac { get; set; }

        // can bo them nguoi xu ly
        public int? intnguoitao { get; set; }

        // thoi gian them nguoi xu ly
        public DateTime? strngaytao { get; set; }
    }

}
