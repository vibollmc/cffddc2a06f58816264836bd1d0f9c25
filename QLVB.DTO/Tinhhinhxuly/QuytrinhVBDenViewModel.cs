using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Tinhhinhxuly
{
    public class QuytrinhVBDenViewModel
    {
        public int intid { get; set; }
        public int idcanbo { get; set; }
        public string strkyhieu { get; set; }
        public string strhoten { get; set; }

        public int idvanban { get; set; }
        public int idhoso { get; set; }
        public int idquytrinh { get; set; }
        public string strtenquytrinh { get; set; }

        public int intVBDaXuly_Dunghan { get; set; }
        public int intVBDaXuly_Trehan { get; set; }
        public int intVBDangXuly { get; set; }
        public int intVBQuahan { get; set; }
        //public int intTrinhky { get; set; }
        public int intTong { get; set; }
    }
}
