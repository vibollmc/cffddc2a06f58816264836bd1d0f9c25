using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QLVB.Domain.Entities
{
    public class Chucdanh
    {
        public Int32 intid { get; set; }
        // ky hieu
        public string strmachucdanh { get; set; }
        public string strtenchucdanh { get; set; }
        public string strghichu { get; set; }
        //kieu chuc danh
        public Int32? intloai { get; set; }

    }
    public class enumchucdanh
    {
        public enum intloai
        {
            [Display(Name = ("Lãnh đạo"))]
            Lanhdao = 1,
            [Display(Name = ("Cán bộ"))]
            Canbo = 2
        }
    }
}
