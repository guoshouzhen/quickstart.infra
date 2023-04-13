namespace QuickStart.Infra.Rabbitmq.ConfigOptions
{
    /// <summary>
    /// RabbitMq server options.
    /// </summary>
    public class ServerOptions
    {
        /// <summary>
        /// RabbitMq connection endpoints.
        /// </summary>
        public List<RabbitMqEndPoint> EndPoints { get; set; } = new List<RabbitMqEndPoint>();
        /// <summary>
        /// Username that connects to server.
        /// </summary>
        public string UserName { get; set; } = "guest";
        /// <summary>
        /// Password for username.
        /// </summary>
        public string PassWord { get; set; } = "guest";
        /// <summary>
        /// RabbitMq virtual host.
        /// </summary>
        public string VirtualHost { get; set; } = "/";
        /// <summary>
        /// Application connection name. Will be displayed in the RabbitMq management page.
        /// </summary>
        public string ClientProvidedName { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 16);
        /// <summary>
        /// PublishConfirmEnabled option, if true, will wait until  published message have been confirmed.
        /// </summary>
        public bool PublishConfirmEnabled { get; set; } = false;
        /// <summary>
        /// Publish confirm timeout value, responsible for how long to wait confirmation.
        /// </summary>
        public int PublishConfirmTimeout { get; set; } = 5;
        /// <summary>
        /// Timeout for connection attempts.(seconds)
        /// </summary>
        public int RequestedConnectionTimeout { get; set; } = 60;
        /// <summary>
        /// Connection heartbeat.(seconds)
        /// </summary>
        public int RequestedHeartbeat { get; set; } = 60;
        /// <summary>
        /// AutomaticRecoveryEnabled option.
        /// </summary>
        public bool AutomaticRecoveryEnabled { get; set; } = true;
        /// <summary>
        /// TopologyRecoveryEnabled option.
        /// </summary>
        public bool TopologyRecoveryEnabled { get; set; } = true;
        /// <summary>
        /// InitialConnectRetries option.
        /// </summary>
        public int InitialConnectRetries { get; set; } = 5;
        /// <summary>
        /// InitialConnectRetries timeout.(milliSeconds)
        /// </summary>
        public int InitialConnectRetryTimeInterval { get; set; } = 500;
    }

    /// <summary>
    /// RabbitMq server host.
    /// </summary>
    public class RabbitMqEndPoint 
    {
        public string Host { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 5672;
        /// <summary>
        /// Ssloptions.
        /// </summary>
        public ServerSsloptions? Ssl { get; set; }
    }
}
