using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Hoso
{
    public class PhoihopXulyViewModel
    {

        public int idhoso { get; set; }

        //====================================================
        public IEnumerable<Donvitructhuoc> donvi { get; set; }
        public IEnumerable<UserPhoihopxuly> canbo { get; set; }
        // cho biet co tong cong may cap phong ban trong don vi
        public int? maxLevelDonvi { get; set; }
        //====================================================

    }

    public class UserPhoihopxuly
    {
        public int intid { get; set; }

        // neu da tham gia xu ly thi ischeck = true
        public bool IsCheck { get; set; }

        // neu la nguoi phoi hop xu ly thi IsPhoihopxuly = true
        // de chi cho phep them/bot nguoi phoi hop xu ly thoi
        public bool IsPhoihopxuly { get; set; }
        public string strkyhieu { get; set; }
        public string strhoten { get; set; }
        public int intiddonvi { get; set; }
    }
}
