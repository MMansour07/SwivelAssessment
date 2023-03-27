using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Swivel.Webclient.Startup))]
namespace Swivel.Webclient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
