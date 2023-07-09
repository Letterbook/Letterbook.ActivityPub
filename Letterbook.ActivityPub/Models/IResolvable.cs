using System.Text.Json.Serialization;

namespace Letterbook.ActivityPub.Models;

[JsonConverter(typeof(ConvertResolvable))]
public interface IResolvable
{
    public Uri? SourceUrl { get; }
    public bool Verified { get; set; }
}