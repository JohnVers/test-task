using System.Text.Json.Serialization;

namespace StranaKart.Assignment.Contracts;

public class CityListDto
{
    [JsonPropertyName("city")] public List<CityDto>? City { get; set; }

    public class CityDto
    {
        [JsonPropertyName("id")] public string? Id { get; set; }
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("code")] public string? Code { get; set; }
        [JsonPropertyName("cityID")] public int? CityId { get; set; }
        [JsonPropertyName("latitude")] public string? Latitude { get; set; }
        [JsonPropertyName("longitude")] public string? Longitude { get; set; }
        [JsonPropertyName("url")] public string? Url { get; set; }
        [JsonPropertyName("timeshift")] public string? Timeshift { get; set; }
        [JsonPropertyName("requestEndTime")] public string? RequestEndTime { get; set; }
        [JsonPropertyName("sfrequestEndTime")] public string? SfrequestEndTime { get; set; }
        [JsonPropertyName("day2dayRequest")] public string? Day2dayRequest { get; set; }
        [JsonPropertyName("day2daySFRequest")] public string? Day2daySFRequest { get; set; }
        [JsonPropertyName("preorderRequest")] public string? PreorderRequest { get; set; }
        [JsonPropertyName("freeStorageDays")] public string? FreeStorageDays { get; set; }
        [JsonPropertyName("terminals")] public TerminalsDto? Terminals { get; set; }
    }

    public class TerminalsDto
    {
        [JsonPropertyName("terminal")] public List<TerminalDto>? Terminal { get; set; }
    }

    public class TerminalDto
    {
        [JsonPropertyName("id")] public string? Id { get; set; }
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("address")] public string? Address { get; set; }
        [JsonPropertyName("fullAddress")] public string? FullAddress { get; set; }
        [JsonPropertyName("latitude")] public string? Latitude { get; set; }
        [JsonPropertyName("longitude")] public string? Longitude { get; set; }
        [JsonPropertyName("isPVZ")] public bool? IsPVZ { get; set; }
        [JsonPropertyName("cashOnDelivery")] public bool? CashOnDelivery { get; set; }
        [JsonPropertyName("phones")] public List<PhoneDto>? Phones { get; set; }
        [JsonPropertyName("storage")] public bool? Storage { get; set; }
        [JsonPropertyName("mail")] public string? Mail { get; set; }
        [JsonPropertyName("isOffice")] public bool? IsOffice { get; set; }
        [JsonPropertyName("receiveCargo")] public bool? ReceiveCargo { get; set; }
        [JsonPropertyName("giveoutCargo")] public bool? GiveoutCargo { get; set; }
        [JsonPropertyName("default")] public bool? Default { get; set; }
        [JsonPropertyName("calcSchedule")] public CalcScheduleDto? CalcSchedule { get; set; }
        [JsonPropertyName("addressCode")] public AddressCodeDto? AddressCode { get; set; }
        [JsonPropertyName("mainPhone")] public string? MainPhone { get; set; }
        [JsonPropertyName("maxWeight")] public double? MaxWeight { get; set; }
        [JsonPropertyName("maxLength")] public double? MaxLength { get; set; }
        [JsonPropertyName("maxWidth")] public double? MaxWidth { get; set; }
        [JsonPropertyName("maxHeight")] public double? MaxHeight { get; set; }
        [JsonPropertyName("maxVolume")] public double? MaxVolume { get; set; }

        [JsonPropertyName("maxShippingWeight")]
        public double? MaxShippingWeight { get; set; }

        [JsonPropertyName("maxShippingVolume")]
        public double? MaxShippingVolume { get; set; }

        [JsonPropertyName("worktables")] public WorktablesDto? Worktables { get; set; }
    }

    public class PhoneDto
    {
        [JsonPropertyName("number")] public string? Number { get; set; }
        [JsonPropertyName("type")] public string? Type { get; set; }
        [JsonPropertyName("comment")] public string? Comment { get; set; }
        [JsonPropertyName("primary")] public bool? Primary { get; set; }
    }

    public class AddressCodeDto
    {
        [JsonPropertyName("street_code")] public string? StreetCode { get; set; }
    }

    public class CalcScheduleDto
    {
        [JsonPropertyName("derival")] public string? Derival { get; set; }
        [JsonPropertyName("arrival")] public string? Arrival { get; set; }
    }

    public class WorktablesDto
    {
        [JsonPropertyName("specialWorktable")] public SpecialWorktableDto? SpecialWorktable { get; set; }
        [JsonPropertyName("worktable")] public List<WorktableDto>? Worktable { get; set; }
    }

    public class WorktableDto
    {
        [JsonPropertyName("department")] public string? Department { get; set; }
        [JsonPropertyName("timetable")] public string? Timetable { get; set; }
        [JsonPropertyName("monday")] public string? Monday { get; set; }
        [JsonPropertyName("tuesday")] public string? Tuesday { get; set; }
        [JsonPropertyName("wednesday")] public string? Wednesday { get; set; }
        [JsonPropertyName("thursday")] public string? Thursday { get; set; }
        [JsonPropertyName("friday")] public string? Friday { get; set; }
        [JsonPropertyName("saturday")] public string? Saturday { get; set; }
        [JsonPropertyName("sunday")] public string? Sunday { get; set; }
    }

    public class SpecialWorktableDto
    {
        [JsonPropertyName("receive")] public List<string>? Receive { get; set; }
        [JsonPropertyName("giveout")] public List<string>? Giveout { get; set; }
    }
}