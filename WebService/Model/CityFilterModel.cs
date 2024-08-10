namespace WebService.Model;

public sealed class CityFilterModel
{
    public string Query { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public int Page { get; set; }

    public int PageSize { get; set; }

    public string Language { get; set; } = string.Empty;
}
