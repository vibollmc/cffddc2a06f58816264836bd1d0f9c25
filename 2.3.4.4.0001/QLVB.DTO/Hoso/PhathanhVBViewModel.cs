using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Hoso
{
    public class PhathanhVBViewModel
    {
        public int idhoso { get; set; }
        public int idvanbanden { get; set; }
        public string strtrichyeu { get; set; }

        public int idnguoiky { get; set; }
        public int idnguoiduyet { get; set; }
        public IEnumerable<CanboViewModel> Nguoiky { get; set; }
        public int intidloaivanban { get; set; }
        public IEnumerable<PhanloaiVanban> PhanloaiVanban { get; set; }

        public DateTime? dteHantraloi { get; set; }

        public string strtraloivb { get; set; }

        public string strlistfile { get; set; }

    }
}
