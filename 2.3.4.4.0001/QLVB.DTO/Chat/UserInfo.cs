using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Chat
{
    public class UserInfo
    {
        public string ConnectionId { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public string UserAgent { get; set; }
        public string UserGroup { get; set; }
        public bool IsConnected { get; set; }
        public DateTime? LastActivity { get; set; }



    }
}
