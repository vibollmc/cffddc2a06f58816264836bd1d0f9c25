using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Vanbandientu
{
    public class ListDonviguiViewModels
    {
        public int intid { get; set; }
        public string strtendonvi { get; set; }
        public DateTime? dtengaygui { get; set; }
        public DateTime? dtengaynhan { get; set; }
        public DateTime? dtengayxuly { get; set; }
        public DateTime? dtengayhoanthanh { get; set; }
        public DateTime? dtengayphancong { get; set; }
        public int? intloaigui { get; set; }
        public string strloaigui { get; set; }
    }
}
