using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{
    
    public class NhomQuyen
    {
        public Int32 intid { get; set; }

        public string strtennhom { get; set; }

        public Nullable<Int32> inttrangthai { get; set; }

        public Nullable<Int32> intorder { get; set; }
    }

    public class enumNhomquyen
    {
        public enum inttrangthai
        {
            IsActive = 1,
            NotActive = 0
        }
    }
}