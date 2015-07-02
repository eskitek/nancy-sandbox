using System.Net;
using Machine.Specifications;

namespace NancyFxPlayground.MyApi.AcceptanceTests.Safes
{
    class GetSafeTests
    {
        [Subject("Get Safe")]
        class When_safe_exists
        {
            Establish context = () =>
            {
                _request = new TestRequest()
                    .WithMethod("GET")
                    .WithAcceptJson()
                    .WithUrl("http://localhost:60155/safes/1");
            };

            Because of = () =>
            {
                _response = _request
                    .GetResponse();
            };

            It should_return_a_200_ok = () => { _response.StatusCode.ShouldEqual(HttpStatusCode.OK); };

            It should_return_the_safe_self_link = () => { _response.Body.ShouldContainSelfLink("http://localhost:60155/safes/1"); };

            It should_return_the_safe_id = () => { _response.Body.Property<int>("id").ShouldEqual(1); };

            It should_return_the_safe_name = () => { _response.Body.Property<string>("name").ShouldEqual("Safe 1"); };

            static TestRequest _request;
            static TestResponse _response;
        }
    }
}
