using WeatherForecastRestAPI.Database;
using WeatherForecastRestAPI.Model;

namespace WeatherForecastRestAPI.Repository;

public static class RepositoryCleaner
{
    public static void Clean(BaseRepository<WeatherRecord> repository)
    {
        foreach (var weatherRecord in repository.GetAll().Where(n => n.City.Equals(StringConstants.CityNotExist)))
            repository.Delete(weatherRecord.Id);
    }
}