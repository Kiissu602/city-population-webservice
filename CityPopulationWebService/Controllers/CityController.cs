using CityPopulationWebService.Model;
using CityPopulationWebService.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CityPopulationWebService.Controllers;

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
    public ActionResult<IReadOnlyList<CityResponseModel>> Search([FromQuery] string? query)
    {
        var results = this.cityService.Search(query ?? string.Empty);
        IReadOnlyList<CityResponseModel> response = results.Select(r => new CityResponseModel()
            {
                Country = r.Country,
                Name = r.Name,
                Poppulation = r.Stat.Population
            })
            .OrderBy(c => c.Name)
            .ToList();

        return this.Ok(response);
    }
}
