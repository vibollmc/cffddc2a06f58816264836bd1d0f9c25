using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{
    public class PhanloaiQuytrinh
    {
        public int intid { get; set; }
        public string strtenloai { get; set; }
        public int inttrangthai { get; set; }
    }
    public class enumPhanloaiQuytrinh
    {
        public enum inttrangthai
        {
            NotActive = 0,
            IsActive = 1
        }
    }

}
