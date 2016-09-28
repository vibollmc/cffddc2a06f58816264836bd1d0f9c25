using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.DTO;
using QLVB.Domain.Entities;

namespace QLVB.DTO.Vanbanden
{
    public class ListUserCapquyenxemViewModel
    {
        public int idvanban { get; set; }

        //====================================================
        public IEnumerable<Donvitructhuoc> donvi { get; set; }
        public IEnumerable<UserXemVBDenModel> canbo { get; set; }
        // cho biet co tong cong may cap phong ban trong don vi
        public int? maxLevelDonvi { get; set; }
        //====================================================
        public int? intpublic { get; set; }
        public string strpublic { get; set; }
    }

    public class UserXemVBDenModel
    {
        public int intid { get; set; }
        // neu da co qyen xem thi ischeck = true
        public bool IsCheck { get; set; }

        public string strkyhieu { get; set; }
        public string strhoten { get; set; }
        public int intiddonvi { get; set; }
    }
}
