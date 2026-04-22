namespace task.Entities;

public class Office
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public int CityCode { get; set; }
    public string? Uuid { get; set; } = Guid.NewGuid().ToString();
    public OfficeType? Type { get; set; }
    public string CountryCode { get; set; } = "RU";
    public Coordinates Coordinates { get; set; }
    public string? AddressRegion { get; set; }
    public string? AddressCity { get; set; }
    public string? AddressStreet { get; set; }
    public string? AddressHouseNumber { get; set; }
    public int? AddressApartment { get; set; }
    public string WorkTime { get; set; }
    public List<Phone> Phones { get; set; }
}
