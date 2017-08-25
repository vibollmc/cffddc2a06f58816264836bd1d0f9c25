using System;
using System.ComponentModel.DataAnnotations;

namespace QLVB.DTO.Account
{
    public class RegisterViewModel
    {
        public int intid { get; set; }

        public int intiddonvi { get; set; }

        [Display(Name = "Mã cán bộ")]
        public string strmacanbo { get; set; }

        [Display(Name = "Ký hiệu tên")]
        public string strkyhieu { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Họ tên")]
        public string strhoten { get; set; }

        [Display(Name = "Số ĐT")]
        public string strdienthoai { get; set; }

        [Display(Name = "Email")]
        public string stremail { get; set; }

        [Display(Name = "Ngày sinh")]
        public string strngaysinh { get; set; }

        [Display(Name = "Giới tính")]
        public Int32? intgioitinh { get; set; }

        [Display(Name = "Chức danh")]
        public Int32? intchucvu { get; set; }

        [Display(Name = "Nhóm quyền")]
        public Int32? intnhomquyen { get; set; }

        [Display(Name = "Quyền ký VB")]
        public bool intkivb { get; set; }

        [Display(Name = "Người XL BĐ")]
        public bool IsNguoiXL { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Tên đăng nhập")]
        public string strusername { get; set; }

        //[Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string strpassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("strpassword", ErrorMessage = "Mật khẩu xác nhận không đúng.")]
        public string ConfirmPassword { get; set; }
    }
}
