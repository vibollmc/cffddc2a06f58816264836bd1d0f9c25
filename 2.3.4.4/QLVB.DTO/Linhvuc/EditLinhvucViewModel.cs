using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QLVB.DTO.Linhvuc
{
    public class EditLinhvucViewModel
    {
        public int intid { get; set; }

        [Required(ErrorMessage = ("*"))]
        [Display(Name = ("Tên Lĩnh vực"))]
        public string strtenlinhvuc { get; set; }

        [Display(Name = ("Ký hiệu"))]
        public string strkyhieu { get; set; }

        [Display(Name = ("Ghi chú"))]
        public string strghichu { get; set; }

    }
}
