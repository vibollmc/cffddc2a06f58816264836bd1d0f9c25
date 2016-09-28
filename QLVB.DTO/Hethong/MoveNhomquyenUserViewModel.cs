using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Hethong
{
    public class MoveNhomquyenUserViewModel
    {
        public IEnumerable<UserViewModel> userSource { get; set; }

        public int idnhomquyen { get; set; }

        public IEnumerable<UserViewModel> userDest { get; set; }
    }
}
