using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Vanbandi
{
    public class ListVanbandiViewModel
    {
        public int intid { get; set; }
        public DateTime? dtengayky { get; set; }
        public int? intso { get; set; }
        public string strsophu { get; set; }
        public string strkyhieu { get; set; }
        public string strtrichyeu { get; set; }
        public string strnoinhan { get; set; }
        public DateTime? dtehanxuly { get; set; }

        public int? inttrangthai { get; set; }

        public bool IsAttach { get; set; }
    }
}
