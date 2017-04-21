using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Hoso
{
    /// <summary>
    /// thong tin luan chuyen van ban trong ho so congviec
    /// </summary>
    public class LuanchuyenvanbanViewModel
    {
        public string strtendonvi { get; set; }
        public string strtencanbo { get; set; }
        public int? intvaitro { get; set; }
        public string strtennguoitao { get; set; }
        public DateTime? dtengaytao { get; set; }
        public string strngaytao { get; set; }
        public int? intvaitrocu { get; set; }
        public string strthaotac { get; set; }

        public string strvaitro { get; set; }

        public DateTime? dtengaychuyen { get; set; }
        public string strngaychuyen { get; set; }

        public string strtennguoichuyen { get; set; }
    }
}
