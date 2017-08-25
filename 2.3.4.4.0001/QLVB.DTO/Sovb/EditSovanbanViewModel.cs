using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using QLVB.Domain.Entities;

namespace QLVB.DTO.Sovb
{
    public class EditSovanbanViewModel
    {
        public int intid { get; set; }

        [Display(Name = "Tên sổ văn bản")]
        public string strten { get; set; }

        [Display(Name = "Ký hiệu")]
        public string strkyhieu { get; set; }

        [Display(Name = "Ghi chú")]
        public string strghichu { get; set; }

        [Display(Name = "Loại sổ văn bản")]
        public int? intloai { get; set; }

        [Required(ErrorMessage = ("*"))]
        [Display(Name = ("Loại sổ văn bản:"))]
        [Range(1, int.MaxValue, ErrorMessage = "Chọn loại sổ văn bản")]
        public enumSovanban.intloai Loaisovanban { get; set; }

        public int? intorder { get; set; }

        [Display(Name = "Hiển thị mặc định")]
        public bool IsDefault { get; set; }


        //===========bo sung so voi qlvb1 ===========
        // dung de cau hinh co cho phep xem toan bo so van ban den/di khong?
        public int? intquyenxemsovb { get; set; }
    }
}
