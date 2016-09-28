using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Tinhhinhxuly
{
    public class ListdonviViewModel
    {
        public int iddonvi { get; set; }
        public IEnumerable<Donvitructhuoc> Donvi { get; set; }

        public int maxLevelDonvi { get; set; }

        public DateTime dteNgaybd { get; set; }

        public DateTime dteNgaykt { get; set; }

        public int idloaingay { get; set; }
        public int idsovb { get; set; }
        public IEnumerable<SoVanban> Sovanban { get; set; }
    }
    public enum enumidloaingay
    {
        Vanbanden = 1,
        Thoihanxuly = 2
    }
}
