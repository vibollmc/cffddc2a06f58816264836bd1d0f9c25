using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace QLVB.Domain.Entities
{
    
    public class UyQuyen
    {
    
        public int intid { get; set; }

        // id user uy quyen
        public int intPersSend { get; set; }

        // id user nhan uy quyen
        public int intPersRec { get; set; }
    }
}
