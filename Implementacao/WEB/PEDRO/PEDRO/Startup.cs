using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PEDRO.Startup))]
namespace PEDRO
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
