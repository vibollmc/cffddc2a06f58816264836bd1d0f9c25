using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Hoso
{
    /// <summary>
    /// thong tin chi tiet van ban
    /// </summary>
    public class DetailHosoViewModel
    {
        public int idhosocongviec { get; set; }
        public string strnguoihoanthanh { get; set; }
        public int? intloai { get; set; }
        public string strsohieuht { get; set; }
        public string strtieude { get; set; }
        public string strtrangthai { get; set; }
        public string strthoihanxuly { get; set; }
        public string strngaymohoso { get; set; }
        public string strngayketthuc { get; set; }
        public string strketqua { get; set; }
        public string strnoidung { get; set; }
        public string strtendonvi { get; set; }
        public string strtenlinhvuc { get; set; }
        public string strmucquantrong { get; set; }

        public int idvanbanden { get; set; }
        public string strsovanbanden { get; set; }
        public string strtrichyeuvanbanden { get; set; }

        public string strlanhdaogiaoviec { get; set; }
        public string strxulychinh { get; set; }
        public string strlanhdaophutrach { get; set; }

        public string strphoihopxuly { get; set; }

        public IEnumerable<LuanchuyenvanbanViewModel> doituongxuly { get; set; }

        public IEnumerable<HosoykienxulyViewModel> hosoykien { get; set; }

        public IEnumerable<PhieutrinhViewModel> phieutrinh { get; set; }

    }
}
