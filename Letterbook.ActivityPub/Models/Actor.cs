using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Letterbook.ActivityPub.Models;

public class Actor : Object
{
    [Required] public Collection Inbox { get; set; } = new Collection();
    [Required] public Collection Outbox { get; set; } = new Collection();
    public Collection? Following { get; set; }
    public Collection? Followers { get; set; }
    public Collection? Liked { get; set; }
    public Collection? Streams { get; set; }
}