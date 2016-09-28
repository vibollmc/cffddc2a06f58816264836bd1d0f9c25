using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Sovb
{
    public class KhoiphathanhViewModel
    {
        public int intid { get; set; }
        public string strtenkhoi { get; set; }

        // khoi phat hanh default cua so van ban den
        public bool IsDefault { get; set; }
    }

    public class ListKhoiphathanhViewModel
    {
        public int idsovb { get; set; }
        public IEnumerable<KhoiphathanhViewModel> Khoiphathanh { get; set; }

    }

}
