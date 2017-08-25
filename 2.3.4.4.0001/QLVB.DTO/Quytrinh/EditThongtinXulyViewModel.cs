using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QLVB.DTO.Quytrinh
{
    public class EditThongtinXulyViewModel
    {
        public int idquytrinh { get; set; }
        public string strtenquytrinh { get; set; }

        public int intidNode { get; set; }
        public string strNodeId { get; set; }

        public string strTenNode { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Đơn vị thực hiện")]
        public int intDonvi { get; set; }
        public IEnumerable<QLVB.DTO.Donvi.EditDonviViewModel> listDonvi { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Người xử lý")]
        public int intCanbo { get; set; }
        public IEnumerable<QLVB.DTO.Donvi.ListUserViewModel> listCanbo { get; set; }

        //[Display(Name = "Vai trò xử lý")]
        //public int intVaitro { get; set; }
        [Display(Name = "Vai trò xử lý")]
        public enumEditThongtinXulyViewModel LoaiVaitro { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Số ngày xử lý")]
        public int intSoNgay { get; set; }

        [Display(Name = "Mặc định hoàn thành")]
        public bool IsHoanthanh { get; set; }

        //[Display(Name = "Chuyển xử lý khi hoàn thành")]
        [Display(Name = "Tự động chuyển xử lý")]
        public bool IsNext { get; set; }

        [Display(Name = "Xử lý đồng thời")]
        public bool IsXulyDongthoi { get; set; }

    }
    public enum enumEditThongtinXulyViewModel
    {
        [Display(Name = ("Chọn vai trò xử lý"))]
        Khongthamgia = 0,
        [Display(Name = ("Lãnh đạo giao việc"))]
        Lanhdaogiaoviec = 1,
        [Display(Name = ("Lãnh đạo phụ trách"))]
        Lanhdaophutrach = 2,
        [Display(Name = ("Xử lý chính"))]
        Xulychinh = 3,
        [Display(Name = ("Phối hợp xử lý"))]
        Phoihopxuly = 4,

    }
}
