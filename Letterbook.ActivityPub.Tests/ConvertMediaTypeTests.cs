using System.Net.Mime;
using System.Text.Json;
using Object = Letterbook.ActivityPub.Models.Object;

namespace Letterbook.ActivityPub.Tests;

[Trait("JsonConvert", "Serialize")]
public class ConvertMediaTypeTests
{
    [Fact]
    public void SerializeMediaType()
    {
        var o = new Object() { Type = "Image", MediaType = new ContentType("image/jpg")};
        
        var actual = JsonSerializer.SerializeToElement(o, JsonOptions.ActivityPub);
        
        Assert.Equal("image/jpg", actual.GetProperty("mediaType").GetString());
    }
}