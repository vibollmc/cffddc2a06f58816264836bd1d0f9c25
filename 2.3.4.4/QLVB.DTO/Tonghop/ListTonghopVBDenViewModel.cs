using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Tonghop
{
    public class ListTonghopVBDenViewModel
    {
        public int intid { get; set; }
        public DateTime? dtengayden { get; set; }
        public int? intsoden { get; set; }
        public string strnoiphathanh { get; set; }
        public string strkyhieu { get; set; }
        public string strtrichyeu { get; set; }
        public string strnoinhan { get; set; }
        public int? inttrangthai { get; set; }
        public bool IsAttach { get; set; }
    }
}
