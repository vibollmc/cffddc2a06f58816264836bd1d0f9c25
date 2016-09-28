using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace QLVB.DTO.Baocao
{
    public class LoaivanbandenViewModel
    {
        public string strTungay { get; set; }
        public string strDenngay { get; set; }
        public int idloaivanban { get; set; }
        public string listidloai { get; set; }

        public string strTenloai { get; set; }
        public string strTendonvi { get; set; }

    }
    public class ReportLoaivanbandenViewModel
    {
        public DataTable Loaivanban { get; set; }
        public LoaivanbandenViewModel Thamso { get; set; }
    }
}
