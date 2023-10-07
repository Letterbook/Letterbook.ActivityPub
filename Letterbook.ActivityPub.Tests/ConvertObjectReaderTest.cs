using System.Globalization;
using System.Reflection;
using System.Text.Json;
using Letterbook.ActivityPub.Models;
using Object = Letterbook.ActivityPub.Models.Object;

namespace Letterbook.ActivityPub.Tests;

public class ConvertObjectReaderTest
{
    private static string DataDir => Path.Join(
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data");

    [Theory]
    [InlineData("mastodon_create_note.json", "Create")]
    [InlineData("mastodon_create_remote_note.json", "Create")]
    [InlineData("mastodon_delete_note.json", "Delete")]
    [InlineData("mastodon_follow.json", "Follow")]
    [InlineData("mastodon_like.json", "Like")]
    [InlineData("mastodon_undo_like.json", "Undo")]
    [InlineData("mastodon_update_note.json", "Update")]
    public void CanDeserializeRealWorldActivities(string activity, string expected)
    {
        using var fs = new FileStream(Path.Join(DataDir, activity), FileMode.Open);
        var actual = JsonSerializer.Deserialize<Activity>(fs, JsonOptions.ActivityPub)!;
        
        Assert.Equal(expected, actual.Type);
        Assert.NotEmpty(actual.LdContext);
    }

    [Fact]
    public void CanDeserializeRealWorldNote()
    {
        using var fs = new FileStream(Path.Join(DataDir, "mastodon_create_note.json"), FileMode.Open);
        var actual = JsonSerializer.Deserialize<Activity>(fs, JsonOptions.ActivityPub)!;

        if (actual.Object.First() is Object note)
        {
            Assert.Equal("<p>Creating Test Data!!! :)</p>", note.Content);
            Assert.Contains("en", note.ContentMap.Languages);
            Assert.Equal("<p>Creating Test Data!!! :)</p>", note.ContentMap["en"]);
        }
        else
        {
            Assert.Fail("Not a note");
        }
    }

    [Fact]
    public void CanDeserializeRealWorldActor()
    {
        using var fs = new FileStream(Path.Join(DataDir, "mastodon_actor.json"), FileMode.Open);
        var actual = JsonSerializer.Deserialize<Actor>(fs, JsonOptions.ActivityPub)!;
        
        Assert.NotNull(actual);
    }
}