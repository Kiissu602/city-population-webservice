namespace CityPopulationWebService.Extexsion;

public static class StringExtensions
{
    public static bool ContainsIgnoreCase(this string source, string value)
    {
        return source.Contains(value, StringComparison.OrdinalIgnoreCase);
    }

    public static bool EqualsIgnoreCase(this string source, string value)
    {
        return source.Equals(value, StringComparison.OrdinalIgnoreCase);
    }
}
