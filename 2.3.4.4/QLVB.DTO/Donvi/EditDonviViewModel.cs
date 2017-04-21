using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QLVB.DTO.Donvi
{
    public class EditDonviViewModel
    {
        public int intid { get; set; }

        [Required(ErrorMessage = ("*"))]
        [Display(Name = ("Tên đơn vị"))]
        public string strtendonvi { get; set; }

        //intType=0: addnew
        //intType=1:edit
        public int intType { get; set; }
    }
}
