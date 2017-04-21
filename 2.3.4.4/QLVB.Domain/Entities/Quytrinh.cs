using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{
    public class Quytrinh
    {
        public int intid { get; set; }
        public int intidloai { get; set; }
        public string strten { get; set; }
        public int? numberOfElements { get; set; }
        public int inttrangthai { get; set; }

        // thieu so ngay xu ly cua 1 quy trinh
        public int intSoNgay { get; set; }

        public DateTime? strNgayApdung { get; set; }
    }

    public class enumQuytrinh
    {
        public enum inttrangthai
        {
            NotActive = 0,
            IsActive = 1
        }
    }
}
