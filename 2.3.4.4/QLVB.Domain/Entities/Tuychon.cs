using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Entities
{
    public class Tuychon
    {
        public int intid { get; set; }

        public string strthamso { get; set; }

        public string strgiatri { get; set; }

        public string strmota { get; set; }

        public int? intorder { get; set; }

        public int? intgroup { get; set; }

        public int? inttrangthai { get; set; }
    }

    public class ThamsoTuychon
    {
        public const string MenuType = "MenuType";
        public const string IsMenuClickable = "IsMenuClickable";
        public const string HomePage = "HomePage";

    }
    public class enumTuychon
    {
        public enum inttrangthai
        {
            NotActive = 0,
            IsActive = 1
        }
    }



}
