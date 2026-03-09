using StranaKart.Assignment.Mapping;
using StranaKart.Assignment.Services;

namespace StranaKart.Assignment.Tests;

public class AddressTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Parse_ReturnsEmptyAddress_WhenInputIsMissing(string? fullAddress)
    {
        var result = AddressParser.Parse(fullAddress);

        Assert.Null(result.Region);
        Assert.Null(result.City);
        Assert.Null(result.Street);
        Assert.Null(result.HouseNumber);
        Assert.Null(result.Apartment);
    }

    [Theory]
    [MemberData(nameof(ParseCases))]
    public void Parse_ExtractsExpectedAddressParts(
        string fullAddress,
        string? expectedRegion,
        string? expectedCity,
        string? expectedStreet,
        string? expectedHouseNumber,
        int? expectedApartment)
    {
        var result = AddressParser.Parse(fullAddress);

        Assert.Equal(expectedRegion, result.Region);
        Assert.Equal(expectedCity, result.City);
        Assert.Equal(expectedStreet, result.Street);
        Assert.Equal(expectedHouseNumber, result.HouseNumber);
        Assert.Equal(expectedApartment, result.Apartment);
    }

    public static TheoryData<string, string?, string?, string?, string?, int?> ParseCases => new()
    {
        {
            "196210, Санкт-Петербург г, Стартовая ул, дом № 8, Литера А, офис 132",
            null,
            "Санкт-Петербург",
            "Стартовая ул",
            "8",
            132
        },
        {
            "187032, Ленинградская обл, Тосненский р-н, Тельмана г, Красноборская дорога ул, з/у 8",
            "Ленинградская обл",
            "Тельмана",
            "Красноборская дорога ул",
            "8",
            null
        },
        {
            "Псковская обл, Псков г, Зональное ш, дом № 26, А, пом.2002",
            "Псковская обл",
            "Псков",
            "Зональное ш",
            "26",
            2002
        },
        {
            "142305, Московская обл, Чехов г, 8-е Марта ул, владение 2",
            "Московская обл",
            "Чехов",
            "8-е Марта ул",
            "2",
            null
        },
        {
            "624350, Свердловская обл, Качканар г, Привокзальная ул, зд.4",
            "Свердловская обл",
            "Качканар",
            "Привокзальная ул",
            "4",
            null
        },
        {
            "654018, Кемеровская область - Кузбасс обл, Новокузнецк г, Полесская  ул., дом № 15",
            "Кемеровская область - Кузбасс обл",
            "Новокузнецк",
            "Полесская ул.",
            "15",
            null
        },
        {
            "428903, Чувашская Республика - Чувашия, Чебоксары г, Лапсарский проезд, дом № 2, Литера А, помещение 1",
            "Чувашская Республика - Чувашия",
            "Чебоксары",
            "Лапсарский проезд",
            "2",
            1
        },
        {
            "633102, Новосибирская обл, Обь г, Мозжерина пр-кт, стр 22",
            "Новосибирская обл",
            "Обь",
            "Мозжерина пр-кт",
            null,
            null
        },
        {
            "Ереван г, Аэропорт Звартноц тер",
            null,
            "Ереван",
            "Аэропорт Звартноц тер",
            null,
            null
        },
        {
            "184511, Мурманская обл, Мончегорск г, Комарова ул, дом № 25, помещ.VI",
            "Мурманская обл",
            "Мончегорск",
            "Комарова ул",
            "25",
            null
        },
        {
            "142715, Московская обл, Видное г, Проектируемый N251 (Северная промзона пр проезд, дом № 1, помещение 5, 1",
            "Московская обл",
            "Видное",
            "Проектируемый N251 (Северная промзона пр проезд",
            "1",
            5
        }
    };
}
