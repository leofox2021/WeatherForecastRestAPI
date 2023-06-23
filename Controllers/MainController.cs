using Microsoft.AspNetCore.Mvc;
using WeatherForecastRestAPI.Model;
using WeatherForecastRestAPI.Repository;
using WeatherForecastRestAPI.Services;

namespace WeatherForecastRestAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class MainController : ControllerBase
{
    private BaseRepository<WeatherRecord> _weatherRecords;
    
    public MainController(BaseRepository<WeatherRecord> weatherRecords) => _weatherRecords = weatherRecords;
    
    [HttpGet]
    public JsonResult Get()
    {
        WeatherFetcher.UpdateExistingRecords(_weatherRecords);
        return new(_weatherRecords.GetAll());
    }
    
    [HttpPost]
    public JsonResult Post(WeatherRecord weatherRecord)
    {
        try
        {
            _weatherRecords.Create(weatherRecord);
            return new("Post successful");
        }
        catch (Exception exception)
        {
            return new($"Post unsuccessful. {exception.Message}");
        }
    } 
    
    [HttpPut]
    public JsonResult Put(WeatherRecord weatherRecord)
    {
        var targetRecord = _weatherRecords.Get(weatherRecord.Id);
        
        try
        {
            if (targetRecord == null) 
                return new("Put was not successful. The entity does not exist.");
            
            targetRecord = _weatherRecords.Update(targetRecord);
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
        var targetRecord = _weatherRecords.Get(id);

        try
        {
            if (targetRecord == null)
                return new("Delete was not successful. Entity does not exist.");
            
            _weatherRecords.Delete(targetRecord.Id);
            return new("Delete successful.");
        }
        catch (Exception exception)
        {
            return new($"Delete was not successful. Exception message: {exception.Message}");
        }
    }
}