using Model;
using QuickStart.Infra.DI.Attributes;
using QuickStart.Infra.Logging;
using QuickStart.Infra.Redis.Cluster;

namespace Service.Impl
{
    [Service]
    public class WeatherForecastService : IWeatherForecastService
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILoggerHelper<WeatherForecastService> _loggerHelper;
        private readonly IRedisClusterService _redisClusterService;
        public WeatherForecastService(ILoggerHelper<WeatherForecastService> loggerHelper, IRedisClusterService redisClusterService)
        {
            _loggerHelper = loggerHelper;
            _redisClusterService = redisClusterService;
        }

        public WeatherForecast[] GetWeatherForecasts() 
        {
            _loggerHelper.InfoLog("request in.");
            _redisClusterService.UnitOfWork.StringSetAsync("gsz_test", 1, TimeSpan.FromSeconds(120));
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
