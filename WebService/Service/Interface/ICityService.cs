using WebService.Model;

namespace WebService.Service.Interface;

public interface ICityService
{
    public CityPagingResponseModel Search(CityFilterModel filters);
}
