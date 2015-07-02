using System;
using Machine.Specifications;
using Moq;
using Nancy;
using Nancy.Testing;
using NancyFxPlayground.MyApi.Safes;
using It = Machine.Specifications.It;

namespace NancyFxPlayground.MyApi.UnitTests.Safes
{
    class GetSafeTests
    {
        [Subject("Get Safe")]
        class When_safe_exists
        {
            Establish context = () =>
            {
                _safeId = 123;
                _safeName = "The Safe Name";
                _hostName = "www.yahoo.com";
                _getSafeRoute = string.Format("/safes/{0}", _safeId);

                _safeRepositoryMock.Setup(repo => repo.Get(_safeId))
                    .Returns(ASafeWith(_safeId, _safeName));

                var testBootstrapper = new TestBootstrapper();
                testBootstrapper.OverrideDependency<ISafeRespository>(_safeRepositoryMock.Object);

                _request = new Browser(testBootstrapper);
            };

            Because of = () =>
            {
                _response = _request.Get(_getSafeRoute, with =>
                {
                    with.HttpRequest();
                    with.HostName(_hostName);
                    with.Accept("application/json");
                });

                PrintResponse(_response);
            };

            It should_return_a_200_ok = () => { _response.StatusCode.ShouldEqual(HttpStatusCode.OK); };

            It should_return_the_safe_self_link = () => { _response.Body.Json().ShouldContainSelfLink(BuildUrl(_hostName, _getSafeRoute)); };

            It should_return_the_safe_id = () => { _response.Body.Json().Property<int>("id").ShouldEqual(_safeId); };

            It should_return_the_safe_name = () => { _response.Body.Json().Property<string>("name").ShouldEqual(_safeName); };

            static Browser _request;
            static BrowserResponse _response;
            static int _safeId;
            static string _safeName;
            static string _getSafeRoute;
            static Mock<ISafeRespository> _safeRepositoryMock = new Mock<ISafeRespository>();
            static string _hostName;
        }

        static Safe ASafeWith(int safeId, string safeName)
        {
            return new Safe
            {
                Id = safeId,
                Name = safeName
            };
        }

        static void PrintResponse(BrowserResponse browserResponse)
        {
            Console.WriteLine("Response Body:\n" + browserResponse.Body.AsString());
        }

        static string BuildUrl(string hostName, string routePath)
        {
            return Uri.UriSchemeHttp + "://" + hostName + routePath;
        }
    }
}