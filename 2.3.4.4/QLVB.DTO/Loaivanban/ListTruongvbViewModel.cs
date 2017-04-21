using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.DTO.Loaivanban
{
    public class ListTruongvbViewModel
    {
        public int idloaivb { get; set; }
        public IEnumerable<PhanloaiTruong> Phanloaitruong { get; set; }

    }
}
