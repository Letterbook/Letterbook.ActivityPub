using System.Text.Json;
using Letterbook.ActivityPub.Models;

namespace Letterbook.ActivityPub.Tests;

public class ConvertObjectWriterTests
{
    [Fact]
    public void CanSerializeSimpleObject()
    {
        var testObject = new Models.Object
        {
            Type = "Object",
            Content = "test content",
            Id = new CompactIri("https://letterbook.example/1")
        };

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
            Id = new CompactIri("https://letterbook.example/1"),
            Type = "Note",
            
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
            Id = new CompactIri("https://letterbook.example/1"),
            Type = "Note"
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
            Type = "Object"
        };
        testObject.AddContext(LdContext.ActivityStreams);

        var actual = JsonSerializer.Serialize(testObject, opts);

        Assert.Matches("\"@context\": ?\"https://www.w3.org/ns/activitystreams\"", actual);
    }

    [Fact]
    public void ExcludeEmptyContext()
    {
        var opts = JsonOptions.ActivityPub;
        var testObject = new Models.Object
        {
            Content = "test content",
            Id = new CompactIri("https://letterbook.example/1"),
            Type = "Object"
        };

        var actual = JsonSerializer.Serialize(testObject, opts);

        Assert.DoesNotMatch("@context", actual);
    }

    [Fact]
    public void SerializeSimpleActivity()
    {
        var opts = JsonOptions.ActivityPub;
        var testActivity = new Activity()
        {
            Content = "test content",
            Id = "https://letterbook.example/1",
            Type = "Like"
        };
        var testTarget = new Models.Object
        {
            Id = "https://mastodon.example/2",
            Type = "Note"
        };
        testActivity.Target.Add(testTarget);

        var actual = JsonSerializer.Serialize(testActivity, opts);

        Assert.DoesNotMatch("@context", actual);
    }
}