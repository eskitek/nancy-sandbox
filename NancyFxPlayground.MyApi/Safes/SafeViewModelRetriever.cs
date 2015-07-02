namespace NancyFxPlayground.MyApi.Safes
{
    internal class SafeViewModelRetriever
    {
        readonly ISafeRespository _safeRespository;

        public SafeViewModelRetriever(ISafeRespository safeRespository)
        {
            _safeRespository = safeRespository;
        }

        public SafeViewModel Get(int safeId)
        {
            var safe = _safeRespository.Get(safeId);
            return new SafeViewModel
            {
                Id = safe.Id,
                Name = safe.Name
            };
        }
    }
}