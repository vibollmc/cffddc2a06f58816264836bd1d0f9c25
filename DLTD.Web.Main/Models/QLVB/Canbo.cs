using System;
using System.ComponentModel.DataAnnotations;

namespace DLTD.Web.Main.Models.QLVB
{
    public class Canbo
    {
        //[Key]
        public int intid { get; set; }

        // intid trong bang chuc danh
        public int? intchucvu { get; set; }

        public int? intgioitinh { get; set; }

        public int? intkivb { get; set; }

        // nguoi xu ly ban dau
        public int? intnguoixuly { get; set; }

        //nhom quyen user
        public int? intnhomquyen { get; set; }
        // user thuoc don vi truc thuoc nao
        public int? intdonvi { get; set; }

        public int inttrangthai { get; set; }

        public string strdienthoai { get; set; }

        public string stremail { get; set; }

        public string strhoten { get; set; }


        public string strkyhieu { get; set; }

        public string strmacanbo { get; set; }

        public DateTime? strngaysinh { get; set; }

        public DateTime? strngaytao { get; set; }

        public DateTime? strngayxoa { get; set; }

        public string strpassword { get; set; }

        public string strRight { get; set; }

        public string strusername { get; set; }

        // ten image profile của user 
        //trong folder content/image/users : iduser_ngayupload.jpg
        public string strImageProfile { get; set; }
    }

    public class enumcanbo
    {
        public enum inttrangthai
        {
            IsActive = 1,
            NotActive = 0
        }
        public enum intgioitinh
        {
            [Display(Name = "Nam")]
            Nam = 1,
            [Display(Name = "Nữ")]
            Nu = 0
        }
        public enum intkivb
        {
            Co = 1,
            Khong = 0
        }
        public enum intnguoixuly
        {
            NotActive = 0,
            IsActive = 1,
            IsDefault = 2  // hien thi mac dinh trong nhap van ban den
        }
        public enum strRight
        {
            PhanXLNhieuVB = 0,
            XulyNhanh = 1

        }
    }
}
