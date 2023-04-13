using QuickStart.Infra.RabbitMq.Consumer;
using QuickStart.Infra.RabbitMq.Extensions;
using QuickStart.Infra.RabbitMq.Models;

namespace QuickStartDemo.Consumers
{
    public class ConsumerA : AbstractConsumingHostedService
    {
        private readonly ILogger<ConsumerA> _logger;
        public ConsumerA(IServiceProvider serviceProvider, ILogger<ConsumerA> logger) 
            : base(serviceProvider) 
        {
            _logger = logger;
        }

        protected override string QueueName => "quickstart.test.direct.queue";
        protected override ushort PrefetchCount => 5;

        public override async Task HandleReceivedMessage(MessageConsumingContext messageContext)
        {
            messageContext.RejectMessage(true);
            //_logger.LogInformation($"消费者AAA收到队列：{QueueName}的消息：{messageContext.Message.GetMessage()}");
            //Thread.Sleep(500);
            //messageContext.AcknowledgeMessage();
            await Task.Yield();
        }
    }
}
