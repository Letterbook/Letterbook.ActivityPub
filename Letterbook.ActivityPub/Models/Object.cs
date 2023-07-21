using System.Net.Mime;
using System.Text.Json.Serialization;

namespace Letterbook.ActivityPub.Models;

public class Object : IResolvable
{
    private IEnumerable<LdContext> _ldContext = new HashSet<LdContext>();

    [JsonPropertyName("@context")]
    [JsonConverter(typeof(ConvertContext))]
    public IEnumerable<LdContext> LdContext
    {
        get => _ldContext;
        set => _ldContext = value;
    }

    public CompactIri? Id { get; set; }
    public required string Type { get; set; }

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> Attachment { get; set; } = new List<IResolvable>();

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> AttributedTo { get; set; } = new List<IResolvable>();

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> Audience { get; set; } = new List<IResolvable>();

    public string? Content { get; set; }
    public ContentMap? ContentMap { get; set; }

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

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> Url { get; set; } = new List<IResolvable>();

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

    [JsonIgnore] bool IResolvable.Verified { get; set; } = false;

    public void AddContext(LdContext item)
    {
        (_ldContext as HashSet<LdContext>)?.Add(item);
    }

    public void AddContext()
    {
        AddContext(Models.LdContext.ActivityStreams);
    }
}