using System.Numerics;

namespace CityPopulationWebService.Model;

public sealed class CityResponseModel
{
    public string Name { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public double Poppulation { get; set; }
}
