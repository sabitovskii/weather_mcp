using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;
using WeatherMcpServer.Services;

public class GetWeatherForecastTool
{
    private readonly WeatherService _weatherService;
    private readonly ILogger<GetWeatherForecastTool> _logger;

    public GetWeatherForecastTool(WeatherService service, ILogger<GetWeatherForecastTool> logger)
    {
        _weatherService = service;
        _logger = logger;
    }

    [McpServerTool]
    [Description("Gets a 3-day weather forecast for the specified city.")]
    public async Task<string> GetWeatherForecast(
        [Description("City name")] string city,
        [Description("Optional: country code (e.g., 'US', 'UK')")] string? countryCode = null)
    {
        if (string.IsNullOrWhiteSpace(city))
            return "City is required.";

        try
        {
            return await _weatherService.GetWeatherForecastAsync(city, countryCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting forecast for {City}", city);
            return $"Could not get forecast for '{city}'.";
        }
    }
}