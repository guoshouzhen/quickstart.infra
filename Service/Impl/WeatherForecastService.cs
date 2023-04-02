using Model;
using QuickStart.Infra.DI.Attributes;
using QuickStart.Infra.Logging;

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
        public WeatherForecastService(ILoggerHelper<WeatherForecastService> loggerHelper)
        {
            _loggerHelper = loggerHelper;
        }

        public WeatherForecast[] GetWeatherForecasts() 
        {
            _loggerHelper.InfoLog("request in.");
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
