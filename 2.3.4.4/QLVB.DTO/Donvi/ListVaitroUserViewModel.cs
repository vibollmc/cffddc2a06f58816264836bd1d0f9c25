using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Donvi
{
    public class ListVaitroUserViewModel
    {
        IEnumerable<VaitroUserViewModel> vaitroUser { get; set; }

    }

    public class VaitroUserViewModel
    {
        public int intid { get; set; }

        public bool IsXulybandau { get; set; }
        // default nguoi xu ly ban dau
        public bool IsDefaultXLBD { get; set; }
        public string strkyhieu { get; set; }
        public string strhoten { get; set; }
        public int intiddonvi { get; set; }
    }
}
