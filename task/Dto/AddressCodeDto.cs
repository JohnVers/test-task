using System.Text.Json.Serialization;

namespace task.Dto;

class AddressCodeDto
{
    [JsonPropertyName("street_code")]
    public string? StreetCode { get; set; }
}