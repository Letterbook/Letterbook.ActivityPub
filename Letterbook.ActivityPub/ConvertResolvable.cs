using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;
using Letterbook.ActivityPub.Models;

namespace Letterbook.ActivityPub;

public class ConvertResolvable : JsonConverter<IResolvable>
{
    public override IResolvable? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            var href = reader.GetString();
            return href != null ? new Link(href) : default;
        }

        var forwardReader = reader;
        while (forwardReader.Read())
        {
            if (forwardReader.TokenType is JsonTokenType.PropertyName)
            {
                if (forwardReader.GetString() == "type")
                {
                    forwardReader.Read();
                    var next = forwardReader.GetString();
                    if (string.Compare(next, "Link", StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        return JsonSerializer.Deserialize<Link>(reader: ref reader, options);
                    }
                    return JsonSerializer.Deserialize<BaseObject>(reader: ref reader, options);
                }

                if (forwardReader.GetString() == "href")
                    return JsonSerializer.Deserialize<Link>(reader: ref reader, options);

                if (forwardReader.GetString() == "id")
                    return JsonSerializer.Deserialize<BaseObject>(reader: ref reader, options);
            }

            if (forwardReader.TokenType is JsonTokenType.StartObject or JsonTokenType.StartArray)
            {
                forwardReader.Skip();
                continue;
            }

            if (forwardReader.TokenType == JsonTokenType.EndObject)
                throw new JsonException($"Unexpected end of object at {forwardReader.Position}");
        }
        throw new JsonException($"Unexpected end of input at {forwardReader.Position}");
    }

    public override void Write(Utf8JsonWriter writer, IResolvable value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, options);
    }
}