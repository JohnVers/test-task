namespace task.Web.Api.Models;

public sealed class CitySearchRequest
{
    public required string City { get; init; }
    public string? Region { get; init; }
}
