using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Entities
{
    public class AttachHoso
    {
        public int intid { get; set; }
        public int? intloai { get; set; }
        public int? intidhoso { get; set; }
        public int? intidtailieu { get; set; }
        public string strtenfile { get; set; }
        public string strmota { get; set; }
        public DateTime? strngaycapnhat { get; set; }
        public int? inttrangthai { get; set; }
        public int? intidnguoitao { get; set; }
        public DateTime? strngayxoa { get; set; }
        public int? intidnguoixoa { get; set; }
        public int? intphathanh { get; set; }
    }

    public class enumAttachHoso
    {
        public enum inttrangthai
        {
            IsActive = 1,
            NotActive = 0
        }

        public enum intloai
        {
            Ykien = 1,
            Phieutrinh = 2,
            VBLQ = 3
        }
        public enum intphathanh
        {
            Khong = 0,
            Co = 1
        }
    }
}
