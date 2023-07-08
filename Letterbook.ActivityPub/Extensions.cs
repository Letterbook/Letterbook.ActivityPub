using Letterbook.ActivityPub.Models;

namespace Letterbook.ActivityPub;

public static class Extensions
{
    public static Models.Object Resolve(this IResolvable self)
    {
        throw new NotImplementedException();
    }

    public static bool TryResolve<T>(this IResolvable self, out T value) where T : Models.Object
    {
        throw new NotImplementedException();
    }
    
    public static Models.Object Verify(this IResolvable self)
    {
        self.Verified = true;
        throw new NotImplementedException();
    }

    public static bool TryVerify<T>(this IResolvable self, out T value) where T : Models.Object
    {
        self.Verified = true;
        
        throw new NotImplementedException();
    }
}