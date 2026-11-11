using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Functional;

public static class FindExtensions
{
    extension<A>(DbSet<A> set)
        where A : class
    {
        public OptionT<IO, A> FindOrNoneIO(params object?[] keyValues) =>
            liftIO<Option<A>>(async env =>
                await set.FindAsync(keyValues, env.Token));
    }
}
