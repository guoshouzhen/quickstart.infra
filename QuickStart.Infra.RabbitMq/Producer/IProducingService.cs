namespace QuickStart.Infra.RabbitMq.Producer
{
    public interface IProducingService
    {
        void Send<TMessage>(TMessage msgObj, string exchangeName, string routingKey) where TMessage : class;
        Task SendAsync<TMessage>(TMessage msgObj, string exchangeName, string routingKey) where TMessage : class;

        bool Send<TMessage>(TMessage msgObj, string exchangeName, string routingKey, PublishConfirmTypeEnum publishConfirmTypeEnum = PublishConfirmTypeEnum.NONE) where TMessage : class;
        Task<bool> SendAsync<TMessage>(TMessage msgObj, string exchangeName, string routingKey, PublishConfirmTypeEnum publishConfirmTypeEnum = PublishConfirmTypeEnum.NONE) where TMessage : class;
    }
}
