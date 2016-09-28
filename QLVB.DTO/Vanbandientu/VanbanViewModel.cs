using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Vanbandientu
{
    public class VanbanViewModel
    {
        public int intsovanban { get; set; }
        public string strkyhieu { get; set; }
        public string strthanhpho { get; set; }
        public string strngayky { get; set; }
        //======================================
        public string strloaivanban { get; set; }
        public string strtenloaivanban { get; set; }
        //=======================================
        public string strtrichyeu { get; set; }
        //=======================================
        public string strnguoiky { get; set; }
        public string strchucvu { get; set; }
        public string strquyenhan { get; set; }
        //=======================================
        public IEnumerable<NoinhanViewModel> ListNoinhan { get; set; }



    }
    public class NoinhanViewModel
    {
        public string strnoinhan { get; set; }
    }
}
