using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BeerReviews.Startup))]
namespace BeerReviews
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
