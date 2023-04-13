using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QuickStart.Infra.Rabbitmq.ConfigOptions;
using QuickStart.Infra.RabbitMq.ConfigOptions;
using QuickStart.Infra.RabbitMq.Enums;
using QuickStart.Infra.RabbitMq.Serevices;
using QuickStart.Infra.RabbitMq.Utils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace QuickStart.Infra.RabbitMq.Producer
{
    public class ProducingService : IProducingService, IDisposable
    {
        /// <summary>
        /// Expire time for the temp queue.
        /// </summary>
        const long QUEUE_EXPIRATION_MILLSECOND_TIME = 60000;

        /// <summary>
        /// Message publishing channel.
        /// </summary>
        private readonly IModel _channel;
        private bool _disposed;

        private readonly ILogger<ProducingService> _logger;
        private readonly IEnumerable<ExchangeOptions>? _exchanges;
        private readonly ServerOptions _serverOptions;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rabbitMqConnectionFactory"></param>
        /// <param name="logger"></param>
        /// <param name="options"></param>
        public ProducingService(IRabbitMqConnectionFactory rabbitMqConnectionFactory, ILogger<ProducingService> logger, IOptions<RabbitMqOptions> options)
        {
            _channel = rabbitMqConnectionFactory.CreateOrGetProducerConnection().CreateModel();
            _logger = logger;
            _exchanges = options.Value.Exchanges?.Where(x => x.Type == ExchangeTypeEnum.PRODUCING || x.Type == ExchangeTypeEnum.BOTH);
            _serverOptions = options.Value.Servers;

            //Register the events.
            _channel.CallbackException += CallbackExceptionHandler;
            _channel.BasicRecoverOk += BasicRecoverOkHandler;

        }

        /// <inheritdoc/>
        public bool Send<TMessage>(TMessage msgObj, string exchangeName, string routingKey, long millisecondsDelay = -1) where TMessage : class
        {
            string json = JsonUtil.Object2Json(msgObj);
            return SendJson(json, exchangeName, routingKey, millisecondsDelay);
        }

        public async Task<bool> SendAsync<TMessage>(TMessage msgObj, string exchangeName, string routingKey, long millisecondsDelay = -1)
            where TMessage : class => await Task.Run(() => Send(msgObj, exchangeName, routingKey, millisecondsDelay)).ConfigureAwait(false);

        /// <inheritdoc/>
        public bool SendJson(string message, string exchangeName, string routingKey, long millisecondsDelay = -1)
        {
            EnsureChannelNotNullAndExchangesExist(exchangeName);
            var props = CreateJsonProperties();

            if (millisecondsDelay > 0) 
            {
                var deadLetterExchange = GetDeadLetterExchange(exchangeName);
                var delayedQueueName = CreateDelayedQueue(exchangeName, deadLetterExchange, routingKey, millisecondsDelay);
                return Send(Encoding.UTF8.GetBytes(message), deadLetterExchange, delayedQueueName);
            }

            return Send(Encoding.UTF8.GetBytes(message), exchangeName, routingKey, props);
        }

        public async Task<bool> SendJsonAsync(string message, string exchangeName, string routingKey, long millisecondsDelay = -1)
            => await Task.Run(() => SendJson(message, exchangeName, routingKey, millisecondsDelay)).ConfigureAwait(false);

        /// <inheritdoc/>
        public bool SendString(string message, string exchangeName, string routingKey, long millisecondsDelay = -1)
        {
            EnsureChannelNotNullAndExchangesExist(exchangeName);
            var props = CreateProperties();

            if (millisecondsDelay > 0)
            {
                var deadLetterExchange = GetDeadLetterExchange(exchangeName);
                var delayedQueueName = CreateDelayedQueue(exchangeName, deadLetterExchange, routingKey, millisecondsDelay);
                return Send(Encoding.UTF8.GetBytes(message), deadLetterExchange, delayedQueueName);
            }

            return Send(Encoding.UTF8.GetBytes(message), exchangeName, routingKey, props);
        }

        public async Task<bool> SendStringAsync(string message, string exchangeName, string routingKey, long millisecondsDelay = -1)
            => await Task.Run(() => SendString(message, exchangeName, routingKey, millisecondsDelay)).ConfigureAwait(false);

        /// <summary>
        /// Disposing current channel.
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
                    _logger.LogInformation($"Producer channel:{_channel} has been closed.");
                }
                _channel?.Dispose();
            }
            _disposed = true;
        }

        /// <summary>
        /// Check Channel and exchange.
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <exception cref="ArgumentException"></exception>
        private void EnsureChannelNotNullAndExchangesExist(string exchangeName)
        {
            if (_channel == null)
            {
                throw new ArgumentException("Channel must be not null.", nameof(_channel));
            }
            if (string.IsNullOrWhiteSpace(exchangeName) == false)
            {
                _channel.ExchangeDeclarePassive(exchangeName);
            }
        }

        /// <summary>
        /// Create json BasicProperties.
        /// </summary>
        /// <returns></returns>
        private IBasicProperties CreateJsonProperties() 
        {
            IBasicProperties props = _channel.CreateBasicProperties();
            props.Persistent = true;
            props.ContentType = "application/json";
            return props;
        }

        /// <summary>
        /// Create a common BasicProperties
        /// </summary>
        /// <returns></returns>
        private IBasicProperties CreateProperties() 
        {
            IBasicProperties props = _channel.CreateBasicProperties();
            props.Persistent = true;
            return props;
        }

        /// <summary>
        /// Get the deadletter exchange name of an exchange.
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private string GetDeadLetterExchange(string exchangeName) 
        {
            var exchange = _exchanges?.Where(x => x.Name == exchangeName).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(exchange?.Settings?.DeadLetterExchange)) 
            {
                throw new ArgumentException($"The deadletterexchange of exchange:{exchangeName}, must be specified.");
            }
            return exchange.Settings.DeadLetterExchange;
        }

        /// <summary>
        /// Declare the temp delayed queue and binds it with the deadletter exchange.
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <param name="deadLetterExchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="millisecondsDelay"></param>
        /// <returns></returns>
        private string CreateDelayedQueue(string exchangeName, string deadLetterExchange, string routingKey, long millisecondsDelay) 
        {
            string delayedQueueName = $"{routingKey}_delayed_queue_{millisecondsDelay}";
            var arguements = new Dictionary<string, object>();
            arguements["x-dead-letter-exchange"] = exchangeName;
            arguements["x-dead-letter-routing-key"] = routingKey;
            arguements["x-message-ttl"] = millisecondsDelay;
            arguements["x-expires"] = millisecondsDelay + QUEUE_EXPIRATION_MILLSECOND_TIME;

            _channel.QueueDeclare(delayedQueueName, true, false, false, arguements);
            _channel.QueueBind(delayedQueueName, deadLetterExchange, delayedQueueName);
            return delayedQueueName;
        }

        /// <summary>
        /// Send data to exchange or queue.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="exchangeName"></param>
        /// <param name="routingKey"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private bool Send(ReadOnlyMemory<byte> bytes, string exchangeName, string routingKey, IBasicProperties properties)
        {
            if (_serverOptions.PublishConfirmEnabled)
            {
                if (_serverOptions.PublishConfirmTimeout <= 0)
                {
                    throw new ArgumentException("The publish confirm timeout value must be a positive number.", nameof(_serverOptions.PublishConfirmTimeout));
                }
                _channel.ConfirmSelect();
            }
            _channel.BasicPublish(exchangeName, routingKey, properties, bytes);
            if (_serverOptions.PublishConfirmEnabled)
            {
                return _channel.WaitForConfirms(TimeSpan.FromSeconds(_serverOptions.PublishConfirmTimeout));
            }
            return true;
        }

        private void CallbackExceptionHandler(object? sender, CallbackExceptionEventArgs e)
        {
            if (e?.Exception != null)
            {
                _logger.LogError($"Exception occurs in producer channel：{e.Exception}");
            }
        }

        private void BasicRecoverOkHandler(object? sender, EventArgs e)
        {
            if (e != null) 
            {
                _logger.LogInformation("Producer connection has been reestablished.");
            }
        }
    }
}
