using System.Reflection;
using System.Text.Json;
using Letterbook.ActivityPub.Models;

namespace Letterbook.ActivityPub.Tests;

public class ConvertObjectReaderTest
{
    private string _dataDir => Path.Join(
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data");

    [Fact]
    public void CanDeserialize()
    {
        var opts = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        var actual = JsonSerializer
            .Deserialize<Activity>(new FileStream(Path.Join(_dataDir, "mastodon_create_note.json"), FileMode.Open),
                opts);

        Assert.Equal("Create", actual.Type.First());
    }
}