using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using QLVB.Domain.Entities;

namespace QLVB.DTO.Donvi
{
    public class EditUserViewModel
    {
        public int intid { get; set; }

        public int? iddonvi { get; set; }

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
        public DateTime? strngaysinh { get; set; }

        [Display(Name = "Giới tính")]
        public enumcanbo.intgioitinh enumgioitinh { get; set; }

        //[Required(ErrorMessage = "*")]
        [Display(Name = "Chức danh")]
        public int? intchucvu { get; set; }

        public IEnumerable<QLVB.Domain.Entities.Chucdanh> Listchucdanh { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Nhóm quyền")]
        public int? intnhomquyen { get; set; }

        public IEnumerable<NhomQuyen> Listnhomquyen { get; set; }

        [Display(Name = "Quyền ký VB")]
        public bool intkivb { get; set; }

        [Display(Name = "Người XL BĐ")]
        public bool IsNguoiXL { get; set; }

        [Display(Name = "Mặc định")]
        public bool IsDefaultXLBD { get; set; }

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


        public DTO.File.UploadFileViewModel ImageProfile { get; set; }

        public string strImageProfile { get; set; }
    }




}
