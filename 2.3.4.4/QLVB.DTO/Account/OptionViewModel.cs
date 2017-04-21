using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Account
{
    public class OptionViewModel
    {
        public int idOption { get; set; }
        public string strmota { get; set; }

        public bool blgiatri { get; set; }

        public string strImageProfile { get; set; }

        public IEnumerable<OptionUserViewModel> ListOption { get; set; }
    }

    /// <summary>
    /// danh sach cac tuy chon cua ca nhan
    /// (table canbo: strRight)
    /// </summary>
    public class OptionUserViewModel
    {
        public int intVitri { get; set; }
        public bool IsValue { get; set; }
        public string strmota { get; set; }

    }
}
