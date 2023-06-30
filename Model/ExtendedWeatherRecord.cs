namespace WeatherForecastRestAPI.Model;

public class ExtendedWeatherRecord : BaseModel
{
    public override Guid Id { get; set; }
    
    public string? City { get; set; }
    public int DegreesCelsius { get; set; }
    public int DegreesFahrenheit { get; set; }
    public int WindSpeed { get; set; }
    public string? Timezone { get; set; }
    public string? Country { get; set; }
    public float Elevation { get; set; }
    public int Population { get; set; }
    public string? CountryCode { get; set; }
    public float RainSum { get; set; }
    public float Precipitation { get; set; }
    public float Showers { get; set; }
    public float Snowfall { get; set; }
    public string? Sunset { get; set; }
    public string? Sunrise { get; set; }
    public float WindDirection { get; set; }
}