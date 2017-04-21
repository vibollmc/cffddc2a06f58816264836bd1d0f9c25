using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Quytrinh
{
    public class CategoryNgayViewModel
    {
        public int idquytrinh { get; set; }
        public IEnumerable<string> ListNgay { get; set; }
    }
}
