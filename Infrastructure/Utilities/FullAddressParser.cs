namespace task.Infrastructure.Utilities;

public static class FullAddressParser
{
    public static string? ParseRegion(string? fullAddress)
    {
        if (string.IsNullOrWhiteSpace(fullAddress))
        {
            return null;
        }

        var parts = fullAddress.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
        {
            return null;
        }

        var candidate = parts[1];
        return string.IsNullOrWhiteSpace(candidate) ? null : candidate;
    }

    public static string? ParseHouseNumber(string? fullAddress)
    {
        if (string.IsNullOrWhiteSpace(fullAddress))
        {
            return null;
        }

        const string marker = "дом №";
        var index = fullAddress.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
        if (index < 0)
        {
            return null;
        }

        var start = index + marker.Length;
        var tail = fullAddress[start..].Trim();
        if (string.IsNullOrWhiteSpace(tail))
        {
            return null;
        }

        var separatorIndex = tail.IndexOf(',');
        return separatorIndex > 0 ? tail[..separatorIndex].Trim() : tail;
    }

    public static int? ParseApartment(string? fullAddress)
    {
        if (string.IsNullOrWhiteSpace(fullAddress))
        {
            return null;
        }

        const string marker = "офис";
        var index = fullAddress.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
        if (index < 0)
        {
            return null;
        }

        var officePart = fullAddress[(index + marker.Length)..].Trim();
        var digits = new string(officePart.TakeWhile(char.IsDigit).ToArray());
        return int.TryParse(digits, out var apartment) ? apartment : null;
    }
}
