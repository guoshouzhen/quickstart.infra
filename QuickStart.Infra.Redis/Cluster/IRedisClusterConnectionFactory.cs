using StackExchange.Redis;

namespace QuickStart.Infra.Redis.Cluster
{
    public interface IRedisClusterConnectionFactory
    {
        IConnectionMultiplexer GetConnection();
    }
}
