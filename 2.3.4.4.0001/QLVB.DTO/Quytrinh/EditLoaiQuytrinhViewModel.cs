using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QLVB.DTO.Quytrinh
{
    public class EditLoaiQuytrinhViewModel
    {
        public int intid { get; set; }

        [Display(Name = "Loại quy trình")]
        public string strtenloaiquytrinh { get; set; }
    }
}
