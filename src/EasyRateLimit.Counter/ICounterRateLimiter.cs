using System;
namespace EasyRateLimit.Counter
{
    public interface ICounterRateLimiter
    {
        RateLimitCounter Process(RequestIdentity requestIdentity,CounterRule rule);
    }

    public class CounterRateLimter : ICounterRateLimiter
    {
        private static readonly object _processLocker = new object();

        private readonly ICounterStore _store;

        public CounterRateLimter(ICounterStore store)
        {
            this._store = store;
        }
            

        public RateLimitCounter Process(RequestIdentity requestIdentity, CounterRule rule)
        {
            var counter = new RateLimitCounter
            {
                Timestamp = DateTime.UtcNow,
                TotalRequests = 1
            };

            var counterId = GetRequestIdentityKey(requestIdentity, rule);

            // serial reads and writes
            lock (_processLocker)
            {
                var entry = _store.Get(counterId);
                if (entry.HasValue)
                {
                    // entry has not expired
                    if (entry.Value.Timestamp.AddSeconds(rule.Period) >= DateTime.UtcNow)
                    {
                        // increment request count
                        var totalRequests = entry.Value.TotalRequests + 1;

                        // deep copy
                        counter = new RateLimitCounter
                        {
                            Timestamp = entry.Value.Timestamp,
                            TotalRequests = totalRequests
                        };
                    }
                }

                // stores: id (string) - timestamp (datetime) - total_requests (long)
                _store.Set(counterId, counter, TimeSpan.FromSeconds( rule.Period));
            }

            return counter;

        }


        /// <summary>
        /// Gets the request identity key.
        /// </summary>
        /// <returns>The request identity key.</returns>
        /// <param name="requestIdentity">Request identity.</param>
        private string GetRequestIdentityKey(RequestIdentity requestIdentity,CounterRule rule)
        {
            var key = $"{requestIdentity.HttpVerb}:{requestIdentity.Path}:{requestIdentity.ClientId}:{rule.Period}";

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
