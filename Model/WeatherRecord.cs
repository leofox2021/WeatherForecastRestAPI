namespace WeatherForecastRestAPI.Model;

public class WeatherRecord : BaseModel
{
    public Guid Id { get; set; }
    
    public string? City { get; set; }
    public int DegreesCelsius { get; set; }
    public float DegreesFahrenheit { get; set; }
    public float Pressure { get; set; }
}