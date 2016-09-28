using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{
    
    public class Home
    {
    
        public Int32 intid { get; set; }

        public string straction { get; set; }

        public string strcontroller { get; set; }
    }
}