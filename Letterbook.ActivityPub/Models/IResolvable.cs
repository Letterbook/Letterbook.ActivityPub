namespace Letterbook.ActivityPub.Models;

public interface IResolvable
{
    public Uri? SourceUrl { get; }
    public bool Verified { get; set; }
}