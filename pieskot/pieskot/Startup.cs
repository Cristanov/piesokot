using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NaSpacerDo.Startup))]
namespace NaSpacerDo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
