using NLog;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using QLVB.Common.Utilities;

namespace QLVB.WebUI.Common.NLog
{
    [LayoutRenderer("username")]
    public class UserIdRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            string user = "";
            if (HttpContext.Current.Session[AppConts.SessionUserName] != null)
            {
                user = HttpContext.Current.Session[AppConts.SessionUserName].ToString();
            }
            else
            {
                user = "Session timeout";
            }
            string struserid = user;
            builder.Append(struserid);
        }
    }
}