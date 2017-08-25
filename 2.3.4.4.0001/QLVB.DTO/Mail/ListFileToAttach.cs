using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Mail
{
    /// <summary>
    /// dung trong attach file to send smtp
    /// </summary>
    public class ListFileToAttach
    {
        public int intidfile { get; set; }
        public string strmota { get; set; }
        public string filePath { get; set; }
    }
}
