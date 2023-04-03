using Microsoft.Extensions.Options;
using QuickStart.Infra.Redis.ConfigOptions;
using StackExchange.Redis;

namespace QuickStart.Infra.Redis.Cluster
{
    public class RedisClusterConnectionFactory : IRedisClusterConnectionFactory
    {
        private readonly RedisClusterOptions _redisClusterOptions;
        private readonly IRedisPwdDecryptor? _passwordDecryptor;

        private IConnectionMultiplexer _connectionMultiplexer;

        public RedisClusterConnectionFactory(IOptions<RedisClusterOptions> redisClusterOptions) : this(redisClusterOptions, null) { }
        public RedisClusterConnectionFactory(IOptions<RedisClusterOptions> redisClusterOptions, IRedisPwdDecryptor? passwordDecryptor)
        {
            _redisClusterOptions = redisClusterOptions.Value;
            _passwordDecryptor = passwordDecryptor;
        }

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
                configOptions.Password = GetPwd();
                configOptions.AbortOnConnectFail = true;
                _connectionMultiplexer = ConnectionMultiplexer.Connect(configOptions);
            }
            return _connectionMultiplexer;
        }

        private string GetPwd()
        {
            if (_passwordDecryptor != null)
            {
                return _passwordDecryptor.Decrypt(_redisClusterOptions.Password);
            }
            return _redisClusterOptions.Password;
        }
    }
}
