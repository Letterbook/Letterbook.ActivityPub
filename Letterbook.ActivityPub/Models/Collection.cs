using System.Text.Json.Serialization;

namespace Letterbook.ActivityPub.Models;

public class Collection : Object
{
    public uint? TotalItems { get; set; }
    public CollectionPage? Current { get; set; }
    public CollectionPage? First { get; set; }
    public CollectionPage? Last { get; set; }

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable>? Items { get; set; }
}