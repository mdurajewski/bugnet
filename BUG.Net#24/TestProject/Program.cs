using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Graylog;
using Serilog.Sinks.Graylog.Core.Transport;
using Serilog.Sinks.SystemConsole.Themes;
using TestProject;

var builder = WebApplication.CreateBuilder(args);
#region const
var appName = "DemoApp";
var appVersion = "1.0.0";
var isProd = true;
#endregion

builder.Logging.ClearProviders();
var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Filter.ByExcluding("Uri like '%/health'")
    .Enrich.FromLogContext()
    .Enrich.WithThreadName()
    .Enrich.WithClientIp()
    .Enrich.WithProperty("App", appName)
    .Enrich.WithProperty("Ver", appVersion)
    .WriteTo.Conditional(evt => !isProd, sinkConfiguration =>
    {
        sinkConfiguration.Console(
            LogEventLevel.Debug,
            outputTemplate:
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
            theme: AnsiConsoleTheme.Literate
        );
    })
    .WriteTo.Conditional(evt => isProd, sinkConfiguration =>
    {
        sinkConfiguration.Graylog(
            hostnameOrAddress: "http://ip_address_or_dns_name", 
            port: 12201, 
            transportType: 
            TransportType.Http, false, 
            LogEventLevel.Error);
    })
    .CreateLogger();

builder.Host.UseSerilog(logger);

builder.Services.AddOpenApi();
var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", ([FromServices] ILogger<Program> loggerI) =>
    {
        loggerI.LogInformation("Getting weather forecast");
        var specyficId = 134;
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        loggerI.LogCritical("Weather forecast critical error with id: {CriticalId}", specyficId);
        return forecast;
    })
    .WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}