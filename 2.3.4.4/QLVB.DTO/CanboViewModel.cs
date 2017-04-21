using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO
{
    /// <summary>
    /// thong tin dung chung cho user
    /// </summary>
    public class CanboViewModel
    {
        public int intid { get; set; }

        public int? iddonvi { get; set; }

        public string strmacanbo { get; set; }

        public string strkyhieu { get; set; }

        public string strhoten { get; set; }

        public string strdienthoai { get; set; }

        public string stremail { get; set; }

        public DateTime? strngaysinh { get; set; }

        public int? intchucvu { get; set; }

        public int? intnhomquyen { get; set; }

        public bool IsKyVB { get; set; }

        public bool IsNguoiXL { get; set; }
    }
}
