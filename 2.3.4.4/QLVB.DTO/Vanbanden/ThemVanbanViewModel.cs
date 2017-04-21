using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Vanbanden
{
    public class ThemVanbanViewModel
    {
        public bool IsSave { get; set; }

        public IEnumerable<PhanloaiTruong> PhanloaiTruong { get; set; }

        public IEnumerable<PhanloaiVanban> PhanloaiVanban { get; set; }

        public int intidloaivanban { get; set; }

        public QLVB.Domain.Entities.Vanbanden Vanbanden { get; set; }

        public string strmota_traloivanban { get; set; }

        public bool IsQPPL { get; set; }

        public IEnumerable<SoVanban> Sovanban { get; set; }

        public int intidsovanban { get; set; }

        public IEnumerable<Khoiphathanh> Khoiphathanh { get; set; }

        public int intidkhoiph { get; set; }

        public IEnumerable<Tochucdoitac> Tochucdoitac { get; set; }

        public IEnumerable<QLVB.Domain.Entities.Linhvuc> Linhvuc { get; set; }

        public IEnumerable<Tinhchatvanban> Vanbankhan { get; set; }

        public IEnumerable<Tinhchatvanban> Vanbanmat { get; set; }

        public IEnumerable<CanboViewModel> Nguoiduyet { get; set; }
        public int idnguoiduyetDefault { get; set; }

        public IEnumerable<Diachiluutru> Diachiluutru { get; set; }

        //public int intsoden { get; set; }

        public IEnumerable<CanboViewModel> Nguoixuly { get; set; }

        public int? idmail { get; set; }
    }

    /// <summary>
    /// cac truong tra ve khi thay doi so van ban trong form them moi van ban den
    /// </summary>
    public class AjaxSovanban
    {
        public int intsoden { get; set; }
        public int idkhoiph { get; set; }
    }
}
