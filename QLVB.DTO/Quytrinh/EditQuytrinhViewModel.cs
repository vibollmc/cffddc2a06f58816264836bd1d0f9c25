using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QLVB.DTO.Quytrinh
{
    public class EditQuytrinhViewModel
    {
        public int intid { get; set; }

        [Display(Name = "Loại Quy trình")]
        public int idloai { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Quy trình")]
        public string strtenquytrinh { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Số ngày xử lý")]
        public int intSongay { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Thời gian áp dụng")]
        public DateTime? dteThoigianApdung { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }

        public IEnumerable<EditLoaiQuytrinhViewModel> LoaiQuytrinh { get; set; }
    }
}
