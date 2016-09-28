using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Quytrinh
{
    public class QuytrinhXulyViewModels
    {
        public int idquytrinh { get; set; }

        public string strtenquytrinh { get; set; }

        public int intTongSongayxuly { get; set; }
        public DateTime dteNgayApdung { get; set; }

        public IEnumerable<CongviecXulyViewModel> congviecs { get; set; }
        public IEnumerable<ThutuXulyViewModel> BuocXuly { get; set; }

    }

    public class CongviecXulyViewModel
    {
        public int idcongviec { get; set; }
        public string nodeid { get; set; }
        public string strtencongviec { get; set; }
        public int? intLeft { get; set; }
        public int? intTop { get; set; }

        public int? intidDonvi { get; set; }
        public int? intidCanbo { get; set; }
        public int? intVaitro { get; set; }
        public int? intSoNgay { get; set; }
        public int? intNext { get; set; }
        public int? Hoanthanh { get; set; }
        public int? intXulyDongthoi { get; set; }


    }

    public class ThutuXulyViewModel
    {
        public int idFrom { get; set; }
        public int idTo { get; set; }
        public string label { get; set; }
    }
}
