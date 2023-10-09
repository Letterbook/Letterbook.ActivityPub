using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Letterbook.ActivityPub;

public class ConvertMediaType : JsonConverter<ContentType>
{
    public override ContentType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException("Invalid Json");
        var value = reader.GetString();
        if (value != null) return new ContentType(value);
        return default;
    }

    public override void Write(Utf8JsonWriter writer, ContentType value, JsonSerializerOptions options)
    {
        writer.WriteRawValue(JsonSerializer.SerializeToUtf8Bytes(value.MediaType, options));
    }
}