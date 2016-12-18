using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(quido.Startup))]
namespace quido
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
