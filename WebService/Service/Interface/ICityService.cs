using WebService.Model;

namespace WebService.Service.Interface;

public interface ICityService
{
    public IReadOnlyList<CityResponseModel> Search(CityFilterModel filters);
}
