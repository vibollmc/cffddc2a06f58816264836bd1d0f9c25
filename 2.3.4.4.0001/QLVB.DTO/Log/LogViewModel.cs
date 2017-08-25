using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Log
{
    public class LogViewModel
    {
        public int intloai { get; set; }
        public Nullable<DateTime> time_stamp { get; set; }

        public string host { get; set; }

        public string username { get; set; }

        public string client { get; set; }

        public string message { get; set; }

        public string level { get; set; }

    }
}
