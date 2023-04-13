using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QuickStart.Infra.Rabbitmq.ConfigOptions;
using QuickStart.Infra.RabbitMq.ConfigOptions;
using QuickStart.Infra.RabbitMq.Enums;
using QuickStart.Infra.RabbitMq.Models;
using QuickStart.Infra.RabbitMq.Utils;
using RabbitMQ.Client;

namespace QuickStart.Infra.RabbitMq.Serevices
{
    public class RabbitMqPreparationService : IRabbitMqPreparationService
    {
        private readonly ILogger<RabbitMqPreparationService> _logger;
        private readonly List<ExchangeOptions> _exchanges;
        private readonly IRabbitMqConnectionFactory _rabbitMqConnectionFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="options"></param>
        /// <param name="rabbitMqConnectionFactory"></param>
        public RabbitMqPreparationService(ILogger<RabbitMqPreparationService> logger, IOptions<RabbitMqOptions> options, IRabbitMqConnectionFactory rabbitMqConnectionFactory)
        {
            _logger = logger;
            _rabbitMqConnectionFactory = rabbitMqConnectionFactory;
            _exchanges = options.Value.Exchanges;
        }

        /// <inheritdoc/>
        public void PrepareRabbitMq()
        {
            var producingExchanges = _exchanges.Where(x => x.Type == ExchangeTypeEnum.PRODUCING || x.Type == ExchangeTypeEnum.BOTH);
            var consumingExchanges = _exchanges.Where(x => x.Type == ExchangeTypeEnum.CONSUMING || x.Type == ExchangeTypeEnum.BOTH);
            if (producingExchanges != null && producingExchanges.Count() > 0)
            {
                //Create a connection to declare exchanges and queues, and ready for publishing messages.
                var tmpChannel = _rabbitMqConnectionFactory.CreateOrGetProducerConnection().CreateModel();
                PrepareExchangesAndQueues(tmpChannel, producingExchanges);
                _logger.LogInformation("The RabbitMq is ready for publishing message.");
                CloseChannel(tmpChannel);
            }
            if (consumingExchanges != null && consumingExchanges.Count() > 0)
            {
                //Create a connection to declare exchanges and queues, and ready for consuming messages.
                var tmpChannel = _rabbitMqConnectionFactory.CreateOrGetConsumerConnection().CreateModel();
                PrepareExchangesAndQueues(tmpChannel, consumingExchanges);
                _logger.LogInformation("The RabbitMq is ready for consuming message.");
                CloseChannel(tmpChannel);
            }
        }

        /// <summary>
        /// Declare the exchanges and queues.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="exchanges"></param>
        private void PrepareExchangesAndQueues(IModel channel, IEnumerable<ExchangeOptions> exchanges)
        {
            //Filter the deadletter exchanges.
            var deadLetterExchanges = exchanges
                .Where(x => x.Settings != null)
                .Select(x => x.Settings)
                .Select(x => new DeadLetterExchange() { Name = x.DeadLetterExchange, Type = x.DeadLetterExchangeType })
                .Distinct(new DeadLetterExchangeEqualityComparer());
            PrepareExchangesAndQueues(channel, exchanges, deadLetterExchanges);
        }

        /// <summary>
        /// Declare the exchanges and queues.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="exchanges"></param>
        /// <param name="deadLetterExchanges"></param>
        private void PrepareExchangesAndQueues(IModel channel, IEnumerable<ExchangeOptions> exchanges, IEnumerable<DeadLetterExchange> deadLetterExchanges)
        {
            if (deadLetterExchanges != null && deadLetterExchanges.Count() > 0)
            {
                foreach (var exchange in deadLetterExchanges)
                {
                    PrepareDeadLetterExchange(channel, exchange);
                }
            }

            if (exchanges != null && exchanges.Count() > 0)
            {
                foreach (var exchange in exchanges)
                {
                    PrepareExchangesAndQueues(channel, exchange);
                }
            }
        }

