using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Vanbandi
{
    public class DetailVBDiViewModel
    {
        public int intid { get; set; }
        public string strnguoisoan { get; set; }
        public string strdonvisoan { get; set; }
        public int intso { get; set; }
        public string strsophu { get; set; }
        public string strkyhieu { get; set; }
        public string strngayky { get; set; }
        public string strsovanban { get; set; }
        public string strloaivanban { get; set; }
        public string strtrichyeu { get; set; }
        public string strnguoiduyet { get; set; }
        public string strnguoiky { get; set; }
        public string strvbmat { get; set; }
        public string strvbkhan { get; set; }
        public string strnguoixulybandau { get; set; }
        public string strnguoixulychinh { get; set; }
        public string strhantraloi { get; set; }
        public string strnoinhan { get; set; }
        public string strsoban { get; set; }
        public string strsoto { get; set; }

        public string strtraloivanban { get; set; }
        public int idvanbanden { get; set; }
        public string strvanbanden { get; set; }
        public int intidhosocongviec { get; set; }
        public string strhosovanban { get; set; }

        public bool isattach { get; set; }

        public IEnumerable<VanbanhoibaoViewModel> Vanbanhoibaos { get; set; }

        public IEnumerable<QLVB.DTO.File.DownloadFileViewModel> DownloadFiles { get; set; }

    }

    public class VanbanhoibaoViewModel
    {
        public int idvanban { get; set; }
        public int intsoden { get; set; }
        public string strngayden { get; set; }
        public DateTime? dtengayden { get; set; }
        public string so_kyhieu { get; set; }
        public string strngayky { get; set; }
        public DateTime? dtengayky { get; set; }
        public string strichyeu { get; set; }
        public string Donvigui { get; set; }
    }
}
