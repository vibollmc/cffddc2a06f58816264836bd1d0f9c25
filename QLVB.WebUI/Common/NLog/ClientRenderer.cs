using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using NLog.Config;
using NLog.LayoutRenderers;
using System.Text;

namespace QLVB.WebUI.Common.NLog
{
    [LayoutRenderer("client")]
    public class ClientRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            //return ip;
            string browser = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];

            string strclientip = HttpContext.Current.Request.UserHostAddress;

            builder.Append(ip + " __ " + browser);
        }
    }
}