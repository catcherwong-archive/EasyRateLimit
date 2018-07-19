namespace EasyRateLimit.TokenBucket
{
    using System.Collections.Generic;

    //{ "client_id_header":"rl-header","rules":[{"client_id":"your id","rules":[{"total":100,"rate":0.1}]}] }

    /// <summary>
    /// Token bucket options.
    /// </summary>
    public class TokenBucketOptions
    {               
        /// <summary>
        /// Gets or sets the client identifier header.
        /// </summary>
        /// <value>The client identifier header.</value>
        public string ClientIdHeader { get; set; }

        /// <summary>
        /// Gets or sets the client rules.
        /// </summary>
        /// <value>The client rules.</value>
        public List<ClientRule> ClientRules { get; set; }
    }

    /// <summary>
    /// Client rule.
    /// </summary>
    public class ClientRule
    {
        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>The client identifier.</value>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the token bucket rules.
        /// </summary>
        /// <value>The token bucket rules.</value>
        public List<TokenBucketRule> TokenBucketRules { get; set; }
    }

    /// <summary>
    /// Token bucket rule.
    /// </summary>
    public class TokenBucketRule
    {
        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>The total.</value>
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets the rate.
        /// </summary>
        /// <value>The rate.</value>
        public decimal Rate { get; set; }
    }
}
