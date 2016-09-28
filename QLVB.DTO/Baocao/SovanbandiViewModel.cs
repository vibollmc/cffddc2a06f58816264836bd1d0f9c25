using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Baocao
{
    public class SovanbandiViewModel
    {
        public string strTungay { get; set; }
        public string strDenngay { get; set; }
        public int idsovanban { get; set; }
        public string listidso { get; set; }

        public string strTenso { get; set; }
        public string strTendonvi { get; set; }
    }
    public class ReportSovanbandiViewModel
    {
        public DataTable Sovanban { get; set; }
        public SovanbandiViewModel Thamso { get; set; }
    }
}
