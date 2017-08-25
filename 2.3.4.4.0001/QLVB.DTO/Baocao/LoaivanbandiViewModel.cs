using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Baocao
{
    public class LoaivanbandiViewModel
    {
        public string strTungay { get; set; }
        public string strDenngay { get; set; }
        public int? idloaivanban { get; set; }
        public List<int> listidloai { get; set; }

        public string strTenloai { get; set; }
        public string strTendonvi { get; set; }
    }
    public class ReportLoaivanbandiViewModel
    {
        public DataTable Loaivanban { get; set; }
        public LoaivanbandiViewModel Thamso { get; set; }
    }
}
