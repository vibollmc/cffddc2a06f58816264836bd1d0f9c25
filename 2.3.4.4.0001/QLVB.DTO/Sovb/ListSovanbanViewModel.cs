using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.DTO.Sovb
{
    public class ListSovanbanViewModel
    {
        public IEnumerable<SoVanban> Sovbden { get; set; }
        public IEnumerable<SoVanban> Sovbdi { get; set; }
    }
}
