using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuickStart.Infra.Redis.Cluster;
using QuickStart.Infra.Redis.ConfigOptions;

namespace QuickStart.Infra.Redis
{
    public static class RedisServiceCollectionExtension
    {
        const string CLUSTER_CONFIG_KEY = "RedisCluster";
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration) 
        {
            AddStackExchangeRedis(services, configuration);
            return services;
        }

        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration, IRedisPwdDecryptor redisPwdDecryptor)
        {
            AddStackExchangeRedis(services, configuration);
            services.AddSingleton(typeof(IRedisPwdDecryptor), redisPwdDecryptor);
            return services;
        }

        private static void AddStackExchangeRedis(IServiceCollection services, IConfiguration configuration) 
        {
            services.Configure<RedisClusterOptions>(configuration.GetSection(CLUSTER_CONFIG_KEY));
            services.AddSingleton<IRedisClusterConnectionFactory, RedisClusterConnectionFactory>();
            services.AddScoped<IRedisClusterService, RedisClusterService>();
        }
    }
}
