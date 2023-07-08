using System.Text.Json;
using System.Text.Json.Serialization;
using Letterbook.ActivityPub.Models;

namespace Letterbook.ActivityPub;

public class ConvertObject : JsonConverter<Models.Object>
{
    public override Models.Object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, Models.Object value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}