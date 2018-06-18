namespace EasyRateLimit
{    
    using Microsoft.AspNetCore.Builder;
    
    public static class RateLimitMiddlewareExtensions
    {
        /// <summary>
        /// Uses the rate limiting.
        /// </summary>
        /// <returns>The rate limiting.</returns>
        /// <param name="builder">Builder.</param>
        public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RateLimitMiddleware>();
        }
    }
}
