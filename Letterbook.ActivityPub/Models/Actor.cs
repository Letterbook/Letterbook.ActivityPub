using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Letterbook.ActivityPub.Models;

[JsonConverter(typeof(ConvertResolvable))]
public class Actor : BaseObject
{
    public Collection Inbox { get; set; }
    public Collection Outbox { get; set; }
    public Collection? Following { get; set; }
    public Collection? Followers { get; set; }
    public Collection? Liked { get; set; }
    public Collection? Streams { get; set; }
}