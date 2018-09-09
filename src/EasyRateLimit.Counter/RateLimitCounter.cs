namespace EasyRateLimit.Counter
{
    using System;

    public struct RateLimitCounter
    {
        /// <summary>
        /// Gets or sets the timestamp(Request begin time).
        /// </summary>
        /// <value>The timestamp.</value>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the total requests.
        /// </summary>
        /// <value>The total requests.</value>
        public long TotalRequests { get; set; }
    }
}
