namespace Letterbook.ActivityPub.Models;

public class CollectionPage : Collection
{
    public string? PartOf { get; set; }
    public string? Next { get; set; }
    public string? Prev { get; set; }
}