namespace QuickStart.Infra.RabbitMq.Producer
{
    /// <summary>
    /// Publish message service.
    /// </summary>
    public interface IProducingService
    {
        /// <summary>
        /// Send a .NET Object Message
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="msgObj"></param>
        /// <param name="exchangeName"></param>
        /// <param name="routingKey"></param>
        /// <param name="millisecondsDelay"></param>
        /// <returns>If the PublishConfirmEnabled in configuration is false, always return true.</returns>
        bool Send<TMessage>(TMessage msgObj, string exchangeName, string routingKey, long millisecondsDelay = -1) where TMessage : class;
        Task<bool> SendAsync<TMessage>(TMessage msgObj, string exchangeName, string routingKey, long millisecondsDelay = -1) where TMessage : class;

        /// <summary>
        /// Send a json Message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exchangeName"></param>
        /// <param name="routingKey"></param>
        /// <param name="millisecondsDelay"></param>
        /// <returns>If the publishConfirmEnabled in configuration is false, always return true.</returns>
        bool SendJson(string message, string exchangeName, string routingKey, long millisecondsDelay = -1);
        Task<bool> SendJsonAsync(string message, string exchangeName, string routingKey, long millisecondsDelay = -1);

        /// <summary>
        /// Send a string Message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exchangeName"></param>
        /// <param name="routingKey"></param>
        /// <param name="millisecondsDelay"></param>
        /// <returns>If the publishConfirmEnabled in configuration is false, always return true.</returns>
        bool SendString(string message, string exchangeName, string routingKey, long millisecondsDelay = -1);
        Task<bool> SendStringAsync(string message, string exchangeName, string routingKey, long millisecondsDelay = -1);
    }
}
