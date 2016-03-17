using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExitPagesMVC.Startup))]
namespace ExitPagesMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
