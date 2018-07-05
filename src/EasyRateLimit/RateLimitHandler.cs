namespace EasyRateLimit
{
    using System;
    using EasyRateLimit.Redis;
    using StackExchange.Redis;

    /// <summary>
    /// Rate limit handler.
    /// </summary>
    public class RateLimitHandler : IRateLimitHandler
    {
        /// <summary>
        /// The redis manager.
        /// </summary>
        private readonly IRedisManager _redisManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EasyRateLimit.RateLimitHandler"/> class.
        /// </summary>
        /// <param name="redisManager">Redis manager.</param>
        public RateLimitHandler(IRedisManager redisManager)
        {
            this._redisManager = redisManager;
        }

        private const string script = @"local rate_limit_info = redis.pcall('HMGET', KEYS[1], 'last_mill_second', 'curr_permits')
    local last_mill_second = rate_limit_info[1]
    local curr_permits = tonumber(rate_limit_info[2])
    local max_size = ARGV[1]
    local per_second = ARGV[2]
    local curr_mill_second = ARGV[3]
    local request_count = ARGV[4]

    local local_curr_permits = max_size;

    if (type(last_mill_second) ~= 'boolean'  and last_mill_second ~= nil) then
        local reverse_permits = math.floor(((curr_mill_second - last_mill_second) / 1000) * per_second)
        local expect_curr_permits = reverse_permits + curr_permits;
        local_curr_permits = math.min(expect_curr_permits, max_size);

        --- 大于0表示不是第一次获取令牌，也没有向桶里添加令牌
        if (reverse_permits > 0) then
            redis.pcall('HSET', KEYS[1], 'last_mill_second', curr_mill_second)
        end
    else
        redis.pcall('HSET', KEYS[1], 'last_mill_second', curr_mill_second)
    end

    local result = -1
    if (local_curr_permits - request_count >= 0) then
        result = 1
        redis.pcall('HSET', KEYS[1], 'curr_permits', local_curr_permits - request_count)
    else
        redis.pcall('HSET', KEYS[1], 'curr_permits', local_curr_permits)
    end

    return result
";

        /// <summary>
        /// Acquire the specified key, total, perSecond and requstCount.
        /// </summary>
        /// <returns>The acquire.</returns>
        /// <param name="key">Key.</param>
        /// <param name="total">Total.</param>
        /// <param name="perSecond">Per second.</param>
        /// <param name="requstCount">Requst count.</param>
        public bool Acquire(string key, long total, int perSecond, int requstCount = 1)
        {
            var res = _redisManager.ExecLuaScript(script, new RedisKey[] { key }, new RedisValue[]
            {
                total, perSecond, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), requstCount
            });

            var intRes = (int)res;

            return intRes >= 0;
        }
    }
}
