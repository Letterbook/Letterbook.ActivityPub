namespace Letterbook.ActivityPub.Models;

public class PropertyValue : Object
{
    public new string Type
    {
        get => "PropertyString";
        set => base.Type = value;
    }

    public new required string Name { get; set; }
    public required string Value { get; set; }
    
}