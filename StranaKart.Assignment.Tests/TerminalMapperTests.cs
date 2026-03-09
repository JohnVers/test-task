using StranaKart.Assignment.Contracts;
using StranaKart.Assignment.Domain.Entities;
using StranaKart.Assignment.Mapping;
using StranaKart.Assignment.Services;

namespace StranaKart.Assignment.Tests;

public class TerminalMapperTests
{
    [Fact]
    public void TryMapToOffice_MapsOffice_WhenInputIsValid()
    {
        var city = CreateCity(code: " 7800000000000000000000000 ", name: " Санкт-Петербург ");
        var terminal = CreateTerminal(
            id: "39",
            fullAddress: "196210, Санкт-Петербург г, Стартовая ул, дом № 8, Литера А, офис 132",
            latitude: "59.807545",
            longitude: "30.314159",
            isPvz: false,
            streetCode: " 7800000000018790000000000 ",
            phones:
            [
                CreatePhone("7 (812) 448-88-88", " "),
                CreatePhone("7-812-448-88-88", "дубль")
            ],
            workTime: " 09:00-19:00 "
        );

        var office = TerminalMapper.TryMapToOffice(city, terminal, out var skipReason);

        Assert.NotNull(office);
        Assert.Null(skipReason);

        Assert.Equal(39, office.Id);
        Assert.Equal("RU", office.CountryCode);
        Assert.Equal("7800000000000000000000000", office.CityCode);
        Assert.Equal("7800000000018790000000000", office.Code);
        Assert.Equal(OfficeType.Warehouse, office.Type);

        Assert.Equal("Санкт-Петербург", office.AddressCity);
        Assert.Equal("Стартовая ул", office.AddressStreet);
        Assert.Equal("8", office.AddressHouseNumber);
        Assert.Equal(132, office.AddressApartment);
        Assert.Null(office.AddressRegion);

        Assert.NotNull(office.Coordinates);
        Assert.NotNull(office.Coordinates!.Latitude);
        Assert.NotNull(office.Coordinates.Longitude);
        Assert.Equal(59.807545, office.Coordinates.Latitude.Value, 6);
        Assert.Equal(30.314159, office.Coordinates.Longitude.Value, 6);

        var phone = Assert.Single(office.Phones);
        Assert.Equal("78124488888", phone.PhoneNumber);
        Assert.Null(phone.Additional);

        Assert.Equal("09:00-19:00", office.WorkTime);
    }

    [Fact]
    public void TryMapToOffice_ReturnsNull_WhenCityCodeMissing()
    {
        var city = CreateCity(code: " ", name: "Москва");
        var terminal = CreateTerminal(id: "1", fullAddress: "Москва г, Тверская ул, дом № 1");

        var office = TerminalMapper.TryMapToOffice(city, terminal, out var skipReason);

        Assert.Null(office);
        Assert.Equal("отсутствует CityCode", skipReason);
    }

    [Fact]
    public void TryMapToOffice_ReturnsNull_WhenTerminalIdIsNotNumber()
    {
        var city = CreateCity(code: "7700000000000000000000000", name: "Москва");
        var terminal = CreateTerminal(id: "abc", fullAddress: "Москва г, Тверская ул, дом № 1");

        var office = TerminalMapper.TryMapToOffice(city, terminal, out var skipReason);

        Assert.Null(office);
        Assert.Equal("TerminalId не является числом: 'abc'", skipReason);
    }

    [Fact]
    public void TryMapToOffice_UsesCityFromAddress_WhenCityNameMissing()
    {
        var city = CreateCity(code: "6600000000000000000000000", name: " ");
        var terminal = CreateTerminal(id: "101", fullAddress: "660020, Красноярск г, Северное ш, дом № 17");

        var office = TerminalMapper.TryMapToOffice(city, terminal, out var skipReason);

        Assert.NotNull(office);
        Assert.Null(skipReason);
        Assert.Equal("Красноярск", office.AddressCity);
    }

    [Theory]
    [InlineData(true, OfficeType.Pvz)]
    [InlineData(false, OfficeType.Warehouse)]
    [InlineData(null, OfficeType.Warehouse)]
    public void TryMapToOffice_MapsType_FromIsPvzFlag(bool? isPvz, OfficeType expectedType)
    {
        var city = CreateCity(code: "7800000000000000000000000", name: "Санкт-Петербург");
        var terminal = CreateTerminal(id: "55", fullAddress: "Санкт-Петербург г, Стартовая ул, дом № 8", isPvz: isPvz);

        var office = TerminalMapper.TryMapToOffice(city, terminal, out var skipReason);

        Assert.NotNull(office);
        Assert.Null(skipReason);
        Assert.Equal(expectedType, office.Type);
    }

    private static CityListDto.CityDto CreateCity(string? code, string? name)
    {
        return new CityListDto.CityDto
        {
            Code = code,
            Name = name
        };
    }

    private static CityListDto.TerminalDto CreateTerminal(
        string? id,
        string? fullAddress,
        string? latitude = null,
        string? longitude = null,
        bool? isPvz = null,
        string? streetCode = null,
        List<CityListDto.PhoneDto>? phones = null,
        string? workTime = null)
    {
        return new CityListDto.TerminalDto
        {
            Id = id,
            FullAddress = fullAddress,
            Latitude = latitude,
            Longitude = longitude,
            IsPVZ = isPvz,
            AddressCode = streetCode is null ? null : new CityListDto.AddressCodeDto { StreetCode = streetCode },
            Phones = phones,
            Worktables = workTime is null
                ? null
                : new CityListDto.WorktablesDto
                {
                    Worktable = [new CityListDto.WorktableDto { Timetable = workTime }]
                }
        };
    }

    private static CityListDto.PhoneDto CreatePhone(string? number, string? comment)
    {
        return new CityListDto.PhoneDto
        {
            Number = number,
            Comment = comment
        };
    }
}
