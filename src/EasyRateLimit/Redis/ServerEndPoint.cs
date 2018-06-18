namespace EasyRateLimit.Redis
{
    /// <summary>
    /// Server end point.
    /// </summary>
    public class ServerEndPoint
    {
        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>The host.</value>
        public string Host { get; set; }
    }
}