using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Capcoquan
{
    /// <summary>
    /// dm cac don vi tu database
    /// </summary>
    public class EmailDonviDBViewModel
    {
        public int id { get; set; }
        public string strten { get; set; }
        public string email { get; set; }
        public string madinhdanh { get; set; }
    }
}
