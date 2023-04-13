namespace QuickStart.Infra.RabbitMq.Enums
{
    /// <summary>
    /// Exchange type enum.
    /// </summary>
    public enum ExchangeTypeEnum
    {
        /// <summary>
        /// Only be used to publish message.
        /// </summary>
        PRODUCING,
        /// <summary>
        /// Only be used to consuming message.
        /// </summary>
        CONSUMING,
        /// <summary>
        /// Both.
        /// </summary>
        BOTH,
    }
}
