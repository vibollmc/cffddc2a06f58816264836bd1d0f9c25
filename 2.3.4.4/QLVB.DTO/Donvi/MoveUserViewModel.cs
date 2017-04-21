using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.DTO.Donvi
{
    public class MoveUserViewModel
    {
        // danh sach cac phong ban
        public IEnumerable<Donvitructhuoc> cacdonvi { get; set; }

        // id cua don vi chuyen toi
        public int iddest { get; set; }
        // danh sach can bo cua don vi chuyen toi
        public IEnumerable<Canbo> canbodest { get; set; }

        // bac cua phong ban
        public int maxLevelDonvi { get; set; }

        // ten don vi user dang truc thuoc
        public string strtendonvi { get; set; }

        // danh sach can bo thuoc don vi dang xet
        public IEnumerable<Canbo> canbosource { get; set; }
    }
}
