using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace Letterbook.ActivityPub.Models;

public class Object : IResolvable
{
    public Uri? Id { get; set; }
    public IList<Uri> Type { get; set; } = new List<Uri>();
    public IList<IResolvable> Attachment { get; set; } = new List<IResolvable>();
    public IList<IResolvable> AttributedTo { get; set; } = new List<IResolvable>();
    public IList<IResolvable> Audience { get; set; } = new List<IResolvable>();
    public ContentMap? Content { get; set; }
    public IList<IResolvable> Context { get; set; } = new List<IResolvable>();
    public ContentMap? Name { get; set; }
    public DateTime? EndTime { get; set; }
    public IList<IResolvable> Generator { get; set; } = new List<IResolvable>();
    public IList<IResolvable> Icon { get; set; } = new List<IResolvable>();
    public IList<IResolvable> Image { get; set; } = new List<IResolvable>();
    public IList<IResolvable> InReplyTo { get; set; } = new List<IResolvable>();
    public IList<IResolvable> Location { get; set; } = new List<IResolvable>();
    public IList<IResolvable> Preview { get; set; } = new List<IResolvable>();
    public DateTime? Published { get; set; }
    public Collection? Replies { get; set; }
    public DateTime? StartTime { get; set; }
    public ContentMap? Summary { get; set; }
    public IList<IResolvable> Tag { get; set; } = new List<IResolvable>();
    public DateTime? Updated { get; set; }
    public IList<Link> Url { get; set; } = new List<Link>();
    public IList<IResolvable> To { get; set; } = new List<IResolvable>();
    public IList<IResolvable> Bto { get; set; } = new List<IResolvable>();
    public IList<IResolvable> Cc { get; set; } = new List<IResolvable>();
    public IList<IResolvable> Bcc { get; set; } = new List<IResolvable>();
    public ContentType? MediaType { get; set; }
    public TimeSpan? Duration { get; set; }

    public Uri? SourceUrl => Id;
    public bool Verified { get; set; }
}