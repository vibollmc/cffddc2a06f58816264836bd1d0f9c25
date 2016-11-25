
using System.ComponentModel.DataAnnotations;

namespace DLTD.Web.Main.Models.Enum
{
    public enum TrangThaiVanBan
    {
        [Display(Name = "Undefined")]
        Undefined = 0,
        [Display(Name = "Mới")]
        Moi = 1,
        [Display(Name = "Đang xử lý")]
        DangXuLy = 2,
        [Display(Name = "Quá hạn xử lý")]
        QuaHan = 3,
        [Display(Name = "Hoàn thành")]
        HoanThanh = 4,
        [Display(Name = "Hoàn thành trễ hạn")]
        HoanThanhTreHan = 5,
        [Display(Name = "Trả lại")]
        TraLai = 6
    }
}
