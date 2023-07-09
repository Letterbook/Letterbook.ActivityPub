using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net.Mime;
using System.Text.Json.Serialization;

namespace Letterbook.ActivityPub.Models;

public class Link : IResolvable
{
    [Required] public Uri Href { get; set; }

    public string? Rel { get; set; }
    public ContentType? MediaType { get; set; }
    public ContentMap? Name { get; set; }
    public CultureInfo? Hreflang { get; set; }
    public uint? Height { get; set; }
    public uint? Width { get; set; }

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> Preview { get; set; } = new List<IResolvable>();

    public Uri? SourceUrl => Href;
    public bool Verified { get; set; }

    public Link(string href) : this(new Uri(href))
    {
    }

    public Link(Uri href)
    {
        Href = href;
    }
}