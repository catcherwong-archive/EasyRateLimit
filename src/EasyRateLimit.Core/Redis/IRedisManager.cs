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
    }
}
