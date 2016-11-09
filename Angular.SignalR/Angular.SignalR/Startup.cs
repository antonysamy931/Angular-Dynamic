using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Angular.SignalR.Startup))]
namespace Angular.SignalR
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}