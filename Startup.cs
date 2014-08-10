using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Careers.Startup))]
namespace Careers
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
