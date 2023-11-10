namespace Letterbook.ActivityPub.Models;

public class PublicKey : Object
{
    public IResolvable Owner { get; set; }
    public string PublicKeyPem { get; set; }
}