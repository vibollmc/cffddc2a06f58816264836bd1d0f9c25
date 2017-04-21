using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Entities
{
    public class ChitietVanbanden
    {
        public int intid { get; set; }

        public int intidvanban { get; set; }

        public int intloai { get; set; }

        public int intidcanbo { get; set; }

        public string strthaotac { get; set; }

        public int? intvaitro { get; set; }

        public int? intnguoitao { get; set; }

        public DateTime? strngaytao { get; set; }

        public int? intvaitrocu { get; set; }

        public int? intnguoichuyen { get; set; }

        public DateTime? strngaychuyen { get; set; }
    }

    public class enumChitietVanbanden
    {
        public enum intvaitro
        {
            Capquyenxem = 1,
            Huyquyenxem = 2,
            CapquyenPublic = 11,
            HuyquyenPublic = 22,
            CapnhatVBDT = 3,
            Dinhkemfile = 4,
            Xoafile = 5,
            CapnhatVBGiay = 6

        }
    }
}
