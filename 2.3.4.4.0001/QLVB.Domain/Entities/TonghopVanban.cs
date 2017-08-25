using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Entities
{
    public class TonghopVanban
    {
        public int intid { get; set; }
        public DateTime? Ngaygui { get; set; }
        public DateTime? Ngaybatdau { get; set; }
        public DateTime? Ngayketthuc { get; set; }
        public int? VBGiayDen { get; set; }
        public int? VBGiayDi { get; set; }
        public int? VBDientuDen { get; set; }
        public int? VBDientuDi { get; set; }
    }
}
