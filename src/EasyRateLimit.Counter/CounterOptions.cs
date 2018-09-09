namespace EasyRateLimit.Counter
{
    using System.Collections.Generic;

    //{ "ClientIdHeader":"rl-header","clientrules":[{"ClientId":"your id","CounterRules":[{"Period":1,"LimitCount":100}]}] }

    public class CounterOptions
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
        /// Gets or sets the counter rules.
        /// </summary>
        /// <value>The counter rules.</value>
        public List<CounterRule> CounterRules { get; set; } = new List<CounterRule>();
    }

    /// <summary>
    /// Counter rule.
    /// </summary>
    public class CounterRule
    {
        /// <summary>
        /// Gets or sets the period(Seconds).
        /// </summary>
        /// <value>The period.</value>
        public int Period { get; set; }

        /// <summary>
        /// Maximum number of requests that a client can make in a defined period
        /// </summary>
        public long LimitCount { get; set; }
    }
}
