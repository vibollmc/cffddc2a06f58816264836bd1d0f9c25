using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Account
{
    public class TuychonViewModel
    {
        public IEnumerable<TuychonCanboViewModel> Tuychon { get; set; }

        public IEnumerable<TuychonSelectListVM> HomePage { get; set; }
        public IEnumerable<TuychonSelectListVM> MenuType { get; set; }

    }

    public class TuychonCanboViewModel
    {
        public int? intid { get; set; }

        public string strthamso { get; set; }

        public string strgiatri { get; set; }
        public string strgiatridefault { get; set; }

        public string strmota { get; set; }

        public int? intorder { get; set; }

        public int? intgroup { get; set; }
    }

    public class TuychonSelectListVM
    {
        public string strmota { get; set; }
        public string strgiatri { get; set; }
        public bool? isSelect { get; set; }
    }
}
