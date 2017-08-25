using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.TracuuLuutru
{
    public class CapnhatLuutruViewModel
    {
        public int intid { get; set; }
        public int intidvanban { get; set; }
        public int intloaivanban { get; set; }
        public int? intso { get; set; }
        public string strsophu { get; set; }
        public int? intsoden { get; set; }
        public string strkyhieu { get; set; }
        public DateTime? dtengayvanban { get; set; }
        public string strngayvanban { get; set; }
        public string strtrichyeu { get; set; }
        //=======================
        public int? inthopso { get; set; }
        public int? intdonvibaoquan { get; set; }
        public string strthoihanbaoquan { get; set; }
        public string strnoidung { get; set; }
        public string strnguoicapnhat { get; set; }
        public string strngaycapnhat { get; set; }

    }

}
