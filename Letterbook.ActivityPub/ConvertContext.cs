using System.Text.Json;
using System.Text.Json.Serialization;
using Letterbook.ActivityPub.Models;

namespace Letterbook.ActivityPub;

public class ConvertContext : JsonConverter<IEnumerable<LdContext>>
{
    public override bool CanConvert(Type typeToConvert)
    {
        // var result = typeof(ICollection<LdContext>).IsAssignableFrom(typeToConvert);
        
        // return !result;
        return true;
    }

    public override IEnumerable<LdContext>? Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        var collection = new HashSet<LdContext>();

        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();
            if (value is not null) collection.Add(new LdContext(value));
            return collection;
        }

        if (reader.TokenType == JsonTokenType.StartArray)
        {
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                if (reader.TokenType == JsonTokenType.String)
                {
                    var value = reader.GetString();
                    if (value is not null) collection.Add(new LdContext(value));
                    continue;
                }

                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                    {
                        collection.Add(ReadContextProperty(ref reader));
                    }
                    continue;
                }

                throw new JsonException("Invalid @context");
            }

            return collection;
        }
        
        throw new JsonException("Invalid @context");
    }

    public override void Write(Utf8JsonWriter writer, IEnumerable<LdContext> value, JsonSerializerOptions options)
    {
        // var v = value as IEnumerable<LdContext>;
        writer.WriteRawValue(JsonSerializer.SerializeToUtf8Bytes(value, options));
    }

    private LdContext ReadContextProperty(ref Utf8JsonReader reader)
    {
        if (reader.TokenType == JsonTokenType.PropertyName)
        {
            var prefix = reader.GetString();
            reader.Read();
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException($"Invalid Json property {reader.TokenType} at {reader.Position}");
            }

            var suffix = reader.GetString();
            if (prefix is null || suffix is null)
                throw new JsonException($"Invalid @context entry at {reader.TokenType}");
            return new LdContext(prefix, suffix);
        }
        throw new JsonException($"Invalid @context entry at {reader.TokenType}");
    }
}