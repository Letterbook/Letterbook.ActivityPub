using Letterbook.ActivityPub.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Letterbook.ActivityPub.Tests;

[Trait("JsonConvert", "Serialize")]
public class ConvertCollectionTests
{
    [Fact]
    public void SerializeCollection()
    {
        var collection = new Collection()
        {
            Type = "Collection", 
            Id = new CompactIri("https://letterbook.example/note/1/replies"),
            Items = new List<IResolvable>() { new Link("https://mastodon.example/user/someguy/101") }
        };
        
        var actual = JsonSerializer.Serialize(collection, JsonOptions.ActivityPub);

        Assert.NotNull(JsonSerializer.Deserialize<IResolvable>(actual));
        Assert.NotNull(actual);
    }
}