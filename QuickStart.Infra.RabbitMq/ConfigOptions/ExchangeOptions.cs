using QuickStart.Infra.RabbitMq.ConfigOptions;
using QuickStart.Infra.RabbitMq.Enums;

namespace QuickStart.Infra.Rabbitmq.ConfigOptions
{
    /// <summary>
    /// Exchange options.
    /// </summary>
    public class ExchangeOptions
    {
        /// <summary>
        /// Exchange Name.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Exchange behavior type enum.
        /// </summary>
        public ExchangeTypeEnum Type { get; set; } = ExchangeTypeEnum.PRODUCING;
        /// <summary>
        /// AutoCreateEnabled option.
        /// </summary>
        public bool AutoCreateEnabled { get; set; } = false;
        /// <summary>
        /// Exchange settings.
        /// </summary>
        public ExchangeSettings Settings { get; set; } = new ExchangeSettings();
        /// <summary>
        /// Queues that are binded with exchange.
        /// </summary>
        public List<QueueOptions> Queues { get; set; } = new List<QueueOptions>();
    }

    /// <summary>
    /// Settings for exchange.
    /// </summary>
    public class ExchangeSettings 
    {
        /// <summary>
        /// Exchange type. eg: direct fanout topic
        /// </summary>
        public string Type { get; set; } = "direct";
        /// <summary>
        /// Durable option.
        /// </summary>
        public bool Durable { get; set; } = true;
        /// <summary>
        /// AutoDelete option.
        /// </summary>
        public bool AutoDelete { get; set; } = false;
        /// <summary>
        /// DisableAutoAck option.
        /// </summary>
        public bool DisableAutoAck { get; set; } = true;
        /// <summary>
        /// DeadLetter exchange name.
        /// </summary>
        public string DeadLetterExchange { get; set; } = "default.deadletter.exchange";
        /// <summary>
        /// DeadLetter exchange type.
        /// </summary>
        public string DeadLetterExchangeType { get; set; } = "direct";
        /// <summary>
        /// Additional arguments.
        /// </summary>
        public List<ArgumentOptions> Arguements { get; set; }
    }
}
