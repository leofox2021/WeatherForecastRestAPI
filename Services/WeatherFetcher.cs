using WeatherForecastRestAPI.Database;
using WeatherForecastRestAPI.Model;
using WeatherForecastRestAPI.Repository;

namespace WeatherForecastRestAPI.Services;

public static class WeatherFetcher
{
    public static void UpdateExistingRecords(BaseRepository<WeatherRecord> repository) 
    {
        for (int i = 0; i < repository.GetAll().Count; i++)
        {
            var targetWeatherRecord = repository.GetAll()[i];
            FetchOne(targetWeatherRecord);
            repository.Update(targetWeatherRecord);
        }
    }
    
    private static void FetchOne(WeatherRecord weatherRecord)
    {
        var openMeteoClient = new OpenMeteo.OpenMeteoClient();
        var weatherData = openMeteoClient.Query(weatherRecord.City);

        weatherRecord.City = weatherData == null ? StringConstants.CityNotExist : weatherRecord.City;
        
        weatherRecord.DegreesCelsius = weatherData == null ? 0 : (int)weatherData.CurrentWeather?.Temperature;
        
        weatherRecord.DegreesFahrenheit = weatherData == null ? 0 
            : GetDegreesFahrenheit(weatherData.CurrentWeather?.Temperature);
        
        weatherRecord.WindSpeed = weatherData == null ? 0 : (int)weatherData.CurrentWeather?.Windspeed; 
    }

    private static int GetDegreesFahrenheit(float? degreesCelsius) => Convert.ToInt32(degreesCelsius * 9 / 5 + 32);
}