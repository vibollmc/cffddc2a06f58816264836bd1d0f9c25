using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Vanbandientu
{
    public class SearchVBDenDientuViewModel
    {
        //===============================================
        // status
        //===============================================
        public bool isSearch { get; set; }
        public bool isBack { get; set; }

        public int intPage { get; set; }
        //===============================================
        // category
        //===============================================
        public int? inttinhtrangcat { get; set; }
        public string strngaynhancat { get; set; }
        public string strngaykycat { get; set; }

        //===============================================
        // search
        //===============================================
        public int? intsodenbd { get; set; }
        public int? intsodenkt { get; set; }

        public string strngaykybd { get; set; }
        public string strngaykykt { get; set; }

        public string strngayguibd { get; set; }
        public string strngayguikt { get; set; }

        public string strngaynhanbd { get; set; }
        public string strngaynhankt { get; set; }

        public string strkyhieu { get; set; }
        public string strnoigui { get; set; }
        public string strtrichyeu { get; set; }

        public string truclienthong { get; set; }
        public string strmadinhdanh { get; set; }

    }
}
