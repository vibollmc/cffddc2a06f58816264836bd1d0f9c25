using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{
    
    public class Hoibaovanban
    {
    
        public int intid { get; set; }

        public int intTransID { get; set; }

        public int intRecID { get; set; }

        public int intloai { get; set; }
    }

    public class enumHoibaovanban
    {
        public enum intloai
        {
            Vanbanden = 1,
            Vanbandi = 2
        }
    }
}
