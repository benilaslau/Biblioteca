using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LoginBiblioteca.Startup))]
namespace LoginBiblioteca
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
