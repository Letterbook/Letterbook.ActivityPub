using System.Text.Json;
using Letterbook.ActivityPub.Models;
using Object = Letterbook.ActivityPub.Models.Object;

namespace Letterbook.ActivityPub.Tests;

[Trait("JsonConvert", "Serialize")]
public class ConvertListTests
{
    [Fact]
    public void SerializeList()
    {
        var obj = new Object()
        {
            Type = "Note",
            Attachment = new List<IResolvable>()
            {
                new Object() {Type = "Image"},
                new Object() {Type = "Image"},
                new Object() {Type = "Image"},
            }
        };

        var actual = JsonSerializer.SerializeToElement(obj, JsonOptions.ActivityPub);
        
        Assert.Equal(3, actual.GetProperty("attachment").GetArrayLength());
        Assert.Equal("Image", actual.GetProperty("attachment")[0].GetProperty("type").GetString());
    }
}