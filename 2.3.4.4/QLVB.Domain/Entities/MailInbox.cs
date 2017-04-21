using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{

    public class MailInbox
    {
        public int intid { get; set; }
        public string strsubject { get; set; }
        public string strcontent { get; set; }
        public string strheader { get; set; }
        public string straddress { get; set; }
        public DateTime? strngaynhan { get; set; }
        public int? intloai { get; set; }
        public int? inttrangthai { get; set; }
        public int? intidnguoinhan { get; set; }
        public DateTime? strngayxoa { get; set; }
        public int? intidnguoixoa { get; set; }

    }
    public class enumMailInbox
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
            Log = 5
        }
    }
}

