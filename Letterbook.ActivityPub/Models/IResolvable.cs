using System.Text.Json.Serialization;

namespace Letterbook.ActivityPub.Models;

[JsonConverter(typeof(ConvertResolvable))]
public interface IResolvable
{
    public CompactIri? Id { get; }
    public bool Verified { get; set; }
    public string Type { get; set; }
}