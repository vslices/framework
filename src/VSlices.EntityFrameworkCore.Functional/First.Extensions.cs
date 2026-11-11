using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Functional;

public static partial class QueryableExtensions
{
    extension<A>(IQueryable<A> query)
    {
        public IO<A> FirstIO() =>
            liftIO(env => query.FirstAsync(env.Token));

        public IO<A> FirstIO(Expression<Func<A, bool>> expression) =>
            query.Where(expression).FirstIO();

        public OptionT<IO, A> FirstOrNoneIO(Expression<Func<A, bool>> expression) =>
            query.Where(expression).FirstOrNoneIO();

        public OptionT<IO, A> FirstOrNoneIO() =>
            liftIO<Option<A>>(async env => await query.FirstOrDefaultAsync(env.Token));
    }
}
