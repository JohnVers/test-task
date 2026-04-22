namespace Domain.Entities
{
    public class Office
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public int CityCode { get; set; }
        public string? Uuid { get; set; }
        public OfficeType? Type { get; set; }
        public string CountryCode { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? AddressRegion { get; set; }
        public string? AddressCity { get; set; }
        public string? AddressStreet { get; set; }
        public string? AddressHouseNumber { get; set; }
        public int? AddressApartment { get; set; }
        public string WorkTime { get; set; } = string.Empty;

        public List<Phone> Phones { get; set; } = new();
    }

}
