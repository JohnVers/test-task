using System.Text.Json;
using System.Text.Json.Serialization;

namespace task.Common;

public sealed class StringToIntConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string stringValue = reader.GetString()!;

            return int.TryParse(stringValue, out int intValue)
                ? intValue
                : throw new JsonException($"{Type.Name}. Не удалось преобразовать тип string '{stringValue}' в int");
        }

        return reader.GetInt32();
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}
