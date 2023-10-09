using System.Globalization;

namespace Letterbook.ActivityPub.Models;

public class ContentMap : Dictionary<string, string>
{
    public ICollection<string> Languages => Keys;

    private readonly string _defaultCulture;

    public ContentMap(string cultureInfo)
    {
        _defaultCulture = cultureInfo;
    }
    
    public ContentMap() : this(CultureInfo.InvariantCulture.Name)
    {}

    public void Add(string defaultValue) => Add(_defaultCulture, defaultValue);
    
    public override string ToString()
    {
        return TryGetValue(_defaultCulture, out var value) ? value : this.FirstOrDefault().Value;
    }
    
}
