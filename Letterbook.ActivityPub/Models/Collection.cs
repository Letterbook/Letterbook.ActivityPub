namespace Letterbook.ActivityPub.Models;

public class Collection : Object
{
    public uint TotalItems { get; set; } = 0;
    public CollectionPage? Current { get; set; }
    public CollectionPage? First { get; set; }
    public CollectionPage? Last { get; set; }
    public CollectionPage? Items { get; set; }
}