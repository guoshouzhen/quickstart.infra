using StackExchange.Redis;

namespace QuickStart.Infra.Redis.Cluster
{
    /// <summary>
    /// StackExchange Redis Service.
    /// </summary>
    public class RedisClusterService : IRedisClusterService
    {
        private readonly IRedisClusterConnectionFactory _redisClusterConnectionFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="redisClusterConnectionFactory"></param>
        public RedisClusterService(IRedisClusterConnectionFactory redisClusterConnectionFactory)
        {
            _redisClusterConnectionFactory = redisClusterConnectionFactory;
        }

        /// <inheritdoc/>
        public IDatabase UnitOfWork => _redisClusterConnectionFactory.GetConnection().GetDatabase();
    }
}
