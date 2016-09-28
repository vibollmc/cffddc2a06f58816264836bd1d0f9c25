using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.DTO.Vanbanden
{
    public class SearchVBViewModel
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
        public int? idloaivb { get; set; }
        public IEnumerable<PhanloaiVanban> Loaivanban { get; set; }

        public int? idsovb { get; set; }
        public IEnumerable<SoVanban> Sovanban { get; set; }

        public int? idkhoiph { get; set; }
        public IEnumerable<Khoiphathanh> Khoiphathanh { get; set; }

        public string strngaydencat { get; set; }

        public string xuly { get; set; }

        //===============================================
        // search
        //===============================================
        public int? intsodenbd { get; set; }
        public int? intsodenkt { get; set; }

        public string strngaydenbd { get; set; }
        public string strngaydenkt { get; set; }

        public string strngaykybd { get; set; }
        public string strngaykykt { get; set; }

        public string strsokyhieu { get; set; }

        public string strnguoiky { get; set; }

        public string strnoigui { get; set; }

        public string strtrichyeu { get; set; }

        public string strnguoixuly { get; set; }

        public IEnumerable<CanboViewModel> Nguoixuly { get; set; }

    }
}
