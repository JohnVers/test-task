namespace task.Entities;

public class Office
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public int CityCode { get; set; }
    public string? Uuid { get; set; }
    public OfficeType? Type { get; set; }
    public string CountryCode { get; set; } = "RU";
    public Coordinates Coordinates { get; set; } = null!;
    public string? AddressRegion { get; set; }
    public string? AddressCity { get; set; }
    public string? AddressStreet { get; set; }
    public string? AddressHouseNumber { get; set; }
    public int? AddressApartment { get; set; }
    public string WorkTime { get; set; } = string.Empty;
    public List<Phone> Phones { get; set; } = [];

    public Office() { }
}
