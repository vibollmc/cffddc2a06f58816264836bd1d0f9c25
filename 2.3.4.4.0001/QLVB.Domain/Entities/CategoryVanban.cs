using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Entities
{
    public class CategoryVanban
    {
        public int intid { get; set; }

        public int? intMa { get; set; }

        public string strten { get; set; }

        public int? intloai { get; set; }

        public int? intbac { get; set; }

        public int? intorder { get; set; }

        public int? inttrangthai { get; set; }
    }

    public class enumCategoryVanban
    {
        public enum intloai
        {
            Vanbanden = 1,
            Vanbanphathanh = 2,
            Vanbanduthao = 3
        }

        public enum inttrangthai
        {
            IsActive = 1,
            NotActive = 0
        }
    }
}
