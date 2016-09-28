using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.DTO.Hethong;

namespace QLVB.DTO.Hethong
{
    public class ListRoleGroupViewModel
    {
        public IEnumerable<Quyen> QuyenVM { get; set; }

        public IEnumerable<QLVB.Domain.Entities.Menu> MenuVM { get; set; }

        public IEnumerable<RoleGroupCheckViewModel> RoleGroupCheckVM { get; set; }

        public int idgroup { get; set; }
    }
}
