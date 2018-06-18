using System.Threading.Tasks;

namespace EasyRateLimit
{
    /// <summary>
    /// Rate limit handler.
    /// </summary>
    public interface IRateLimitHandler
    {
        /// <summary>
        /// Adds the token async.
        /// </summary>
        /// <returns>The token async.</returns>
        /// <param name="name">Name.</param>
        /// <param name="len">Length.</param>
        Task<bool> AddTokenAsync(string name, int len);

        /// <summary>
        /// Gets the token async.
        /// </summary>
        /// <returns>The token async.</returns>
        /// <param name="name">Name.</param>
        /// <param name="total">Total.</param>
        /// <param name="perSecond">Per second.</param>
        Task<bool> GetTokenAsync(string name, long total, int perSecond);
    }
}
