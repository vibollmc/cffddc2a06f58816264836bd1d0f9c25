using System.ComponentModel.DataAnnotations;

namespace DLTD.Web.Main.Models.Enum
{
    public enum DoQuanTrong
    {
        [Display(Name = "Thường")]
        Thuong = 1,
        [Display(Name = "Quan Trọng")]
        Quantrong = 2,
        [Display(Name = "Rất quan trọng")]
        Ratquantrong = 3
    }
}