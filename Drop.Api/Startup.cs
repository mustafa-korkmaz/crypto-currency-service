using Microsoft.Owin;
using MoneyMarket.Api;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace MoneyMarket.Api
{
    /// <summary>
    /// app startup configs
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
