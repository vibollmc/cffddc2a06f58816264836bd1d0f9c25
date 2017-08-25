using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Entities
{
    /// <summary>
    /// thong tin tong hop cua can bo: vanbanden/di, hoso can xu ly
    /// </summary>
    public class TonghopCanbo
    {
        public int intid { get; set; }
        public int intidcanbo { get; set; }
        public int intidtailieu { get; set; }
        public int intloaitailieu { get; set; }
        public int? intloai { get; set; }
        public DateTime? strngaytao { get; set; }
        public int? intidnguoitao { get; set; }
        public int? inttrangthai { get; set; }
        public DateTime? strngayxem { get; set; }

    }
    public class enumTonghopCanbo
    {
        public enum intloaitailieu
        {
            Vanbanden = 1,
            Vanbandi = 2,
            HosoCV = 3,
            YKCD = 4

        }
        public enum inttrangthai
        {
            Chuaxem = 0,
            Daxem = 1
        }
        public enum intloai
        {
            Vanbanmoi = 1,
            Debiet = 2,
            HosoXLVBDen = 3,
            Ykienxuly = 4,
            Phieutrinh = 5,
            Trinhky = 6,
            YKCD = 7
        }
    }
}
