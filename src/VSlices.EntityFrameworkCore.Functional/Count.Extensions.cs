using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Functional;

public static partial class QueryableExtensions
{
    extension<A>(IQueryable<A> query)
    {
        public IO<int> CountIO() =>
            liftIO(async env => await query.CountAsync(env.Token));

        public IO<int> CountIO(Expression<Func<A, bool>> expression) =>
            query.Where(expression).CountIO();
    }
}
