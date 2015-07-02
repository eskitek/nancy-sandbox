namespace NancyFxPlayground.MyApi.Safes
{
    internal class SafeResourceTranslator
    {
        readonly IBuildRouteUrls _urlBuilder;

        public SafeResourceTranslator(IBuildRouteUrls urlBuilder)
        {
            _urlBuilder = urlBuilder;
        }

        public SafeResource Translate(SafeViewModel safeViewModel)
        {
            var safeResource = new SafeResource
            {
                Id = safeViewModel.Id,
                Name = safeViewModel.Name
            };

            safeResource.Links.AddSelfLink(_urlBuilder.BuildAbsoluteUri("GetSafe", new { id = safeResource.Id }));

            return safeResource;
        }
    }
}