using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{

    public class Hosoykienxuly
    {

        public int intid { get; set; }

        public int? intiddoituongxuly { get; set; }

        public DateTime? strthoigian { get; set; }

        public string strykien { get; set; }

        public int? inttrangthai { get; set; }

        public int? intidnguoilap { get; set; }
    }

    public class enumHosoykienxuly
    {
        public enum inttrangthai
        {
            DaXoaYkien = 0,
            Dachoykien = 1,
            //danh dau dang cho y kien de attach file
            DangchoYkien = 2
        }
    }
}