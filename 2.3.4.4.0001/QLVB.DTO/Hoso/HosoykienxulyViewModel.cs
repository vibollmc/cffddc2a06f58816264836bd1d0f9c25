using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Hoso
{
    /// <summary>
    /// cac y kien xu ly trong ho so cong viec
    /// </summary>
    public class HosoykienxulyViewModel
    {
        public int idykien { get; set; }
        public string strtendonvi { get; set; }
        public string strtencanbo { get; set; }
        public string strkykien { get; set; }
        public DateTime? dtethoigian { get; set; }
        public string strthoigian { get; set; }

        public IEnumerable<FileAttachViewModel> files { get; set; }

        public IEnumerable<QLVB.DTO.File.DownloadFileViewModel> DownloadFiles { get; set; }
    }
}
