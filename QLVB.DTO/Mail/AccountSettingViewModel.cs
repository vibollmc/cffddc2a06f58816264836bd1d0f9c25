using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Mail
{
    /// <summary>
    /// thong tin account dung de gui vbdt
    /// </summary>
    public class AccountSettingViewModel
    {
        // account
        public string emailAddress { get; set; }
        public string accountName { get; set; }
        public string password { get; set; }
        public string displayName { get; set; }

        // pop3
        public string incomingMailServer { get; set; }
        public int portIncomingServer { get; set; }
        public bool isIncomeSecureConnection { get; set; }
        public bool portIncomingChecked { get; set; }

        // smtp
        public string outgoingServer { get; set; }
        public int portOutgoingServer { get; set; }
        public bool isOutgoingSecureConnection { get; set; }
        public bool isOutgoingWithAuthentication { get; set; }
        public bool portOutgoingChecked { get; set; }


        public Guid _AccountID { get; set; }
        public string loginId { get; set; }
        public string charset { get; set; }

    }
}
