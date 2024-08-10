using WebService.Model;
using WebService.Service.Interface;
using Microsoft.AspNetCore.Mvc;

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
    public ActionResult<IReadOnlyList<CityResponseModel>> Search(
        [FromQuery] string? query = "",
        [FromQuery] string? country = "",
        [FromQuery] string? language = "",
        [FromQuery] int? page = 0)
    {
        var filters = new CityFilterModel()
        {
            Query = query ?? string.Empty,
            Country = country ?? string.Empty,
            Page = page ?? 0,
            Language = language ?? string.Empty,
        };
        var results = this.cityService.Search(filters);
        return this.Ok(results);
    }
}
