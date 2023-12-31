﻿using System.Text.Json;
using Letterbook.ActivityPub.Models;

namespace Letterbook.ActivityPub.Tests;

[Trait("JsonConvert", "Serialize")]
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

        var actual = JsonSerializer.Serialize(testObject, JsonOptions.ActivityPub);

        Assert.NotNull(JsonSerializer.Deserialize<IResolvable>(actual));
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

        Assert.NotNull(JsonSerializer.Deserialize<IResolvable>(actual));
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

        Assert.NotNull(JsonSerializer.Deserialize<IResolvable>(actual));
        Assert.Matches("https://www.w3.org/ns/activitystreams", actual);
        Assert.Matches("https://mastodon.example/schema#", actual);
        Assert.Matches("@context", actual);
    }

    [Fact]
    public void SerializeSupportedLdContext()
    {
        var opts = JsonOptions.ActivityPub;
        var testObject = new Models.Object
        {
            LdContext = LdContext.SupportedContexts,
            Content = "test content",
            Id = new CompactIri("https://letterbook.example/1"),
            Type = "Note"
        };

        var actual = JsonSerializer.Serialize(testObject, opts);

        Assert.NotNull(JsonSerializer.Deserialize<IResolvable>(actual));
        Assert.Matches("https://www.w3.org/ns/activitystreams", actual);
        Assert.Matches("https://w3id.org/security/v1", actual);
        Assert.Matches("http://schema.org", actual);
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

        Assert.NotNull(JsonSerializer.Deserialize<IResolvable>(actual));
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

        Assert.NotNull(JsonSerializer.Deserialize<IResolvable>(actual));
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

        Assert.NotNull(JsonSerializer.Deserialize<IResolvable>(actual));
        Assert.DoesNotMatch("@context", actual);
    }

    [Fact]
    public void SerializeLinksAsString_WhenOnlyHRefIsSet()
    {
        var link = new Link("https://example.com/");
        var output = JsonSerializer.Serialize<IResolvable>(link, JsonOptions.ActivityPub);
        
        Assert.NotNull(JsonSerializer.Deserialize<IResolvable>(output));
        Assert.Equal("\"https://example.com/\"", output);
    }

    [Fact]
    public void SerializeLinksAsObject_WhenPropertiesAreSet()
    {
        var link = new Link("https://example.com/")
        {
            Rel = "me"
        };

        var output = JsonSerializer.SerializeToElement<IResolvable>(link, JsonOptions.ActivityPub);

        Assert.Equal(JsonValueKind.Object, output.ValueKind);
        Assert.True(output.TryGetProperty("href", out var href));
        Assert.Equal("https://example.com/", href.GetString());
        Assert.True(output.TryGetProperty("rel", out var rel));
        Assert.Equal("me", rel.GetString());
    }

    [Fact]
    public void SerializeEmptyCollectionAsString()
    {
        var col = new Collection() { Id = "https://example.com/collection/0" };

        var actual = JsonSerializer.Serialize(col, JsonOptions.ActivityPub);

        Assert.NotNull(JsonSerializer.Deserialize<IResolvable>(actual));
        Assert.Equal("\"https://example.com/collection/0\"", actual);
    }

    [Fact]
    public void SerializeNestedEmptyCollectionAsString()
    {
        var actor = new Actor()
        {
            Id = "https://example.com/actor/0",
            Type = "Person",
            Inbox = new Collection() { Id = "https://example.com/actor/0/inbox" },
            Outbox = new Collection() { Id = "https://example.com/actor/0/outbox" },
            Followers = new Collection() { Id = "https://example.com/actor/0/followers" },
            Following = new Collection() { Id = "https://example.com/actor/0/following" },
        };

        var actual = JsonSerializer.Serialize(actor, JsonOptions.ActivityPub);
        const string expected = @"{""id"":""https://example.com/actor/0""," +
                                @"""type"":""Person""," +
                                @"""inbox"":""https://example.com/actor/0/inbox""," +
                                @"""outbox"":""https://example.com/actor/0/outbox""," +
                                @"""following"":""https://example.com/actor/0/following""," +
                                @"""followers"":""https://example.com/actor/0/followers""}";
        Assert.NotNull(JsonSerializer.Deserialize<IResolvable>(actual));
        Assert.Equal(expected, actual);
    }
}