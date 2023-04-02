using Model;

namespace Service
{
    public interface IWeatherForecastService
    {
        WeatherForecast[] GetWeatherForecasts();
    }
}
