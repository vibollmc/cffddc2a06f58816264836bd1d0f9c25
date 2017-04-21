using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.DTO.Baocao
{
    public class CategoryBaocaoViewModel
    {
        public IEnumerable<QLVB.Domain.Entities.Baocao> Vanbanden { get; set; }

        public IEnumerable<QLVB.Domain.Entities.Baocao> Vanbandi { get; set; }
    }
}
