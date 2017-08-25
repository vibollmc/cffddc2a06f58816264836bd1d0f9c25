using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QLVB.DTO.Vanbanden
{
    public class ListVanbandenlienquanViewModel
    {
        // van ban nay da duoc chon chua?
        public bool isCheck { get; set; }

        //==============================
        public int intid { get; set; }
        public DateTime? dtengayden { get; set; }
        public int? intsoden { get; set; }
        public string strnoiphathanh { get; set; }
        public string strkyhieu { get; set; }
        public string strtrichyeu { get; set; }
        public string strnoinhan { get; set; }
        public int? inttrangthai { get; set; }
        // truong nhan biet van ban dien tu
        public bool IsVbdt { get; set; }

        public bool IsAttach { get; set; }

        // tinh trang xu ly ho so 
        public int? inttinhtrangxuly { get; set; }

        public int? intidhoso { get; set; }
        // xem user co ghi y kien xu ly vao van ban nay chua
        public bool isykien { get; set; }

        public int? idcanboykien { get; set; }
    }


}
