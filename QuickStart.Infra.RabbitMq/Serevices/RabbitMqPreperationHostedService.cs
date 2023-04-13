using Microsoft.Extensions.Hosting;

namespace QuickStart.Infra.RabbitMq.Serevices
{
    /// <summary>
    /// Hosted service, used to preper rabiitmq.
    /// </summary>
    public class RabbitMqPreperationHostedService : IHostedService
    {
        private readonly IRabbitMqPreparationService _rabbitMqPreparationService;
        public RabbitMqPreperationHostedService(IRabbitMqPreparationService rabbitMqPreparationService)
        {
            _rabbitMqPreparationService = rabbitMqPreparationService;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _rabbitMqPreparationService.PrepareRabbitMq();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
