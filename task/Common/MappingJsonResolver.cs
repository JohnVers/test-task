using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace task.Common;

public sealed class MappingJsonResolver : DefaultJsonTypeInfoResolver
{
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        var jsonTypeInfo = base.GetTypeInfo(type, options);

        if (JsonMappings.Map.TryGetValue(type, out var mappings))
        {
            foreach (var prop in jsonTypeInfo.Properties)
            {
                if (mappings.TryGetValue(prop.Name, out var desc))
                {
                    prop.Name = desc.JsonPropertyName;
                    prop.CustomConverter = desc.CustomComverterType is not null
                        ? Activator.CreateInstance(desc.CustomComverterType) as JsonConverter
                        : null;
                }
            }
        }

        return jsonTypeInfo;
    }
}
