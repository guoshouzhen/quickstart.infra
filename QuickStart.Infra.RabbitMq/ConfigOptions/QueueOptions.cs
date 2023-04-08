namespace QuickStart.Infra.Rabbitmq.ConfigOptions
{
    public class QueueOptions
    {
        public string Name { get; set; } = string.Empty;
        public HashSet<string> RoutingKeys { get; set; } = new HashSet<string>();
        public bool AutoCreateEnabled { get; set; } = false;
        public QueueSettings? Settings { get; set; }
    }

    public class QueueSettings 
    {
        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }
        public string[] Arguements { get; set; } = new string[0];
    }
}
