using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace QLVB.DTO.Baocao
{
    public class ReportSoVanbandenViewModel
    {
        public DataTable Sovanban { get; set; }
        public SoVanbandenViewModel Thamso { get; set; }
    }

    public class SoVanbandenViewModel
    {
        public string strTungay { get; set; }
        public string strDenngay { get; set; }
        public int? idsovanban { get; set; }
        public List<int> listidso { get; set; }

        public string strTenso { get; set; }

        public string strTendonvi { get; set; }

        public bool IsXuly { get; set; }
        public int iddonvi { get; set; }

        public string strtenphong { get; set; }
    }
}
