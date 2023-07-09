using System.Text.Json;
using System.Text.Json.Serialization;

namespace Letterbook.ActivityPub;

public class ConvertList<T> : JsonConverter<IList<T>>
{
    public override IList<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
            return JsonSerializer.Deserialize<List<T>>(reader: ref reader, options);

        var result = new List<T>();
        var element = JsonSerializer.Deserialize<T>(reader: ref reader, options);

        if (element is not null) result.Add(element);
        return result;
    }

    public override void Write(Utf8JsonWriter writer, IList<T> value, JsonSerializerOptions options)
    {
        switch (value.Count)
        {
            case 1:
                writer.WriteRawValue(JsonSerializer.SerializeToUtf8Bytes(value));
                break;
            case >1:
                writer.WriteRawValue(JsonSerializer.SerializeToUtf8Bytes(value.First()));
                break;
            default:
                writer.WriteNullValue();
                break;
        }
    }
}