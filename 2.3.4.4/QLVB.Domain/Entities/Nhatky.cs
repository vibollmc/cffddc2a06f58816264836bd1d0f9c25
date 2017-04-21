using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{
    
    public class Nhatky
    {
    
        public Int32 Id { get; set; }

        public Nullable<DateTime> time_stamp { get; set; }

        public string host { get; set; }

        public string type { get; set; }

        public string source { get; set; }

        public string username { get; set; }

        public string client { get; set; }

        public string message { get; set; }

        public string level { get; set; }

        public string logger { get; set; }

        public string stacktrace { get; set; }

        public string allxml { get; set; }
    }
}