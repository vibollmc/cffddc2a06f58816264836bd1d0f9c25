using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Tinhhinhxuly
{
    public class ListQuytrinhViewModel
    {
        public DateTime dteNgaybd { get; set; }

        public DateTime dteNgaykt { get; set; }

        //=====================quy trinh

        [Display(Name = "Loại quy trình")]
        public int intidloaiquytrinh { get; set; }

        public IEnumerable<QLVB.DTO.Quytrinh.EditLoaiQuytrinhViewModel> Loaiquytrinh { get; set; }

        [Display(Name = "Quy trình xử lý")]
        public int intidquytrinh { get; set; }

        public IEnumerable<QLVB.DTO.Quytrinh.EditQuytrinhViewModel> Quytrinh { get; set; }
    }
}
