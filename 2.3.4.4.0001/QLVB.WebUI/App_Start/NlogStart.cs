using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog.Config;

namespace QLVB.WebUI.App_Start
{
    public class NlogStart
    {
        public static void SetNLog()
        {
            // ---Register custom NLog Layout renderers----
            ConfigurationItemFactory.Default.LayoutRenderers
                    .RegisterDefinition("web_variables", typeof(QLVB.WebUI.Common.NLog.WebVariablesRenderer));
            ConfigurationItemFactory.Default.LayoutRenderers
                    .RegisterDefinition("username", typeof(QLVB.WebUI.Common.NLog.UserIdRenderer));
            ConfigurationItemFactory.Default.LayoutRenderers
                    .RegisterDefinition("client", typeof(QLVB.WebUI.Common.NLog.ClientRenderer));
        }
    }
}