using System.ComponentModel.DataAnnotations;

namespace DLTD.Web.Main.Models.Enum
{
    public enum NguonChiDao
    {
         [Display(Name = "Tỉnh Ủy")]
        Tinhuy,
         [Display(Name = "Chính Phủ")]
       Chinhphu = 1,
         [Display(Name = "Bộ Ngành, Trung Ương")]
       Trunguong = 2,
         [Display(Name = "Hội đồng Nhân dân")]
         HDND = 2
    }
}