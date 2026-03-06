using System.Text.Json.Serialization;
using task.Entities;

namespace task.JsonModels;

public class Terminal
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;

    [JsonPropertyName("fullAddress")]
    public string FullAddress { get; set; } = string.Empty;

    [JsonPropertyName("latitude")]
    public string Latitude { get; set; } = string.Empty;

    [JsonPropertyName("longitude")]
    public string Longitude { get; set; } = string.Empty;

    [JsonPropertyName("isPVZ")]
    public bool IsPVZ { get; set; }

    [JsonPropertyName("cashOnDelivery")]
    public bool CashOnDelivery { get; set; }

    [JsonPropertyName("phones")]
    public List<Phone>? Phones { get; set; }

    [JsonPropertyName("storage")]
    public bool Storage { get; set; }

    [JsonPropertyName("mail")]
    public string? Mail { get; set; }

    [JsonPropertyName("isOffice")]
    public bool IsOffice { get; set; }

    [JsonPropertyName("receiveCargo")]
    public bool ReceiveCargo { get; set; }

    [JsonPropertyName("giveoutCargo")]
    public bool GiveoutCargo { get; set; }

    [JsonPropertyName("maps")]
    public object? Maps { get; set; } // Можно использовать JsonElement, но для простоты object

    [JsonPropertyName("default")]
    public bool Default { get; set; }

    [JsonPropertyName("calcSchedule")]
    public CalcSchedule? CalcSchedule { get; set; }

    [JsonPropertyName("addressCode")]
    public AddressCode? AddressCode { get; set; }

    [JsonPropertyName("mainPhone")]
    public string? MainPhone { get; set; }

    [JsonPropertyName("maxWeight")]
    public double? MaxWeight { get; set; }

    [JsonPropertyName("maxLength")]
    public double? MaxLength { get; set; }

    [JsonPropertyName("maxWidth")]
    public double? MaxWidth { get; set; }

    [JsonPropertyName("maxHeight")]
    public double? MaxHeight { get; set; }

    [JsonPropertyName("maxVolume")]
    public double? MaxVolume { get; set; }

    [JsonPropertyName("maxShippingWeight")]
    public double? MaxShippingWeight { get; set; }

    [JsonPropertyName("maxShippingVolume")]
    public double? MaxShippingVolume { get; set; }

    [JsonPropertyName("worktables")]
    public Worktables? Worktables { get; set; }

    // Собирает все неописанные поля (например, maps, если не хотим описывать отдельно)
    [JsonExtensionData]
    public Dictionary<string, object>? ExtensionData { get; set; }
}