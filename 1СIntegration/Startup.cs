using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_1СIntegration.Startup))]
namespace _1СIntegration
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
