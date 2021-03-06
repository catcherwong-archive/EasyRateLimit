﻿namespace EasyRateLimit
{
    /// <summary>
    /// Request identity.
    /// </summary>
    public class RequestIdentity
    {
        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the http verb.
        /// </summary>
        /// <value>The http verb.</value>
        public string HttpVerb { get; set; }
    }
}
