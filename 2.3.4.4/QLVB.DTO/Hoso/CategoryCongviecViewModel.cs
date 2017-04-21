using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Hoso
{
    public class CategoryCongviecViewModel
    {
        public int Songayhienthi { get; set; }

        public VaitroXuly VaitroXuly { get; set; }
    }

    public class VaitroXuly
    {
        public const string Dangxuly = "Đang xử lý";
        public const string Xulychinh = "Xử lý chính";
        public const string Phoihopxuly = "Phối hợp xử lý";
        public const string Hoanthanh = "Đã xử lý";

    }
}
