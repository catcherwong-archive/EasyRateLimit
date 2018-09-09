namespace EasyRateLimit.Counter
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class CounterRateLimitMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ICounterRateLimiter _rateLimiter;

        private readonly ILogger _logger;

        private readonly CounterOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EasyRateLimit.TokenBucket.RateLimitMiddleware"/> class.
        /// </summary>
        /// <param name="next">Next.</param>
        /// <param name="rateLimiter">Rate limiter.</param>
        /// <param name="factory">Factory.</param>
        /// <param name="options">Options.</param>
        public CounterRateLimitMiddleware(
            RequestDelegate next
            , ICounterRateLimiter rateLimiter
            , ILoggerFactory factory
            , IOptions<CounterOptions> options)
        {
            this._next = next;
            this._rateLimiter = rateLimiter;
            this._logger = factory.CreateLogger<CounterRateLimitMiddleware>();
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
                    var canProcess = await HandleRateLimitAsync(httpContext, requestIdentity);

                    if(!canProcess)
                    {
                        httpContext.Response.StatusCode = _options.HttpStatusCode;
                        await httpContext.Response.WriteAsync(_options.Message);
                        return;
                    }
                }
            }

            await _next.Invoke(httpContext);
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
        /// Handles the rate limit async.
        /// </summary>
        /// <returns>The rate limit async.</returns>
        /// <param name="httpContext">Http context.</param>
        /// <param name="requestIdentity">Request identity.</param>
        private Task<bool> HandleRateLimitAsync(HttpContext httpContext, RequestIdentity requestIdentity)
        {
            var limitRule = _options.ClientRules.Where(x => x.ClientId == requestIdentity.ClientId).SelectMany(x => x.CounterRules).FirstOrDefault();

            if (limitRule != null)
            {
                // increment counter
                var counter = _rateLimiter.Process(requestIdentity, limitRule);

                // check if limit is reached
                if (counter.TotalRequests > limitRule.LimitCount)
                {
                    //will be blocked
                    _logger.LogInformation($"Request {requestIdentity.ClientId},{requestIdentity.HttpVerb},{requestIdentity.Path} has been blocked!");
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }
    }
}
