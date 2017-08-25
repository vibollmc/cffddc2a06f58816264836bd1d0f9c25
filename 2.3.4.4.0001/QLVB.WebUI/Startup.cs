using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Owin;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(QLVB.WebUI.Startup))]
namespace QLVB.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //http://www.tomdupont.net/2014/01/dependency-injection-with-signalr-and.html

            var unityHubActivator = new MvcHubActivator();

            GlobalHost.DependencyResolver.Register(
                typeof(IHubActivator),
                () => unityHubActivator);


            ConfigureAuth(app);
            app.MapSignalR();
        }
        public class MvcHubActivator : IHubActivator
        {
            public IHub Create(HubDescriptor descriptor)
            {
                return (IHub)DependencyResolver.Current
                    .GetService(descriptor.HubType);
            }
        }
    }
}
