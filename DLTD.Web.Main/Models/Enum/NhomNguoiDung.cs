using System.ComponentModel.DataAnnotations;

namespace DLTD.Web.Main.Models.Enum
{
    public enum NhomNguoiDung
    {
        [Display(Name = "")]
        Undefined = 0,

        [Display(Name = "Quản Trị Hệ Thống")]
        QuanTriHeThong = 1,

        [Display(Name = "Chuyên Viên")]
        ChuyenVien = 2,
        
        [Display(Name = "Nhân Viên Văn Thư")]
        NhanVienVanThu = 3,

        [Display(Name = "Lãnh Đạo")]
        LanhDao = 4,

        [Display(Name = "Hệ Thống Dùng Chung")]
        HeThongDungChung = 6,

        [Display(Name = "Lãnh Đạo Phòng")]
        LanhDaoPhong = 10
    }
}