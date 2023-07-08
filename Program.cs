using Microsoft.EntityFrameworkCore;
using WeatherForecastRestAPI.Database;
using WeatherForecastRestAPI.Interfaces;
using WeatherForecastRestAPI.Model;
using WeatherForecastRestAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddMvc();
builder.Services.AddDbContext<ApplicationContext>(n => n.UseSqlite(connectionString));

builder.Services.AddTransient<BaseRepository<ExtendedWeatherRecord>>();

// Add services to the container.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();