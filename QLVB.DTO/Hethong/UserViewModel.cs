using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Hethong
{
    public class UserViewModel
    {
        public int intid { get; set; }
        public string strhoten { get; set; }
        public string strkyhieu { get; set; }

        public string strchucvu { get; set; }
        public string strphongban { get; set; }

        public int? idnhomquyen { get; set; }
    }
}
