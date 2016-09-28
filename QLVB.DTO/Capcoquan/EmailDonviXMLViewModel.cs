using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Capcoquan
{
    /// <summary>
    /// dung de load du lieu tu file xml danh muc cac don vi gui/nhan vbdt
    /// </summary>
    public class EmailDonviXMLViewModel
    {
        public int id { get; set; }
        public string strten { get; set; }
        public string email { get; set; }
        public string madinhdanh { get; set; }
    }
}
