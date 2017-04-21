using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Vanbandi
{
    public class ThemvanbanViewModel
    {
        public bool IsSave { get; set; }

        public IEnumerable<PhanloaiTruong> PhanloaiTruong { get; set; }

        public IEnumerable<PhanloaiVanban> PhanloaiVanban { get; set; }

        public int intidloaivanban { get; set; }

        public QLVB.Domain.Entities.Vanbandi Vanbandi { get; set; }

        public string strmota_traloivanban { get; set; }

        public bool IsQPPL { get; set; }


        public IEnumerable<SoVanban> Sovanban { get; set; }

        public int intidsovanban { get; set; }

        //public IEnumerable<Khoiphathanh> KhoiphathanhModel { get; set; }
        //public int intidkhoiph { get; set; }

        public IEnumerable<Tochucdoitac> Tochucdoitac { get; set; }

        public string jsTochuc { get; set; }

        public IEnumerable<DonviguivanbanViewModel> Donviguivb { get; set; }

        public IEnumerable<QLVB.Domain.Entities.Linhvuc> Linhvuc { get; set; }

        public IEnumerable<Tinhchatvanban> Vanbankhan { get; set; }

        public IEnumerable<Tinhchatvanban> Vanbanmat { get; set; }

        public IEnumerable<CanboViewModel> Nguoiduyet { get; set; }

        public IEnumerable<Diachiluutru> Diachiluutru { get; set; }

        public IEnumerable<CanboViewModel> Nguoiky { get; set; }

        public IEnumerable<CanboViewModel> Nguoisoan { get; set; }

    }

    public class DonviguivanbanViewModel
    {
        public string strtendonvi { get; set; }
    }

    /// <summary>
    /// cac truong tra ve khi thay doi so van ban trong form them moi van ban di
    /// </summary>
    public class AjaxSovanban
    {
        public int intso { get; set; }

    }

    public class TraloiVanbanViewModel
    {
        public enum intTrangthai {            
            Khong = 0 ,
            Co = 1,
            KhongthayVBDen = 2
        }
        public int intloai { get; set; }
        public string strSoKyhieu { get; set; }
        public string strNgay { get; set; }
        public string strNguoisoan { get; set; }
    }

}
