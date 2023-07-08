using System.Text.Json;
using System.Text.Json.Serialization;
using Letterbook.ActivityPub.Models;

namespace Letterbook.ActivityPub;

public class ConvertObject : JsonConverter<BaseObject>
{
    public override BaseObject? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException($"Unexpected value at {reader.Position}");
        }
        
        var forwardReader = reader;
        while (forwardReader.Read())
        {
            if (forwardReader.TokenType == JsonTokenType.PropertyName)
            {
                if (forwardReader.GetString() == "type")
                {
                    forwardReader.Read();
                    var next = forwardReader.GetString();
                    if (string.Compare(next, "Link", StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        throw new JsonException("Cannot deserialize Link as Object");
                    }
                    if (string.Compare(next, "Actor", StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        return JsonSerializer.Deserialize<Actor>(reader: ref reader, options);
                    }
                    if (string.Compare(next, "Collection", StringComparison.InvariantCultureIgnoreCase) == 0 ||
                        string.Compare(next, "OrderedCollection", StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        return JsonSerializer.Deserialize<Collection>(reader: ref reader, options);
                    }
                    if (string.Compare(next, "CollectionPage", StringComparison.InvariantCultureIgnoreCase) == 0 ||
                        string.Compare(next, "OrderedCollectionPage", StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        return JsonSerializer.Deserialize<CollectionPage>(reader: ref reader, options);
                    }
                    return JsonSerializer.Deserialize<Models.Object>(reader: ref reader, options);
                }
            }

            if (forwardReader.TokenType is JsonTokenType.StartObject or JsonTokenType.StartArray)
                forwardReader.Skip();

            if (forwardReader.TokenType == JsonTokenType.EndObject)
                throw new JsonException($"Unexpected end of object at {forwardReader.Position}");
        }
        throw new JsonException($"Unexpected end of input at {forwardReader.Position}");
    }

    public override void Write(Utf8JsonWriter writer, BaseObject value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, options);
    }
}