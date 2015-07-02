using Nancy;
using Nancy.TinyIoc;

namespace NancyFxPlayground.MyApi
{
    public class MyApiBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            container.Register<IProvideNancyContext>((c, o) => new NancyContextProvider(context));
            container.Register<IBuildRouteUrls, NancyRouteUrlBuilder>();
        }
    }
}