using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QuickStart.Infra.Rabbitmq.ConfigOptions;
using QuickStart.Infra.RabbitMq.ConfigOptions;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace QuickStart.Infra.RabbitMq.Serevices
{
    /// <summary>
    /// RabbitMq connection factory, should be used in singleton mode.
    /// </summary>
    public class RabbitMqConnectionFactory : IRabbitMqConnectionFactory, IDisposable
    {
        private IConnection? _producerConn;
        private IConnection? _consumerConn;
        private bool _disposed;

        private readonly ILogger<RabbitMqConnectionFactory> _logger;
        private readonly RabbitMqOptions _rabbitMqOptions;
        private readonly IRabbitMqPwdDescryptor? _rabbitMqPwdDescryptor;
        public RabbitMqConnectionFactory(ILogger<RabbitMqConnectionFactory> logger, IOptions<RabbitMqOptions> options) : this(logger, options, null) { }
        public RabbitMqConnectionFactory(ILogger<RabbitMqConnectionFactory> logger, IOptions<RabbitMqOptions> options, IRabbitMqPwdDescryptor? rabbitMqPwdDescryptor)
        {
            _logger = logger;
            _rabbitMqOptions = options.Value;
            _rabbitMqPwdDescryptor = rabbitMqPwdDescryptor;
        }

        /// <inheritdoc/>
        public IConnection CreateOrGetProducerConnection()
        {
            if (_producerConn == null || _producerConn.IsOpen == false)
            {
                _logger.LogInformation("Start to create rabbitmq connection for producer...");
                _producerConn = CreateRabbitMqConnection(_rabbitMqOptions.Servers);
            }
            _logger.LogInformation("Rabbitmq connection for producer successfuly created.");
            return _producerConn;
        }

        /// <inheritdoc/>
        public IConnection CreateOrGetConsumerConnection()
        {
            if (_consumerConn == null || _consumerConn.IsOpen == false)
            {
                _logger.LogInformation("Start to create rabbitmq connection for comsumer...");
                _consumerConn = CreateRabbitMqConnection(_rabbitMqOptions.Servers);
            }
            _logger.LogInformation("Rabbitmq connection for comsumer successfuly cteated.");
            return _consumerConn;
        }

        /// <summary>
        /// Create new connection.
        /// </summary>
        /// <param name="serverOptions"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private IConnection CreateRabbitMqConnection(ServerOptions serverOptions)
        {
            EnsureConfigParams(serverOptions);

            ConnectionFactory connectionFactory = new ConnectionFactory();

            IList<AmqpTcpEndpoint> lstEndPoints = new List<AmqpTcpEndpoint>(serverOptions.EndPoints.Count);
            foreach (var endPoint in serverOptions.EndPoints)
            {
                var sslOption = endPoint.Ssl;
                if (sslOption != null)
                {
                    var convertedOption = new SslOption(sslOption.ServerName, sslOption.CertificatePath, sslOption.Enable);
                    if (!string.IsNullOrEmpty(sslOption.CertificatePassphrase))
                    {
                        convertedOption.CertPassphrase = sslOption.CertificatePassphrase;
                    }

                    if (sslOption.SslPolicyErrors != null)
                    {
                        convertedOption.AcceptablePolicyErrors = sslOption.SslPolicyErrors.Value;
                    }

                    lstEndPoints.Add(new AmqpTcpEndpoint(endPoint.Host, endPoint.Port, convertedOption));
                }
                else
                {
                    lstEndPoints.Add(new AmqpTcpEndpoint(endPoint.Host, endPoint.Port));
                }
            }
            connectionFactory.UserName = serverOptions.UserName;
            connectionFactory.Password = _rabbitMqPwdDescryptor == null ? serverOptions.PassWord : _rabbitMqPwdDescryptor.DecrptyPwd(serverOptions.PassWord);
            connectionFactory.VirtualHost = serverOptions.VirtualHost;
            if (string.IsNullOrWhiteSpace(serverOptions.ClientProvidedName) == false)
            {
                connectionFactory.ClientProvidedName = serverOptions.ClientProvidedName;
            }
            connectionFactory.AutomaticRecoveryEnabled = serverOptions.AutomaticRecoveryEnabled;
            connectionFactory.TopologyRecoveryEnabled = serverOptions.TopologyRecoveryEnabled;
            connectionFactory.RequestedConnectionTimeout = TimeSpan.FromSeconds(serverOptions.RequestedConnectionTimeout);
            connectionFactory.RequestedHeartbeat = TimeSpan.FromSeconds(serverOptions.RequestedHeartbeat);
            connectionFactory.DispatchConsumersAsync = true;

            int attempts = 0;
            BrokerUnreachableException? latestException = null;
            //Retry if connect failed.
            while (attempts < serverOptions.InitialConnectRetries)
            {
                try
                {
                    if (attempts > 0)
                    {
                        Thread.Sleep(serverOptions.InitialConnectRetryTimeInterval);
                    }
                    return connectionFactory.CreateConnection(lstEndPoints);
                }
                catch (BrokerUnreachableException ex)
                {
                    attempts++;
                    latestException = ex;
                }
            }
            throw new Exception($"Connect to rabbitmq server failed, has retried: {attempts} times.", latestException);
        }

        /// <summary>
        /// Check params.
        /// </summary>
        /// <param name="serverOptions"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        private void EnsureConfigParams(ServerOptions serverOptions)
        {
            if (serverOptions == null)
            {
                throw new ArgumentNullException("RabbitMq server config must be not null.", nameof(serverOptions));
            }

            if (serverOptions.EndPoints == null || serverOptions.EndPoints.Count == 0)
            {
                throw new ArgumentException("Rabbitmq server host must specify at least one.", nameof(serverOptions.EndPoints));
            }

            if (serverOptions.InitialConnectRetries < 1)
            {
                throw new ArgumentException("Initial connect retries count should be a positive number.", nameof(serverOptions.InitialConnectRetries));
            }

            if (serverOptions.InitialConnectRetryTimeInterval < 1)
            {
                throw new ArgumentException("Initial connect retries timeinterval should be a positive number.", nameof(serverOptions.InitialConnectRetryTimeInterval));
            }
        }

        /// <summary>
        /// Disposing connections.
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
                if (_producerConn?.IsOpen == true)
                {
                    _producerConn.Close();
                }
                if (_consumerConn?.IsOpen == true)
                {
                    _consumerConn.Close();
                }
                _producerConn?.Dispose();
                _consumerConn?.Dispose();
            }

            _disposed = true;
        }
    }
}
