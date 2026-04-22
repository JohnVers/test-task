using System.Text.Json.Serialization;

namespace Infrastructure.Models
{
    public class RootJson
    {
        [JsonPropertyName("city")]
        public List<CityJson> City { get; set; } = new();
    }

    public class CityJson
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("cityID")]
        public int? CityId { get; set; }  

        [JsonPropertyName("latitude")]
        public string? Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public string? Longitude { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("timeshift")]
        public string? Timeshift { get; set; }

        [JsonPropertyName("terminals")]
        public TerminalsWrapperJson? Terminals { get; set; }
    }

    public class TerminalsWrapperJson
    {
        [JsonPropertyName("terminal")]
        public List<TerminalJson> Terminal { get; set; } = new();
    }

    public class TerminalJson
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("cityCode")]
        public int CityCode { get; set; }

        [JsonPropertyName("uuid")]
        public string? Uuid { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("countryCode")]
        public string? CountryCode { get; set; }

        [JsonPropertyName("latitude")]
        public string? Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public string? Longitude { get; set; }

        [JsonPropertyName("addressRegion")]
        public string? AddressRegion { get; set; }

        [JsonPropertyName("addressCity")]
        public string? AddressCity { get; set; }

        [JsonPropertyName("addressStreet")]
        public string? AddressStreet { get; set; }

        [JsonPropertyName("addressHouseNumber")]
        public string? AddressHouseNumber { get; set; }

        [JsonPropertyName("addressApartment")]
        public int? AddressApartment { get; set; }

        [JsonPropertyName("workTime")]
        public string? WorkTime { get; set; }

        [JsonPropertyName("phones")]
        public List<PhoneJson> Phones { get; set; } = new();
    }

    public class PhoneJson
    {
        [JsonPropertyName("number")]
        public string? Number { get; set; }

        [JsonPropertyName("additional")]
        public string? Additional { get; set; }
    }
}
