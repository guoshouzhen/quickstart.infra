namespace QuickStart.Infra.Rabbitmq.ConfigOptions
{
    public class ServerOptions
    {
        public List<RabbitMqEndPoint> EndPoints { get; set; } = new List<RabbitMqEndPoint>();
        public string UserName { get; set; } = "guest";
        public string PassWord { get; set; } = "guest";
        public string VirtualHost { get; set; } = "/";
        public string ClientProvidedName { get; set; } = string.Empty;
        public int RequestedConnectionTimeout { get; set; } = 60;
        public int RequestedHeartbeat { get; set; } = 60;
        public bool AutomaticRecoveryEnabled { get; set; } = true;
        public bool TopologyRecoveryEnabled { get; set; } = true;
        public int InitialConnectRetries { get; set; } = 5;
        public int InitialConnectRetryTimeInterval { get; set; } = 500;
    }

    public class RabbitMqEndPoint 
    {
        public string Host { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 5672;
        public ServerSsloptions? Ssl { get; set; }
    }
}
