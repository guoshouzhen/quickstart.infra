using QuickStart.Infra.RabbitMq.Consumer;
using QuickStart.Infra.RabbitMq.Extensions;
using QuickStart.Infra.RabbitMq.Models;

namespace QuickStartDemo.Consumers
{
    public class ConsumerB : AbstractConsumingHostedService
    {
        private readonly ILogger<ConsumerB> _logger;
        public ConsumerB(IServiceProvider serviceProvider, ILogger<ConsumerB> logger)
            : base(serviceProvider)
        {
            _logger = logger;
        }
        protected override string QueueName => "quickstart.test.direct.queue";
        protected override ushort PrefetchCount => 5;

        public override async Task HandleReceivedMessage(MessageConsumingContext messageContext)
        {
            _logger.LogInformation($"消费者BBB收到队列：{QueueName}的消息：{messageContext.Message.GetMessage()}");
            Thread.Sleep(250);
            messageContext.AcknowledgeMessage();
            await Task.Yield();
        }
    }
}
