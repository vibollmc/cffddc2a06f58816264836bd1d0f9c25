using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QLVB.DTO.Capcoquan
{
    public class EditKhoiphViewModel
    {
        public int intid { get; set; }

        [Display(Name = ("Tên cấp cơ quan"))]
        [Required(ErrorMessage = ("*"))]
        public string strtenkhoi { get; set; }
    }
}
