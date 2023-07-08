using WeatherForecastRestAPI.Database;
using WeatherForecastRestAPI.Model;
using WeatherForecastRestAPI.Repository;

namespace WeatherForecastRestAPI.Services;

public static class RepositoryCleaner
{
    public static void Clean(BaseRepository<ExtendedWeatherRecord> repository)
    {
        // Remove not found cities
        foreach (var weatherRecord in repository.GetAll().Where(n => n.City.Equals(StringConstants.CityNotExist)))
            repository.Delete(weatherRecord.Id);

        // Remove duplicates 
        foreach (var weatherRecord in repository.GetAll())
        {
            var reportsWithSameCity = repository.GetAll().Where(n => n.City.Equals(weatherRecord.City)).ToList();
            
            if (reportsWithSameCity.Count == 1 || !reportsWithSameCity.Any()) 
                return;
            
            for (int i = 1; i < reportsWithSameCity.Count(); i++)
                repository.Delete(reportsWithSameCity.ElementAt(i).Id);
        }
    }
}