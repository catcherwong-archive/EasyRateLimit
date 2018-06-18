namespace EasyRateLimit
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using EasyRateLimit.Redis;

    /// <summary>
    /// Rate limit service collection extensions.
    /// </summary>
    public static class RateLimitServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the rate limit.
        /// </summary>
        /// <returns>The rate limit.</returns>
        /// <param name="services">Services.</param>
        /// <param name="providerAction">Provider action.</param>
        public static IServiceCollection AddRateLimit(
            this IServiceCollection services,
            Action<RateLimitOptions> providerAction)
        {            
            services.AddOptions();
            services.Configure(providerAction);

            services.TryAddSingleton<IRedisDataBaseProvider, RedisDataBaseProvider>();
            services.TryAddSingleton<IRedisManager, RedisManager>();
            services.TryAddSingleton<IRateLimitHandler, RateLimitHandler>();

            return services;
        } 
    }
}
