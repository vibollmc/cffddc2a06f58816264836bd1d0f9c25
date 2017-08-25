using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Entities
{
    public class HosoQuytrinh
    {
        public int intid { get; set; }
        public int intidhoso { get; set; }
        public int intidquytrinh { get; set; }
        public DateTime? strNgayApdung { get; set; }
        // idnode tam ngung xl
        public int? intidFrom { get; set; }
        public int? intSongayNgungXuly { get; set; }
        public int? intidcanbo { get; set; }
        public DateTime? strNgayNgungXuly { get; set; }
        public string strLydoNgungXuly { get; set; }
        public DateTime? strNgayTieptucXuly { get; set; }
        public int inttrangthai { get; set; }

    }
    public class enumHosoQuytrinh
    {
        public enum inttrangthai
        {
            Dangtamngung = 1,
            TieptucXL = 2
        }
    }
}
