using System.Threading.Tasks;

namespace EasyRateLimit
{
    /// <summary>
    /// Rate limit handler.
    /// </summary>
    public interface IRateLimitHandler
    {
        /// <summary>
        /// Acquire the specified key, total, perSecond and requstCount.
        /// </summary>
        /// <returns>The acquire.</returns>
        /// <param name="key">Key.</param>
        /// <param name="total">Total.</param>
        /// <param name="perSecond">Per second.</param>
        /// <param name="requstCount">Requst count.</param>
        bool Acquire(string key, long total, int perSecond, int requstCount = 1);

    }
}
