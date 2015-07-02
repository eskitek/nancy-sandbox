using System;
using System.IO;
using Nancy.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NancyFxPlayground.MyApi.UnitTests
{
    internal static class JsonExtensions
    {
        public static JContainer Json(this BrowserResponseBodyWrapper value)
        {
            return value.AsString().ToJson();
        }

        static JContainer ToJson(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            if (IsJsonArray(value))
            {
                return ParseJArray(value);
            }

            return ParseJObject(value);
        }

        static JContainer ParseJArray(string value)
        {
            JsonReader reader = new JsonTextReader(new StringReader(value));
            reader.DateParseHandling = DateParseHandling.None;
            return JArray.Load(reader);
        }

        static JObject ParseJObject(string value)
        {
            JsonReader reader = new JsonTextReader(new StringReader(value));
            reader.DateParseHandling = DateParseHandling.None;
            return JObject.Load(reader);
        }

        static bool IsJsonArray(string value)
        {
            return value.StartsWith("[");
        }

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