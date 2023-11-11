using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;
using Letterbook.ActivityPub.Models;
using Object = Letterbook.ActivityPub.Models.Object;

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
                    if (Actor.Types.Contains(next, StringComparer.InvariantCultureIgnoreCase))
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
                    if (string.Compare(next, "PropertyValue", StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        return JsonSerializer.Deserialize<PropertyValue>(ref reader, options);
                    }
                    
                    return JsonSerializer.Deserialize<Object>(ref reader, options);
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
        if (value is Object asObject)
        {
            // Check if the object has properties worth serializing
            var props = asObject.GetType().GetProperties()
                .Where(p =>
                {
                    var v = p.GetValue(asObject);
                    return v switch
                    {
                        null => false, // exclude nulls
                        IEnumerable e => e.GetEnumerator().MoveNext(), // exclude empty enumerables
                        var other => !string.IsNullOrEmpty(other.ToString()) // exclude objects with no stringification
                    };
                })
                .ToList();
            if (props.Count == 1 && props.First().Name == "Id")
            {
                WriteLink(writer, new Link(asObject.Id!), options);
                return;
            }
            JsonSerializer.Serialize(writer, asObject, options);
        }
        
        else if (value is Link asLink)
            WriteLink(writer, asLink, options);
        
        else
            throw new JsonException($"ConvertResolvable can't convert unknown IResolvable of type {value.GetType()}");
    }

    private static void WriteLink(Utf8JsonWriter writer, Link asLink, JsonSerializerOptions options)
    {
        // AS links with *only* a target (href) should be serialized as a string instead
        if (LinkHasOnlyHRef(asLink))
            JsonSerializer.Serialize(writer, asLink.Href, options);
        
        else
            JsonSerializer.Serialize(writer, asLink, options);
    }

    private static bool LinkHasOnlyHRef(Link asLink) =>
        asLink.Rel == null &&
        asLink.MediaType == null &&
        asLink.Name == null &&
        asLink.Hreflang == null &&
        asLink.Height == null &&
        asLink.Width == null &&
        asLink.Preview.Count == 0;
}