using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Vanbandientu.EdXML
{
    public class edXMLOtherInfo
    {
        public int Priority { get; set; }
    }
    public class enumEdXMLOtherInfo
    {
        public enum Priority
        {
            Thuong = 0,
            Khan = 1,
            ThuongKhan = 2,
            Hoatoc = 3,
            HoatocHengio = 4
        }
    }
}
