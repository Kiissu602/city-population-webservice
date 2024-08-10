using System.Text.Json.Serialization;

namespace WebService.Model;

public sealed class City
{
    [JsonPropertyName("id")]
    public double Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("stat")]
    public Stat Stat { get; set; } = new Stat();

    [JsonPropertyName("langs")]
    public IReadOnlyDictionary<string, string> Languages { get; set; } = new Dictionary<string, string>();
}
