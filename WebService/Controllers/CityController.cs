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
    public ActionResult<IReadOnlyList<CityResponseModel>> Search([FromQuery] string? query = "", [FromQuery] int? page = 0)
    {
        var results = this.cityService.Search(query, page);
        return this.Ok(results);
    }
}
