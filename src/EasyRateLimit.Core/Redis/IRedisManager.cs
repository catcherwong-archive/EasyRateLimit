namespace EasyRateLimit.Core.Redis
{    
    using StackExchange.Redis;
    
    public interface IRedisManager
    {     
        /// <summary>
        /// Execs the lua script.
        /// </summary>
        /// <returns>The lua script.</returns>
        /// <param name="luaScript">Lua script.</param>
        /// <param name="redisKeys">Redis keys.</param>
        /// <param name="redisValues">Redis values.</param>
        RedisResult ExecLuaScript(string luaScript, RedisKey[] redisKeys = null, RedisValue[] redisValues = null);

        /// <summary>
        /// Set the specified cacheKey, cacheValue and expiration.
        /// </summary>
        /// <param name="cacheKey">Cache key.</param>
        /// <param name="cacheValue">Cache value.</param>
        /// <param name="expiration">Expiration.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        void Set<T>(string cacheKey,T cacheValue,System.TimeSpan expiration);

        /// <summary>
        /// Remove the specified cacheKey.
        /// </summary>
        /// <param name="cacheKey">Cache key.</param>
        void Remove(string cacheKey);

        /// <summary>
        /// Get the specified cacheKey.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="cacheKey">Cache key.</param>
        RedisValue Get(string cacheKey);

    }
}
