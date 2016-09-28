using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using QLVB.Domain.Entities;

namespace QLVB.DTO.Chucdanh
{
    public class EditChucdanhViewModel
    {
        public int intid { get; set; }

        [Display(Name = ("Mã chức danh:"))]
        public string strmachucdanh { get; set; }

        [Required(ErrorMessage = ("*"))]
        [Display(Name = ("Tên nhóm:"))]
        public string strtenchucdanh { get; set; }

        [Display(Name = ("Ghi chú:"))]
        public string strghichu { get; set; }

        [Display(Name = ("Loại chức danh:"))]
        public int intloai { get; set; }

        [Required(ErrorMessage = ("*"))]
        [Display(Name = ("Loại chức danh:"))]
        [Range(1, int.MaxValue, ErrorMessage = "Chọn loại chức danh")]
        public enumchucdanh.intloai Loaichucdanh { get; set; }

    }
}
