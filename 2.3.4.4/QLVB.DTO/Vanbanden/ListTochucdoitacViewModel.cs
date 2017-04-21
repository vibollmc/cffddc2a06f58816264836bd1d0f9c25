using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Vanbanden
{
    /// <summary>
    /// hien thi ten don vi trong autocomplete ten co quan phat hanh
    /// </summary>
    public class ListTochucdoitacViewModel
    {
        public string strkyhieu { get; set; }

        public string strmadinhdanh { get; set; }

        public string strten { get; set; }

        /// <summary>
        /// ket hop ky hieu va ten: kyhieu || ten
        /// </summary>
        public string strkyhieu_ten { get; set; }


    }
}
