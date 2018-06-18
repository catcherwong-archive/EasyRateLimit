namespace EasyRateLimit
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EasyRateLimit.Redis;
    using StackExchange.Redis;

    /// <summary>
    /// Token bucket handler.
    /// </summary>
    public class TokenBucketHandler : IRateLimitHandler
    {
        /// <summary>
        /// The redis manager.
        /// </summary>
        private readonly IRedisManager _redisManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EasyRateLimit.TokenBucketHandler"/> class.
        /// </summary>
        /// <param name="redisManager">Redis manager.</param>
        public TokenBucketHandler(IRedisManager redisManager)
        {
            this._redisManager = redisManager;
        }

        /// <summary>
        /// Adds the token async.
        /// </summary>
        /// <returns>The token async.</returns>
        /// <param name="name">Name.</param>
        /// <param name="len">Length.</param>
        public async Task<bool> AddTokenAsync(string name, int len)
        {
            var size = await _redisManager.GetLenAsnyc(name);
            //can not up to limit
            if (size + 1 < len)
            {
                var values = new List<RedisValue>();
                values.Add(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                await _redisManager.PushAsync(name, values.ToArray());
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the token async.
        /// </summary>
        /// <returns>The token async.</returns>
        /// <param name="name">Name.</param>
        /// <param name="total">Total.</param>
        /// <param name="perSecond">Per second.</param>
        public async Task<bool> GetTokenAsync(string name, long total, int perSecond)
        {
            //can use

            var isFirst = await _redisManager.SetIfNotExists($"{name}:ex");

            if (isFirst)
            {
                var list = new List<RedisValue>();

                for (int i = 0; i < perSecond; i++)
                {
                    list.Add(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                }

                await _redisManager.PushAsync(name, list.ToArray());
            }

            var size = await _redisManager.GetLenAsnyc(name);

            var lastTime = await _redisManager.PopAsync(name);

            if (lastTime <= 0)
            {
                return false;
            }
            else
            {
                //use lua!
                DateTimeOffset last = DateTimeOffset.FromUnixTimeMilliseconds(lastTime);

                var second = DateTimeOffset.UtcNow.Subtract(last).Seconds;

                var calNeedToAdd = perSecond * second;

                var realNeedToAdd = 0L;

                if (calNeedToAdd + size > total)
                {
                    realNeedToAdd = total - size;
                }
                else
                {
                    realNeedToAdd = calNeedToAdd;
                }

                //Add tokens
                var tokens = new List<RedisValue>();
                for (int i = 0; i < realNeedToAdd; i++)
                {
                    tokens.Add(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                }

                await _redisManager.PushAsync(name, tokens.ToArray());

                return true;
            }
        }
    }
}
