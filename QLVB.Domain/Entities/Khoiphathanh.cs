using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{
    
    public class Khoiphathanh
    {
    
        public Int32 intid { get; set; }
        public string strkyhieu { get; set; }

        public string strtenkhoi { get; set; }

        public bool IsDefault { get; set; }

        public Int32 inttrangthai { get; set; }
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