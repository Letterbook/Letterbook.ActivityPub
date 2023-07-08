using System.Collections.ObjectModel;

namespace Letterbook.ActivityPub.Models;

public class Actor : BaseObject
{
    public Collection Inbox { get; set; }
    public Collection Outbox { get; set; }
    public Collection? Following { get; set; }
    public Collection? Followers { get; set; }
    public Collection? Liked { get; set; }
    public Collection? Streams { get; set; }
}