using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ImportExitPages.Startup))]
namespace ImportExitPages
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
