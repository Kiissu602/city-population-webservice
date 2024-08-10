using System.Numerics;
using System.Text.Json.Serialization;

namespace WebService.Model;

public sealed class Stat
{
    [JsonPropertyName("level")]
    public decimal Level { get; set; }

    [JsonPropertyName("population")]
    public double Population { get; set; }
}
