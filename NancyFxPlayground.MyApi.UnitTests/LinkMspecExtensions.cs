using Machine.Specifications;
using Newtonsoft.Json.Linq;

namespace NancyFxPlayground.MyApi.UnitTests
{
    public static class LinkMspecExtensions
    {
        public static void ShouldContainLink(this JToken val, string rel, string href)
        {
            val["links"][rel].Property<string>("href").ShouldEqual(href);
        }

        public static void ShouldContainSelfLink(this JToken val, string href)
        {
            val.ShouldContainLink("self", href);
        }
    }
}