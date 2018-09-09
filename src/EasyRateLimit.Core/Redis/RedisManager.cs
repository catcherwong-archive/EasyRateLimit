namespace EasyRateLimit.Core.Redis
{
    using System;
    using StackExchange.Redis;

    public class RedisManager : IRedisManager
    {
        /// <summary>
        /// The database.
        /// </summary>
        private readonly IDatabase _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EasyRateLimit.Redis.RedisManager"/> class.
        /// </summary>
        /// <param name="provider">Provider.</param>
        public RedisManager(IRedisDataBaseProvider provider)
        {
            this._database = provider.GetDatabase();
        }
              
        /// <summary>
        /// Execs the lua script.
        /// </summary>
        /// <returns>The lua script.</returns>
        /// <param name="luaScript">Lua script.</param>
        /// <param name="redisKeys">Redis keys.</param>
        /// <param name="redisValues">Redis values.</param>
        public RedisResult ExecLuaScript(string luaScript,RedisKey[] redisKeys = null,RedisValue[] redisValues = null)
        {
            return _database.ScriptEvaluate(luaScript, redisKeys, redisValues);
        }

        /// <summary>
        /// Get the specified cacheKey.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="cacheKey">Cache key.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public RedisValue Get(string cacheKey)
        {
            return _database.StringGet(cacheKey);
        }

        /// <summary>
        /// Remove the specified cacheKey.
        /// </summary>
        /// <param name="cacheKey">Cache key.</param>
        public void Remove(string cacheKey)
        {
            _database.KeyDelete(cacheKey);
        }

        /// <summary>
        /// Set the specified cacheKey, cacheValue and expiration.
        /// </summary>
        /// <param name="cacheKey">Cache key.</param>
        /// <param name="cacheValue">Cache value.</param>
        /// <param name="expiration">Expiration.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public void Set<T>(string cacheKey, T cacheValue, TimeSpan expiration)
        {
            _database.StringSet(cacheKey,Newtonsoft.Json.JsonConvert.SerializeObject(cacheValue),expiration);
        }
    }
}
