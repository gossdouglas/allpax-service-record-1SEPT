using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(allpax_service_record.Startup))]
namespace allpax_service_record
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
