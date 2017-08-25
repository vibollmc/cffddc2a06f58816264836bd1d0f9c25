using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{
    
    public class MailContent
    {
        public int intid { get; set; }

        public int intloai { get; set; }

        public string strsubject { get; set; }

        public string strcontent { get; set; }

        public DateTime? strngay { get; set; }

        public int? inttrangthai { get; set; }
    }

    public class enumMailContent
    {
        public enum intloai
        {
            Vanbanden = 1,
            Vanbandi = 2
        }

        public enum inttrangthai
        {
            Vanbandientu = 1,
            Xacthucvanban = 2,
            Khac = 3
        }
    }
}