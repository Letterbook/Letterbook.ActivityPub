namespace Letterbook.ActivityPub.Tests;

[Trait("Models", "CompactIri")]
public class CompactIriTest
{
    [Theory]
    [InlineData("as:activity", "https://www.w3.org/ns/activitystreams#activity")]
    [InlineData("xsd:doc", "http://www.w3.org/2001/XMLSchema#doc")]
    [InlineData("schema:person", "http://schema.org/person")]
    [InlineData("ldp:jobtitle", "http://www.w3.org/ns/ldp#jobtitle")]
    public void ParseCompactStrings(string compact, string expected)
    {
        var success = CompactIri.TryCreateCompact(compact, out var actual);
        
        Assert.True(success);
        Assert.Equal(new Uri(expected).ToString(), actual!.ToString());
    }
    
    [Theory]
    [InlineData("https://www.w3.org/ns/activitystreams#activity")]
    [InlineData("http://www.w3.org/2001/XMLSchema#doc")]
    [InlineData("ftp://example.com/source")]
    [InlineData("user:password@example.com:8080/path?q=a#a:1")]
    [InlineData("as:no:no")]
    [InlineData("as:")]
    [InlineData("invalid-prefix:no")]
    [InlineData("http://192.168.1.1:8080/")]
    public void FallbackToUri(string uri)
    {
        var created = CompactIri.TryCreateCompact(uri, out var actual);
        
        Assert.True(created);
        Assert.Null(actual!.Namespace);
        Assert.Equal(uri, actual.ToString());
    }
    
    [Theory]
    [InlineData("as:activity")]
    [InlineData("xsd:doc")]
    [InlineData("schema:person")]
    [InlineData("ldp:jobtitle")]
    public void FormatCompactUri(string compact)
    {
        var success = CompactIri.TryCreateCompact(compact, out var actual);
        
        Assert.True(success);
        Assert.Equal(compact, actual!.ToCompact());
    }
}