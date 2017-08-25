using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Hoso
{
    public class ThongtinHosoViewModel
    {
        public int idhosocongviec { get; set; }
        public int intloai { get; set; }
        public string strsohieuht { get; set; }
        public string strloaihoso { get; set; }
        public string strmucquantrong { get; set; }
        public string strtenlinhvuc { get; set; }
        public string strngaymohoso { get; set; }
        public string strthoihanxuly { get; set; }
        public string strngayketthuc { get; set; }
        public string strtieude { get; set; }
        public string strnoidung { get; set; }

        public int idvanban { get; set; }
        public string strtrichyeuvanban { get; set; }

        public string strtrangthai { get; set; }

        // co van ban lien quan khong
        public bool isVBDenLQ { get; set; }
        public bool isVBDiLQ { get; set; }
        public bool isVBAttachLQ { get; set; }

    }
}
