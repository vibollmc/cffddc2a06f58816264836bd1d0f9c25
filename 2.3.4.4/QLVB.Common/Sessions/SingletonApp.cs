using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QLVB.Common.Sessions
{
    public class SingletonApp
    {
        public static string IsSend
        {
            get
            {
                return HttpContext.Current.Application["IsSend"] as string;
            }
            set
            {
                HttpContext.Current.Application["IsSend"] = value;
            }
        }
        public static string IsReceive
        {
            get
            {
                return HttpContext.Current.Application["IsReceive"] as string;
            }
            set
            {
                HttpContext.Current.Application["IsReceive"] = value;
            }
        }
    }
}
