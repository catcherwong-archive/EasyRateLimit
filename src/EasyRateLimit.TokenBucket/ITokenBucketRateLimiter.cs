namespace EasyRateLimit.TokenBucket
{
    using System;

    public interface ITokenBucketRateLimiter
    {
        /// <summary>
        /// Acquire redis lock
        /// </summary>
        /// <param name="key"></param>
        /// <param name="total"></param>
        /// <param name="rate"></param>
        /// <param name="requstCount"></param>
        /// <returns></returns>
        bool Acquire(string key, long total, double rate, int requstCount = 1);
    }
}
