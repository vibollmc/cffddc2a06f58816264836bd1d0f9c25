using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DLTD.Web.Main.DAL;

namespace DLTD.Web.Main
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Task.Run(() => SyncDataFromQlvb.Sync());
        }

        protected void Session_Start()
        {
            var cookie = Request.Cookies["QLVBTokens"];

            if (cookie != null && cookie.Value != null)
                Session["AccessToken"] = cookie["AccessToken"];
        }
    }
}
