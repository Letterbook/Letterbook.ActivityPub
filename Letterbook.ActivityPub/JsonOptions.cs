using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Letterbook.ActivityPub.Models;

namespace Letterbook.ActivityPub;

public static class JsonOptions
{
    private static readonly Func<JsonSerializerOptions> OptionGenerator = () => new JsonSerializerOptions()
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
                        // Prevent serializing IEnumerable fields with length 0
                        // But leave strings alone
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
    private static readonly Lazy<JsonSerializerOptions> Lazy = new(OptionGenerator!);
    public static JsonSerializerOptions ActivityPub => Lazy.Value;
}