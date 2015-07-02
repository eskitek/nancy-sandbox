using Owin;

namespace NancyFxPlayground.MyApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
        }
    }
}