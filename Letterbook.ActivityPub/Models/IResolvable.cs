using System.Text.Json.Serialization;

namespace Letterbook.ActivityPub.Models;

[JsonConverter(typeof(ConvertResolvable))]
public interface IResolvable
{
    public CompactIri? Id { get; }
    public string Type { get; }
}