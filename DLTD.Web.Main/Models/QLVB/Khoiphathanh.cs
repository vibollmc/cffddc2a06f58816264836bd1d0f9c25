using System;

namespace DLTD.Web.Main.Models.QLVB
{
    
    public class Khoiphathanh
    {
    
        public int intid { get; set; }
        public string strkyhieu { get; set; }

        public string strtenkhoi { get; set; }

        public bool IsDefault { get; set; }

        public int inttrangthai { get; set; }
    }

    public class enumKhoiphathanh
    {
        public enum inttrangthai
        {
            IsActive = 1,
            NotActive = 0
        }
    }
}