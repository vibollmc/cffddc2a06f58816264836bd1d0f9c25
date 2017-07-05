using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Vanbandi
{
    public class ListEmailDonviViewModel
    {
        public int idvanban { get; set; }

        public string strtieude { get; set; }

        public string strnoidung { get; set; }

        public bool IsAutoSend { get; set; }

        public IEnumerable<EmailDonviViewModel> donvi { get; set; }

        public IEnumerable<EmailDonviViewModel> donvikhac { get; set; }

        public IEnumerable<QLVB.DTO.Mail.ListFileToAttach> listfile { get; set; }
    }

    /// <summary>
    /// don vi tham gia gui/nhan vanban dien tu
    /// </summary>
    public class EmailDonviViewModel
    {
        public int iddonvi { get; set; }
        public string strtendonvi { get; set; }

        public string stremailvbdt { get; set; }

        public string stremail { get; set; }
        public int intloai { get; set; }

        public bool IsVbdt { get; set; }
        public bool IsSend { get; set; }

        public string strmatructinh { get; set; }
    }
}
