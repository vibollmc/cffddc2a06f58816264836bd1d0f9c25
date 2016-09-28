using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QLVB.DTO.Tinhchatvb
{
    public class EditTinhchatViewModel
    {
        public int intid { get; set; }

        [Required(ErrorMessage = ("*"))]
        [Display(Name = ("Tên tính chất"))]
        public string strtentinhchatvb { get; set; }

        [Display(Name = ("Ký hiệu"))]
        public string strkyhieu { get; set; }

        [Display(Name = ("Mô tả"))]
        public string strmota { get; set; }

        public int intloai { get; set; }

    }
}
