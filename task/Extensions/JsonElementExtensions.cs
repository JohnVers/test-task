using System.Text.Json;

namespace task.Extensions;

internal static class JsonElementExtensions
{
    public static bool TryGetPropertyIgnoreCase(this JsonElement element, string propertyName, out JsonElement jsonElement)
    {
        try
        {
            jsonElement = element.GetPropertyIgnoreCase(propertyName);
            return true;
        }
        catch (ArgumentNullException)
        {
            throw;
        }
        catch (Exception)
        {
            jsonElement = default;
            return false;
        }
    }

    public static JsonElement GetPropertyIgnoreCase(this JsonElement element, string propertyName)
    {
        var jsonElement = element.EnumerateObject().First(o => propertyName.Equals(o.Name, StringComparison.InvariantCultureIgnoreCase));
        return jsonElement.Value;
    }

    public static JsonElement? GetPropertyOrNull(this JsonElement jsonElement, string propertyName)
    {
        return jsonElement.TryGetPropertyIgnoreCase(propertyName, out var property)
            ? property
            : (JsonElement?)null;
    }
}
