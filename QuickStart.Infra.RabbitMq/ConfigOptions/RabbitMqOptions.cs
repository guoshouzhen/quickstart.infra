using QuickStart.Infra.Rabbitmq.ConfigOptions;

namespace QuickStart.Infra.RabbitMq.ConfigOptions
{
    public class RabbitMqOptions
    {
        public ServerExchangeOptions? Producer { get; set; }
        public ServerExchangeOptions? Consumer { get; set; }
    }

    public class ServerExchangeOptions 
    {
        public ServerOptions Servers { get; set; }
        public ExchangeOptions Exchanges { get; set; }
    }
}
