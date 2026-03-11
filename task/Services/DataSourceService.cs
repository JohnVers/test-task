using System.Globalization;
using System.Text;
using System.Text.Json;
using task.Common;
using task.Contract;
using task.Entities;
using task.Extensions;

namespace task.Services;

internal sealed class DataSourceService() : IDataSourceService
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        TypeInfoResolver = new MappingJsonResolver()
    };

    async Task<IList<Office>> IDataSourceService.LoadAsync(string filePath, CancellationToken cancellationToken)
    {
        var result = new List<Office>();

        using (var fileStream = File.OpenRead(filePath))
        {
            using (JsonDocument doc = await JsonDocument.ParseAsync(fileStream, cancellationToken: cancellationToken))
            {
                var cityElements = doc.RootElement.EnumerateObject().FirstOrDefault().Value;

                if (cityElements.ValueKind == JsonValueKind.Undefined)
                    return await Task.FromResult(Enumerable.Empty<Office>().ToList());

                foreach (var cityElement in cityElements.EnumerateArray())
                {
                    var terminalsElements = cityElement
                        .GetPropertyIgnoreCase("terminals")
                        .GetPropertyIgnoreCase("terminal");

                    foreach (var terminalElement in terminalsElements.EnumerateArray())
                    {
                        var rawText = terminalElement.GetRawText();

                        Office? office = null;

                        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(rawText)))
                        {
                            office = await JsonSerializer.DeserializeAsync<Office>(stream, JsonSerializerOptions, cancellationToken);
          
                            if (office is null)
                            {
                                throw new JsonException("Не удалось десериализовать объект Office");
                            }

                            office.Code ??= cityElement.GetPropertyOrNull("code")?.GetString();
                            office.CityCode = cityElement.GetPropertyOrNull("cityID")?.GetInt32() ?? default;
                            office.AddressCity = cityElement.GetPropertyOrNull("name")?.GetString();
                            office.Coordinates = new Coordinates
                            {
                                Latitude = GetDouble(terminalElement, "latitude"),
                                Longitude = GetDouble(terminalElement, "longitude")
                            };
                            office.Type = GetOfficeType(terminalElement);

                            office.Phones.ForEach(p =>
                            {
                                p.OfficeId = office.Id;
                                p.Office = office;
                            });
                        }

                        result.Add(office);
                    }
                }
            }

            return await Task.FromResult(result);
        }
    }

    private static double GetDouble(JsonElement jsonElement, string propertyName)
    {
        return double.TryParse(jsonElement.GetPropertyOrNull(propertyName)?.GetString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var value)
            ? value
            : default;
    }

    private static OfficeType? GetOfficeType(JsonElement terminalNode)
    {
        return terminalNode.GetPropertyOrNull("isPVZ")?.GetBoolean() ?? false
                            ? OfficeType.PVZ
                            : terminalNode.GetPropertyOrNull("isOffice")?.GetBoolean() ?? false
                            ? OfficeType.POSTAMAT
                            : terminalNode.GetPropertyOrNull("storage")?.GetBoolean() ?? false
                            ? OfficeType.WAREHOUSE
                            : null;
    }
}
