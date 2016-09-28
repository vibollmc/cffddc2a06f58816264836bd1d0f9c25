using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{
    
    public class Systems
    {
        public Int32 intid { get; set; }

        public string strthamso { get; set; }

        public string strgiatri { get; set; }
    }
}