using Letterbook.ActivityPub.Models;

namespace Letterbook.ActivityPub;

public static class Extensions
{
    public static Models.Object? Resolve(this IResolvable self)
    {
        if (self is Models.Object o) return o;
        // TODO: else it's a Link, so fetch it
        return default;
    }

    public static bool TryResolve<T>(this IResolvable self, out T? value) where T : Models.Object
    {
        if (self.Resolve() is T o)
        {
            value = o;
            return true;
        }
        value = default;
        return false;
    }
}