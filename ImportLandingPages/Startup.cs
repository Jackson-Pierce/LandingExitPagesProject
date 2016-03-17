using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ImportLandingPages.Startup))]
namespace ImportLandingPages
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
