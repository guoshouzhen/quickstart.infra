using QuickStart.Infra.RabbitMq.ConfigOptions;

namespace QuickStart.Infra.Rabbitmq.ConfigOptions
{
    /// <summary>
    /// Queue options.
    /// </summary>
    public class QueueOptions
    {
        /// <summary>
        /// Queue name.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Routing keys.
        /// </summary>
        public HashSet<string> RoutingKeys { get; set; } = new HashSet<string>();
        /// <summary>
        /// AutoCreateEnabled option.
        /// </summary>
        public bool AutoCreateEnabled { get; set; } = false;
        /// <summary>
        /// Queue settings.
        /// </summary>
        public QueueSettings? Settings { get; set; } = new QueueSettings();
    }

    /// <summary>
    /// Queue settings.
    /// </summary>
    public class QueueSettings 
    {
        /// <summary>
        /// Durable option.
        /// </summary>
        public bool Durable { get; set; } = true;
        /// <summary>
        /// Exclusive option.
        /// </summary>
        public bool Exclusive { get; set; } = false;
        /// <summary>
        /// AutoDelete option.
        /// </summary>
        public bool AutoDelete { get; set; } = false;
        /// <summary>
        /// Additional arguments.
        /// </summary>
        public List<ArgumentOptions> Arguements { get; set; }
    }
}
