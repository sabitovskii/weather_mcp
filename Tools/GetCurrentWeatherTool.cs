using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;
using WeatherMcpServer.Services;

public class GetCurrentWeatherTool
{
    private readonly WeatherService _weatherService;
    private readonly ILogger<GetCurrentWeatherTool> _logger;

    public GetCurrentWeatherTool(WeatherService service, ILogger<GetCurrentWeatherTool> logger)
    {
        _weatherService = service;
        _logger = logger;
    }

    [McpServerTool]
    [Description("Gets current weather conditions for the specified city.")]
    public async Task<string> GetCurrentWeather(
        [Description("City name")] string city,
        [Description("Country code (e.g. US)")] string? countryCode = null)
    {
        if (string.IsNullOrWhiteSpace(city))
            return "City is required.";

        try
        {
            return await _weatherService.GetCurrentWeatherAsync(city, countryCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting weather for {City}", city);
            return $"Could not get weather for '{city}'.";
        }
    }
}
