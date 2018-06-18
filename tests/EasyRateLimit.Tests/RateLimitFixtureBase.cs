namespace EasyRateLimit.Tests
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using System;
    using System.Net.Http;
    using EasyRateLimit;

    public class RateLimitFixtureBase : IDisposable
    {
        private readonly TestServer _server;

        public RateLimitFixtureBase(string baseUri)
        {
            var builder = new WebHostBuilder().ConfigureServices(c=>
            {
                c.AddRateLimit(x => 
                {
                    x.Total = 2;
                    x.PerSencond = 1;
                    x.RedisOptions = new Redis.RedisOptions
                    {
                        Configuration = "localhost",
                    };
                });

            }).Configure(x=>
            {
                x.UseRateLimiting();
            });
            _server = new TestServer(builder);

            Client = _server.CreateClient();
            Client.BaseAddress = new Uri(baseUri);
        }

        public HttpClient Client { get; }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }

    public class RateLimitFixture : RateLimitFixtureBase
    {
        public RateLimitFixture() : base("http://localhost:5000")
        {
        }
    }
}
