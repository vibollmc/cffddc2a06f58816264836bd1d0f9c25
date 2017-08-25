using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Hethong
{
    public class BackupViewModel
    {
        public string strPath { get; set; }
        public string strDatabase { get; set; }
        public string strFileName { get; set; }

        public bool IsCheckPath { get; set; }
    }
}
