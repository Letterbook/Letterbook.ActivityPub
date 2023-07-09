using System.Text.Json.Serialization;

namespace Letterbook.ActivityPub.Models;

public class Activity : Object
{
    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> Actor { get; set; } = new List<IResolvable>();

    [JsonConverter(typeof(ConvertList<IResolvable>))]

    public IList<IResolvable> Object { get; set; } = new List<IResolvable>();

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> Target { get; set; } = new List<IResolvable>();

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> Result { get; set; } = new List<IResolvable>();

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> Origin { get; set; } = new List<IResolvable>();

    [JsonConverter(typeof(ConvertList<IResolvable>))]
    public IList<IResolvable> Instrument { get; set; } = new List<IResolvable>();
}