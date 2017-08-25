using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO
{
   public class TonghopVanbanViewModel
    {
        public int Id { get; set; }
        public DateTime? Ngaynhan { get; set; }
        public string Donvi { get; set; }
        public DateTime? Ngaygui { get; set; }
        public DateTime? Ngaytonghopbd { get; set; }
        public DateTime? Ngaytonghopkt { get; set; }
        public int? VBGiayDen { get; set; }
        public int? VBGiayDi { get; set; }
        public int? VBDientuDen { get; set; }
        public int? VBDientuDi { get; set; }
    }
}
