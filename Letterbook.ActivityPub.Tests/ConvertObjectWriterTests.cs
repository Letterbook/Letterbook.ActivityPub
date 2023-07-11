using System.Text.Json;
using Letterbook.ActivityPub.Models;

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
    
    [Fact]
    public void SerializeLdContext()
    {
        var opts = JsonOptions.ActivityPub;
        var testObject = new Models.Object
        {
            Content = "test content",
            Id = new CompactIri("https://letterbook.example/1")
        };
        testObject.AddContext(LdContext.ActivityStreams);
        testObject.AddContext(new LdContext("toot", "https://mastodon.example/schema#"));

        var actual = JsonSerializer.Serialize(testObject, opts);

        Assert.Matches("https://www.w3.org/ns/activitystreams", actual);
        Assert.Matches("https://mastodon.example/schema#", actual);
        Assert.Matches("@context", actual);
    }
    
    [Fact]
    public void SerializeSingleLdContext()
    {
        var opts = JsonOptions.ActivityPub;
        var testObject = new Models.Object
        {
            Content = "test content",
            Id = new CompactIri("https://letterbook.example/1"),
            Type = new List<string>{"Object"}
        };
        testObject.AddContext(LdContext.ActivityStreams);

        var actual = JsonSerializer.Serialize(testObject, opts);

        Assert.Matches("\"@context\": ?\"https://www.w3.org/ns/activitystreams\"", actual);
    }
}