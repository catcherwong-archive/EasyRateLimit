namespace EasyRateLimit.Redis
{
    using System.Threading.Tasks;
    using StackExchange.Redis;
    
    public interface IRedisManager
    {
        /// <summary>
        /// Pushs the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        Task<long> PushAsync(string key, RedisValue[] value);

        /// <summary>
        /// Gets the length asnyc.
        /// </summary>
        /// <returns>The length asnyc.</returns>
        /// <param name="key">Key.</param>
        Task<long> GetLenAsnyc(string key);

        /// <summary>
        /// Pops the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="key">Key.</param>
        Task<long> PopAsync(string key);

        /// <summary>
        /// Sets if not exists.
        /// </summary>
        /// <returns>The if not exists.</returns>
        /// <param name="key">Key.</param>
        Task<bool> SetIfNotExists(string key);
    }
}
