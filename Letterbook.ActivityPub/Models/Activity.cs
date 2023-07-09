using System.Text.Json.Serialization;

namespace Letterbook.ActivityPub.Models;

// [JsonConverter(typeof(ConvertObject))]
public class Activity : Object
{
    public static List<string> Types = new(new[]
    {
        "Accept",
        "Add",
        "Announce",
        "Arrive",
        "Block",
        "Create",
        "Delete",
        "Dislike",
        "Flag",
        "Follow",
        "Ignore",
        "Invite",
        "Join",
        "Leave",
        "Like",
        "Listen",
        "Move",
        "Offer",
        "Question",
        "Reject",
        "Read",
        "Remove",
        "TentativeReject",
        "TentativeAccept",
        "Travel",
        "Undo",
        "Update",
        "View",
    });


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