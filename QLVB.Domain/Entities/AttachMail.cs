using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Entities
{
    public class AttachMail
    {
        public int intid { get; set; }
        public int? intloai { get; set; }
        public int? intidmail { get; set; }
        public string strtenfile { get; set; }
        public string strmota { get; set; }
        public DateTime? strngaycapnhat { get; set; }
        public int? inttrangthai { get; set; }
        public int? intidnguoitao { get; set; }
        public DateTime? strngayxoa { get; set; }
        public int? intidnguoixoa { get; set; }
    }
    public class enumAttachMail
    {
        public enum intloai
        {
            Vanbandendientu = 1,
            MailInbox = 2,
            MailOutbox = 3
        }
        public enum inttrangthai
        {
            IsActive = 1,
            NotActive = 0
        }
    }
}
