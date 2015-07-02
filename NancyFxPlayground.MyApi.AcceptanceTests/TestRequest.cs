using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace NancyFxPlayground.MyApi.AcceptanceTests
{
    public class TestRequest
    {
        string _accessToken;
        string _body;
        string _contentType;
        string _url;
        string _method;
        string _basicAuthCredentials;
        bool _giveMeJson;
        //readonly IAccessTokenRequester _accessTokenRequester;

//        public TestRequest()
//            : this(new OAuthAccessTokenRequester())
//        {
//        }
//
//        public TestRequest(IAccessTokenRequester accessTokenRequester)
//        {
//            _method = "GET";
//            _accessTokenRequester = accessTokenRequester;
//        }

        public TestRequest()
        {
            _method = "GET";
            _giveMeJson = true;
        }

        public TestResponse GetResponse()
        {
            Console.WriteLine("\nIssuing web request...\n{0}\n", this);

            var webRequest = BuildWebRequest();

            var webResponse = GetWebResponse(webRequest);

            var testResponse = BuildTestResponse(webResponse);

            Console.WriteLine("Response\n{0}\n", testResponse);

            return testResponse;
        }

        HttpWebRequest BuildWebRequest()
        {
            var request = WebRequest.Create(_url) as HttpWebRequest;
            request.Method = _method;
            if (!string.IsNullOrEmpty(_basicAuthCredentials))
            {
                request.Headers.Add(HttpRequestHeader.Authorization, BuildBasicAuthHeader(_basicAuthCredentials));
            }
            if (!string.IsNullOrEmpty(_accessToken))
            {
                request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + _accessToken);
            }
            if (request.Method == "POST" || request.Method == "PUT")
            {
                if (_body == null)
                {
                    _body = string.Empty;
                }
                
                var body = new ASCIIEncoding().GetBytes(_body);
                var bodyStream = request.GetRequestStream();
                bodyStream.Write(body, 0, body.Length);
                request.ContentType = _contentType;
            }

            if (_giveMeJson)
            {
                request.Accept = "application/json";
            }
            return request;
        }

        static HttpWebResponse GetWebResponse(HttpWebRequest webRequest)
        {
            HttpWebResponse webResponse;
            try
            {
                webResponse = (HttpWebResponse) webRequest.GetResponse();
            }
            catch (WebException we)
            {
                webResponse = we.Response as HttpWebResponse;
                if (webResponse == null)
                    throw;
            }
            return webResponse;
        }

        static TestResponse BuildTestResponse(HttpWebResponse webResponse)
        {
            try
            {
                return new TestResponse(webResponse);
            }
            finally
            {
                if (webResponse != null)
                {
                    webResponse.Dispose();
                }
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", _method, _url);
        }

        /// <summary>
        /// Tells TestRequest to issue the request using the specified access token
        /// </summary>
        /// <returns></returns>
        /// <remarks>An access token can be obtained using <see cref="T:Secure.TestHelpers.OAuthAccessTokenRequester"/></remarks>
        public TestRequest WithAccessToken(string accessToken)
        {
            _accessToken = accessToken;
            return this;
        }

//        /// <summary>
//        /// Tells TestRequest to issue the request using an access token obtained using the default test user's credentials.
//        /// </summary>
//        /// <returns></returns>
//        public TestRequest WithDefaultAccessToken()
//        {
//            var accessToken = _accessTokenRequester
//                .WithDefaultCredentials()
//                .Get();
//            _accessToken = accessToken;
//            return this;
//        }

        public TestRequest WithBody(string body)
        {
            _body = body;
            return this;
        }

        public TestRequest WithBody(object body)
        {
            _body = JsonConvert.SerializeObject(body);
            return this;
        }

        public TestRequest WithContentType(string contentType)
        {
            _contentType = contentType;
            return this;
        }

        public TestRequest WithUrl(string url)
        {
            _url = url;
            return this;
        }

        public TestRequest WithMethod(string method)
        {
            _method = method;
            return this;
        }

        public TestRequest WithBasicAuthentication(string username, string password)
        {
            _basicAuthCredentials = string.Format("{0}:{1}", username, password);
            return this;
        }

        string BuildBasicAuthHeader(string credentials)
        {
            return "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
        }

        public TestRequest WithAcceptJson()
        {
            _giveMeJson = true;
            return this;
        }
    }
}