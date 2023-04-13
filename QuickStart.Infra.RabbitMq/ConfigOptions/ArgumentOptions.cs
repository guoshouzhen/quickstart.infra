using QuickStart.Infra.RabbitMq.Enums;

namespace QuickStart.Infra.RabbitMq.ConfigOptions
{
    /// <summary>
    /// Options for exchange or queue additional arguments.
    /// </summary>
    public class ArgumentOptions
    {
        /// <summary>
        /// Argument key.
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Argument value.
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// Argument value type.
        /// </summary>
        public ArgumentTypeEnum Type { get; set; }
    }
}
