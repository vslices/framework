using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EntityFrameworkCore.Functional;

public static partial class QueryableExtensions
{
    extension<A>(IQueryable<A> query)
    {
        public IO<int> ExecuteDeleteIO() =>
            liftIO(async env => await query.ExecuteDeleteAsync(env.Token));

        public IO<int> ExecuteUpdateIO(Action<UpdateSettersBuilder<A>> setters) =>
            liftIO(async env => await query.ExecuteUpdateAsync(setters, env.Token));
    }
}
