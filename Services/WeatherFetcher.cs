using System.Diagnostics;
using OpenMeteo;
using WeatherForecastRestAPI.Database;
using WeatherForecastRestAPI.Model;
using WeatherForecastRestAPI.Repository;

namespace WeatherForecastRestAPI.Services;

public static class WeatherFetcher
{
    private static readonly OpenMeteo.OpenMeteoClient Client = new();
    
    public static void UpdateExistingRecords(BaseRepository<WeatherRecord> repository) 
    {
        for (int i = 0; i < repository.GetAll().Count; i++)
        {
            var targetWeatherRecord = repository.GetAll()[i];
            FetchOne(targetWeatherRecord);
            repository.Update(targetWeatherRecord);
        }
    }
    
    public static void UpdateExistingRecords(BaseRepository<ExtendedWeatherRecord> repository) 
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
        var weatherData = GetWeatherQuery(weatherRecord.City);

        weatherRecord.City = weatherData == null ? StringConstants.CityNotExist : weatherRecord.City;
        weatherRecord.DegreesCelsius = weatherData == null ? 0 : (int)weatherData.CurrentWeather?.Temperature;
        weatherRecord.DegreesFahrenheit = weatherData == null ? 0 
            : GetDegreesFahrenheit(weatherData.CurrentWeather?.Temperature);
        weatherRecord.WindSpeed = weatherData == null ? 0 : (int)weatherData.CurrentWeather?.Windspeed; 
    }
    
    private static void FetchOne(ExtendedWeatherRecord extendedWeatherRecord)
    {
        string notAvaliableMessage = "not available for the current city";
        
        var weatherData = GetWeatherQuery(extendedWeatherRecord.City);
        var cityData = GetCityData(extendedWeatherRecord.City);
        
        extendedWeatherRecord.City = weatherData == null ? StringConstants.CityNotExist : extendedWeatherRecord.City;
        extendedWeatherRecord.DegreesCelsius = weatherData == null ? 0 : (int)weatherData.CurrentWeather?.Temperature;
        extendedWeatherRecord.DegreesFahrenheit = weatherData == null ? 0 
            : GetDegreesFahrenheit(weatherData.CurrentWeather?.Temperature);
        extendedWeatherRecord.WindSpeed = weatherData == null ? 0 : (int)weatherData.CurrentWeather?.Windspeed;
        
        extendedWeatherRecord.Country = cityData?.Country ?? notAvaliableMessage;
        extendedWeatherRecord.Timezone = cityData?.Timezone ?? notAvaliableMessage;
        extendedWeatherRecord.Elevation = cityData?.Elevation ?? 0;
        extendedWeatherRecord.Population = cityData?.Population ?? 0;
        extendedWeatherRecord.CountryCode = cityData?.CountryCode ?? notAvaliableMessage;

    }

    private static LocationData? GetCityData(string city)
    {
        var geocodingOptions = new GeocodingOptions(city);
        var apiResponse = Client.GetLocationDataAsync(geocodingOptions).Result;
        return apiResponse?.Locations?[0];
    }

    private static WeatherForecast? GetWeatherQuery(string city) => Client.Query(city);
    
    private static int GetDegreesFahrenheit(float? degreesCelsius) => Convert.ToInt32(degreesCelsius * 9 / 5 + 32);
}