using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.DTO.File;

namespace QLVB.DTO.Vanbandi
{
    public class UploadVBDiViewModel
    {
        public int idvanban { get; set; }

        public IEnumerable<UploadFileViewModel> listfile { get; set; }

    }

}
