//using System;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Xunit;

//namespace EasyRateLimit.Tests
//{
//    public class RateLimitTests: IClassFixture<RateLimitFixture>
//    {        
//        public RateLimitTests(RateLimitFixture fixture)
//        {
//            Client = fixture.Client;
//        }

//        public HttpClient Client { get; }

//        [Theory]
//        [InlineData("GET")]
//        public async Task Test1(string verb)
//        {
//            // Arrange
//            int responseStatusCode = 0;

//            // Act    
//            for (int i = 0; i < 4; i++)
//            {
//                var request = new HttpRequestMessage(new HttpMethod(verb), "/api/values");
//                var response = await Client.SendAsync(request);
//                responseStatusCode = (int)response.StatusCode;
//            }

//            // Assert
//            Assert.Equal(429, responseStatusCode);
//        }

//        [Theory]
//        [InlineData("GET")]
//        public async Task Test2(string verb)
//        {
//            // Arrange
//            int responseStatusCode = 0;

//            // Act    

//            var request = new HttpRequestMessage(new HttpMethod(verb), "/api/values/1");
//            var response = await Client.SendAsync(request);
//            responseStatusCode = (int)response.StatusCode;


//            // Assert
//            Assert.NotEqual(429, responseStatusCode);
//        }

//    }
//}
