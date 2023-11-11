using System.Text.Json;
using System.Text.Json.Serialization;
using Letterbook.ActivityPub.Models;

namespace Letterbook.ActivityPub;

public class ConvertCollection : JsonConverter<Collection>
{
    public override Collection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            var id = reader.GetString();
            return id != null ? new Collection { Id = id, Type = default! } : default;
        }

        var opts = new JsonSerializerOptions(options);
        opts.Converters.RemoveAt(opts.Converters.IndexOf(this));
        return JsonSerializer.Deserialize<Collection>(ref reader, opts);
    }

    public override void Write(Utf8JsonWriter writer, Collection value, JsonSerializerOptions options)
    {
        var props = value.GetType().GetProperties()
            .Where(p =>
            {
                var v = p.GetValue(value);
                return v switch
                {
                    null => false, // exclude nulls
                    System.Collections.IEnumerable e => e.GetEnumerator().MoveNext(), // exclude empty enumerables
                    var other => !string.IsNullOrEmpty(other.ToString()) // exclude objects with no stringification
                };
            })
            .Select(p => p.Name)
            .ToList();

        var opts = new JsonSerializerOptions(options);
        opts.Converters.RemoveAt(opts.Converters.IndexOf(this));
        writer.WriteRawValue(props.Union(new[] { "Id", "Type" }).Count() > 2
            ? JsonSerializer.SerializeToUtf8Bytes(value, opts)
            : JsonSerializer.SerializeToUtf8Bytes(value.Id, opts));
    }
}    