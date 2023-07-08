using OpenMeteo;
using WeatherForecastRestAPI.Database;
using WeatherForecastRestAPI.Model;
using WeatherForecastRestAPI.Repository;

namespace WeatherForecastRestAPI.Services;

public static class WeatherFetcher
{
    private static readonly OpenMeteoClient Client = new();

    public static void UpdateExistingRecords(BaseRepository<ExtendedWeatherRecord> repository) 
    {
        for (int i = 0; i < repository.GetAll().Count; i++)
        {
            var targetWeatherRecord = repository.GetAll()[i];
            FetchOne(targetWeatherRecord);
            repository.Update(targetWeatherRecord);
        }
    }

    private static void FetchOne(ExtendedWeatherRecord extendedWeatherRecord)
    {
        string notAvaliableMessage = "not available for the current city";
        
        var cityData = GetCityData(extendedWeatherRecord.City);
        var weatherData = GetExtendedWeatherQuery(extendedWeatherRecord.City, cityData?.Timezone);
        
        var weatherDataNull = weatherData == null;
        var dailyNull = weatherData?.Daily == null;
        
        extendedWeatherRecord.City = weatherDataNull ? StringConstants.CityNotExist : extendedWeatherRecord.City;
        extendedWeatherRecord.DegreesCelsius = weatherDataNull ? 0 : (int)weatherData.CurrentWeather?.Temperature;
        extendedWeatherRecord.DegreesFahrenheit = weatherDataNull ? 0 
            : GetDegreesFahrenheit(weatherData.CurrentWeather?.Temperature);
        extendedWeatherRecord.WindSpeed = weatherDataNull ? 0 : (int)weatherData.CurrentWeather?.Windspeed;
        
        extendedWeatherRecord.Country = cityData?.Country ?? notAvaliableMessage;
        extendedWeatherRecord.Timezone = cityData?.Timezone ?? notAvaliableMessage;
        extendedWeatherRecord.Elevation = cityData?.Elevation ?? 0;
        extendedWeatherRecord.Population = cityData?.Population ?? 0;
        extendedWeatherRecord.CountryCode = cityData?.CountryCode ?? notAvaliableMessage;
        
        extendedWeatherRecord.RainSum = dailyNull ? 0 : weatherData.Daily.Rain_sum[0];
        extendedWeatherRecord.Precipitation = dailyNull ? 0 : weatherData.Daily.Precipitation_sum[0];
        extendedWeatherRecord.Showers = dailyNull ? 0 : weatherData.Daily.Showers_sum[0];
        extendedWeatherRecord.Snowfall = dailyNull ? 0 : weatherData.Daily.Snowfall_sum[0];
        extendedWeatherRecord.Sunset = dailyNull ? notAvaliableMessage : weatherData.Daily.Sunset[0].Split('T')[1];
        extendedWeatherRecord.Sunrise = dailyNull ? notAvaliableMessage : weatherData.Daily.Sunrise[0].Split('T')[1];
        extendedWeatherRecord.WindDirection = dailyNull ? 0 : weatherData.Daily.Winddirection_10m_dominant[0];
    }

    private static LocationData? GetCityData(string city)
    {
        var geocodingOptions = new GeocodingOptions(city);
        var apiResponse = Client.GetLocationDataAsync(geocodingOptions).Result;
        return apiResponse?.Locations?[0];
    }

    private static WeatherForecast? GetWeatherQuery(string city) => Client.Query(city);
    
    private static WeatherForecast? GetExtendedWeatherQuery(string city, string? timezone)
    {
        var dailyOptions = new DailyOptions(new []
        {
            DailyOptionsParameter.rain_sum,
            DailyOptionsParameter.precipitation_sum,
            DailyOptionsParameter.precipitation_sum,
            DailyOptionsParameter.showers_sum,
            DailyOptionsParameter.snowfall_sum,
            DailyOptionsParameter.sunset,
            DailyOptionsParameter.sunrise,
            DailyOptionsParameter.winddirection_10m_dominant
        });
        
        var weatherForecastOptions = new WeatherForecastOptions { Daily = dailyOptions, Timezone = timezone ?? "" };
        return Client.Query(city, weatherForecastOptions);
    }
    
    private static int GetDegreesFahrenheit(float? degreesCelsius) => Convert.ToInt32(degreesCelsius * 1.8f + 32);
}