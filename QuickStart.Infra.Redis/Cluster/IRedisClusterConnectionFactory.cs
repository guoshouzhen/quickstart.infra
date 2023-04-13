using StackExchange.Redis;

namespace QuickStart.Infra.Redis.Cluster
{
    /// <summary>
    /// StackExchange Redis connection factory, should be used in singleton mode.
    /// </summary>
    public interface IRedisClusterConnectionFactory
    {
        /// <summary>
        /// Create a new redis connection when any connection does not exist.
        /// </summary>
        /// <returns></returns>
        IConnectionMultiplexer GetConnection();
    }
}
