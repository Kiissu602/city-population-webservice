namespace WebService.Model;

public sealed class CityPagingResponseModel
{
    public int PageIndex { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public IReadOnlyList<CityResponseModel> Response { get; set; } = Array.Empty<CityResponseModel>();
}
