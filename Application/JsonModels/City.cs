using System.Text.Json.Serialization;
using Domain.Entities;

namespace Application.JsonModels;

public class City
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("cityID")]
    public int? CityId { get; set; }

    [JsonPropertyName("latitude")]
    public string Latitude { get; set; } = string.Empty;

    [JsonPropertyName("longitude")]
    public string Longitude { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("timeshift")]
    public string Timeshift { get; set; } = string.Empty;

    [JsonPropertyName("requestEndTime")]
    public string RequestEndTime { get; set; } = string.Empty;

    [JsonPropertyName("sfrequestEndTime")]
    public string SfRequestEndTime { get; set; } = string.Empty;

    [JsonPropertyName("day2dayRequest")]
    public string Day2DayRequest { get; set; } = string.Empty;

    [JsonPropertyName("day2daySFRequest")]
    public string Day2DaySFRequest { get; set; } = string.Empty;

    [JsonPropertyName("preorderRequest")]
    public string PreorderRequest { get; set; } = string.Empty;

    [JsonPropertyName("freeStorageDays")]
    public string FreeStorageDays { get; set; } = string.Empty;

    [JsonPropertyName("terminals")]
    public TerminalsContainer Terminals { get; set; } = new();

    public IEnumerable<Office> GetTerminals()
    {
        var offices = Terminals.Terminals.Select(terminal =>
        {
            var office = terminal.CreateEntity();

            office.Id = Convert.ToInt32(Id);
            office.CountryCode = "RU";
            office.AddressCity = Name;

            return office;
        }).ToList();
        
        return offices;
    }
}