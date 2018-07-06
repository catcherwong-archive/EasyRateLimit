namespace EasyRateLimit.Core.Redis
{
    using System;
    using StackExchange.Redis;

    public interface IRedisDataBaseProvider
    {
        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <returns>The database.</returns>
        IDatabase GetDatabase();
    }
}
