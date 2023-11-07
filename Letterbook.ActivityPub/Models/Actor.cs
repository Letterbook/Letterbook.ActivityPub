using System.ComponentModel.DataAnnotations;

namespace Letterbook.ActivityPub.Models;

public class Actor : Object
{
    public static List<string> Types = new(new[]
    {
        "Actor",
        "Application",
        "Group",
        "Organization",
        "Person",
        "Service",
    });
    
    [Required] public Collection Inbox { get; set; } = new Collection { Type = "OrderedCollection" };
    [Required] public Collection Outbox { get; set; } = new Collection { Type = "OrderedCollection" };
    public Collection? Following { get; set; }
    public Collection? Followers { get; set; }
    public Collection? Liked { get; set; }
    public Collection? Streams { get; set; }
    public ActorEndpoints? Endpoints { get; set; }
    public PublicKey? PublicKey { get; set; }

    public class ActorEndpoints
    {
        public Uri? ProxyUrl { get; set; }
        public Uri? OauthAuthorizationEndpoint { get; set; }
        public Uri? OauthTokenEndpoint { get; set; }
        public Uri? ProvideClientKey { get; set; }
        public Uri? SignClientKey { get; set; }
        public Uri? SharedInbox { get; set; }
    }
}