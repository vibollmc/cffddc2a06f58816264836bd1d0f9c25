using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DLTD.Web.Main.Startup))]
namespace DLTD.Web.Main
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
