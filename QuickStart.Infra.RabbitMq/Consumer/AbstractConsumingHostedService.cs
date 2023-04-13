using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuickStart.Infra.RabbitMq.Models;
using QuickStart.Infra.RabbitMq.Serevices;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace QuickStart.Infra.RabbitMq.Consumer
{
    /// <summary>
    /// Asbtract comsumer.
    /// All Comsumers should be inherited from this, and register into server collections as hosted service.
    /// </summary>
    public abstract class AbstractConsumingHostedService : IHostedService, IDisposable
    {
        private bool _disposed;
        /// <summary>
        /// Channel for current consumer.
        /// </summary>
        private readonly IModel _channel;

        private readonly ILogger<AbstractConsumingHostedService> _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public AbstractConsumingHostedService(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetRequiredService<ILogger<AbstractConsumingHostedService>>();
            _channel = serviceProvider.GetRequiredService<IRabbitMqConnectionFactory>().CreateOrGetConsumerConnection().CreateModel();
        }

        /// <summary>
        /// PrefetchSize, default value is 0.
        /// </summary>
        protected virtual uint PrefetchSize { get; } = 0;
        /// <summary>
        /// Listened queue name by current consumer.
        /// </summary>
        protected abstract string QueueName { get;}
        /// <summary>
        /// PrefetchCount, if specified, will enabled fair consumption pattern.
        /// </summary>
        protected virtual ushort PrefetchCount { get; } = 0;

        /// <summary>
        /// Method implementation method in IHostedService.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            //Start the comsumer.
            StartConsuming();
            _logger.LogInformation($"The RabbitMq Consumer listening on: {QueueName}");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Start a comsumer.
        /// </summary>
        private void StartConsuming()
        {
            EnsureChannelAndParams();
            if (PrefetchCount > 0) 
            {
                _channel.BasicQos(PrefetchSize, PrefetchCount, false);
            }
            
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (sender, eventArgs) =>
            {
                var context = new MessageConsumingContext(eventArgs, AckAction, RejectAction);
                await HandleReceivedMessage(context).ConfigureAwait(false);
            };
            _channel.BasicConsume(QueueName, false, consumer);
        }

        /// <summary>
        /// Check channel and params.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        private void EnsureChannelAndParams()
        {
            if (_channel == null)
            {
                throw new ArgumentException("Channel must be not null.", nameof(_channel));
            }

            if (string.IsNullOrWhiteSpace(QueueName)) 
            {
                throw new ArgumentNullException("Consuming queue name must be not null.", nameof(QueueName));
            }
        }

        /// <summary>
        /// Acknowledge action.
        /// </summary>
        /// <param name="eventArgs"></param>
        private void AckAction(BasicDeliverEventArgs eventArgs) => _channel.BasicAck(eventArgs.DeliveryTag, false);

        /// <summary>
        /// Reject action.
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <param name="isReQueue"></param>
        private void RejectAction(BasicDeliverEventArgs eventArgs, bool isReQueue = false) => _channel.BasicNack(eventArgs.DeliveryTag, false, isReQueue);

        /// <summary>
        /// Handle logic for received message.
        /// </summary>
        /// <param name="messageContext"></param>
        /// <returns></returns>
        public abstract Task HandleReceivedMessage(MessageConsumingContext messageContext);

        /// <summary>
        /// Method implementation method in IHostedService.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        /// <summary>
        /// Dispose resource.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (_channel?.IsOpen == true)
                {
                    _channel?.Close();
                }
                _channel?.Dispose();
            }

            _disposed = true;
        }
    }
}
