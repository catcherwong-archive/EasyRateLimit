namespace EasyRateLimit.Core.Redis
{    
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
    }
}
