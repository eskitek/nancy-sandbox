using System.Collections.Generic;
using System.Linq;
using Secure.Api.Resources;

namespace NancyFxPlayground.MyApi.Resources
{
    public class LinksResource : Dictionary<string, LinkResource>
    {
        public void Add(string rel, string href)
        {
            Add(rel, new LinkResource { Href = href });
        }

        /// <summary>
        /// Adds a self link
        /// </summary>
        /// <param name="selfUri">The self URI</param>
        public void AddSelfLink(string selfUri)
        {
            Add("self", new LinkResource { Href = selfUri });
        }

        public LinkResource Get(string rel)
        {
            if(ContainsKey(rel))
                return this.Single(l => l.Key == rel).Value;

            return null;
        }
    }
}