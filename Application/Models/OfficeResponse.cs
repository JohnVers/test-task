using task.Domain.Entities;

namespace task.Application.Models;

public sealed record OfficeResponse(
    int Id,
    string? Code,
    int CityCode,
    string? Uuid,
    OfficeType? Type,
    string CountryCode,
    double Latitude,
    double Longitude,
    string? AddressRegion,
    string? AddressCity,
    string? AddressStreet,
    string? AddressHouseNumber,
    int? AddressApartment,
    string WorkTime,
    IReadOnlyCollection<PhoneResponse> Phones);

public sealed record PhoneResponse(string PhoneNumber, string? Additional);
