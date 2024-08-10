using WebService.Model;
using WebService.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebService.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CityController : ControllerBase
{
    private readonly ICityService cityService;

    public CityController(ICityService cityService)
    {
       this.cityService = cityService;
    }

    [HttpGet("search")]
    public ActionResult<IReadOnlyList<CityViewModel>> Search(
        [FromQuery, SwaggerParameter(Description = "The name or ids of the city to search for.")] string? query = "",
        [FromQuery, SwaggerParameter(Description = "The coutry of city for filter.")] string? country = "",
        [FromQuery, SwaggerParameter(Description = "The language of country name  for filter.")] string? language = "",
        [FromQuery, SwaggerParameter(Description = "The page of search result.")] int? page = 1)
    {
        var filters = new CityFilterModel()
        {
            Query = query ?? string.Empty,
            Country = country ?? string.Empty,
            Page = page ?? 0,
            Language = language ?? string.Empty,
        };
        var results = this.cityService.Search(filters);
        var response = results.Select(r => new CityViewModel()
        {
            CityName = r.Name,
            Country = r.Country,
            Population = r.Population
        }).ToList();

        return this.Ok(response);
    }
}
