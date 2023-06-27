using WeatherForecastRestAPI.Database;
using WeatherForecastRestAPI.Model;
using WeatherForecastRestAPI.Repository;

namespace WeatherForecastRestAPI.Services;

public static class RepositoryCleaner
{
    public static void Clean(BaseRepository<WeatherRecord> repository)
    {
        foreach (var weatherRecord in repository.GetAll().Where(n => n.City.Equals(StringConstants.CityNotExist)))
            repository.Delete(weatherRecord.Id);
    }
    
    public static void Clean(BaseRepository<ExtendedWeatherRecord> repository)
    {
        foreach (var weatherRecord in repository.GetAll().Where(n => n.City.Equals(StringConstants.CityNotExist)))
            repository.Delete(weatherRecord.Id);
    }
}