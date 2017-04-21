using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.File
{
    public class DocxViewerModel
    {
        public int idfile { get; set; }
        public int? intloai { get; set; }

        public string filename { get; set; }

        public string physicalFilePath { get; set; }

        //public string html { get; set; }


    }
    public class DocxToHtml
    {
        public string html { get; set; }
    }
}
