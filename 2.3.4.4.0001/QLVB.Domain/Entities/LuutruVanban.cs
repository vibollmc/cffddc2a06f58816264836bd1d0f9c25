using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Entities
{
    public class LuutruVanban
    {
        public int intid { get; set; }
        public int intidvanban { get; set; }
        public int? intloaivanban { get; set; }
        public int? inthopso { get; set; }
        public int? intdonvibaoquan { get; set; }
        public string strthoihanbaoquan { get; set; }
        public string strnoidung { get; set; }
        public int? intidnguoicapnhat { get; set; }
        public DateTime? strngaycapnhat { get; set; }
    }

    public class enumLuutruVanban
    {
        public enum intloaivanban
        {
            vanbanden = 1,
            vanbandi = 2
        }
    }
}
