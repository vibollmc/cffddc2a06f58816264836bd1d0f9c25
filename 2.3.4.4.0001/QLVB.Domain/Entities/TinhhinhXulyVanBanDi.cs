using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Entities
{
    public class TinhhinhXulyVanBanDi
    {
        public int intid { get; set; }

        public int? intidguivanban { get; set; }

        public DateTime? strngayxuly { get; set; }

        public string strnguoixuly { get; set; }

        public string strphongban { get; set; }

        public string strdiengiai { get; set; }
    }
}
