using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Account
{
    public class ListUserUyquyenViewModel
    {
        // tat ca user trong don vi
        public IEnumerable<UserUyquyenViewModel> AllUser { get; set; }

        // cac user trong don vi da duoc uy quyen
        public IEnumerable<UserUyquyenViewModel> UyquyenUser { get; set; }
    }
}
