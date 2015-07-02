using Nancy;

namespace NancyFxPlayground.MyApi.Safes
{
    public class SafeController : NancyModule
    {
        readonly SafeViewModelRetriever _viewModelRetriever;
        readonly SafeResourceTranslator _resourceTranslator;

        public SafeController(ISafeRespository safeRespository, IBuildRouteUrls routeUrlBuilder)
        {
            _viewModelRetriever = new SafeViewModelRetriever(safeRespository);
            _resourceTranslator = new SafeResourceTranslator(routeUrlBuilder);

            this.GetHandler<GetSafeRequest>("GetSafe", "/safes/{id}", GetSafe);
        }

        public object GetSafe(GetSafeRequest request)
        {
            var viewModel = _viewModelRetriever.Get(request.Id);

            var resource = _resourceTranslator.Translate(viewModel);

            return Negotiate
                .WithStatusCode(HttpStatusCode.OK)
                .WithModel(resource);
        }
    }
}