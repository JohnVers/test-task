using System.Text.RegularExpressions;

namespace StranaKart.Assignment.Mapping;

internal static class AddressParser
{
    private static readonly string[] StreetSuffixes =
    [
        " ул", " пр-кт", " ш", " пер", " проезд",
        " тракт", " б-р", " наб", " дор", " км", " тер"
    ];

    private static readonly Regex PostalCodeRegex = new(@"^\d{6}$", RegexOptions.Compiled);
    private static readonly Regex RegionRegex =
        new(@"\b(?:обл(?:асть)?|край|респ(?:ублика)?|АО|округ)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static readonly Regex CityRegex =
        new(@"\s(?:г|п|пгт|рп|с)\.?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static readonly Regex HousePrefixRegex =
        new(@"^(?:дом\s*№|зд\.?|з/у|владение)\s*", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static readonly Regex ApartmentRegex =
        new(@"^(?:офис|помещение|помещ\.?|пом\.?)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static readonly Regex DigitsRegex = new(@"\d+", RegexOptions.Compiled);
    private static readonly Regex MultiSpaceRegex = new(@"\s{2,}", RegexOptions.Compiled);

    public static Address Parse(string? fullAddress)
    {
        if (string.IsNullOrWhiteSpace(fullAddress))
            return new Address();

        var parts = fullAddress
            .Split(',')
            .Select(p => p.Trim())
            .Where(p => !string.IsNullOrEmpty(p))
            .Select(NormalizeSpaces)
            .ToArray();

        string? region = null;
        string? city = null;
        string? street = null;
        string? houseNumber = null;
        int? apartment = null;

        foreach (var rawPart in parts)
        {
            if (PostalCodeRegex.IsMatch(rawPart))
                continue;

            var part = rawPart.TrimEnd('.', ' ');

            if (region is null && RegionRegex.IsMatch(part))
            {
                region = rawPart;
                continue;
            }

            if (city is null && CityRegex.IsMatch(part))
            {
                city = NormalizeCity(rawPart);
                continue;
            }

            if (street is null && StreetSuffixes.Any(suffix => part.EndsWith(suffix, StringComparison.OrdinalIgnoreCase)))
            {
                street = rawPart;
                continue;
            }

            if (houseNumber is null && TryParseHouseNumber(rawPart, out var parsedHouse))
            {
                houseNumber = parsedHouse;
                continue;
            }

            if (apartment is null && TryParseApartment(rawPart, out var parsedApartment))
                apartment = parsedApartment;
        }

        return new Address(
            City: city,
            Region: region,
            Street: street,
            HouseNumber: houseNumber,
            Apartment: apartment
        );
    }

    private static bool TryParseHouseNumber(string part, out string? houseNumber)
    {
        houseNumber = null;
        if (!HousePrefixRegex.IsMatch(part))
            return false;

        var parsed = HousePrefixRegex.Replace(part, string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(parsed))
            return false;

        houseNumber = parsed;
        return true;
    }

    private static bool TryParseApartment(string part, out int? apartment)
    {
        apartment = null;
        if (!ApartmentRegex.IsMatch(part))
            return false;

        var match = DigitsRegex.Match(part);
        if (!match.Success)
            return false;

        if (!int.TryParse(match.Value, out var parsed))
            return false;

        apartment = parsed;
        return true;
    }

    private static string NormalizeCity(string rawPart)
    {
        var trimmed = rawPart.Trim();
        var cityWithoutSuffix = CityRegex.Replace(trimmed, string.Empty).Trim();
        return string.IsNullOrWhiteSpace(cityWithoutSuffix) ? trimmed : NormalizeSpaces(cityWithoutSuffix);
    }

    private static string NormalizeSpaces(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;

        return MultiSpaceRegex.Replace(value.Trim(), " ");
    }
}
