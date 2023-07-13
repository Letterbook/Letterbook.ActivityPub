using System.ComponentModel.DataAnnotations;

namespace Letterbook.ActivityPub.Models;

public class Actor : Object
{
    [Required] public Collection Inbox { get; set; } = new Collection { Type = "OrderedCollection" };
    [Required] public Collection Outbox { get; set; } = new Collection { Type = "OrderedCollection" };
    public Collection? Following { get; set; }
    public Collection? Followers { get; set; }
    public Collection? Liked { get; set; }
    public Collection? Streams { get; set; }
}