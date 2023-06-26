using Microsoft.EntityFrameworkCore;
using WeatherForecastRestAPI.Model;

namespace WeatherForecastRestAPI.Database;

public class ApplicationContext : DbContext
{
    public DbSet<WeatherRecord> WeatherRecords { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base (options) =>
        Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => 
        optionsBuilder.UseSqlite($"Filename={StringConstants.DbPath}");
}