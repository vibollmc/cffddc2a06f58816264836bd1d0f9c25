using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Vanbandientu
{
    public class SearchVBDiDientuViewModel
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
        public string strngaynhancat { get; set; }
        public string strngaykycat { get; set; }

        public string strDonviguicat { get; set; }
        //===============================================
        // search
        //===============================================
        public int? intsodibd { get; set; }
        public int? intsodikt { get; set; }

        public string strngaykybd { get; set; }
        public string strngaykykt { get; set; }

        public string strngayguibd { get; set; }
        public string strngayguikt { get; set; }

        public string strngaynhanbd { get; set; }
        public string strngaynhankt { get; set; }

        public string strkyhieu { get; set; }
        public string strnoigui { get; set; }
        public string strtrichyeu { get; set; }

    }
}
