using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.DTO.Menu
{
    public class MenuViewModels
    {
        public IEnumerable<QLVB.Domain.Entities.Menu> RootMenu { get; set; }

        public int ActiveMenu { get; set; }
        public IEnumerable<QLVB.Domain.Entities.Menu> SubMenu { get; set; }

        public HeaderViewModel headerViewModel { get; set; }
    }
}
