using System.Net.Mime;
using System.Text.Json.Serialization;

namespace Letterbook.ActivityPub.Models;

[JsonConverter(typeof(ConvertResolvable))]
public abstract class BaseObject : IResolvable
{
    public Uri? Id { get; set; }

    [JsonConverter(typeof(ConvertList<Uri>))]
    public IList<Uri> Type { get; set; } = new List<Uri>();

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> Attachment { get; set; } = new List<IResolvable>();

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> AttributedTo { get; set; } = new List<IResolvable>();

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> Audience { get; set; } = new List<IResolvable>();

    public ContentMap? Content { get; set; }

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> Context { get; set; } = new List<IResolvable>();

    public ContentMap? Name { get; set; }
    public DateTime? EndTime { get; set; }

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> Generator { get; set; } = new List<IResolvable>();

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> Icon { get; set; } = new List<IResolvable>();

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> Image { get; set; } = new List<IResolvable>();

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> InReplyTo { get; set; } = new List<IResolvable>();

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> Location { get; set; } = new List<IResolvable>();

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> Preview { get; set; } = new List<IResolvable>();

    public DateTime? Published { get; set; }
    public Collection? Replies { get; set; }
    public DateTime? StartTime { get; set; }
    public ContentMap? Summary { get; set; }

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> Tag { get; set; } = new List<IResolvable>();

    public DateTime? Updated { get; set; }

    [JsonConverter(typeof(ConvertList<Link>))]
    public IList<Link> Url { get; set; } = new List<Link>();

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> To { get; set; } = new List<IResolvable>();

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> Bto { get; set; } = new List<IResolvable>();

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> Cc { get; set; } = new List<IResolvable>();

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> Bcc { get; set; } = new List<IResolvable>();

    public ContentType? MediaType { get; set; }
    public TimeSpan? Duration { get; set; }

    public Uri? SourceUrl => Id;
    public bool Verified { get; set; } = false;
}