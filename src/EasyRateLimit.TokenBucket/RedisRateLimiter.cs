using System;
namespace EasyRateLimit.TokenBucket
{
    using EasyRateLimit.Core.Redis;

    public class RedisRateLimiter: ITokenBucketRateLimiter
    {
        private readonly IRedisManager _redisManager;

        public RedisRateLimiter(IRedisManager redisManager)
        {
            this._redisManager = redisManager;
        }
      
        /// <summary>
        /// The lua script.
        /// </summary>
        /// <remarks>
        /// Here use Hashes to store the token information.
        /// </remarks>
        private const string lua_script = @"
    local rate_limit_info = redis.pcall('HMGET', KEYS[1], 'last_mill_second', 'curr_tokens')
    local last_mill_second = rate_limit_info[1]
    local curr_tokens = tonumber(rate_limit_info[2])
    local max_size = ARGV[1]
    local rate = ARGV[2]
    local curr_ts = ARGV[3]
    local request_count = ARGV[4]

    local local_curr_tokens = max_size;

    if (type(last_mill_second) ~= 'boolean'  and last_mill_second ~= nil) then
        local reverse_permits = math.floor(((curr_ts - last_mill_second) / 1000) * rate)
        local expect_curr_tokens = reverse_permits + curr_tokens;
        local_curr_tokens = math.min(expect_curr_tokens, max_size);

        --- not first time to get tokens
        if (reverse_permits > 0) then
            redis.pcall('HSET', KEYS[1], 'last_mill_second', curr_ts)
        end
    else
        redis.pcall('HSET', KEYS[1], 'last_mill_second', curr_ts)
    end

    local result = -1
    if (local_curr_tokens - request_count >= 0) then
        result = 1
        redis.pcall('HSET', KEYS[1], 'curr_tokens', local_curr_tokens - request_count)
    else
        redis.pcall('HSET', KEYS[1], 'curr_tokens', local_curr_tokens)
    end

    return result
";

        public bool Acquire(string key, long total, int perSecond, int requstCount = 1)
        {



            return false;
        }
    }
}
