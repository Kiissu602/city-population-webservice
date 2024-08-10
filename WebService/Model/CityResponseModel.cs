using System.Numerics;

namespace WebService.Model;

public sealed class CityResponseModel
{
    public double Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public double Population { get; set; }

    public string Language { get; set; } = string.Empty;
}
