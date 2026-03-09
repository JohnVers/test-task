namespace StranaKart.Assignment.Domain.Entities;

public class Office
{
    public int Id { get; set; }
    public Guid? Uuid { get; set; }

    public string? Code { get; set; }
    public required string CityCode { get; set; }

    public OfficeType? Type { get; set; }

    public required string CountryCode { get; set; }

    public string? AddressRegion { get; set; }
    public string? AddressCity { get; set; }
    public string? AddressStreet { get; set; }
    public string? AddressHouseNumber { get; set; }
    public int? AddressApartment { get; set; }

    public Coordinates? Coordinates { get; set; }
    public string? WorkTime { get; set; }

    public required ICollection<OfficePhone> Phones { get; init; } = new List<OfficePhone>();

    public Office()
    {
    }
}