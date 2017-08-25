using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Vanbandientu
{
    public class DetailVBDenViewModel
    {
        public int intid { get; set; }
        public int? intso { get; set; }
        public string strkyhieu { get; set; }
        public string strngayky { get; set; }
        public string strloaivanban { get; set; }
        public string strtrichyeu { get; set; }
        public string strnguoiky { get; set; }
        public string strvbmat { get; set; }
        public string strvbkhan { get; set; }
        public string strnoinhan { get; set; }

        public string strnoiguivb { get; set; }
        public string stremail { get; set; }

        public string strngaygui { get; set; }
        public string strngaynhan { get; set; }

        public bool isattach { get; set; }

        public IEnumerable<QLVB.DTO.File.DownloadFileViewModel> DownloadFiles { get; set; }

        //=====bo sung ma dinh danh ====================================
        public string strmadinhdanh { get; set; }

        public string strdienthoai { get; set; }
        public string strdiachi { get; set; }

    }

}
