using StackExchange.Redis;

namespace QuickStart.Infra.Redis.Cluster
{
    public interface IRedisClusterService
    {
        public IDatabase UnitOfWork { get; }
    }
}
