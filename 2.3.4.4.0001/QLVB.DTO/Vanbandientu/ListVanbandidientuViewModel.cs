using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Vanbandientu
{
    public class ListVanbandidientuViewModel
    {
        public int intid { get; set; }
        public DateTime? dtengayky { get; set; }
        public int? intso { get; set; }
        public string strsophu { get; set; }
        public string strkyhieu { get; set; }
        public string strtrichyeu { get; set; }

        public bool IsAttach { get; set; }
    }
}
