using System;
using Newtonsoft.Json.Linq;

namespace NancyFxPlayground.MyApi.AcceptanceTests
{
    internal static class JsonExtensions
    {
        public static T Property<T>(this JToken value, string propertyName)
        {
            try
            {
                var jToken = value[propertyName];
                return jToken.Value<T>();
            }
            catch (Exception)
            {
                throw new Exception(string.Format("There is no json property named '{0}'", propertyName));
            }
        }
    }
}