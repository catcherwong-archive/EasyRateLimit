namespace EasyRateLimit
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using EasyRateLimit.Redis;

    public static class RateLimitServiceCollectionExtensions
    {
        public static IServiceCollection AddRateLimit(
            this IServiceCollection services,
            Action<RateLimitOptions> providerAction)
        {            
            services.AddOptions();
            services.Configure(providerAction);

            services.TryAddSingleton<IRedisDataBaseProvider, RedisDataBaseProvider>();
            services.TryAddSingleton<IRedisManager, RedisManager>();
            services.TryAddSingleton<IRateLimitHandler, TokenBucketHandler>();

            return services;
        } 
    }
}
