using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.DTO.Vanbanden
{
    public class CategoryViewModel
    {
        public IEnumerable<CategoryVanban> Category { get; set; }

        public int Songayhienthi { get; set; }

        public IEnumerable<Khoiphathanh> Khoiphathanh { get; set; }

        public IEnumerable<PhanloaiVanban> Loaivanban { get; set; }

        public IEnumerable<SoVanban> Sovanban { get; set; }

        public Xulyvanban Xulyvanban { get; set; }
    }

    public class Xulyvanban
    {
        public const string Chuaduyet = "Chưa duyệt";
        public const string Dangxuly = "Đang xử lý";
        public const string Xulychinh = "Xử lý chính";
        public const string Phoihopxuly = "Phối hợp xử lý";
        public const string Hoanthanh = "Đã xử lý";

    }


}
