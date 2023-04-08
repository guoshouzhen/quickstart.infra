namespace QuickStart.Infra.RabbitMq.Producer
{
    public class ProducingService : IProducingService, IDisposable
    {
        public void Send<TMessage>(TMessage msgObj, string exchangeName, string routingKey) where TMessage : class
        {
            throw new NotImplementedException();
        }

        public Task SendAsync<TMessage>(TMessage msgObj, string exchangeName, string routingKey) where TMessage : class
        {
            throw new NotImplementedException();
        }

        public bool Send<TMessage>(TMessage msgObj, string exchangeName, string routingKey, PublishConfirmTypeEnum publishConfirmTypeEnum = PublishConfirmTypeEnum.NONE) where TMessage : class
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendAsync<TMessage>(TMessage msgObj, string exchangeName, string routingKey, PublishConfirmTypeEnum publishConfirmTypeEnum = PublishConfirmTypeEnum.NONE) where TMessage : class
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
