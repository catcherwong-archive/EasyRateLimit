using System;
using EasyRateLimit.Core.Redis;

namespace EasyRateLimit.Counter
{
    public interface ICounterStore
    {        
        RateLimitCounter? Get(string key);
        void Remove(string key);
        void Set(string key, RateLimitCounter counter, TimeSpan expirationTime);
    }

    public class RedisCounterStore : ICounterStore
    {
        private readonly IRedisManager _manager;

        public RedisCounterStore(IRedisManager manger)
        {
            this._manager = manger;
        }

        public RateLimitCounter? Get(string key)
        {
            var res = _manager.Get(key);
            if(res.HasValue) return Newtonsoft.Json.JsonConvert.DeserializeObject<RateLimitCounter>(res);
            else return null;
        }

        public void Remove(string key)
        {
            _manager.Remove(key);
        }

        public void Set(string key, RateLimitCounter counter, TimeSpan expiration)
        {
            _manager.Set(key, counter, expiration);
        }
    }
}
