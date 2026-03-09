namespace StranaKart.Assignment.Domain.Entities;

public class OfficePhone
{
    public int OfficeId { get; set; }
    public required string PhoneNumber { get; set; }
    public string? Additional { get; set; }
}