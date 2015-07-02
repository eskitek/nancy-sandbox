using NancyFxPlayground.MyApi.Resources;

namespace NancyFxPlayground.MyApi.Safes
{
    public class SafeResource
    {
        public SafeResource()
        {
            Links = new LinksResource();
        }
        public LinksResource Links { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}