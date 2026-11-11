using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Functional;

public static partial class QueryableExtensions
{
    extension<A>(IQueryable<A> query)
    {
        public IO<bool> AllIO(Expression<Func<A, bool>> expression) =>
            liftIO(async env => await query.AllAsync(expression, env.Token));
    }
}
