namespace StranaKart.Assignment.Mapping;

internal sealed record Address(
    string? City = null,
    string? Region = null,
    string? Street = null,
    string? HouseNumber = null,
    int? Apartment = null
);
