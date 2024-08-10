namespace WebService.Extexsion;

public static class StringExtensions
{
    public static bool ContainsIgnoreCase(this string source, string value)
    {
        return source?.Contains(value, StringComparison.OrdinalIgnoreCase) ?? false;
    }

    public static bool EqualsIgnoreCase(this string source, string value)
    {
        return source?.Equals(value, StringComparison.OrdinalIgnoreCase) ?? false;
    }
}
