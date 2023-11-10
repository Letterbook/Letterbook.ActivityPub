namespace Letterbook.ActivityPub.Models;

public class LdContext : IEquatable<LdContext>
{
    public static LdContext ActivityStreams = new LdContext("https://www.w3.org/ns/activitystreams");

    public static IEnumerable<LdContext> SupportedContexts = new List<LdContext>
    {
        ActivityStreams,
        new("https://w3id.org/security/v1"),
        new("http://schema.org"),
        new("as", "https://www.w3.org/ns/activitystreams#"),
        new("sec", "https://w3id.org/security/v1#"),
        new("publicKey", "sec:publicKey"),
        new("schema", "http://schema.org#"),
        new("PropertyValue", "schema:PropertyValue"),
    };
    
    public string? Prefix { get; set; }
    public string Suffix { get; set; }

    public LdContext(string suffix)
    {
        Suffix = suffix;
    }

    public LdContext(string? prefix, string suffix) : this(suffix)
    {
        Prefix = prefix;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((LdContext)obj);
    }

    public bool Equals(LdContext? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Prefix == other.Prefix && Suffix.Equals(other.Suffix);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Prefix, Suffix);
    }

    public static bool operator ==(LdContext? left, LdContext? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(LdContext? left, LdContext? right)
    {
        return !Equals(left, right);
    }

    public static string? AsListItem(LdContext item)
    {
        return item.Prefix is null ? item.Suffix : null;
    }

    public static (string key, string value)? AsMapItem(LdContext item)
    {
        return item.Prefix is null 
            ? null 
            : (key: item.Prefix, value: item.Suffix);
    }
}