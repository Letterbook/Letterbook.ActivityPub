using System.Globalization;
using System.Net.Mime;

namespace Letterbook.ActivityPub.Models;

public class Link : IResolvable
{
    public Uri Href { get; set; }
    public string? Rel { get; set; }
    public ContentType? MediaType { get; set; }
    public ContentMap? Name { get; set; }
    public CultureInfo? Hreflang { get; set; }
    public uint? Height { get; set; }
    public uint? Width { get; set; }
    public IList<IResolvable> Preview { get; set; } = new List<IResolvable>();

    public Uri? SourceUrl => Href;
    public bool Verified { get; set; }
}