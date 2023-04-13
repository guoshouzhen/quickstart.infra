using Model;
using QuickStart.Infra.DI.Attributes;
using QuickStart.Infra.Logging;
using QuickStart.Infra.RabbitMq.Producer;
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
        private readonly IProducingService _producingService;
        public WeatherForecastService(ILoggerHelper<WeatherForecastService> loggerHelper, IRedisClusterService redisClusterService, IProducingService producingService)
        {
            _loggerHelper = loggerHelper;
            _redisClusterService = redisClusterService;
            _producingService = producingService;
        }

        /// <summary>
        /// Test
        /// </summary>
        /// <returns></returns>
        public WeatherForecast[] GetWeatherForecasts() 
        {
            _loggerHelper.InfoLog("request in.");
            _redisClusterService.UnitOfWork.StringSetAsync("gsz_test", 1, TimeSpan.FromSeconds(120));

            var msgObj = new { id = Guid.NewGuid().ToString(), type = "test" };
            string exchangeName = "quickstart.test.direct.exchange";
            string routingKey = "quickstart.test.direct.rk";

            for (int i = 0; i < 20; i++) 
            {
                _producingService.Send(msgObj, exchangeName, routingKey);
            }

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
