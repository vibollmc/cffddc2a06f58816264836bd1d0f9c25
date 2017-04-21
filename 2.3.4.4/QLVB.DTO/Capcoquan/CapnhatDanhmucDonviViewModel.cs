using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Capcoquan
{
    public class CapnhatDanhmucDonviViewModel
    {
        public string strNgayCapnhatXML { get; set; }
        public List<EmailDonviXMLViewModel> listdonvixml { get; set; }
        public List<EmailDonviDBViewModel> listdonviDB { get; set; }

    }
}
