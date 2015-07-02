using Nancy;

namespace NancyFxPlayground.MyApi
{
    public interface IProvideNancyContext
    {
        NancyContext Get();
    }
}