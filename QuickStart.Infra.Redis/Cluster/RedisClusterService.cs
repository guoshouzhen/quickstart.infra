using StackExchange.Redis;

namespace QuickStart.Infra.Redis.Cluster
{
    public class RedisClusterService : IRedisClusterService
    {
        private readonly IRedisClusterConnectionFactory _redisClusterConnectionFactory;
        public RedisClusterService(IRedisClusterConnectionFactory redisClusterConnectionFactory)
        {
            _redisClusterConnectionFactory = redisClusterConnectionFactory;
        }
        public IDatabase UnitOfWork => _redisClusterConnectionFactory.GetConnection().GetDatabase();
    }
}
