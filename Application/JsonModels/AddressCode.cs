using System.Text.Json.Serialization;

namespace Application.JsonModels;

public class AddressCode
{
    [JsonPropertyName("street_code")]
    public string? StreetCode { get; set; }

    [JsonPropertyName("place_code")]
    public string? PlaceCode { get; set; }
}