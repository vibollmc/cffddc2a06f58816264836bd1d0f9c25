using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.File
{
    /// <summary>
    /// cac thong tin chung ve file attach
    /// </summary>
    public class CommonFileViewer
    {
        public int idfile { get; set; }
        public int? intloai { get; set; }

        public string filename { get; set; }

        public string physicalFilePath { get; set; }

        public DateTime dteNgaycapnhat { get; set; }

        public string strLoaiFile { get; set; }
    }
}
