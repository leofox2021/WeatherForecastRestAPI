namespace WeatherForecastRestAPI.Model;

public class WeatherRecord : BaseModel
{
    public override Guid Id { get; set; }
    
    public string? City { get; set; }
    public int DegreesCelsius { get; set; }
    public int DegreesFahrenheit { get; set; }
    public int WindSpeed { get; set; }
}