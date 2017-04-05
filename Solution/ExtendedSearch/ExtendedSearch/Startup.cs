using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExtendedSearch.Startup))]
namespace ExtendedSearch
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           
        }
    }
}
