using Nancy;

namespace NancyFxPlayground.MyApi
{
    public class NancyContextProvider : IProvideNancyContext
    {
        readonly NancyContext _nancyContext;

        public NancyContextProvider(NancyContext nancyContext)
        {
            _nancyContext = nancyContext;
        }

        public NancyContext Get()
        {
            return _nancyContext;
        }
    }
}