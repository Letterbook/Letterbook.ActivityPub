using System.Reflection;
using System.Text.Json;
using Letterbook.ActivityPub.Models;

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
        var fs = new FileStream(Path.Join(DataDir, activity), FileMode.Open);
        var opts = new JsonSerializerOptions(JsonOptions.ActivityPub);
        var actual = JsonSerializer.Deserialize<Activity>(fs, opts)!;
        
        Assert.Equal(expected, actual.Type.First());
        Assert.NotEmpty(actual.LdContext);
    }
}