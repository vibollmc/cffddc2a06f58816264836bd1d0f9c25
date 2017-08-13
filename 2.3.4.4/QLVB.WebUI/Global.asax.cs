using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using StackExchange.Profiling;
using StackExchange.Profiling.EntityFramework6;
using QLVB.WebUI.App_Start;
using QLVB.WebUI.Common.Security;
using FluentSecurity;
using System.Web.Helpers;
using System.Security.Claims;
using System.Threading;
using QLVB.WebUI.Hubs;
using Microsoft.AspNet.SignalR;
using QLVB.Core.Contract;
using QLVB.Core.Implementation;

namespace QLVB.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private AutoResetEvent _autoResetEvent;

        private Timer _timer;

        protected void Application_Start()
        {


            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            SecurityServices.SetupFluentSecurity();
            //required for FluentSecurity
            GlobalFilters.Filters.Add(new HandleSecurityAttribute(), 0);

            NlogStart.SetNLog();

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;// NameIdentifier;

            //MiniProfilerEF6.Initialize();

            //http://www.asp.net/signalr/overview/signalr-20/hubs-api/handling-connection-lifetime-events
            // Make long polling connections wait a maximum of 110 seconds for a
            // response. When that time expires, trigger a timeout command and
            // make the client reconnect.
            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(110);

            // Wait a maximum of 30 seconds after a transport connection is lost
            // before raising the Disconnected event to terminate the SignalR connection.
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(30);

            // For transports other than long polling, send a keepalive packet every
            // 10 seconds. 
            // This value must be no more than 1/3 of the DisconnectTimeout value.
            GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(10);

            //RouteTable.Routes.MapHubs();

            using (var context = new QLVB.DAL.QLVBDatabase())
            {
                // reset connection in signalR
                context.Database.ExecuteSqlCommand("update connection set connected=0 ");
            }

            QLVB.Common.Sessions.SingletonApp.IsSend = "false";
            QLVB.Common.Sessions.SingletonApp.IsReceive = "false";


            // fix loi web api synch file
            var serializerSettings = GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;
            var contractResolver = (Newtonsoft.Json.Serialization.DefaultContractResolver)serializerSettings.ContractResolver;
            contractResolver.IgnoreSerializableAttribute = true;

            _autoResetEvent = new AutoResetEvent(false);

            var isProcessing = false;

            _timer = new Timer((o) =>
            {
                if (!NinjectWebCommon.Started) return;

                if (isProcessing) return;

                isProcessing = true;

                var edxmlManager = NinjectWebCommon.Resolve<IEdxmlManager>();

                if (edxmlManager != null)
                {
                    edxmlManager.ReceiveStatusFile();
                }

                isProcessing = false;

            }, _autoResetEvent, 1000, 200000);
        }

        //protected void Application_BeginRequest()
        //{
        //    if (Request.IsLocal)
        //    {
        //        MiniProfiler.Start();
        //    }

        //}

        //protected void Application_EndRequest()
        //{
        //    MiniProfiler.Stop();
        //}

    }
}
