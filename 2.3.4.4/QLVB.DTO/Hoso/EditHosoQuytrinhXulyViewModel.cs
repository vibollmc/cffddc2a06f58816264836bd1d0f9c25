using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Hoso
{
    public class EditHosoQuytrinhXulyViewModel
    {
        public int idhoso { get; set; }
        public string strtenquytrinh { get; set; }

        public int intidNode { get; set; }
        public string strNodeId { get; set; }

        public string strTenNode { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Đơn vị thực hiện")]
        public int intDonvi { get; set; }
        public IEnumerable<QLVB.DTO.Donvi.EditDonviViewModel> listDonvi { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Người xử lý")]
        public int intCanbo { get; set; }
        public IEnumerable<QLVB.DTO.Donvi.ListUserViewModel> listCanbo { get; set; }

        //[Display(Name = "Vai trò xử lý")]
        //public int intVaitro { get; set; }
        [Display(Name = "Vai trò xử lý")]
        public QLVB.DTO.Quytrinh.enumEditThongtinXulyViewModel LoaiVaitro { get; set; }



    }
}
