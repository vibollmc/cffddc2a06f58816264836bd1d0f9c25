using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Mail
{
    /// <summary>
    /// cac truong trong van ban khi gui email
    /// </summary>
    public class ThongtinVanbanViewModel
    {
        public IEnumerable<Noinhan> listnoinhan { get; set; }
        public string strsodi { get; set; }
        public string strkyhieu { get; set; }
        public string strnoiky { get; set; }
        public string strngayky { get; set; }
        public string strloaivanban { get; set; }
        public string strtrichyeu { get; set; }
        public string strnguoiky { get; set; }

        //=========================================
        // thong tin phan hoi
        public string strngaygui { get; set; }
        public string stridvanban { get; set; }
        public string strFromAddress { get; set; }
        public string strIdDonvi { get; set; }

    }

    public class Noinhan
    {
        public string MaDinhDanh { get; set; }
        public string Tendonvi { get; set; }
        public string Email { get; set; }
    }
}
