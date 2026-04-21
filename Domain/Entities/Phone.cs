namespace task.Domain.Entities;

public sealed class Phone
{
    public int Id { get; set; }
    public int OfficeId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Additional { get; set; }
    public Office Office { get; set; } = null!;
}
