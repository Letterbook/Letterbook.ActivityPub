using System.Text.Json;

namespace Letterbook.ActivityPub.Tests;

public class ConvertObjectWriterTests
{
    [Fact]
    public void CanSerializeSimpleObject()
    {
        var testObject = new Models.Object();
        testObject.Content = "test content";
        testObject.Id = new CompactIri("https://letterbook.example/1");

        var actual = JsonSerializer.Serialize(testObject);

        Assert.Matches("https://letterbook.example/1", actual);
    }

    [Fact]
    public void ExcludesNull()
    {
        var opts = JsonOptions.ActivityPub;
        var testObject = new Models.Object
        {
            Content = "test content",
            Id = new CompactIri("https://letterbook.example/1")
        };

        var actual = JsonSerializer.Serialize(testObject, opts);

        Assert.DoesNotMatch("bto", actual);
        Assert.DoesNotMatch("null", actual);
        Assert.Matches("test content", actual);
    }
}