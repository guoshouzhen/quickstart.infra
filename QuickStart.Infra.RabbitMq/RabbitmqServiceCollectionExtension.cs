using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuickStart.Infra.RabbitMq;
using QuickStart.Infra.RabbitMq.ConfigOptions;

namespace QuickStart.Infra.Rabbitmq
{
    public static class RabbitmqServiceCollectionExtension
    {
        const string RABBITMQ_CONFIG_KEY = "RabbitMq";
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration) 
        {
            AddRabbitMqConfig(services, configuration);
            return services;
        }

        public static IServiceCollection AddRabbitMq<TPwdDescryptor>(this IServiceCollection services, IConfiguration configuration)
            where TPwdDescryptor : class, IRabbitMqPwdDescryptor
        {
            services.AddScoped<IRabbitMqPwdDescryptor, TPwdDescryptor>();
            AddRabbitMqConfig(services, configuration);
            return services;
        }

        private static void AddRabbitMqConfig(IServiceCollection services, IConfiguration configuration) 
        {
            services.Configure<RabbitMqOptions>(configuration.GetSection(RABBITMQ_CONFIG_KEY));
            services.AddScoped<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
        }
    }
}
