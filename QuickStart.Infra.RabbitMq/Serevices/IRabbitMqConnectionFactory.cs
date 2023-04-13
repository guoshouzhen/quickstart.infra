using RabbitMQ.Client;

namespace QuickStart.Infra.RabbitMq.Serevices
{
    /// <summary>
    /// RabbitMq connection factory, should be used in singleton mode.
    /// </summary>
    public interface IRabbitMqConnectionFactory
    {
        /// <summary>
        /// Create a new connection for publishing messages when any connection does not exist.
        /// </summary>
        /// <returns></returns>
        IConnection CreateOrGetProducerConnection();

        /// <summary>
        /// Create a new connection for consuming messages when any connection does not exist.
        /// </summary>
        /// <returns></returns>
        IConnection CreateOrGetConsumerConnection();
    }
}
