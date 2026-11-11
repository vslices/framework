using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Functional;

public static partial class QueryableExtensions
{
    extension<A>(IQueryable<A> query)
    {
        public IO<bool> AnyIO() =>
            liftIO(async env => await query.AnyAsync(env.Token));

        public IO<bool> AnyIO(Expression<Func<A, bool>> expression) =>
            query.Where(expression).AnyIO();
    }
}
