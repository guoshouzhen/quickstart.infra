using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuickStart.Infra.RabbitMq.ConfigOptions;
using QuickStart.Infra.RabbitMq.Producer;
using QuickStart.Infra.RabbitMq.Serevices;

namespace QuickStart.Infra.RabbitMq.Extensions
{
    public static class RabbitMqServiceCollectionExtension
    {
        const string RABBITMQ_CONFIG_KEY = "RabbitMq";

        /// <summary>
        /// Add RabbitMq to application.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            AddRabbitMqConfig(services, configuration);
            return services;
        }

        /// <summary>
        /// Add RabbitMq to application.
        /// </summary>
        /// <typeparam name="TPwdDescryptor">The Descryptor used to decrypt the rabbitmq password.</typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddRabbitMq<TPwdDescryptor>(this IServiceCollection services, IConfiguration configuration)
            where TPwdDescryptor : class, IRabbitMqPwdDescryptor
        {
            services.AddSingleton<IRabbitMqPwdDescryptor, TPwdDescryptor>();
            AddRabbitMqConfig(services, configuration);
            return services;
        }
        
        /// <summary>
        /// Regiser the RabbitMq services.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void AddRabbitMqConfig(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqOptions>(configuration.GetSection(RABBITMQ_CONFIG_KEY));
            services.AddSingleton<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
            services.AddSingleton<IRabbitMqPreparationService, RabbitMqPreparationService>();
            services.AddScoped<IProducingService, ProducingService>();
            services.AddHostedService<RabbitMqPreperationHostedService>();
        }
    }
}
