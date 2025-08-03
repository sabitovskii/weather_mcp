using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace WeatherMcpServer.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public WeatherService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["OPENWEATHER_API_KEY"] ?? throw new InvalidOperationException("Missing API key");
        }

        public async Task<string> GetCurrentWeatherAsync(string city, string? countryCode = null)
        {
            var query = string.IsNullOrWhiteSpace(countryCode) ? city : $"{city},{countryCode}";
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(query)}&appid={_apiKey}&units=metric";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to get weather: {response.StatusCode}");

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var temp = doc.RootElement.GetProperty("main").GetProperty("temp").GetDouble();
            var desc = doc.RootElement.GetProperty("weather")[0].GetProperty("description").GetString();
            var name = doc.RootElement.GetProperty("name").GetString();

            return $"{name}: {temp}°C, {desc}";
        }
        public async Task<string> GetWeatherForecastAsync(string city, string? countryCode = null)
        {
            var query = string.IsNullOrWhiteSpace(countryCode) ? city : $"{city},{countryCode}";
            var url = $"https://api.openweathermap.org/data/2.5/forecast?q={Uri.EscapeDataString(query)}&appid={_apiKey}&units=metric";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to get forecast: {response.StatusCode}");

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var list = doc.RootElement.GetProperty("list");

            var forecast = new Dictionary<string, List<string>>();

            foreach (var item in list.EnumerateArray())
            {
                var dt = item.GetProperty("dt_txt").GetString()!;
                var date = dt.Split(' ')[0];

                var temp = item.GetProperty("main").GetProperty("temp").GetDouble();
                var desc = item.GetProperty("weather")[0].GetProperty("description").GetString();

                if (!forecast.ContainsKey(date))
                    forecast[date] = new List<string>();

                forecast[date].Add($"{dt}: {temp}°C, {desc}");
            }

            // Возьмем первые 3 дня
            var result = string.Join("\n\n",
                forecast.Take(3).Select(kvp => $"{kvp.Key}:\n" + string.Join("\n", kvp.Value)));

            return result;
        }
    }
}
