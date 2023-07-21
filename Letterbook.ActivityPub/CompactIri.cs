using System.Text.Json.Serialization;

namespace Letterbook.ActivityPub;

/// <summary>
/// <c>CompactIri</c> are Internationalized Resource Identifier's. IRI's are an extension of <c>Uri</c>'s with support for Unicode characters instead of just ASCII characters.
/// </summary>
/// <seealso href="https://www.w3.org/International/articles/idn-and-iri/"/>
/// <seealso href="https://www.w3.org/International/O-URL-and-ident.html"/>

[JsonConverter(typeof(ConvertCompactIri))]
public class CompactIri : Uri
{
    public static Dictionary<string, string> Namespaces = new(new List<KeyValuePair<string, string>>(new[]
    {
        new KeyValuePair<string, string>("dc11", "http://purl.org/dc/elements/1.1/"),
        new KeyValuePair<string, string>("dcterms", "http://purl.org/dc/terms/"),
        new KeyValuePair<string, string>("cred", "https://w3id.org/credentials#"),
        new KeyValuePair<string, string>("foaf", "http://xmlns.com/foaf/0.1/"),
        new KeyValuePair<string, string>("geojson", "https://purl.org/geojson/vocab#"),
        new KeyValuePair<string, string>("prov", "http://www.w3.org/ns/prov#"),
        new KeyValuePair<string, string>("i18n", "https://www.w3.org/ns/i18n#"),
        new KeyValuePair<string, string>("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#"),
        new KeyValuePair<string, string>("schema", "http://schema.org/"),
        new KeyValuePair<string, string>("skos", "http://www.w3.org/2004/02/skos/core#"),
        new KeyValuePair<string, string>("xsd", "http://www.w3.org/2001/XMLSchema#"),
        new KeyValuePair<string, string>("as", "https://www.w3.org/ns/activitystreams#"),
        new KeyValuePair<string, string>("ldp", "http://www.w3.org/ns/ldp#"),
        new KeyValuePair<string, string>("vcard", "http://www.w3.org/2006/vcard/ns#"),
    }));

    public string? Namespace { get; }
    public string? Prefix { get; }
    public string? Suffix { get; }

    public CompactIri(string ns, string prefix, string suffix) : base(ns + suffix)
    {
        Namespace = ns;
        Prefix = prefix;
        Suffix = suffix;
    }

    public CompactIri(string prefix, string suffix) : this(Namespaces[prefix], prefix, suffix)
    {
    }

    public CompactIri(string uri) : base(uri)
    {
    }
    
    public static implicit operator CompactIri(string s) => new (s);

    public static CompactIri FromUri(Uri u) => new (u.ToString());

    public static bool TryCreateCompact(string compactUrl, out CompactIri? value)
    {
        if (IsCompactUri(compactUrl, out var prefix, out var suffix)
            && Namespaces.ContainsKey(prefix!)
            && prefix != string.Empty
            && suffix != string.Empty)
        {
            value = new CompactIri(prefix!, suffix!);
            return true;
        }

        var result = TryCreate(compactUrl, UriKind.Absolute, out var uri);
        value = result ? new CompactIri(uri!.ToString()) : default;
        return result;
    }

    public static bool IsCompactUri(string compactUrl, out string? prefix, out string? suffix)
    {
        prefix = default;
        suffix = default;
        // unknown indicates it's likely not a valid hostname
        // false on scheme name indicates it doesn't have a valid scheme
        // in either case, this is likely a compacturi, rather than absolute
        if (CheckHostName(compactUrl) != UriHostNameType.Unknown && CheckSchemeName(compactUrl))
        {
            return false;
        }

        var parts = compactUrl.Split(":");
        if (parts.Length != 2)
        {
            return false;
        }

        prefix = parts[0];
        suffix = parts[1];
        return true;
    }

    public string ToCompact() => Prefix != null ? string.Join(":", Prefix, Suffix) : ToString();
}