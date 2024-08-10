using System.Numerics;
using System.Text.Json.Serialization;

namespace CityPopulationWebService.Model;

public sealed class Stat
{
    [JsonPropertyName("level")]
    public decimal Level { get; set; }

    [JsonPropertyName("population")]
    public double Population { get; set; }
}
