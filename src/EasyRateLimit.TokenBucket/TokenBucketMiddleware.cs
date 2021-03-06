namespace EasyRateLimit.TokenBucket
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ITokenBucketRateLimiter _rateLimiter;

        private readonly ILogger _logger;

        private readonly TokenBucketOptions _options;

        private static object lockObj = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EasyRateLimit.TokenBucket.RateLimitMiddleware"/> class.
        /// </summary>
        /// <param name="next">Next.</param>
        /// <param name="rateLimiter">Rate limiter.</param>
        /// <param name="factory">Factory.</param>
        /// <param name="options">Options.</param>
        public RateLimitMiddleware(
            RequestDelegate next
            , ITokenBucketRateLimiter rateLimiter
            , ILoggerFactory factory
            , IOptions<TokenBucketOptions> options)
        {
            this._next = next;
            this._rateLimiter = rateLimiter;
            this._logger = factory.CreateLogger<RateLimitMiddleware>();
            this._options = options.Value;
        }

        /// <summary>
        /// Invoke the specified httpContext.
        /// </summary>
        /// <returns>The invoke.</returns>
        /// <param name="httpContext">Http context.</param>
        public async Task Invoke(HttpContext httpContext)
        {
            //build request identity
            var requestIdentity = BuildRequestIdentity(httpContext);

            if (_options.ClientWhitelist != null && _options.ClientWhitelist.Any())
            {
                if (!_options.ClientWhitelist.Contains(requestIdentity.ClientId))
                {
                    await HandleRateLimitAsync(httpContext, requestIdentity);
                    return;
                }
            }

            await _next.Invoke(httpContext);
            return;
        }

        /// <summary>
        /// Builds the request identity.
        /// </summary>
        /// <returns>The request identity.</returns>
        /// <param name="httpContext">Http context.</param>
        private RequestIdentity BuildRequestIdentity(HttpContext httpContext)
        {
            var clientId = "anonymous";
            if (httpContext.Request.Headers.Keys.Contains(_options.ClientIdHeader, StringComparer.CurrentCultureIgnoreCase))
            {
                clientId = httpContext.Request.Headers[_options.ClientIdHeader].First();
            }

            return new RequestIdentity
            {
                Path = httpContext.Request.Path.ToString().ToLowerInvariant(),
                HttpVerb = httpContext.Request.Method.ToLowerInvariant(),
                ClientId = clientId
            };
        }

        /// <summary>
        /// Gets the request identity key.
        /// </summary>
        /// <returns>The request identity key.</returns>
        /// <param name="requestIdentity">Request identity.</param>
        private string GetRequestIdentityKey(RequestIdentity requestIdentity)
        {
            var key = $"{requestIdentity.HttpVerb}:{requestIdentity.Path}:{requestIdentity.ClientId}";

            var idBytes = System.Text.Encoding.UTF8.GetBytes(key);

            byte[] hashBytes;

            using (var algorithm = System.Security.Cryptography.SHA1.Create())
            {
                hashBytes = algorithm.ComputeHash(idBytes);
            }

            return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
        }

        /// <summary>
        /// Handles the rate limit async.
        /// </summary>
        /// <returns>The rate limit async.</returns>
        /// <param name="httpContext">Http context.</param>
        /// <param name="requestIdentity">Request identity.</param>
        private async Task HandleRateLimitAsync(HttpContext httpContext, RequestIdentity requestIdentity)
        {
            var limitRule = _options.ClientRules.Where(x => x.ClientId == requestIdentity.ClientId).SelectMany(x => x.TokenBucketRules).FirstOrDefault();

            if (limitRule != null)
            {
                var canProcess = true;
                //hash
                var key = GetRequestIdentityKey(requestIdentity);

                lock (lockObj)
                {
                    //read from 
                    canProcess = _rateLimiter.Acquire(key, limitRule.Total, limitRule.Rate);
                }

                if (!canProcess)
                {
                    //will be blocked
                    _logger.LogInformation($"Request {requestIdentity.ClientId},{requestIdentity.HttpVerb},{requestIdentity.Path} has been blocked!");

                    httpContext.Response.StatusCode = _options.HttpStatusCode;
                    await httpContext.Response.WriteAsync(_options.Message);
                }
            }
        }
    }
}
