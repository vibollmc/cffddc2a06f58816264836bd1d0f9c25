using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{

    public class MailOutbox
    {

        public int intid { get; set; }
        public string strsubject { get; set; }
        public string strcontent { get; set; }
        public string straddress { get; set; }
        public DateTime? strngaygui { get; set; }
        public int? intloai { get; set; }
        public int? inttrangthai { get; set; }
        public int? intidnguoigui { get; set; }

        public int? intidguivanban { get; set; }
    }
    public class enumMailOutbox
    {
        public enum inttrangthai
        {
            IsActive = 1,
            NotActive = 0
        }
        public enum intloai
        {
            // tieu chuan cu TTTH
            TTTH = 1,
            // tieu chuan moi cua BTTTT
            BTTTT_512 = 2,
            BTTTT_2803 = 3,
            Khac = 4,
            Tructinh = 5,
            Chinhphu = 6
        }
    }

}
