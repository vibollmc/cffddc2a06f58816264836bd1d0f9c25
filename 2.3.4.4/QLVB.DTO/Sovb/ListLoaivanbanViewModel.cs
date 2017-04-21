using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Sovb
{
    public class ListLoaivanbanViewModel
    {
        public int idsovb { get; set; }
        public IEnumerable<LoaivanbanViewModel> Loaivanban { get; set; }
    }

    public class LoaivanbanViewModel
    {
        public int intid { get; set; }
        public string strtenloaivanban { get; set; }
        public bool IsDefault { get; set; }
    }
}
