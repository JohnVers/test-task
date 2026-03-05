namespace task.Dto;

class TerminalDto
{
    public string? Id { get; set; }

    public string? Address { get; set; }

    public string? FullAddress { get; set; }

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    public bool IsPVZ { get; set; }

    public bool Storage { get; set; }

    public bool IsOffice { get; set; }

    public PhoneDto[] Phones { get; set; } = [];

    public WorktablesDto? Worktables { get; set; }

    public CalcScheduleDto? CalcSchedule { get; set; }

    public AddressCodeDto? AddressCode { get; set; }
}