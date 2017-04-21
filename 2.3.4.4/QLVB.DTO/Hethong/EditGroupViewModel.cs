using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QLVB.DTO.Hethong
{
    public class EditGroupViewModel
    {

        public int intid { get; set; }

        [Required(ErrorMessage = ("*"))]
        [Display(Name = ("Tên nhóm:"))]
        public string strtennhomquyen { get; set; }
    }
}