        /// <summary>
        /// Declare the deadletter exchanges.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="deadLetterExchange"></param>
        private void PrepareDeadLetterExchange(IModel channel, DeadLetterExchange deadLetterExchange)
        {
            EnsureChannelNotNull(channel);
            if (deadLetterExchange != null)
            {
                channel.ExchangeDeclare(deadLetterExchange.Name, deadLetterExchange.Type, true, false, null);
            }
        }

        /// <summary>
        /// Declare the exchanges and bind with queues.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="exchange"></param>
        private void PrepareExchangesAndQueues(IModel channel, ExchangeOptions exchange)
        {
            EnsureChannelNotNull(channel);
            if (exchange == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(exchange.Name) == false)
            {
                PrepareExchange(channel, exchange);
            }

            if (exchange.Queues != null && exchange.Queues.Count > 0)
            {
                foreach (var queue in exchange.Queues)
                {
                    PrepareQueue(channel, exchange.Name, queue);
                }
            }
        }

        /// <summary>
        /// Declare a exchange.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="exchange"></param>
        /// <exception cref="ArgumentException"></exception>
        private void PrepareExchange(IModel channel, ExchangeOptions exchange)
        {
            if (exchange.AutoCreateEnabled)
            {
                if (exchange.Settings == null)
                {
                    throw new ArgumentException("The settings configuration of exchange must not be null when autocreateenabled of exchange is true.", nameof(exchange.Settings));
                }
                channel.ExchangeDeclare(exchange.Name, exchange.Settings.Type, exchange.Settings.Durable, exchange.Settings.AutoDelete, RabbitMqArgumentUtil.ConvertToRabbitMqArguments(exchange.Settings.Arguements));
            }
            else
            {
                //only check if already exists, if not, an excpetion will be throwed.
                channel.ExchangeDeclarePassive(exchange.Name);
            }
        }

        /// <summary>
        /// Declare a queue.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="exchangeName"></param>
        /// <param name="queue"></param>
        /// <exception cref="ArgumentException"></exception>
        private void PrepareQueue(IModel channel, string exchangeName, QueueOptions queue)
        {
            if (queue == null)
            {
                return;
            }

            if (queue.AutoCreateEnabled)
            {
                if (queue.Settings == null)
                {
                    throw new ArgumentException("The settings configuration of queue must not be null when autocreateenabled of queue is true.", nameof(queue.Settings));
                }

                channel.QueueDeclare(queue.Name, queue.Settings.Durable, queue.Settings.Exclusive, queue.Settings.AutoDelete, RabbitMqArgumentUtil.ConvertToRabbitMqArguments(queue.Settings.Arguements));

                //Binding queue with exchange.
                BindingQueue(channel, queue.Name, exchangeName, queue.RoutingKeys);
            }
            else
            {
                //only check if already exists, if not, an excpetion will be throwed.
                channel.QueueDeclarePassive(queue.Name);
            }
        }

        /// <summary>
        /// Bind the queue with exchange.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="queueName"></param>
        /// <param name="exchangeName"></param>
        /// <param name="routingKeys"></param>
        private void BindingQueue(IModel channel, string queueName, string exchangeName, HashSet<string> routingKeys)
        {
            if (string.IsNullOrWhiteSpace(exchangeName))
            {
                return;
            }
            if (routingKeys == null || routingKeys.Count == 0)
            {
                channel.QueueBind(queueName, exchangeName, "");
            }
            else
            {
                foreach (var routingKey in routingKeys)
                {
                    channel.QueueBind(queueName, exchangeName, routingKey);
                }
            }
        }

        /// <summary>
        /// Close and dispose the temp channel.
        /// </summary>
        /// <param name="tmpChannel"></param>
        private void CloseChannel(IModel tmpChannel)
        {
            if (tmpChannel?.IsOpen == true)
            {
                tmpChannel.Close();
            }
            tmpChannel?.Dispose();
            _logger.LogInformation($"The temp channel has been closed.");
        }

        /// <summary>
        /// Ensure channel is not null.
        /// </summary>
        /// <param name="channel"></param>
        /// <exception cref="ArgumentException"></exception>
        private void EnsureChannelNotNull(IModel channel)
        {
            if (channel == null)
            {
                throw new ArgumentException("Channel must be not null.", nameof(channel));
            }
        }
    }
}
