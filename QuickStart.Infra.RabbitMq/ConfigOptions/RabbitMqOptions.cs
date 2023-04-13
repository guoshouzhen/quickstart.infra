using QuickStart.Infra.Rabbitmq.ConfigOptions;

namespace QuickStart.Infra.RabbitMq.ConfigOptions
{
    /// <summary>
    /// RabbitMq Options, include RabbitMq server options and Exchanges options.
    /// </summary>
    public class RabbitMqOptions
    {
        /// <summary>
        /// RabbitMq server Configuration.
        /// </summary>
        public ServerOptions Servers { get; set; } = new ServerOptions();
        /// <summary>
        /// Exchanges configuration.
        /// </summary>
        public List<ExchangeOptions> Exchanges { get; set; } = new List<ExchangeOptions>();
    }
}
