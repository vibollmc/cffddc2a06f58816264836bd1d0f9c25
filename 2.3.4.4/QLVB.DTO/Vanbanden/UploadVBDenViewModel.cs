using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.DTO.File;

namespace QLVB.DTO.Vanbanden
{
    public class UploadVBDenViewModel
    {
        public int idvanban { get; set; }

        public IEnumerable<UploadFileViewModel> listfile { get; set; }

    }

}
