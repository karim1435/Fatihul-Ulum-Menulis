using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartupAttribute(typeof(ScraBoy.Startup))]
namespace ScraBoy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath =new PathString("/admin/login")
            });
            app.MapSignalR();
        }
    }
}
