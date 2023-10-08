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
            return id != null ? new Collection { Id = id, Type = null } : default;
        }

        var opts = new JsonSerializerOptions(options);
        opts.Converters.RemoveAt(opts.Converters.IndexOf(this));
        return JsonSerializer.Deserialize<Collection>(ref reader, opts);
    }

    public override void Write(Utf8JsonWriter writer, Collection value, JsonSerializerOptions options)
    {
        var opts = new JsonSerializerOptions(options);
        opts.Converters.RemoveAt(opts.Converters.IndexOf(this));
        writer.WriteRawValue(JsonSerializer.SerializeToUtf8Bytes(value, opts));
    }
}