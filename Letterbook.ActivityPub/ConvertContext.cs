using System.Text.Json;
using System.Text.Json.Serialization;
using Letterbook.ActivityPub.Models;

namespace Letterbook.ActivityPub;

public class ConvertContext : JsonConverter<IEnumerable<LdContext>>
{
    
    public override IEnumerable<LdContext> Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        var collection = new HashSet<LdContext>();

        switch (reader.TokenType)
        {
            case JsonTokenType.String:
                ReadContextString(ref reader, collection);
                break;
            
            case JsonTokenType.StartObject:
                ReadContextObject(ref reader, collection);
                break;
            
            case JsonTokenType.StartArray:
                ReadContextArray(ref reader, collection);
                break;
            
            default:
                throw new JsonException("Invalid @context");
        }

        return collection;
    }
    
    private static void ReadContextArray(ref Utf8JsonReader reader, HashSet<LdContext> collection)
    {
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    ReadContextString(ref reader, collection);
                    break;
                
                case JsonTokenType.StartObject:
                    ReadContextObject(ref reader, collection);
                    break;
                
                default:
                    throw new JsonException("Invalid @context");
            }
        }
    }
    
    private static void ReadContextString(ref Utf8JsonReader reader, HashSet<LdContext> collection)
    {
        var value = reader.GetString();
        if (value is not null)
            collection.Add(new LdContext(value));
    }

    private static void ReadContextObject(ref Utf8JsonReader reader, HashSet<LdContext> collection)
    {
        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            var context = ReadContextProperty(ref reader);
            if (context != null) collection.Add(context);
        }
    }

    private static LdContext? ReadContextProperty(ref Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.PropertyName)
            throw new JsonException($"Invalid @context entry at {reader.TokenType}");

        var prefix = reader.GetString();
        if (!reader.Read())
            throw new JsonException("Unexpected end of Json input");
        if (reader.TokenType == JsonTokenType.String)
        {
            var suffix = reader.GetString();
            return new LdContext(prefix, suffix);
        }

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            var suffix = ReadContextPropertyObject(ref reader);
            return suffix != null ? new LdContext(prefix, suffix) : default;
        }
        
        throw new JsonException($"Invalid Json property {reader.TokenType} at {reader.Position}");
    }

    private static string? ReadContextPropertyObject(ref Utf8JsonReader reader)
    {
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject) return default;
            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException($"Invalid @context entry at {reader.TokenType}");
            var key = reader.GetString();
            if (key == "@id")
            {
                break;
            }
            reader.Skip();
        }

        if (!reader.Read() || reader.TokenType != JsonTokenType.String) return default;
        var value = reader.GetString();

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            reader.Skip();
        }
        return value;

    }

    public override void Write(Utf8JsonWriter writer, IEnumerable<LdContext> values, JsonSerializerOptions options)
    {
        var allValues = values.ToList();
        
        // Values without a prefix should be written as a string
        var stringValues = allValues
            .Select(LdContext.AsListItem)
            .Where(item => item is not null)!
            .ToList<string>();
        
        // Values *with* a prefix should be combined into an object
        var objectValues = allValues
            .Select(LdContext.AsMapItem)
            .Where(item => item is not null)
            .ToDictionary(pair => pair!.Value.key, pair => pair!.Value.value);
        
        // If we have only a string, then write it as-is
        if (stringValues.Count == 1 && objectValues.Count == 0)
            writer.WriteStringValue(stringValues.Single());
        
        // If we have only object values, then write a single object
        else if (stringValues.Count == 0)
            WriteObject(writer, objectValues);

        // Otherwise, we write as list
        else
            WriteArray(writer, stringValues, objectValues);
    }
    
    private static void WriteArray(Utf8JsonWriter writer, List<string> stringValues, Dictionary<string, string> objectValues)
    {
        writer.WriteStartArray();

        foreach (var str in stringValues)
        {
            writer.WriteStringValue(str);
        }

        if (objectValues.Any())
        {
            WriteObject(writer, objectValues);
        }
        
        writer.WriteEndArray();
    }

    private static void WriteObject(Utf8JsonWriter writer, Dictionary<string, string> objectValues)
    {
        writer.WriteStartObject();

        foreach (var (key, value) in objectValues)
        {
            writer.WritePropertyName(key);
            writer.WriteStringValue(value);
        }
        
        writer.WriteEndObject();
    }
}