using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Tinhhinhxuly
{
    public class ListTonghopVBDenViewModel
    {
        public bool IsBack { get; set; }
        public int intPage { get; set; }
        public int idcanbo { get; set; }
        public int iddonvi { get; set; }
        public int intloai { get; set; }
        public string strloaivanban { get; set; }
        public string strngaybd { get; set; }
        public string strngaykt { get; set; }

        public int? idquytrinh { get; set; }
        public int? idloaiquytrinh { get; set; }

        public string NodeId { get; set; }
        public int? idloaingay { get; set; }
        public int? idsovb { get; set; }

    }
}
