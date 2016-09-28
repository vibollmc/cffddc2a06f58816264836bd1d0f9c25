using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Entities
{
    public class Baocao
    {
        public int intid { get; set; }

        public string strten { get; set; }

        public int? intloai { get; set; }

        public int? intorder { get; set; }

        public int? inttrangthai { get; set; }
    }
    public class enumBaocao
    {
        public enum inttrangthai
        {
            IsActive = 1,
            NotActive = 0
        }

        public enum intloai
        {
            Vanbanden = 1,
            Vanbandi = 2
        }
    }
}
