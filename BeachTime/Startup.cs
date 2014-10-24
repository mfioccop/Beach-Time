using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BeachTime.Startup))]
namespace BeachTime
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
