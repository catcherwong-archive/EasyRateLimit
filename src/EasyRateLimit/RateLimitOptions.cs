namespace EasyRateLimit
{
    using EasyRateLimit.Redis;

    /// <summary>
    /// Rate limit options.
    /// </summary>
    public class RateLimitOptions
    {
        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>The total.</value>
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets the per sencond.
        /// </summary>
        /// <value>The per sencond.</value>
        public int PerSencond { get; set; }

        /// <summary>
        /// Gets or sets the redis options.
        /// </summary>
        /// <value>The redis options.</value>
        public RedisOptions RedisOptions { get; set; }
    }
}
