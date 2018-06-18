namespace EasyRateLimit.Redis
{
    using System.Threading.Tasks;
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
        /// Pushs the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public Task<long> PushAsync(string key, RedisValue[] value)
        {
            return _database.ListLeftPushAsync(key, value);
        }

        /// <summary>
        /// Gets the length asnyc.
        /// </summary>
        /// <returns>The length asnyc.</returns>
        /// <param name="key">Key.</param>
        public Task<long> GetLenAsnyc(string key)
        {

            return _database.ListLengthAsync(key);
        }

        /// <summary>
        /// Pops the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="key">Key.</param>
        public async Task<long> PopAsync(string key)
        {
            var res = await _database.ListRightPopAsync(key);

            if (res.HasValue)
            {
                return (long)res;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Sets if not exists.
        /// </summary>
        /// <returns>The if not exists.</returns>
        /// <param name="key">Key.</param>
        public Task<bool> SetIfNotExists(string key)
        {
            return _database.StringSetAsync(key, 1, when: When.NotExists);
        }
    }
}
