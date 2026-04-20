namespace task.Dto;

internal sealed class TerminalsRoot
{
    public List<CityDto> City { get; set; } = new();
}

internal sealed class CityDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public int? CityId { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public TerminalsWrapper? Terminals { get; set; }
}

internal sealed class TerminalsWrapper
{
    public List<TerminalDto>? Terminal { get; set; }
}

internal sealed class TerminalDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? FullAddress { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public bool IsPvz { get; set; }
    public bool IsOffice { get; set; }
    public List<TerminalPhoneDto>? Phones { get; set; }
    public CalcScheduleDto? CalcSchedule { get; set; }
}

internal sealed class TerminalPhoneDto
{
    public string? Number { get; set; }
    public string? Type { get; set; }
    public string? Comment { get; set; }
    public bool Primary { get; set; }
}

internal sealed class CalcScheduleDto
{
    public string? Derival { get; set; }
    public string? Arrival { get; set; }
}
