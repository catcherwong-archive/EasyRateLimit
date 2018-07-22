namespace EasyRateLimit.TokenBucket
{
    using System.Collections.Generic;

    //{ "ClientIdHeader":"rl-header","clientrules":[{"ClientId":"your id","TokenBucketRules":[{"total":100,"rate":0.1}]}] }

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
        /// Gets or sets the client whitelist.
        /// </summary>
        /// <value>The client whitelist.</value>
        public List<string> ClientWhitelist { get; set; }

        /// <summary>
        /// Gets or sets the http status code.
        /// </summary>
        /// <value>The http status code.</value>
        public int HttpStatusCode { get; set; } = 429;

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; } = "To Many Request.";

        /// <summary>
        /// Gets or sets the client rules.
        /// </summary>
        /// <value>The client rules.</value>
        public List<ClientRule> ClientRules { get; set; } = new List<ClientRule>();
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
        public List<TokenBucketRule> TokenBucketRules { get; set; } = new List<TokenBucketRule>();
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
        public int Total { get; set; } = 100;

        /// <summary>
        /// Gets or sets the rate.
        /// </summary>
        /// <value>The rate.</value>
        public double Rate { get; set; } = 0.1;
    }
}
