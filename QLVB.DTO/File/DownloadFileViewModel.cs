using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.File
{
    public class DownloadFileViewModel
    {
        public int intid { get; set; }
        public int intloai { get; set; }
        public string strtenfile { get; set; }

        public string strfiletypeimages { get; set; }

        public string fileExt { get; set; }

        public string fileIcon { get; set; }

        //trong hscv: danh dau vb da duoc phat hanh
        public bool IsPhathanh { get; set; }
    }
    public class enumDownloadFileViewModel
    {
        public enum intloai
        {
            Vanbanden = 1,
            Vanbandi = 2,
            VBDT = 3,
            Vanbanduthao = 4,
            HSCV_Ykien = 5
        }
    }
}
