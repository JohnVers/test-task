using task.Entities;

namespace task.Common;

internal static class JsonMappings
{
    private static readonly Dictionary<string, ProperyDescription> OfficeMap = new()
    {
        { nameof(Office.Id), new("id", typeof(StringToIntConverter)) },
        { nameof(Office.Code), new ("code") },
        { nameof(Office.AddressCity), new ("name") },
        { nameof(Office.AddressStreet), new ("address") },
        { nameof(Office.AddressHouseNumber), new ("fullAddress") },
        //{ nameof(Office.WorkTime), new ("") }, // Не совсем ясно какое из расписаний брать из worktables
        { nameof(Office.Phones), new ("phones") },
    };

    private static readonly Dictionary<string, ProperyDescription> PhoneMap = new()
    {
        { nameof(Phone.PhoneNumber), new("number") },
        { nameof(Phone.Additional), new("comment") }
    };

    public static readonly Dictionary<Type, Dictionary<string, ProperyDescription>> Map = new()
    {
        { typeof(Office), OfficeMap },
        { typeof(Phone), PhoneMap }
    };
}
