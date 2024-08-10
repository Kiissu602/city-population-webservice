using CityPopulationWebService.Model;

namespace CityPopulationWebService.Service.Interface;

public interface ICityService
{
    public IReadOnlyList<City> Search(string? name = "");
}
