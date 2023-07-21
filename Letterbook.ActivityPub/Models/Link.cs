using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net.Mime;
using System.Text.Json.Serialization;

namespace Letterbook.ActivityPub.Models;

public class Link : IResolvable
{
    public CompactIri Href { get; set; }
    public string Type { get; set; } = "Link";

    public string? Rel { get; set; }
    public ContentType? MediaType { get; set; }
    public ContentMap? Name { get; set; }
    public CultureInfo? Hreflang { get; set; }
    public uint? Height { get; set; }
    public uint? Width { get; set; }

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> Preview { get; set; } = new List<IResolvable>();

    [JsonIgnore]
    public CompactIri? Id
    {
        get => Href;
        set => Href = value!;
    }
    [JsonIgnore] bool IResolvable.Verified { get; set; } = false;

    public Link(string href) : this(new CompactIri(href))
    {
    }

    public Link(CompactIri href)
    {
        Href = href;
    }
}