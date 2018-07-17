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
        
        public RateLimitMiddleware(RequestDelegate next)
        {
            this._next = next;            
        }

        public async Task Invoke(HttpContext httpContext)
        {            
            await _next.Invoke(httpContext);
        }

       

    }
}
