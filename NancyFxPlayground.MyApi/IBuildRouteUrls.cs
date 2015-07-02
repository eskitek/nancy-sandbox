namespace NancyFxPlayground.MyApi
{
    public interface IBuildRouteUrls
    {
        string BuildAbsoluteUri(string routeName, object parameters = null);
    }
}