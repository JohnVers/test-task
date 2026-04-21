using task.Domain.Entities;
using task.Infrastructure.Import;

namespace task.Infrastructure.Utilities;

public static class TerminalDictionaryMapper
{
    public static List<Office> MapToOffices(TerminalDictionaryRoot? payload)
    {
        var cities = payload?.Cities;
        if (cities is null || cities.Count == 0)
            return [];

        var offices = new List<Office>(cities.Sum(c => c.TerminalsContainer?.Terminals?.Count ?? 0));
        foreach (var city in cities)
        {
            if (!city.CityId.HasValue)
                continue;

            var terminals = city.TerminalsContainer?.Terminals ?? [];
            if (terminals.Count == 0)
                continue;

            var cityCode = city.CityId.Value;
            foreach (var terminal in terminals)
            {
                offices.Add(new Office
                {
                    Code = terminal.Id,
                    CityCode = cityCode,
                    Uuid = Guid.NewGuid().ToString("N"),
                    Type = ResolveOfficeType(terminal),
                    CountryCode = "RU",
                    Coordinates = new Coordinates
                    {
                        Latitude = InvariantDoubleParser.ParseOrZero(terminal.Latitude),
                        Longitude = InvariantDoubleParser.ParseOrZero(terminal.Longitude)
                    },
                    AddressRegion = FullAddressParser.ParseRegion(terminal.FullAddress),
                    AddressCity = city.Name,
                    AddressStreet = terminal.Address,
                    AddressHouseNumber = FullAddressParser.ParseHouseNumber(terminal.FullAddress),
                    AddressApartment = FullAddressParser.ParseApartment(terminal.FullAddress),
                    WorkTime = ResolveWorkTime(terminal),
                    Phones = MapPhones(terminal.Phones)
                });
            }
        }

        return offices;
    }

    private static string ResolveWorkTime(TerminalJsonModel terminal)
    {
        return terminal.CalcSchedule?.Arrival
            ?? terminal.CalcSchedule?.Derival
            ?? "N/A";
    }

    private static List<Phone> MapPhones(List<TerminalPhoneJsonModel>? phones)
    {
        if (phones is null || phones.Count == 0)
            return [];

        return phones
            .Where(x => !string.IsNullOrWhiteSpace(x.Number))
            .Select(x => new Phone
            {
                PhoneNumber = x.Number!,
                Additional = x.Comment
            })
            .ToList();
    }

    private static OfficeType ResolveOfficeType(TerminalJsonModel terminal)
    {
        if (terminal.IsPvz)
            return OfficeType.PVZ;

        if (terminal.IsWarehouse)
            return OfficeType.WAREHOUSE;

        return terminal.IsOffice ? OfficeType.POSTAMAT : OfficeType.WAREHOUSE;
    }
}
