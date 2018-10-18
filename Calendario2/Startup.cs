using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Calendario2.Startup))]
namespace Calendario2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
