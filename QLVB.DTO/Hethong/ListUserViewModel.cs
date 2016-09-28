using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Hethong
{
    public class ListUserViewModel
    {
        public IEnumerable<UserViewModel> user { get; set; }
        public int idgroup { get; set; }
    }
}
