using System.ComponentModel.DataAnnotations;

namespace QLVB.DTO.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Duy trì đăng nhập")]
        public bool RememberMe { get; set; }

        public string TenDonvi { get; set; }
    }
}