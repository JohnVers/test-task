using System.ComponentModel.DataAnnotations;

namespace task.Models;

public class Phone
{
	[Key]
	public int Id { get; set; }

	public int OfficeId { get; set; }

	public string PhoneNumber { get; set; } = null!;

	public string? Additional { get; set; }

	public Office? Office { get; set; }
}
