using System.Text.Json;
using System.Text.Json.Serialization;

namespace Letterbook.ActivityPub;

public class ConvertCompactIri : JsonConverter<CompactIri>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(CompactIri).IsAssignableFrom(typeToConvert);
    }
    
    public override CompactIri? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Unexpected value at {reader.Position}");
        }

        var jsonValue = reader.GetString();
        if (jsonValue is null)
        {
            throw new JsonException($"Invalid value at {reader.Position}");
        }

        CompactIri.TryCreateCompact(jsonValue, out var value);
        return value;
    }

    public override void Write(Utf8JsonWriter writer, CompactIri value, JsonSerializerOptions options)
    {
        writer.WriteRawValue(JsonSerializer.SerializeToUtf8Bytes(value.ToCompact(), options));
    }
}