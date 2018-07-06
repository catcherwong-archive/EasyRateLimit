namespace EasyRateLimit.Core.Redis
{
    using System;
    using StackExchange.Redis;
    using Microsoft.Extensions.Options;

    public class RedisDataBaseProvider : IRedisDataBaseProvider
    {
        /// <summary>
        /// The options.
        /// </summary>
        private readonly RedisOptions _options;

        /// <summary>
        /// The connection multiplexer.
        /// </summary>
        private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EasyCaching.Redis.RedisDatabaseProvider"/> class.
        /// </summary>
        /// <param name="options">Options.</param>
        public RedisDataBaseProvider(IOptionsMonitor<RedisOptions> options)
        {
            _options = options.CurrentValue;
            _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer);
        }

        /// <summary>
        /// Gets the database connection.
        /// </summary>
        public IDatabase GetDatabase()
        {
            return _connectionMultiplexer.Value.GetDatabase();
        }

        /// <summary>
        /// Creates the connection multiplexer.
        /// </summary>
        /// <returns>The connection multiplexer.</returns>
        private ConnectionMultiplexer CreateConnectionMultiplexer()
        {
            if (string.IsNullOrWhiteSpace(_options.Configuration))
            {
                var configurationOptions = new ConfigurationOptions
                {
                    ConnectTimeout = _options.ConnectionTimeout,
                    Password = _options.Password,
                    Ssl = _options.IsSsl,
                    SslHost = _options.SslHost,
                    AllowAdmin = _options.AllowAdmin,
                    DefaultDatabase = _options.Database
                };

                foreach (var endpoint in _options.Endpoints)
                {
                    configurationOptions.EndPoints.Add(endpoint.Host, endpoint.Port);
                }

                return ConnectionMultiplexer.Connect(configurationOptions.ToString());
            }
            else
            {
                return ConnectionMultiplexer.Connect(_options.Configuration);
            }
        }
    }
}
