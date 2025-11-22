using System.Text.Json;

namespace ConsoleAgent.Services;

public class WeatherService(string apiKey)
{
    private readonly HttpClient _httpClient = new();

    public async Task<string[]> GetWeatherInCity(string city, CancellationToken cancellationToken = default)
    {
        string url = $"https://api.weatherapi.com/v1/current.json?key={apiKey}&q={Uri.EscapeDataString(city)}&aqi=no";
        HttpResponseMessage response = await _httpClient.GetAsync(url, cancellationToken);
        string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Error calling Weather API: {response.StatusCode} - {response.Content}");
        }

        using JsonDocument doc = JsonDocument.Parse(responseContent);
        JsonElement root = doc.RootElement;
        JsonElement descriptionElement = root.GetProperty("current").GetProperty("condition").GetProperty("text");

        string[] descriptions = [descriptionElement.GetString()!];

        return descriptions;
    }
}