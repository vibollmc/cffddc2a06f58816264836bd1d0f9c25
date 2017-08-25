using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Entities
{
    public class LogGuiTonghopVB
    {
        public int intid { get; set; }
        public DateTime Ngaygui { get; set; }
        public int intTrangthai { get; set; }
    }

    public class enumLogGuiTonghopVB
    {
        public enum intTrangthai
        {
            Khong = 0,
            Dagui = 1
        }
    }
}
