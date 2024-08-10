namespace WebService.Model;

public sealed class CityPagingViewModel
{
    public int PageIndex { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public IReadOnlyList<CityViewModel> Cities { get; set; } = Array.Empty<CityViewModel>();
}
