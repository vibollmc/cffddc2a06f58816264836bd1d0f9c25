using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Entities
{
    public class AttachVanban
    {
        public int intid { get; set; }

        public int? intloai { get; set; }

        public int? intidvanban { get; set; }

        // ten file luu xuong server
        public string strtenfile { get; set; }

        // ten file dinh kem
        public string strmota { get; set; }

        // so trang cua file dinh kem
        // co the dung de thong ke so trang van ban
        public int? intsotrang { get; set; }

        public DateTime? strngaycapnhat { get; set; }

        public int? inttrangthai { get; set; }

        public int? intidnguoitao { get; set; }

        public DateTime? strngayxoa { get; set; }

        public int? intidnguoixoa { get; set; }
    }

    public class enumAttachVanban
    {
        public enum inttrangthai
        {
            IsActive = 1,
            NotActive = 0
        }

        public enum intloai
        {
            Vanbanden = 1,
            Vanbandi = 2,
            Vanbanduthao = 3
            //Vanbandenmail = 4,
            //EmailContent = 5
        }
    }
}
