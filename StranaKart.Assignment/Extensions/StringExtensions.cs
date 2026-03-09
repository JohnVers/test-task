namespace StranaKart.Assignment.Extensions;

internal static class StringExtensions
{
    public static string? NullIfWhiteSpace(this string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
