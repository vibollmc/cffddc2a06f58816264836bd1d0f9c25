using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.DTO.Baocao
{
    public class ListdonviViewModel
    {
        public int iddonvi { get; set; }
        public IEnumerable<Donvitructhuoc> Donvi { get; set; }

        public int maxLevelDonvi { get; set; }

        // chi in cac van ban den da phan xu ly
        public bool IsXuly { get; set; }

    }
}
