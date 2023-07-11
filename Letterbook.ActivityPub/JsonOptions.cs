using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Letterbook.ActivityPub.Models;

namespace Letterbook.ActivityPub;

public static class JsonOptions
{
    public static JsonSerializerOptions ActivityPub = new JsonSerializerOptions()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        TypeInfoResolver = new DefaultJsonTypeInfoResolver
        {
            Modifiers =
            {
                (info =>
                {
                    if (!typeof(IResolvable).IsAssignableFrom(info.Type)) return;
                    foreach (var propertyInfo in info.Properties)
                    {
                        if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType) &&
                            propertyInfo.PropertyType != typeof(string))
                        {
                            propertyInfo.ShouldSerialize = static (obj, value) =>
                            {
                                var v = (value as IEnumerable<object>);
                                return v?.Any() == true;
                            };
                        }
                    }
                }),
            }
        }
    };
}