using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Tinhhinhxuly
{
    public class D3DashboardXLVBDenViewModel
    {
        public int id { get; set; }
        public string ten { get; set; }
        public ThongkeXLVBDenVM thongke { get; set; }

    }
    public class ThongkeXLVBDenVM
    {
        public int luuhs { get; set; }
        public int daxuly { get; set; }
        public int dangxuly { get; set; }
        public int trinhky { get; set; }
        public int quahan { get; set; }
    }
}
