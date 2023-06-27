using Microsoft.AspNetCore.Mvc;
using WeatherForecastRestAPI.Model;
using WeatherForecastRestAPI.Repository;
using WeatherForecastRestAPI.Services;

namespace WeatherForecastRestAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ExtendedController : ControllerBase
{
    private readonly BaseRepository<ExtendedWeatherRecord> _extendedWeatherRecords;

    public ExtendedController(BaseRepository<ExtendedWeatherRecord> extendedWeatherRecords) =>
        _extendedWeatherRecords = extendedWeatherRecords;
    
    [HttpGet]
    public JsonResult Get()
    {
        WeatherFetcher.UpdateExistingRecords(_extendedWeatherRecords);
        RepositoryCleaner.Clean(_extendedWeatherRecords);
        return new(_extendedWeatherRecords.GetAll());
    }
    
    [HttpPost]
    public JsonResult Post(ExtendedWeatherRecord extendedWeatherRecord)
    {
        try
        {
            _extendedWeatherRecords.Create(extendedWeatherRecord);
            return new("Post successful");
        }
        catch (Exception exception)
        {
            return new($"Post unsuccessful. {exception.Message}");
        }
    } 
    
    [HttpPut]
    public JsonResult Put(ExtendedWeatherRecord extendedWeatherRecord)
    {
        if (_extendedWeatherRecords.Get(extendedWeatherRecord.Id) == null)
            return new("Put was not successful. The entity does not exist.");
        
        try
        {
            var targetRecord = _extendedWeatherRecords.Update(_extendedWeatherRecords.Get(extendedWeatherRecord.Id));
            return new($"Put successful {targetRecord.Id}");

        }
        catch (Exception exception)
        {
            return new($"Put was not successful. Exception message: {exception.Message}");
        }
    }
    
    [HttpDelete]
    public JsonResult Delete(Guid id)
    {
        if (_extendedWeatherRecords.Get(id) == null)
            return new("Delete was not successful. Entity does not exist.");

        try
        {
            _extendedWeatherRecords.Delete(_extendedWeatherRecords.Get(id).Id);
            return new("Delete successful.");
        }
        catch (Exception exception)
        {
            return new($"Delete was not successful. Exception message: {exception.Message}");
        }
    }
}