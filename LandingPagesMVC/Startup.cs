using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LandingPagesMVC.Startup))]
namespace LandingPagesMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
