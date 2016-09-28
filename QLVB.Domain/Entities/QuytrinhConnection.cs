using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Entities
{
    public class QuytrinhConnection
    {
        public int intid { get; set; }
        public int intidFrom { get; set; }
        public int intidTo { get; set; }
        public string strlabel { get; set; }
    }
}
