using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Vanbanden
{
    public class DetailVBDenViewModel
    {
        public int intid { get; set; }
        public string strngayden { get; set; }
        public int intsoden { get; set; }
        public string strkyhieu { get; set; }
        public string strsovanban { get; set; }
        public string strloaivanban { get; set; }
        public string strtenkhoiphathanh { get; set; }
        public string strtencoquanphathanh { get; set; }
        public string strtrichyeu { get; set; }
        public string strngayky { get; set; }
        public string strnguoiky { get; set; }
        public string strvbmat { get; set; }
        public string strvbkhan { get; set; }
        public string strnguoixulybandau { get; set; }
        public string strnguoixulychinh { get; set; }
        public string strhantraloi { get; set; }

        public string strtraloivanban { get; set; }
        public int idvanbandi { get; set; }
        public string strvanbandi { get; set; }

        public int idvanbanphathanh { get; set; }
        public string strvanbanphathanh { get; set; }

        public int intidhosocongviec { get; set; }
        public string strhosovanban { get; set; }

        public bool isattach { get; set; }

        public IEnumerable<QLVB.DTO.File.DownloadFileViewModel> DownloadFiles { get; set; }
    }

}
