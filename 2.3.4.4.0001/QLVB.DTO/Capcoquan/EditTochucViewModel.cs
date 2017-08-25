using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QLVB.DTO.Capcoquan
{
    public class EditTochucViewModel
    {
        public int intid { get; set; }

        public int intidkhoiph { get; set; }

        [Display(Name = ("Tên đơn vị"))]
        [Required(ErrorMessage = ("*"))]
        public string strtentochucdoitac { get; set; }

        [Display(Name = ("Ký hiệu"))]
        public string strmatochucdoitac { get; set; }

        [Display(Name = ("Điện thoại"))]
        public string strphone { get; set; }

        [Display(Name = ("Địa chỉ"))]
        public string strdiachi { get; set; }

        [Display(Name = ("Email"))]
        public string stremail { get; set; }

        [Display(Name = ("Tham gia hồi báo"))]
        public bool IsHoibao { get; set; }

        [Display(Name = ("Nhận văn bản điện tử"))]
        public bool Isvbdt { get; set; }

        [Display(Name = ("Email văn bản"))]
        public string stremailvbdt { get; set; }

        [Display(Name = ("Mã định danh"))]
        public string strmadinhdanh { get; set; }

        [Display(Name = ("Mã trục tỉnh"))]
        public string strmatructinh { get; set; }

    }
}
