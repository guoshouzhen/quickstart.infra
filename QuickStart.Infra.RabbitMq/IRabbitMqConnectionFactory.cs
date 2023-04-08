using RabbitMQ.Client;

namespace QuickStart.Infra.Rabbitmq
{
    public interface IRabbitMqConnectionFactory
    {
        IConnection CreateOrGetProducerConnection();
        IConnection CreateOrGetConsumerConnection();
    }
}
