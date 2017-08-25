using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Menu
{
    public class HeaderViewModel
    {
        // ten user dang login
        public string UserName { get; set; }
        public string Chucdanh { get; set; }
        public string Nhomquyen { get; set; }

        public IEnumerable<UyquyenUserViewModel> uyquyen { get; set; }

        public string strngay { get; set; }

        public string strImageProfile { get; set; }

    }

    public class UyquyenUserViewModel
    {
        public int intid { get; set; }
        public bool isRealUser { get; set; }
        public string strkyhieu { get; set; }
        public string strhoten { get; set; }
        public int intiddonvi { get; set; }
        public int intnhomquyen { get; set; }
    }

}
