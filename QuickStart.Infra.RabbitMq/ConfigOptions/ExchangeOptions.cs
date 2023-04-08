namespace QuickStart.Infra.Rabbitmq.ConfigOptions
{
    public class ExchangeOptions
    {
        public string Name { get; set; } = string.Empty;
        public bool AutoCreateEnabled { get; set; } = false;
        public ExchangeSettings? Settings { get; set; }
    }

    public class ExchangeSettings 
    {
        public string Type { get; set; } = string.Empty;
        public bool Durable { get; set; } = true;
        public bool AutoDelete { get; set; } = false;
        public bool DisableAutoAck { get; set; } = true;
        public string[] Arguements { get; set; } = new string[0];
    }
}
