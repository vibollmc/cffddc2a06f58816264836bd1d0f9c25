using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Edxml
{
    public class AgencyViewModel
    {
        public string Id { get; set; }
        public string Pid { get; set; }
        public string Madinhdanh { get; set; }
        public string strtendonvi { get; set; }

        public bool IsSend { get; set; }
    }

    public class DonviEdxmlViewModel
    {
        public int idvanban { get; set; }

        public IEnumerable<AgencyViewModel> listdonvi { get; set; }
    }

}
