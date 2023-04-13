using Microsoft.Extensions.Options;
using QuickStart.Infra.Redis.ConfigOptions;
using StackExchange.Redis;

namespace QuickStart.Infra.Redis.Cluster
{
    /// <summary>
    /// StackExchange Redis connection factory, should be used in singleton mode.
    /// </summary>
    public class RedisClusterConnectionFactory : IRedisClusterConnectionFactory
    {
        private readonly RedisClusterOptions _redisClusterOptions;
        private readonly IRedisPwdDecryptor? _passwordDecryptor;

        private IConnectionMultiplexer _connectionMultiplexer;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="redisClusterOptions"></param>
        public RedisClusterConnectionFactory(IOptions<RedisClusterOptions> redisClusterOptions) : this(redisClusterOptions, null) { }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="redisClusterOptions"></param>
        /// <param name="passwordDecryptor"></param>
        public RedisClusterConnectionFactory(IOptions<RedisClusterOptions> redisClusterOptions, IRedisPwdDecryptor? passwordDecryptor)
        {
            _redisClusterOptions = redisClusterOptions.Value;
            _passwordDecryptor = passwordDecryptor;
        }

        /// <inheritdoc/>
        public IConnectionMultiplexer GetConnection()
        {
            if ((_connectionMultiplexer == null) || _connectionMultiplexer.IsConnected == false)
            {
                var configOptions = new ConfigurationOptions();
                var clusterNodes = _redisClusterOptions.EndPoints;
                foreach (var node in clusterNodes)
                {
                    configOptions.EndPoints.Add(node);
                }
                configOptions.TieBreaker = "";
                configOptions.CommandMap = CommandMap.Default;
                configOptions.Password = _passwordDecryptor == null ? _redisClusterOptions.Password : _passwordDecryptor.Decrypt(_redisClusterOptions.Password);
                configOptions.AbortOnConnectFail = true;
                _connectionMultiplexer = ConnectionMultiplexer.Connect(configOptions);
            }
            return _connectionMultiplexer;
        }
    }
}
