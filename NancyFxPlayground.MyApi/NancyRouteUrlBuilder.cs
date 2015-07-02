using Nancy.Linker;

namespace NancyFxPlayground.MyApi
{
    public class NancyRouteUrlBuilder : IBuildRouteUrls
    {
        readonly IResourceLinker _resourceLinker;
        readonly IProvideNancyContext _nancyContextProvider;

        public NancyRouteUrlBuilder(IResourceLinker resourceLinker, IProvideNancyContext nancyContextProvider)
        {
            _resourceLinker = resourceLinker;
            _nancyContextProvider = nancyContextProvider;
        }

        public string BuildAbsoluteUri(string routeName, object parameters = null)
        {
            return _resourceLinker.BuildAbsoluteUri(_nancyContextProvider.Get(), routeName, parameters).AbsoluteUri;
        }
    }
}