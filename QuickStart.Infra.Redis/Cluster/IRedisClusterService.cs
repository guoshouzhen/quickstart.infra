using StackExchange.Redis;

namespace QuickStart.Infra.Redis.Cluster
{
    /// <summary>
    /// StackExchange Redis Service.
    /// </summary>
    public interface IRedisClusterService
    {
        /// <summary>
        /// Redis work unit.
        /// </summary>
        public IDatabase UnitOfWork { get; }
    }
}
