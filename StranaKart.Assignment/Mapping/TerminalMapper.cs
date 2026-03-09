using System.Globalization;
using StranaKart.Assignment.Contracts;
using StranaKart.Assignment.Domain.Entities;
using StranaKart.Assignment.Extensions;

namespace StranaKart.Assignment.Mapping;

internal static class TerminalMapper
{
    private const string DefaultCountryCode = "RU";

    public static Office? TryMapToOffice(CityListDto.CityDto city, CityListDto.TerminalDto terminal,
        out string? skipReason)
    {
        skipReason = null;

        var cityCode = city.Code.NullIfWhiteSpace();
        if (cityCode is null)
        {
            skipReason = "отсутствует CityCode";
            return null;
        }

        if (string.IsNullOrWhiteSpace(terminal.Id))
        {
            skipReason = "отсутствует TerminalId";
            return null;
        }

        if (!int.TryParse(terminal.Id, out var terminalId))
        {
            skipReason = $"TerminalId не является числом: '{terminal.Id}'";
            return null;
        }

        var address = AddressParser.Parse(terminal.FullAddress);
        var cityName = city.Name.NullIfWhiteSpace() ?? address.City.NullIfWhiteSpace();

        if (cityName is null)
        {
            skipReason = "не удалось определить название города";
            return null;
        }

        return new Office
        {
            Id = terminalId,
            CountryCode = DefaultCountryCode,
            CityCode = cityCode,
            Code = terminal.AddressCode?.StreetCode.NullIfWhiteSpace(),
            Type = terminal.IsPVZ == true ? OfficeType.Pvz : OfficeType.Warehouse,
            AddressCity = cityName.NullIfWhiteSpace(),
            AddressApartment = address.Apartment,
            AddressStreet = address.Street.NullIfWhiteSpace(),
            AddressHouseNumber = address.HouseNumber.NullIfWhiteSpace(),
            AddressRegion = address.Region.NullIfWhiteSpace(),
            Coordinates = ParseCoordinates(terminal.Latitude, terminal.Longitude),
            Phones = ParsePhones(terminal),
            WorkTime = terminal.Worktables?.Worktable?.FirstOrDefault()?.Timetable.NullIfWhiteSpace(),
        };
    }

    private static Coordinates? ParseCoordinates(string? lat, string? lon)
    {
        var hasLat = double.TryParse(lat, NumberStyles.Any, CultureInfo.InvariantCulture, out var latitude);
        var hasLon = double.TryParse(lon, NumberStyles.Any, CultureInfo.InvariantCulture, out var longitude);

        return hasLat && hasLon
            ? new Coordinates { Latitude = latitude, Longitude = longitude }
            : null;
    }

    private static List<OfficePhone> ParsePhones(CityListDto.TerminalDto terminal)
    {
        return terminal.Phones?
            .Select(x => new
            {
                PhoneNumber = NormalizePhoneNumber(x.Number),
                Additional = x.Comment.NullIfWhiteSpace(),
            })
            .Where(x => x.PhoneNumber is not null)
            .DistinctBy(x => x.PhoneNumber)
            .Select(x => new OfficePhone
            {
                PhoneNumber = x.PhoneNumber!,
                Additional = x.Additional,
            })
            .ToList() ?? [];
    }

    private static string? NormalizePhoneNumber(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return null;

        var digitsOnly = new string(phoneNumber.Where(char.IsDigit).ToArray());
        return digitsOnly.Length == 0 ? null : digitsOnly;
    }

}
