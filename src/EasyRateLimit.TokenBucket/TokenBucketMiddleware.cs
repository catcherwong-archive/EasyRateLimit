namespace EasyRateLimit.TokenBucket
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ITokenBucketRateLimiter _rateLimiter;

        private readonly ILogger _logger;

        private static object lockObj = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EasyRateLimit.TokenBucket.RateLimitMiddleware"/> class.
        /// </summary>
        /// <param name="next">Next.</param>
        /// <param name="rateLimiter">Rate limiter.</param>
        /// <param name="factory">Factory.</param>
        public RateLimitMiddleware(
            RequestDelegate next
            , ITokenBucketRateLimiter rateLimiter
            , ILoggerFactory factory)
        {
            this._next = next;
            this._rateLimiter = rateLimiter;
            this._logger = factory.CreateLogger<RateLimitMiddleware>();
        }

        /// <summary>
        /// Invoke the specified httpContext.
        /// </summary>
        /// <returns>The invoke.</returns>
        /// <param name="httpContext">Http context.</param>
        public async Task Invoke(HttpContext httpContext)
        {
            //build request identity
            var requestIdentity = new RequestIdentity
            {
                Path = httpContext.Request.Path.ToString().ToLowerInvariant(),
                HttpVerb = httpContext.Request.Method.ToLowerInvariant(),
                //auth
                Name = httpContext.User?.Identity?.Name
            };
            //hash
            var key = GetRequestIdentityKey(requestIdentity);

            var canProcess = true;

            lock(lockObj)
            {
                //read from cache
                canProcess = _rateLimiter.Acquire(key, 100, 0.1);
            }

            if(canProcess)
            {
                await _next.Invoke(httpContext);
                return;
            }
            else
            {
                //will be blocked
                _logger.LogInformation($"Request {requestIdentity.HttpVerb},{requestIdentity.Path} has been blocked!");

                httpContext.Response.StatusCode = 429;
                await httpContext.Response.WriteAsync("up to limit");
                return;
            }
        }

        /// <summary>
        /// Gets the request identity key.
        /// </summary>
        /// <returns>The request identity key.</returns>
        /// <param name="requestIdentity">Request identity.</param>
        private string GetRequestIdentityKey(RequestIdentity requestIdentity)
        {
            var key = $"{requestIdentity.HttpVerb}:{requestIdentity.Path}:{requestIdentity.Name}";

            var idBytes = System.Text.Encoding.UTF8.GetBytes(key);

            byte[] hashBytes;

            using (var algorithm = System.Security.Cryptography.SHA1.Create())
            {
                hashBytes = algorithm.ComputeHash(idBytes);
            }

            return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
        }

    }
}
