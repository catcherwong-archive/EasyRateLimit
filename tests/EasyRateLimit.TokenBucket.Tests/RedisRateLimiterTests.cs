namespace EasyRateLimit.TokenBucket.Tests
{
    using NSubstitute;
    using EasyRateLimit.TokenBucket;
    using EasyRateLimit.Core.Redis;
    using Microsoft.Extensions.Options;
    using Xunit;
    using StackExchange.Redis;
    using System;

    public class RedisRateLimiterTests
    {
        private readonly RedisRateLimiter _limiter;
        private readonly IDatabase _database;
        public RedisRateLimiterTests()
        {
            var options = Substitute.For<IOptionsSnapshot<RedisOptions>>();
            var redisOptions = new RedisOptions();
            redisOptions.Endpoints.Add(new ServerEndPoint() { Host = "localhost", Port = 6379 });
            options.Value.Returns(redisOptions);
            var redisDataBaseProvider = new RedisDataBaseProvider(options);
            var manager = new RedisManager(redisDataBaseProvider);
            _database = redisDataBaseProvider.GetDatabase();
            _limiter = new RedisRateLimiter(manager);
        }

        [Fact]
        public void First_Request_Should_Acquire_Succeed()
        {
            var key = Guid.NewGuid().ToString("N");
            var flag = _limiter.Acquire(key, 100, 0.1);
            Assert.True(flag);
            var val = _database.HashGet(key, "curr_tokens");
            Assert.Equal(99, Convert.ToInt32(val.ToString()));
        }
    }
}
