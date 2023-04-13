namespace QuickStart.Infra.RabbitMq.Enums
{
    /// <summary>
    /// Argument value type enum for exchange or queue additional arguments.
    /// </summary>
    public enum ArgumentTypeEnum
    {
        /// <summary>
        /// Source type.
        /// </summary>
        SOURCE,
        /// <summary>
        /// Will be tryed converted to Int32.
        /// </summary>
        INT,
        /// <summary>
        /// Will be tryed converted to Int64.
        /// </summary>
        LONG
    }
}
