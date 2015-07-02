using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NancyFxPlayground.MyApi.AcceptanceTests
{
    public class TestResponse
    {
        static string _rawBody;

        public TestResponse(HttpWebResponse response)
        {
            StatusCode = response.StatusCode;
            Body = SafeGetJsonBody(response);
        }

        public JContainer Body { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }

        static JContainer SafeGetJsonBody(HttpWebResponse response)
        {
            try
            {
                return GetJsonBody(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while getting json from the response body:\n" + ex + "\n");
                return null;
            }
        }

        static JContainer GetJsonBody(HttpWebResponse response)
        {
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream == null) return null;

                using (var reader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    _rawBody = reader.ReadToEnd();

                    return ParseJson(_rawBody);
                }
            }
        }

        static JContainer ParseJson(string value)
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

        public override string ToString()
        {
            return string.Format("Status: {0}\nBody: {1}",
                StatusCode,
                PrintableBody);
        }

        string PrintableBody
        {
            get
            {
                //Setting a max length prevents huge responses from slowing down tests due to marathon logging
                const int maxLength = 1000;

                if (Body != null)
                    return Body.ToString().Left(maxLength);

                if (!string.IsNullOrEmpty(_rawBody))
                    return _rawBody.Left(maxLength);

                return "[empty]";
            }
        }
    }

    internal static class StringExtensions
    {
        internal static string Left(this string value, int length)
        {
            return new string(value.Take(length).ToArray());
        }
    }
}