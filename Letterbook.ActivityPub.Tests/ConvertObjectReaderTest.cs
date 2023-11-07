using System.Reflection;
using System.Text.Json;
using Letterbook.ActivityPub.Models;
using Object = Letterbook.ActivityPub.Models.Object;
#pragma warning disable CS8602 // Dereference of a possibly null reference.

namespace Letterbook.ActivityPub.Tests;

public class ConvertObjectReaderTest
{
    private static string DataDir => Path.Join(
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data");

    [Trait("JsonConvert", "Marshall")]
    [Theory]
    [InlineData("mastodon_create_note.json", "Create")]
    [InlineData("mastodon_create_remote_note.json", "Create")]
    [InlineData("mastodon_delete_note.json", "Delete")]
    [InlineData("mastodon_follow.json", "Follow")]
    [InlineData("mastodon_like.json", "Like")]
    [InlineData("mastodon_undo_like.json", "Undo")]
    [InlineData("mastodon_update_note.json", "Update")]
    public void CanMarshallRealWorldActivities(string activity, string expected)
    {
        using var fs = new FileStream(Path.Join(DataDir, activity), FileMode.Open);
        var actual = JsonSerializer.Deserialize<Activity>(fs, JsonOptions.ActivityPub)!;
        
        Assert.Equal(expected, actual.Type);
        Assert.NotEmpty(actual.LdContext);
    }

    [Trait("JsonConvert", "Marshall")]
    [Fact]
    public void CanMarshallRealWorldNote()
    {
        using var fs = new FileStream(Path.Join(DataDir, "mastodon_create_note.json"), FileMode.Open);
        var actual = JsonSerializer.Deserialize<Activity>(fs, JsonOptions.ActivityPub)!;

        if (actual.Object.First() is Object note)
        {
            Assert.Equal("<p>Creating Test Data!!! :)</p>", note.Content);
            Assert.Contains("en", note.ContentMap.Languages);
            Assert.Equal("<p>Creating Test Data!!! :)</p>", note.ContentMap["en"]);
            Assert.Equal("Creating Test Data!!! :)", note.Source?.Content);
        }
        else
        {
            Assert.Fail("Not a note");
        }
    }

    [Trait("JsonConvert", "Marshall")]
    [Fact]
    public void CanMarshallRealWorldActor()
    {
        using var fs = new FileStream(Path.Join(DataDir, "mastodon_actor.json"), FileMode.Open);
        var actual = JsonSerializer.Deserialize<Actor>(fs, JsonOptions.ActivityPub)!;
        
        Assert.NotNull(actual);
        Assert.Equal("https://mastodon.example/users/test_actor/following", actual.Following.Id.ToString());
        Assert.Equal("https://mastodon.example/users/test_actor/followers", actual.Followers.Id.ToString());
        Assert.Equal("https://mastodon.example/users/test_actor/inbox", actual.Inbox.Id.ToString());
        Assert.Equal("https://mastodon.example/users/test_actor/outbox", actual.Outbox.Id.ToString());
        Assert.NotNull(actual.PublicKey?.PublicKeyPem);
        Assert.True(actual.Attachment
            .Aggregate(false, (result, each) => result || (each as PropertyValue)?.Name == "email"),
            "Name missing");
        Assert.True(actual.Attachment
            .Aggregate(false, (result, each) => result || (each as PropertyValue)?.Value == "test_actor@example.com"),
            "Value missing");
        // Assert.Contains(actual.Attachment, new PropertyValue() { Name = "email", Value = "test_actor@example.com" });
    }

    [Trait("JsonConvert", "Marshall")]
    [Fact]
    public void CanMarshallMediaType()
    {
        var json = """
                   {
                     "type": "Image",
                     "mediaType": "image/jpeg",
                     "url": "https://cdn.mastodon.example/test_actor/accounts/avatars/109/497/783/827/254/564/original/b0adb5063df194a6.jpg"
                   }
                   """;
        var actual = JsonSerializer.Deserialize<Object>(json, JsonOptions.ActivityPub);
        Assert.NotNull(actual);
    }
}