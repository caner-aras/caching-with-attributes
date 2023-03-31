using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace caching_with_attributes_api
{
    public interface IWeatherService
    {
        IEnumerable<WeatherForecast> GetWeather();
    }
}