using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.DTO.Vanbandi
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


        public string strngaykycat { get; set; }

        public string strvbphathanh { get; set; }

        public string hoibao { get; set; }

        //===============================================
        // search
        //===============================================
        public int? intsobd { get; set; }
        public int? intsokt { get; set; }

        public string strngaykybd { get; set; }
        public string strngaykykt { get; set; }

        public string strkyhieu { get; set; }

        public string strnguoiky { get; set; }

        public IEnumerable<CanboViewModel> Nguoixuly { get; set; }

        public string strnguoisoan { get; set; }

        public string strnguoiduyet { get; set; }

        public string strnoinhan { get; set; }

        public string strtrichyeu { get; set; }

        public string strhantraloi { get; set; }

        public string strdonvisoan { get; set; }

        public int? idkhan { get; set; }
        public IEnumerable<Tinhchatvanban> Vanbankhan { get; set; }

        public int? idmat { get; set; }
        public IEnumerable<Tinhchatvanban> Vanbanmat { get; set; }

    }
}
