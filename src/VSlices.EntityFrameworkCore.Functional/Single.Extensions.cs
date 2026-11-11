using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Functional;

public static partial class QueryableExtensions
{
    extension<A>(IQueryable<A> query)
    {
        public IO<A> SingleIO() =>
            liftIO(env => query.SingleAsync(env.Token));

        public IO<A> SingleIO(Expression<Func<A, bool>> expression) =>
            query.Where(expression).SingleIO();

        public OptionT<IO, A> SingleOrNoneIO(Expression<Func<A, bool>> expression) =>
            query.Where(expression).SingleOrNoneIO();

        public OptionT<IO, A> SingleOrNoneIO() =>
            liftIO<Option<A>>(async env => await query.SingleOrDefaultAsync(env.Token));
    }
}
