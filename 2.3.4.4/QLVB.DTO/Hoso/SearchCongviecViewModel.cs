using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Hoso
{
    public class SearchCongviecViewModel
    {
        //===============================================
        // status
        //===============================================
        public bool isSearch { get; set; }
        public bool isBack { get; set; }

        public int intPage { get; set; }

        public string strngayhscat { get; set; }
        public string strxuly { get; set; }

        //===============================================
        // search
        //===============================================
        public int? intsobd { get; set; }
        public int? intsokt { get; set; }

        public string strngayhsbd { get; set; }
        public string strngayhskt { get; set; }

        public IEnumerable<CanboViewModel> Nguoixuly { get; set; }

        public string strxulychinh { get; set; }

        public string strtieude { get; set; }

        public string strhantraloi { get; set; }

        public int? idlinhvuc { get; set; }

        public IEnumerable<QLVB.DTO.Linhvuc.EditLinhvucViewModel> ListLinhvuc { get; set; }


    }
}
