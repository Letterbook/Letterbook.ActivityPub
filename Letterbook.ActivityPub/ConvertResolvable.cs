using System.Text.Json;
using System.Text.Json.Serialization;
using Letterbook.ActivityPub.Models;

namespace Letterbook.ActivityPub;

public class ConvertResolvable : JsonConverter<IResolvable>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(IResolvable).IsAssignableFrom(typeToConvert);
    }
    
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
                var nextPropertyName = forwardReader.GetString();
                if (nextPropertyName == "type")
                {
                    forwardReader.Read();
                    var next = forwardReader.GetString();
                    if (string.Compare(next, "Link", StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        return JsonSerializer.Deserialize<Link>(reader: ref reader, options);
                    }
                    if (string.Compare(next, "Actor", StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        return JsonSerializer.Deserialize<Actor>(ref reader, options);
                    }
                    if (string.Compare(next, "Collection", StringComparison.InvariantCultureIgnoreCase) == 0 ||
                        string.Compare(next, "OrderedCollection", StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        return JsonSerializer.Deserialize<Collection>(ref reader, options);
                    }
                    if (string.Compare(next, "CollectionPage", StringComparison.InvariantCultureIgnoreCase) == 0 ||
                        string.Compare(next, "OrderedCollectionPage", StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        return JsonSerializer.Deserialize<CollectionPage>(ref reader, options);
                    }
                    if (Activity.Types.Contains(next, StringComparer.InvariantCultureIgnoreCase))
                    {
                        return JsonSerializer.Deserialize<Activity>(ref reader, options);
                    }
                    
                    return JsonSerializer.Deserialize<Models.Object>(ref reader, options);
                }
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
        if(value is Models.Object asObject) writer.WriteRawValue(JsonSerializer.SerializeToUtf8Bytes(asObject, options));
        if(value is Link asLink) writer.WriteRawValue(JsonSerializer.SerializeToUtf8Bytes(asLink, options));
    }
}