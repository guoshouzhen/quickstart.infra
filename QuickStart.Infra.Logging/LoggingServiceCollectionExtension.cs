using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Config;
using QuickStart.Infra.Logging.ConfigOptions;

namespace QuickStart.Infra.Logging
{
    public static class LoggingServiceCollectionExtension
    {
        const string DEFAULT_CONFIG_FILE_NAME = "nlog.config";
        const string NLOG_CONFIG_SECTION_KEY = "NLogConfig";
        const string CONFIG_FILE_REL_PATH_KEY = "ConfigFileRelativePath";

        public static IServiceCollection AddNlog(this IServiceCollection services, IConfiguration configuration) 
        {
            string configFilePath = configuration.GetSection(NLOG_CONFIG_SECTION_KEY)?[CONFIG_FILE_REL_PATH_KEY] ?? DEFAULT_CONFIG_FILE_NAME;
            LogManager.Configuration = new XmlLoggingConfiguration(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFilePath));

            services.Configure<NLogOptions>(configuration.GetSection(NLOG_CONFIG_SECTION_KEY));
            services.Configure<TraceIdOptions>(option => option.TraceId = Guid.NewGuid().ToString());
            services.AddScoped(typeof(ILoggerHelper<>), typeof(LoggerHelper<>));
            return services;
        }

        public static IServiceCollection UseLoggerHelper(this IServiceCollection services, Type type)
        {
            services.AddScoped(typeof(ILoggerHelper<>), type);
            return services;
        }
    }
}
