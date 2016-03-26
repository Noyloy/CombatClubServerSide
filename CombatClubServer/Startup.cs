using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CombatClubServer.Startup))]
namespace CombatClubServer
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
