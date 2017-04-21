using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QLVB.DTO.Loaivanban
{
    public class EditLoaivanbanViewModel
    {
        public int intid { get; set; }
        [Display(Name = "Mã văn bản")]
        public string strmavanban { get; set; }

        [Required(ErrorMessage = ("*"))]
        [Display(Name = "Tên loại văn bản")]
        public string strtenvanban { get; set; }

        [Display(Name = "Ghi chú")]
        public string strghichu { get; set; }

        // --- mac dinh chon trong combox loai van ban khi them moi van ban
        [Display(Name = "Hiển thị mặc định")]
        public bool IsDefault { get; set; }

    }
}
