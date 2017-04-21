using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Vanbandi
{

    public class CategoryViewModel
    {
        public IEnumerable<CategoryVanban> Category { get; set; }

        public int Songayhienthi { get; set; }


        public IEnumerable<PhanloaiVanban> Loaivanban { get; set; }

        public IEnumerable<SoVanban> Sovanban { get; set; }


    }

}
