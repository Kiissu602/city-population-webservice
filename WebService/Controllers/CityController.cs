using WebService.Model;
using WebService.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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
    public ActionResult<IReadOnlyList<CityResponseModel>> Search([FromQuery] string? query = "")
    {
        var results = this.cityService.Search(query);
        return this.Ok(results);
    }
}
