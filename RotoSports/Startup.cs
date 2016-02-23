using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RotoSports.Startup))]
namespace RotoSports
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
