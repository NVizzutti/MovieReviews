using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MovieReviews.Startup))]
namespace MovieReviews
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
