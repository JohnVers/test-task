using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace task.Models;

public class Office
{
	[Key]
	public int Id { get; set; }
	public string? Code { get; set; }
	public int CityCode { get; set; }
	public string? Uuid { get; set; }
	public OfficeType? Type { get; set; }
	public string CountryCode { get; set; } = "RU";
	public Coordinates Coordinates { get; set; } = new();
	public string? AddressRegion { get; set; }
	public string? AddressCity { get; set; }
	public string? AddressStreet { get; set; }
	public string? AddressHouseNumber { get; set; }
	public int? AddressApartment { get; set; }
	
	[Column(TypeName = "jsonb")]
	public JsonElement WorkTime { get; set; }

	public ICollection<Phone> Phones { get; set; } = new List<Phone>();
}