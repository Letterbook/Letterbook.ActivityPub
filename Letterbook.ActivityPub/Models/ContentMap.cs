using System.Globalization;

namespace Letterbook.ActivityPub.Models;

public class ContentMap : Dictionary<CultureInfo, string>
{
    public ICollection<CultureInfo> Languages => Keys;

    private readonly CultureInfo _defaultCulture;

    public ContentMap(CultureInfo cultureInfo)
    {
        _defaultCulture = cultureInfo;
    }
    
    public ContentMap() : this(CultureInfo.InvariantCulture)
    {}

    public override string ToString()
    {
        return TryGetValue(_defaultCulture, out var value) ? value : this.FirstOrDefault().Value;
    }

    public void Add(string key, string value) => Add(CultureInfo.GetCultureInfoByIetfLanguageTag(key), value);

    public void Add(KeyValuePair<string, string> kvp) => Add(kvp.Key, kvp.Value);
}
