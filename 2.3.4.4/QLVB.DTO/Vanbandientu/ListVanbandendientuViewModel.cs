using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Vanbandientu
{
    public class ListVanbandendientuViewModel
    {
        public int intid { get; set; }

        public DateTime? dtengayky { get; set; }

        public string strkyhieu { get; set; }

        // ban cu: intidphanloaicongvanden
        public int? intidphanloaivanbanden { get; set; }

        public int? intkhan { get; set; }

        public int? intmat { get; set; }

        public int? intso { get; set; }

        public string strtrichyeu { get; set; }

        public string strnguoiky { get; set; }

        public string strnoigui { get; set; }

        public string strloaivanban { get; set; }

        public int? intguitra { get; set; }

        public int? inttrangthai { get; set; }

        public bool IsAttach { get; set; }

        // d/c mail  trong noidung message
        public string strAddressSend { get; set; }
        // d/c mail thuc te trong mail server
        public string strFromAddress { get; set; }

        public int? intloai { get; set; }

        public string strnoiguivb { get; set; }

        public DateTime? dtengayguivb { get; set; }

        public DateTime? dtengaynhanvb { get; set; }
    }
}
