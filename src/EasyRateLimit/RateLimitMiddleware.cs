namespace EasyRateLimit
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RateLimitMiddleware> _logger;
        private readonly IRateLimitHandler _handler;

        private readonly RateLimitOptions _options;

        private static object lockObj = new object();

        public RateLimitMiddleware(RequestDelegate next
                                   , ILogger<RateLimitMiddleware> logger
                                   , IRateLimitHandler handler
                                   , IOptions<RateLimitOptions> optionsAcc)
        {
            this._next = next;
            this._logger = logger;
            this._handler = handler;
            this._options = optionsAcc.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            // compute identity from request
            var identity = SetIdentity(httpContext);

            var key = ComputeCounterKey(identity);

            var canProcess = true;

            lock (lockObj)
            {
                canProcess = _handler.GetTokenAsync(key, _options.Total, _options.PerSencond).Result;
            }

            if (canProcess)
            {
                await _next.Invoke(httpContext);
                return;
            }

            httpContext.Response.StatusCode = 429;
            await httpContext.Response.WriteAsync("up to limit");

            await _next.Invoke(httpContext);
        }

        public virtual RequestIdentity SetIdentity(HttpContext httpContext)
        {
            return new RequestIdentity
            {
                Path = httpContext.Request.Path.ToString().ToLowerInvariant(),
                HttpVerb = httpContext.Request.Method.ToLowerInvariant(),
            };
        }

        private string ComputeCounterKey(RequestIdentity requestIdentity)
        {
            var key = $"{requestIdentity.HttpVerb}_{requestIdentity.Path}";

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
