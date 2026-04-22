namespace Api.Models
{
    public class SearchByCityViewModel
    {
        public int Id { get; set; }
        public string AddressCity { get; set; }
        public string AddressRegion { get; set; }
        public string AddressStreet { get; set; }
        public string AddressHouseNumber { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public List<string> Phones { get; set; }
        public CoordinatesViewModel Coordinates { get; set; }
    }

    public class CoordinatesViewModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}