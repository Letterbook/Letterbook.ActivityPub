using System.Text.Json.Serialization;

namespace Letterbook.ActivityPub.Models;

[JsonConverter(typeof(ConvertResolvable))]
public class Object : BaseObject
{
    
}