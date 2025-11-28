using System.Text.Json;

namespace ConsoleAgent.Services;

public class DictionaryService
{
    private readonly HttpClient _httpClient = new();

    public async Task<string[]> GetDefinitionsForWord(string word, CancellationToken cancellationToken = default)
    {
        string url = $"https://api.dictionaryapi.dev/api/v2/entries/en/{word}";
        HttpResponseMessage response = await _httpClient.GetAsync(url, cancellationToken);
        string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Error calling Dictionary API: {response.StatusCode} - {response.Content}");
        }

        using JsonDocument doc = JsonDocument.Parse(responseContent);
        JsonElement root = doc.RootElement[0];
        JsonElement meaningsElement = root.GetProperty("meanings");
        
        string[] definitions = meaningsElement.EnumerateArray()
            .SelectMany(m => m.GetProperty("definitions").EnumerateArray())
            .Select(d => d.GetProperty("definition").GetString())
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Distinct()
            .ToArray()!;

        return definitions;
    }
}